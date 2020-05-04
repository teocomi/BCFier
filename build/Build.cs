using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Utilities.Collections;
using Nuke.GitHub;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.IO.TextTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;
using static Nuke.Common.Tools.NuGet.NuGetTasks;
using static Nuke.GitHub.GitHubTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
  public static int Main() => Execute<Build>(x => x.PublishGitHubRelease);

  [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
  readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

  [Parameter] readonly string GitHubAuthenticationToken;

  [PackageExecutable("Tools.InnoSetup", "tools/ISCC.exe")] readonly Tool InnoSetup;

  [Solution] readonly Solution Solution;
  [GitRepository] readonly GitRepository GitRepository;
  [GitVersion] readonly GitVersion GitVersion;

  AbsolutePath OutputDirectory => RootDirectory / "output";

  Target Clean => _ => _
      .Before(Restore)
      .Executes(() =>
      {
        GlobDirectories(RootDirectory, "**/bin", "**/obj")
              // Excluding the build directory from cleanup, since this solution is set up with
              // no subdirectories for the actual sources
              .Where(d => !d.StartsWith(RootDirectory / "build"))
              .ForEach(DeleteDirectory);
        EnsureCleanDirectory(OutputDirectory);
      });

  Target Restore => _ => _
      .Executes(() =>
      {
        RestorePackages();
      });

  private void RestorePackages()
  {
    DotNetRestore(s => s
      .SetProjectFile(Solution));
    // This separate call uses the NuGet.CommandLine package to do a traditional
    // MSBuild restore. This is required to correctly store the conditional references
    // in the BCFier.Revit/packages.config file, which restores different Revit API
    // NuGet packages depending on the build configuration
    NuGetRestore(c => c.SetSolutionDirectory(RootDirectory));
  }

  Target Compile => _ => _
      .After(Clean)
      .DependsOn(Restore)
      .Executes(() =>
      {
        DotNetBuild(s => s
              .SetProjectFile(Solution)
              .SetConfiguration(Configuration)
              .SetAssemblyVersion(GitVersion.GetNormalizedAssemblyVersion())
              .SetFileVersion(GitVersion.GetNormalizedFileVersion())
              .SetInformationalVersion(GitVersion.InformationalVersion)
              .EnableNoRestore());
      });

  Target Test => _ => _
       .DependsOn(Clean)
       .DependsOn(Restore)
       .Executes(() =>
        {
          // The solution is compiled for the Debug-2020 target for testing
          MSBuild(c => c
            .SetSolutionFile(Solution.FileName)
            .SetConfiguration("Debug-2020"));
          DotNetTest(c => c
            .SetNoBuild(true)
            // The test dlls are copied to the bin/Debug folder
            .SetConfiguration("Debug")
            .SetProjectFile(RootDirectory / "Bcfier.Tests" / "Bcfier.Tests.csproj")
            .SetTestAdapterPath(".")
            .SetLogger($"xunit;LogFilePath={OutputDirectory / "testresults.xml"}"));
        });

  Target CompileReleaseConfigurations => _ => _
      .DependsOn(Clean)
      .DependsOn(Restore)
      .Executes(() =>
      {
        // This array should specify the configuration and the used package version
        // for the Revit API package
        var releaseConfigurations = new[,]
        {
          { "2020", "2020.0.1" },
          { "2019", "2019.0.1" }
        };

        MSBuild(opt => opt
          .SetVerbosity(MSBuildVerbosity.Quiet)
          .SetTargetPlatform(MSBuildTargetPlatform.x64)
          .SetProjectFile(Solution.Directory / "Bcfier.Win" / "Bcfier.Win.csproj")
          .SetConfiguration("Release-2020")
          .SetOutDir(OutputDirectory / "Bcfier.Win")
          .SetAssemblyVersion(GitVersion.GetNormalizedAssemblyVersion())
          .SetFileVersion(GitVersion.GetNormalizedFileVersion())
          .SetInformationalVersion(GitVersion.InformationalVersion));

        var csprojPath = Solution.Directory / "Bcfier.Revit" / "Bcfier.Revit.csproj";
        var originalCsproj = ReadAllText(csprojPath);
        var xDoc = XDocument.Parse(originalCsproj);
        var revitApiVersionElement = xDoc.Root
        .Descendants()
        .Where(d => d.Name.LocalName == "Version"
          && d.Parent.Name.LocalName == "PackageReference"
          && d.Parent.Attribute("Include").Value == "Revit_All_Main_Versions_API_x64")
          .Single();
        var originalRevitApiVersion = revitApiVersionElement.Value;

        try
        {
          for (var i = 0; i < releaseConfigurations.Length / 2; i++)
          {
            revitApiVersionElement.Value = releaseConfigurations[i, 1];
            WriteAllText(csprojPath, ToStringWithDeclaration(xDoc));
            // To ensure the file is written to disk
            Task.Delay(1_000).ConfigureAwait(false).GetAwaiter().GetResult();
            RestorePackages();
            var configuration = releaseConfigurations[i, 0];
            MSBuild(opt => opt
                .SetVerbosity(MSBuildVerbosity.Quiet)
                .SetProjectFile(Solution.Directory / "Bcfier.Revit" / "Bcfier.Revit.csproj")
                .SetConfiguration($"Release-{configuration}")
                .SetOutDir(OutputDirectory / configuration / "Bcfier.Revit")
                .SetAssemblyVersion(GitVersion.GetNormalizedAssemblyVersion())
                .SetFileVersion(GitVersion.GetNormalizedFileVersion())
                .SetInformationalVersion(GitVersion.InformationalVersion));

            CopyDirectoryRecursively(OutputDirectory / "Bcfier.Win", OutputDirectory / configuration / "Bcfier.Revit" / "Bcfier.Win");
          }
        }
        finally
        {
          revitApiVersionElement.Value = originalRevitApiVersion;
          WriteAllText(csprojPath, ToStringWithDeclaration(xDoc));
        }
      });

  public static string ToStringWithDeclaration(XDocument doc)
  {
    if (doc == null)
    {
      throw new ArgumentNullException("doc");
    }
    var builder = new StringBuilder();
    using (var writer = new StringWriter(builder))
    {
      doc.Save(writer);
    }
    return builder.ToString();
  }

  Target CreateSetup => _ => _
      .DependsOn(CompileReleaseConfigurations)
      .Executes(() =>
      {
        // The Inno Setup tool generates a single, self contained setup application
        // in the root directory as BCFier.exe. This can be distributed for installation
        InnoSetup($"{RootDirectory / "InnoSetup" / "BCFier.iss"}");
      });

  Target PublishGitHubRelease => _ => _
      .DependsOn(CreateSetup)
      .Requires(() => GitHubAuthenticationToken)
      .Executes(async () =>
      {
        var releaseTag = $"v{GitVersion.SemVer}";
        var isStableRelease = GitVersion.BranchName.Equals("master") || GitVersion.BranchName.Equals("origin/master");

        var repositoryInfo = GetGitHubRepositoryInfo(GitRepository);
        var installationFile = GlobFiles(OutputDirectory, "BCFier.exe").NotEmpty().ToArray();

        await PublishRelease(x => x
              .SetPrerelease(!isStableRelease)
              .SetArtifactPaths(installationFile)
              .SetCommitSha(GitVersion.Sha)
              .SetRepositoryName(repositoryInfo.repositoryName)
              .SetRepositoryOwner(repositoryInfo.gitHubOwner)
              .SetTag(releaseTag)
              .SetToken(GitHubAuthenticationToken));
      });
}

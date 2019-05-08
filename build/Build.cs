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
using System.Linq;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
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
        DotNetRestore(s => s
              .SetProjectFile(Solution));
        // This separate call uses the NuGet.CommandLine package to do a traditional
        // MSBuild restore. This is required to correctly store the conditional references
        // in the BCFier.Revit/packages.config file, which restores different Revit API
        // NuGet packages depending on the build configuration
        NuGetRestore(c => c.SetSolutionDirectory(RootDirectory));
      });

  Target Compile => _ => _
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

  Target CompileReleaseConfigurations => _ => _
      .DependsOn(Clean)
      .DependsOn(Restore)
      .Executes(() =>
      {
        Solution.Configurations
              // That select statements simply returns only the configuration part, e.g.
              // "Release-2020|Any CPU" -> "Release-2020"
              .Select(c => c.Key.Split("|").First())
              .Where(c => c.Contains("Release-"))
              .ForEach(configuration => MSBuild(opt => opt
                  .SetSolutionFile(Solution.FileName)
                  .SetConfiguration(configuration)
                  .SetAssemblyVersion(GitVersion.GetNormalizedAssemblyVersion())
                  .SetFileVersion(GitVersion.GetNormalizedFileVersion())
                  .SetInformationalVersion(GitVersion.InformationalVersion)));
      });

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
        var installationFile = GlobFiles(RootDirectory, "BCFier.exe").NotEmpty().ToArray();

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

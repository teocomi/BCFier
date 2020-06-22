using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace OpenProject.Tests
{
  public static class TestFilesFactory
  {
    public static Stream GetTestFileStream(TestFile testFile)
    {
      var assembly = typeof(TestFilesFactory).GetTypeInfo().Assembly;
      var exactResourceNames = assembly.GetManifestResourceNames()
          .Where(name => name.Contains(testFile.ToString()))
          .ToList();
      if (exactResourceNames.Count != 1)
      {
        throw new Exception("Resource name ambigious");
      }
      return assembly.GetManifestResourceStream(exactResourceNames.First());
    }
  }
}

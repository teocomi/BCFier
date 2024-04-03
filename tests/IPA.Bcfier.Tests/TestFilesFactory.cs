namespace IPA.Bcfier.Tests
{
    public static class TestFilesFactory
    {
        public static List<object[]> TestFiles => Enum.GetValues(typeof(TestFile))
            .Cast<TestFile>()
            .Select(f => new object[] { f })
            .ToList();

        public static Stream GetTestFileStream(TestFile testFile)
        {
            var assembly = typeof(TestFilesFactory).Assembly;
            var resourceName = $"IPA.Bcfier.Tests.Resources.{testFile.ToString().Replace("_", ".")}";
            resourceName = assembly.GetManifestResourceNames()
                .Single(r => r.StartsWith(resourceName));
            var resourceStream = assembly.GetManifestResourceStream(resourceName);
            if (resourceStream == null)
            {
                throw new Exception($"Failed to find the resource stream for test file: {testFile}");
            }

            return resourceStream;
        }
    }
}
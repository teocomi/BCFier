using IPA.Bcfier.Services;

namespace IPA.Bcfier.Tests.Services
{
    public class BcfImportServiceTests
    {
        public static List<object[]> TestFiles => TestFilesFactory.TestFiles;

        [Fact]
        public void FindsTestFiles()
        {
            Assert.NotEmpty(TestFiles);
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public async Task CanImportBcfFile(TestFile testFile)
        {
            using var bcfStream = TestFilesFactory.GetTestFileStream(testFile);
            var bcfImportService = new BcfImportService();
            var bcfFile = await bcfImportService.ImportBcfFileAsync(bcfStream, testFile.ToString());
            Assert.NotNull(bcfFile);
        }
    }
}

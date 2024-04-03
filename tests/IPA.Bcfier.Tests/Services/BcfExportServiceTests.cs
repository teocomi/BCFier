using IPA.Bcfier.Services;

namespace IPA.Bcfier.Tests.Services
{
    public class BcfExportServiceTests
    {
        public static List<object[]> TestFiles => TestFilesFactory.TestFiles;

        [Fact]
        public void FindsTestFiles()
        {
            Assert.NotEmpty(TestFiles);
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public async Task CanImportThenExportBcfFile(TestFile testFile)
        {
            using var bcfStream = TestFilesFactory.GetTestFileStream(testFile);
            var bcfImportService = new BcfImportService();
            var bcfFile = await bcfImportService.ImportBcfFileAsync(bcfStream, testFile.ToString());
            var exportedFile = new BcfExportService().ExportBcfFile(bcfFile);
            Assert.NotNull(exportedFile);
            Assert.True(exportedFile.Length > 0);
        }
    }
}

using IPA.Bcfier.Models.Bcf;
using System.IO;

namespace IPA.Bcfier.Services
{
    public class BcfExportService
    {
        public Stream ExportBcfFile(BcfFile bcfFile)
        {
            var bcfv3Container = new BcfConversionFromModelService().ConvertBcfFileToBcfContainer(bcfFile);
            var memStream = new MemoryStream();
            bcfv3Container.WriteStream(memStream);
            memStream.Position = 0;
            return memStream;
        }
    }
}

using IPA.Bcfier.Models.Bcf;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IPA.Bcfier.Services
{
    public class BcfImportService
    {
        public async Task<BcfFile> ImportBcfFileAsync(Stream bcfFileStream, string fileName)
        {
            using var bcfStreamCopy = new MemoryStream();
            await bcfFileStream.CopyToAsync(bcfStreamCopy);
            var bcfContainer = ReadBcfFile(bcfStreamCopy);
            var bcfConversionService = new BcfConversionToModelService();
            return bcfConversionService.ConvertBcfContainerToBcfFile(bcfContainer, fileName);
        }

        private Dangl.BCF.BCFv3.BCFv3Container ReadBcfFile(Stream bcfFileStream)
        {
            var bcfVersion = DetermineBcfFileType(bcfFileStream);
            bcfFileStream.Position = 0;

            Dangl.BCF.BCFv3.BCFv3Container bcfContainer;
            switch (bcfVersion)
            {
                case BcfFileType.V2:
                    var v2Container = Dangl.BCF.BCFv2.BCFv2Container.ReadStream(bcfFileStream);
                    var v21Fromv2Container = new Dangl.BCF.Converter.V2ToV21(v2Container).Convert();
                    bcfContainer = new Dangl.BCF.Converter.V21ToV3(v21Fromv2Container).Convert();
                    break;

                case BcfFileType.V21:
                    var v21Container = Dangl.BCF.BCFv21.BCFv21Container.ReadStream(bcfFileStream);
                    bcfContainer = new Dangl.BCF.Converter.V21ToV3(v21Container).Convert();
                    break;

                case BcfFileType.V3:
                    bcfContainer = Dangl.BCF.BCFv3.BCFv3Container.ReadStream(bcfFileStream);
                    break;

                default:
                    throw new NotSupportedException("Unsupported BCF version");
            }

            return bcfContainer;
        }

        private BcfFileType DetermineBcfFileType(Stream bcfFileStream)
        {
            using var bcfZipArchive = new ZipArchive(bcfFileStream, ZipArchiveMode.Read, leaveOpen: true);
            var versionEntry = bcfZipArchive.Entries.FirstOrDefault(e => string.Equals(e.FullName, "bcf.version", StringComparison.OrdinalIgnoreCase));
            if (versionEntry != null)
            {
                using var versionEntryStream = versionEntry.Open();
                var xDoc = XDocument.Load(versionEntryStream);
                var versionId = xDoc.Descendants().Where(d => d.Attributes().Any(a => a.Name.LocalName.Equals("VersionId", StringComparison.InvariantCultureIgnoreCase)))
                    .Select(d => d.Attribute("VersionId").Value)
                    .FirstOrDefault();
                return versionId switch
                {
                    "2.0" => BcfFileType.V2,
                    "2.1" => BcfFileType.V21,
                    "3.0" => BcfFileType.V3,
                    _ => BcfFileType.Unknown
                };
            }

            return BcfFileType.Unknown;
        }

        private enum BcfFileType
        {
            Unknown,
            V2,
            V21,
            V3
        }
    }
}

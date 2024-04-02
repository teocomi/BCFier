using ElectronNET.API;
using ElectronNET.API.Entities;
using IPA.Bcfier.App.Services;
using IPA.Bcfier.Models.Bcf;
using IPA.Bcfier.Services;
using Microsoft.AspNetCore.Mvc;

namespace IPA.Bcfier.App.Controllers
{
    [ApiController]
    [Route("api/bcf-conversion")]
    public class BcfConversionController : ControllerBase
    {
        private readonly ElectronWindowProvider _electronWindowProvider;

        public BcfConversionController(ElectronWindowProvider electronWindowProvider)
        {
            _electronWindowProvider = electronWindowProvider;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportBcfFileAsync()
        {
            var electronWindow = _electronWindowProvider.BrowserWindow;
            if (electronWindow == null)
            {
                return BadRequest();
            }

            var fileSelectionResult = await Electron.Dialog.ShowOpenDialogAsync(electronWindow, new OpenDialogOptions
            {
                Filters = new []
                {
                    new FileFilter
                    {
                        Name = "BCF File",
                        Extensions = new string[] { "bcf", "bcfzip" }
                    }
                }
            });

            if (fileSelectionResult == null)
            {
                return NoContent();
            }

            try
            {
                using var bcfFileStream = System.IO.File.OpenRead(fileSelectionResult.First());
                var bcfFileName = Path.GetFileName(fileSelectionResult.FirstOrDefault());
                var bcfResult = await new BcfImportService().ImportBcfFileAsync(bcfFileStream, bcfFileName ?? "issue.bcf");
                return Ok(bcfResult);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [HttpPost("export")]
        public async Task<IActionResult> ExportBcfFileAsync([FromBody] BcfFile bcfFile)
        {
            var bcfFileResult = new BcfExportService().ExportBcfFile(bcfFile);
            if (bcfFileResult == null)
            {
                return BadRequest();
            }

            var electronWindow = _electronWindowProvider.BrowserWindow;
            if (electronWindow == null)
            {
                return BadRequest();
            }

            var fileSaveSelectResult = await Electron.Dialog.ShowSaveDialogAsync(electronWindow, new SaveDialogOptions
            {
                DefaultPath = bcfFile.FileName ?? "issue.bcf",
                Filters = new []
                {
                    new FileFilter
                    {
                        Name = "BCF File",
                        Extensions = new string[] { "bcf" }
                    }
                }
            });

            if (fileSaveSelectResult == null)
            {
                return NoContent();
            }

            using var fs = System.IO.File.Create(fileSaveSelectResult);
            await bcfFileResult.CopyToAsync(fs);
            return NoContent();
        }
    }
}

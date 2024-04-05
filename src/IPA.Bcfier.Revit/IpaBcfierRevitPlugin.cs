using Autodesk.Revit.UI;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace IPA.Bcfier.Revit
{
    public class IpaBcfierRevitPlugin : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {


            var buttonData = new PushButtonData("openPluginButton", "Open Plugin", Assembly.GetExecutingAssembly().Location, "IPA.Bcfier.Revit.OpenIpaBcfierWindowCommand");
            var pushButton = application.CreateRibbonPanel("IPA").AddItem(buttonData) as PushButton;
            pushButton.ToolTip = "Launch IPA.Bcfier Revit Plugin";
            pushButton.Image = GetBitmapImage("button16.png");
            pushButton.LargeImage = GetBitmapImage("button32.png");

            return Result.Succeeded;
        }

        private BitmapImage GetBitmapImage(string resourceName)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream($"IPA.Bcfier.Revit.Resources.{resourceName}");
            bitmapImage.EndInit();
            return bitmapImage;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}

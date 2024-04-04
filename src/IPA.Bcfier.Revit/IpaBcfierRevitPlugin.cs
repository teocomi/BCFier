using Autodesk.Revit.UI;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace IPA.Bcfier.Revit
{
    public class IpaBcfierRevitPlugin : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("IPA.Bcfier.Revit.Resources.ButtonLogo.png");
            bitmapImage.EndInit();

            var buttonData = new PushButtonData("openPluginButton", "Open Plugin", Assembly.GetExecutingAssembly().Location, "IPA.Bcfier.Revit.OpenIpaBcfierWindowCommand");
            var pushButton = application.CreateRibbonPanel("IPA").AddItem(buttonData) as PushButton;
            pushButton.ToolTip = "Launch IPA.Bcfier Revit Plugin";
            pushButton.LargeImage = bitmapImage;

            application.Idling += IdlingHandler.OnIdling;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}

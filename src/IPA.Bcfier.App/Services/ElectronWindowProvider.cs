using ElectronNET.API;

namespace IPA.Bcfier.App.Services
{
    public class ElectronWindowProvider
    {
        public BrowserWindow? BrowserWindow { get; private set; }

        public void SetBrowserWindow(BrowserWindow browserWindow)
        {
            BrowserWindow = browserWindow;
        }
    }
}

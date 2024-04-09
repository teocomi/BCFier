using ElectronNET.API;
using ElectronNET.API.Entities;
using IPA.Bcfier.App.Configuration;
using IPA.Bcfier.App.Services;
using IPA.Bcfier.Ipc;
using Newtonsoft.Json;

namespace IPA.Bcfier.App
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();

                await host.StartAsync();

                var browserWindowOptions = new BrowserWindowOptions
                {
                    Title = "IPA.BCFier",
                    Icon = Path.Combine(Directory.GetCurrentDirectory(), "bcfier.png"),
                    Height = 800,
                    Width = 1200,
                    AutoHideMenuBar = true
                };
                var window = await Electron.WindowManager.CreateWindowAsync(browserWindowOptions);

                var hasRevitIntegration = false;
                using (var scope = host.Services.CreateScope())
                {
                    scope.ServiceProvider.GetRequiredService<ElectronWindowProvider>().SetBrowserWindow(window);
                    hasRevitIntegration = await Electron.App.CommandLine.HasSwitchAsync("revit-integration");
                    scope.ServiceProvider.GetRequiredService<RevitParameters>().IsConnectedToRevit = hasRevitIntegration;
                }

                await Electron.IpcMain.On("closeApp", async (e) =>
                {
                    if (hasRevitIntegration)
                    {
                        try
                        {
                            using var ipcHandler = new IpcHandler(thisAppName: "BcfierApp", otherAppName: "Revit");
                            await ipcHandler.SendMessageAsync(JsonConvert.SerializeObject(new IpcMessage
                            {
                                CorrelationId = Guid.NewGuid(),
                                Command = IpcMessageCommand.AppClosed,
                                Data = null
                            }), timeout: 500);
                        }
                        catch (Exception ex)
                        {
                            // We're not really handling failures here, just write them to
                            // the console and then continue with closing the window
                            Console.WriteLine(ex);
                        }
                    }
                    window.Close();
                });

                await host.WaitForShutdownAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging(c => c.AddDebug().AddConsole())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseElectron(args);
                    webBuilder.UseStartup<Startup>();
                });
    }
}

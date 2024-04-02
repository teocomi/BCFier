using ElectronNET.API;
using ElectronNET.API.Entities;
using IPA.Bcfier.App.Services;

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
                    AutoHideMenuBar = true,
                };

                var window = await Electron.WindowManager.CreateWindowAsync(browserWindowOptions);

                using (var scope = host.Services.CreateScope())
                {
                    scope.ServiceProvider.GetRequiredService<ElectronWindowProvider>().SetBrowserWindow(window);
                }

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

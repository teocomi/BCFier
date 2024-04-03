using Dangl.Data.Shared.AspNetCore.SpaUtilities;
using ElectronNET.API;
using IPA.Bcfier.App.Services;
using IPA.Bcfier.Services;

namespace IPA.Bcfier.App
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddElectron();
            services.AddLocalizedSpaStaticFiles(".IPA.App.Locale", new[] { "en" }, "dist");
            services.AddSingleton<ElectronWindowProvider>();
            services.AddTransient<SettingsService>();
            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (env.IsDevelopment())
            {
                app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "../ipa-bcfier-ui";
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                });
            }
            else
            {
                app.UseLocalizedSpaStaticFiles("index.html", "dist", cacheFilesInRootPath: false);
            }
        }
    }
}

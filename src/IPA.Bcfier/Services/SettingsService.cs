﻿using IPA.Bcfier.Models.Settings;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace IPA.Bcfier.Services
{
    public class SettingsService
    {
        public async Task<Settings> LoadSettingsAsync()
        {
            var settingsPath = GetPathToSettingsFile();
            if (!File.Exists(settingsPath))
            {
                return new Settings
                {
                    Username = Environment.UserName
                };
            }

            var serializedSettings = await File.ReadAllTextAsync(settingsPath);
            var deserializedSettings = JsonConvert.DeserializeObject<Settings>(serializedSettings);


            if (deserializedSettings == null)
            {
                return new Settings
                {
                    Username = Environment.UserName
                };
            }

            return deserializedSettings;
        }

        public async Task SaveSettingsAsync(Settings settings)
        {
            var serializedSettings = JsonConvert.SerializeObject(settings);
            await File.WriteAllTextAsync(GetPathToSettingsFile(), serializedSettings);
        }

        private string GetPathToSettingsFile()
        {
            var settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "IPA.BCFier",
                "settings.json");

            if (!Directory.Exists(Path.GetDirectoryName(settingsPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(settingsPath));
            }

            return settingsPath;
        }
    }
}

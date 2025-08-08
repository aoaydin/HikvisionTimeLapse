using System;
using System.IO;
using HikvisionTimeLapse.Core.Models;
using Newtonsoft.Json;

namespace HikvisionTimeLapse.Core.Services;

public interface ISettingsService
{
    AppSettings Load();
    void Save(AppSettings settings);
    string SettingsFilePath { get; }
}

public sealed class SettingsService : ISettingsService
{
    private readonly string _settingsPath;

    public SettingsService(string? baseDirectory = null)
    {
        var appData = baseDirectory ?? AppContext.BaseDirectory;
        _settingsPath = Path.Combine(appData, "settings.json");
    }

    public string SettingsFilePath => _settingsPath;

    public AppSettings Load()
    {
        try
        {
            if (!File.Exists(_settingsPath))
            {
                var defaults = new AppSettings();
                Save(defaults);
                return defaults;
            }

            var json = File.ReadAllText(_settingsPath);
            var settings = JsonConvert.DeserializeObject<AppSettings>(json) ?? new AppSettings();
            // Migrate legacy single-camera fields into Cameras list if empty
            if (settings.Cameras.Count == 0 && !string.IsNullOrWhiteSpace(settings.RtspUrl))
            {
                settings.Cameras.Add(new CameraConfig
                {
                    Name = "Kamera 1",
                    RtspUrl = settings.RtspUrl,
                    Username = settings.Username,
                    Password = settings.Password,
                    Enabled = true
                });
            }
            return settings;
        }
        catch
        {
            return new AppSettings();
        }
    }

    public void Save(AppSettings settings)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_settingsPath)!);
        var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
        File.WriteAllText(_settingsPath, json);
    }
}



using HikvisionTimeLapse.Core.Models;
using HikvisionTimeLapse.Core.Services;

namespace HikvisionTimeLapse.Tests;

[TestClass]
public sealed class Test1
{
    [TestMethod]
    public void Settings_Save_And_Load_Works()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        try
        {
            var settingsService = new SettingsService(tempDir);
            var settings = settingsService.Load();
            settings.RtspUrl = "rtsp://example/stream";
            settingsService.Save(settings);
            var loaded = settingsService.Load();
            Assert.AreEqual("rtsp://example/stream", loaded.RtspUrl);
        }
        finally
        {
            Directory.Delete(tempDir, true);
        }
    }
}

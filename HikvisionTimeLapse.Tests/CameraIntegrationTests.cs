using System;
using System.IO;
using System.Threading.Tasks;
using HikvisionTimeLapse.Core.Models;
using HikvisionTimeLapse.Core.Services;

namespace HikvisionTimeLapse.Tests;

[TestClass]
public sealed class CameraIntegrationTests
{
    private static AppSettings CreateSettings()
    {
        var rtsp = Environment.GetEnvironmentVariable("CAM_RTSP") ?? "rtsp://127.0.0.1/stream";
        var user = Environment.GetEnvironmentVariable("CAM_USER") ?? "user";
        var pass = Environment.GetEnvironmentVariable("CAM_PASS") ?? "pass";
        return new AppSettings
        {
            RtspUrl = rtsp,
            Username = user,
            Password = pass,
            PhotosRootDirectory = Path.Combine(Path.GetTempPath(), "tl_photos_" + Guid.NewGuid().ToString("N")),
            OutputDirectory = Path.Combine(Path.GetTempPath(), "tl_out_" + Guid.NewGuid().ToString("N")),
            CaptureOpenTimeoutMs = 8000
        };
    }

    [TestMethod]
    public async Task Test_Camera_Connection()
    {
        var settings = CreateSettings();
        var capture = new CaptureService();
        var ok = await capture.TestConnectionAsync(settings.BuildEffectiveRtsp(), settings.CaptureOpenTimeoutMs);
        if (!ok)
        {
            Assert.Inconclusive("Kamera bağlantısı kurulamadı. IP/erişim kontrolü gerekir.");
        }
    }

    [TestMethod]
    public async Task Test_Capture_Single_Frame()
    {
        var settings = CreateSettings();
        var capture = new CaptureService();
        var (success, path, error) = await capture.CaptureAndSaveSingleFrameAsync(settings);
        try
        {
            if (!success)
            {
                Assert.Inconclusive("Yakalama başarısız: " + error);
            }
            Assert.IsTrue(File.Exists(path!), "Dosya oluşturulmadı");
        }
        finally
        {
            try
            {
                if (Directory.Exists(settings.PhotosRootDirectory)) Directory.Delete(settings.PhotosRootDirectory, true);
            }
            catch { }
        }
    }
}




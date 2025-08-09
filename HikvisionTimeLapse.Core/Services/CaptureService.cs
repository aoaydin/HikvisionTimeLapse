using System;
using System.IO;
using System.Threading.Tasks;
using HikvisionTimeLapse.Core.Models;
using OpenCvSharp;

namespace HikvisionTimeLapse.Core.Services;

public interface ICaptureService
{
    Task<(bool success, string? savedPath, string? error)> CaptureAndSaveSingleFrameAsync(AppSettings settings);
    Task<(bool success, string? savedPath, string? error)> CaptureAndSaveSingleFrameAsync(CameraConfig camera, AppSettings settings);
    Task<bool> TestConnectionAsync(string rtspUrl, int openTimeoutMs);
}

public sealed class CaptureService : ICaptureService
{
    public async Task<(bool success, string? savedPath, string? error)> CaptureAndSaveSingleFrameAsync(AppSettings settings)
    {
        var rtspUrl = settings.BuildEffectiveRtsp();
        var retries = Math.Max(1, settings.CaptureRetryCount);
        var delay = Math.Max(0, settings.CaptureRetryDelayMs);

        for (var attempt = 1; attempt <= retries; attempt++)
        {
            try
            {
                // Prefer FFmpeg backend explicitly for RTSP
                using var capture = new VideoCapture(rtspUrl, VideoCaptureAPIs.FFMPEG);
                capture.Open(rtspUrl, VideoCaptureAPIs.FFMPEG);

                if (!capture.IsOpened())
                {
                    if (attempt < retries) await Task.Delay(delay);
                    continue;
                }

                // Read a couple of warm-up frames
                using var frame = new Mat();
                for (int i = 0; i < 3; i++)
                {
                    capture.Read(frame);
                }
                if (frame.Empty())
                {
                    if (attempt < retries) await Task.Delay(delay);
                    continue;
                }

                var now = DateTime.Now;
                var dayDir = Path.Combine(settings.PhotosRootDirectory, now.ToString("yyyy-MM-dd"));
                Directory.CreateDirectory(dayDir);
                var filename = now.ToString("HH-mm-ss") + ".jpg";
                var path = Path.Combine(dayDir, filename);
                Cv2.ImWrite(path, frame);
                return (true, path, null);
            }
            catch (Exception ex)
            {
                if (attempt >= retries)
                {
                    return (false, null, ex.Message);
                }
                await Task.Delay(delay);
            }
        }

        return (false, null, "Bilinmeyen hata: yakalama başarısız");
    }

    public async Task<(bool success, string? savedPath, string? error)> CaptureAndSaveSingleFrameAsync(CameraConfig camera, AppSettings settings)
    {
        var rtspUrl = camera.BuildEffectiveRtsp();
        var retries = Math.Max(1, settings.CaptureRetryCount);
        var delay = Math.Max(0, settings.CaptureRetryDelayMs);

        for (var attempt = 1; attempt <= retries; attempt++)
        {
            try
            {
                using var capture = new VideoCapture(rtspUrl, VideoCaptureAPIs.FFMPEG);
                capture.Open(rtspUrl, VideoCaptureAPIs.FFMPEG);

                if (!capture.IsOpened())
                {
                    if (attempt < retries) await Task.Delay(delay);
                    continue;
                }

                using var frame = new Mat();
                for (int i = 0; i < 3; i++) capture.Read(frame);
                if (frame.Empty())
                {
                    if (attempt < retries) await Task.Delay(delay);
                    continue;
                }

                var now = DateTime.Now;
                var dayDir = Path.Combine(settings.PhotosRootDirectory, camera.Id, now.ToString("yyyy-MM-dd"));
                Directory.CreateDirectory(dayDir);
                var filename = now.ToString("HH-mm-ss") + ".jpg";
                var path = Path.Combine(dayDir, filename);
                Cv2.ImWrite(path, frame);
                return (true, path, null);
            }
            catch (Exception ex)
            {
                if (attempt >= retries)
                {
                    return (false, null, ex.Message);
                }
                await Task.Delay(delay);
            }
        }

        return (false, null, "Bilinmeyen hata: yakalama başarısız");
    }

    public async Task<bool> TestConnectionAsync(string rtspUrl, int openTimeoutMs)
    {
        using var cts = new System.Threading.CancellationTokenSource(openTimeoutMs);
        try
        {
            using var capture = new VideoCapture();
            var token = cts.Token;
            var openTask = Task.Run(() => capture.Open(rtspUrl, VideoCaptureAPIs.FFMPEG), token);
            using var delayCts = new System.Threading.CancellationTokenSource(openTimeoutMs);
            var completed = await Task.WhenAny(openTask, Task.Delay(openTimeoutMs, delayCts.Token));
            if (completed != openTask)
            {
                try { capture.Release(); } catch { }
                return false;
            }
            if (!capture.IsOpened()) return false;
            using var frame = new Mat();
            // Warm-up with cancellation checks
            var readOk = false;
            for (int i = 0; i < 3 && !token.IsCancellationRequested; i++)
            {
                readOk = capture.Read(frame) || readOk;
            }
            capture.Release();
            return readOk && !frame.Empty();
        }
        catch
        {
            return false;
        }
    }
}



using System;

namespace HikvisionTimeLapse.Core.Models;

public sealed class AppSettings
{
    // Legacy single-camera fields (kept for backward compatibility)
    public string RtspUrl { get; set; } = "";
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";

    // Multi-camera configuration
    public List<CameraConfig> Cameras { get; set; } = new();

    // Output and storage
    public string PhotosRootDirectory { get; set; } = "photos";
    public string OutputDirectory { get; set; } = "output";

    // Capture
    public int CaptureRetryCount { get; set; } = 3;
    public int CaptureRetryDelayMs { get; set; } = 1500;
    public int CaptureOpenTimeoutMs { get; set; } = 5000;

    // Schedule times in HH:mm format
    public List<string> DailyCaptureTimes { get; set; } = new() { "10:00", "13:00", "16:30" };

    // Timelapse defaults
    public int DefaultFps { get; set; } = 25;
    public int DefaultWidth { get; set; } = 1920;
    public int DefaultHeight { get; set; } = 1080;

    public string BuildEffectiveRtsp()
    {
        if (string.IsNullOrWhiteSpace(RtspUrl)) return string.Empty;
        // If URL already contains credentials, keep as is
        var withCreds = RtspUrl;
        if (!RtspUrl.Contains("@"))
        {
            if (!RtspUrl.StartsWith("rtsp://", StringComparison.OrdinalIgnoreCase)) return RtspUrl;
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password)) return RtspUrl;
            var rest = RtspUrl.Substring("rtsp://".Length);
            withCreds = $"rtsp://{Uri.EscapeDataString(Username)}:{Uri.EscapeDataString(Password)}@{rest}";
        }

        // Force TCP transport to avoid UDP drops
        if (!withCreds.Contains("rtsp_transport=tcp", StringComparison.OrdinalIgnoreCase))
        {
            withCreds += withCreds.Contains('?') ? "&rtsp_transport=tcp" : "?rtsp_transport=tcp";
        }

        return withCreds;
    }
}



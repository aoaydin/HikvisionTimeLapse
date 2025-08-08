using System;

namespace HikvisionTimeLapse.Core.Models;

public sealed class CameraConfig
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Name { get; set; } = "Kamera";
    public string RtspUrl { get; set; } = "";
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public bool Enabled { get; set; } = true;

    public string BuildEffectiveRtsp()
    {
        if (string.IsNullOrWhiteSpace(RtspUrl)) return string.Empty;
        var withCreds = RtspUrl;
        if (!RtspUrl.Contains("@") && RtspUrl.StartsWith("rtsp://", StringComparison.OrdinalIgnoreCase)
            && !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password))
        {
            var rest = RtspUrl.Substring("rtsp://".Length);
            withCreds = $"rtsp://{Uri.EscapeDataString(Username)}:{Uri.EscapeDataString(Password)}@{rest}";
        }
        if (!withCreds.Contains("rtsp_transport=tcp", StringComparison.OrdinalIgnoreCase))
        {
            withCreds += withCreds.Contains('?') ? "&rtsp_transport=tcp" : "?rtsp_transport=tcp";
        }
        return withCreds;
    }
}




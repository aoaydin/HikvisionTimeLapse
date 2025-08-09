using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HikvisionTimeLapse.Core.Models;
using OpenCvSharp;

namespace HikvisionTimeLapse.Core.Services;

public interface ITimelapseService
{
    Task<(bool success, string? outputPath, string? error)> CreateTimelapseAsync(AppSettings settings, string? startDate = null, string? endDate = null, int? fps = null, int? width = null, int? height = null, IProgress<double>? progress = null);
    Task<(bool success, string? outputPath, string? error)> CreateTimelapseForCamerasAsync(AppSettings settings, IEnumerable<string> cameraIds, string? startDate = null, string? endDate = null, int? fps = null, int? width = null, int? height = null, IProgress<double>? progress = null);
}

public sealed class TimelapseService : ITimelapseService
{
    public async Task<(bool success, string? outputPath, string? error)> CreateTimelapseAsync(AppSettings settings, string? startDate = null, string? endDate = null, int? fps = null, int? width = null, int? height = null, IProgress<double>? progress = null)
    {
        return await Task.Run<(bool, string?, string?)>(() =>
        {
            try
            {
                var targetFps = fps ?? settings.DefaultFps;
                var outWidth = width ?? settings.DefaultWidth;
                var outHeight = height ?? settings.DefaultHeight;

                var allDayDirs = new DirectoryInfo(settings.PhotosRootDirectory).Exists
                    ? Directory.GetDirectories(settings.PhotosRootDirectory)
                    : Array.Empty<string>();

                var selectedDirs = FilterByDateRange(allDayDirs, startDate, endDate);

                var allImages = new List<string>();
                foreach (var dir in selectedDirs)
                {
                    var images = Directory.GetFiles(dir, "*.jpg", SearchOption.TopDirectoryOnly)
                        .OrderBy(p => p)
                        .ToArray();
                    allImages.AddRange(images);
                }

                if (allImages.Count == 0)
                {
                    return (false, null, "Fotoğraf bulunamadı");
                }

                Directory.CreateDirectory(settings.OutputDirectory);
                var first = Path.GetFileName(Path.GetDirectoryName(allImages.First()) ?? string.Empty);
                var last = Path.GetFileName(Path.GetDirectoryName(allImages.Last()) ?? string.Empty);
                var outPath = Path.Combine(settings.OutputDirectory, $"timelapse_{first}_to_{last}.mp4");

                using var writer = CreateWriterWithFallback(outPath, targetFps, outWidth, outHeight);
                if (writer == null || !writer.IsOpened())
                {
                    return (false, null, "VideoWriter açılamadı (uygun codec bulunamadı)");
                }

                for (int i = 0; i < allImages.Count; i++)
                {
                    using var img = Cv2.ImRead(allImages[i]);
                    if (img.Empty()) continue;
                    using var resized = new Mat();
                    Cv2.Resize(img, resized, new OpenCvSharp.Size(outWidth, outHeight));
                    writer.Write(resized);
                    progress?.Report((i + 1) / (double)allImages.Count);
                }

                writer.Release();
                return (true, outPath, null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        });
    }

    public async Task<(bool success, string? outputPath, string? error)> CreateTimelapseForCamerasAsync(AppSettings settings, IEnumerable<string> cameraIds, string? startDate = null, string? endDate = null, int? fps = null, int? width = null, int? height = null, IProgress<double>? progress = null)
    {
        return await Task.Run<(bool, string?, string?)>(() =>
        {
            try
            {
                var targetFps = fps ?? settings.DefaultFps;
                var outWidth = width ?? settings.DefaultWidth;
                var outHeight = height ?? settings.DefaultHeight;

                var camerasSet = new HashSet<string>(cameraIds ?? Enumerable.Empty<string>());

                // Collect frames synchronized by timestamp across selected cameras, simple interleave strategy
                var allImages = new List<string>();
                foreach (var cameraId in camerasSet)
                {
                    var cameraRoot = Path.Combine(settings.PhotosRootDirectory, cameraId);
                    if (!Directory.Exists(cameraRoot)) continue;
                    var dayDirs = Directory.GetDirectories(cameraRoot);
                    var selectedDirs = FilterByDateRange(dayDirs, startDate, endDate);
                    foreach (var dir in selectedDirs)
                    {
                        var images = Directory.GetFiles(dir, "*.jpg", SearchOption.TopDirectoryOnly)
                            .OrderBy(p => p)
                            .ToArray();
                        allImages.AddRange(images);
                    }
                }

                if (allImages.Count == 0)
                {
                    return (false, null, "Seçilen kameralar için fotoğraf bulunamadı");
                }

                // Interleave strictly by timestamp parsed from path: .../{cameraId}/YYYY-MM-DD/HH-mm-ss.jpg
                DateTime? TryGetTimestamp(string path)
                {
                    try
                    {
                        var file = System.IO.Path.GetFileNameWithoutExtension(path);
                        if (!DateTime.TryParseExact(file, "HH-mm-ss", null, System.Globalization.DateTimeStyles.None, out var time)) return null;
                        var dayDir = System.IO.Path.GetDirectoryName(path);
                        var dayName = System.IO.Path.GetFileName(dayDir!);
                        if (!DateTime.TryParseExact(dayName, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var day)) return null;
                        return new DateTime(day.Year, day.Month, day.Day, time.Hour, time.Minute, time.Second, DateTimeKind.Local);
                    }
                    catch { return null; }
                }

                allImages = allImages
                    .Select(p => new { Path = p, Ts = TryGetTimestamp(p) })
                    .Where(x => x.Ts.HasValue)
                    .OrderBy(x => x.Ts!.Value)
                    .Select(x => x.Path)
                    .ToList();

                Directory.CreateDirectory(settings.OutputDirectory);
                var first = Path.GetFileName(Path.GetDirectoryName(allImages.First()) ?? string.Empty);
                var last = Path.GetFileName(Path.GetDirectoryName(allImages.Last()) ?? string.Empty);
                var cams = string.Join("_", camerasSet.Take(3));
                var outPath = Path.Combine(settings.OutputDirectory, $"timelapse_{cams}_{first}_to_{last}.mp4");

                using var writer = CreateWriterWithFallback(outPath, targetFps, outWidth, outHeight);
                if (writer == null || !writer.IsOpened())
                {
                    return (false, null, "VideoWriter açılamadı (uygun codec bulunamadı)");
                }

                for (int i = 0; i < allImages.Count; i++)
                {
                    using var img = Cv2.ImRead(allImages[i]);
                    if (img.Empty()) continue;
                    using var resized = new Mat();
                    Cv2.Resize(img, resized, new OpenCvSharp.Size(outWidth, outHeight));
                    writer.Write(resized);
                    progress?.Report((i + 1) / (double)allImages.Count);
                }

                writer.Release();
                return (true, outPath, null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        });
    }

    private static VideoWriter? CreateWriterWithFallback(string outPath, int fps, int width, int height)
    {
        // Try H264 → MP4V → XVID
        var size = new OpenCvSharp.Size(width, height);
        var writer = new VideoWriter(outPath, FourCC.H264, fps, size);
        if (writer.IsOpened()) return writer;
        writer.Dispose();

        writer = new VideoWriter(outPath, FourCC.MP4V, fps, size);
        if (writer.IsOpened()) return writer;
        writer.Dispose();

        writer = new VideoWriter(outPath, FourCC.XVID, fps, size);
        if (writer.IsOpened()) return writer;
        writer.Dispose();

        return null;
    }

    private static IEnumerable<string> FilterByDateRange(IEnumerable<string> directories, string? startDate, string? endDate)
    {
        bool HasValidDate(string dir, out DateTime dt)
        {
            dt = default;
            var name = Path.GetFileName(dir);
            return DateTime.TryParse(name, out dt);
        }

        DateTime? start = null, end = null;
        if (!string.IsNullOrWhiteSpace(startDate) && DateTime.TryParse(startDate, out var s)) start = s.Date;
        if (!string.IsNullOrWhiteSpace(endDate) && DateTime.TryParse(endDate, out var e)) end = e.Date;

        var result = new List<(DateTime day, string dir)>();
        foreach (var dir in directories)
        {
            if (!HasValidDate(dir, out var day)) continue;
            if (start.HasValue && day.Date < start.Value) continue;
            if (end.HasValue && day.Date > end.Value) continue;
            result.Add((day.Date, dir));
        }

        return result.OrderBy(r => r.day).Select(r => r.dir);
    }
}



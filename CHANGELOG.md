# Changelog

All notable changes to this project will be documented in this file.

## [1.0.0] - 2025-08-08
### Added
- Initial Windows Forms app and solution structure (UI + Core + Tests).
- NuGet packages: OpenCvSharp4 (+runtime.win), Quartz, Newtonsoft.Json, NLog, System.Drawing.Common.
- Settings JSON support with migration from legacy single‑camera fields to multi‑camera `Cameras` list.
- Core services:
  - `SettingsService` (load/save + migration)
  - `LoggingService` (NLog)
  - `CaptureService` (FFMPEG backend, TCP enforced, warm‑up frames, per‑camera capture)
  - `TimelapseService` (single and multi‑camera interleave, codec fallback)
  - `SchedulerService` (Quartz jobs, per‑camera jobs, trigger reschedule if exists)
- UI forms:
  - `Form1` with Start/Stop, Create Timelapse, FPS/Size controls, log, tray minimize, About dialog
  - `CamerasForm` (camera grid with Test and Edit actions, daily times editor)
  - `CameraEditForm` (add/edit camera, connection test, snapshot preview)
- Tests:
  - Settings load/save test
  - Camera integration tests (connection + single frame) [may be inconclusive depending on environment]

### Changed
- Minimize behavior: minimized to tray; close button exits app (user request).
- Start/Stop buttons toggle enabled state according to running scheduler.
- Time list normalization to HH:mm, de‑duplication and ordering.
- Multi‑camera timelapse ordering now uses parsed datetime (yyyy-MM-dd/HH-mm-ss) from file path, not raw path string.
- `TestConnectionAsync` uses CancellationToken and ensures proper release on timeout.
- Tests read camera credentials from environment variables (CAM_RTSP, CAM_USER, CAM_PASS).

### Fixed
- Quartz ObjectAlreadyExistsException for jobs/triggers by rescheduling when existing.
- Designer initialization issue for test capture button.
- Removed unused `Class1.cs` from Core project.

### Notes
- Multi‑camera timelapse interleaves frames by timestamp order; optional per‑camera video output can be added later.
- Ensure proper codecs/FFmpeg support on the machine for best results.



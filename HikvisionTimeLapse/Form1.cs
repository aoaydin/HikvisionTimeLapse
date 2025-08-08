using HikvisionTimeLapse.Core.Models;
using HikvisionTimeLapse.Core.Services;

namespace HikvisionTimeLapse;

public partial class Form1 : Form
{
    private readonly ISettingsService _settingsService;
    private readonly ICaptureService _captureService;
    private readonly ITimelapseService _timelapseService;
    private readonly ISchedulerService _schedulerService;
    private readonly ILoggingService _loggingService;
    private AppSettings _settings;
    private bool _isRunning;

    public Form1()
    {
        InitializeComponent();
        _settingsService = new SettingsService();
        _captureService = new CaptureService();
        _timelapseService = new TimelapseService();
        _schedulerService = new SchedulerService();
        _loggingService = new LoggingService();
        _settings = _settingsService.Load();
    }

    private void AppendLog(string message)
    {
        if (txtLog.InvokeRequired)
        {
            txtLog.Invoke(new Action(() => AppendLog(message)));
            return;
        }
        txtLog.AppendText($"{DateTime.Now:HH:mm:ss} - {message}{Environment.NewLine}");
    }

    private async void btnStart_Click(object sender, EventArgs e)
    {
        if (_isRunning) return;
        lblProgramStatus.Text = "Durum: Zamanlayıcı başlatılıyor";
        if (_settings.Cameras.Count > 0)
        {
            await _schedulerService.StartAsync(_settings, async (cameraId) =>
            {
                var cam = _settings.Cameras.FirstOrDefault(c => c.Id == cameraId);
                if (cam == null || !cam.Enabled) return;
                AppendLog($"[{cam.Name}] zamanlanmış çekim");
                var (success, path, error) = await _captureService.CaptureAndSaveSingleFrameAsync(cam, _settings);
                if (success)
                {
                    AppendLog($"[{cam.Name}] kaydedildi: {path}");
                }
                else
                {
                    AppendLog($"[{cam.Name}] hata: {error}");
                }
            });
        }
        else
        {
            await _schedulerService.StartAsync(_settings, async () =>
            {
                AppendLog("Zamanlanmış çekim başladı");
                var (success, path, error) = await _captureService.CaptureAndSaveSingleFrameAsync(_settings);
                if (success)
                {
                    AppendLog($"Fotoğraf kaydedildi: {path}");
                    lblPhotoFolder.Text = $"Fotoğraf klasörü: {_settings.PhotosRootDirectory}";
                }
                else
                {
                    AppendLog($"Hata: {error}");
                }
            });
        }
        lblSchedulerStatus.Text = "Zamanlayıcı: Çalışıyor";
        lblProgramStatus.Text = "Durum: Başlatıldı";
        _isRunning = true;
        btnStart.Enabled = false;
        btnStop.Enabled = true;
        AppendLog("Zamanlayıcı başlatıldı");
    }

    private async void btnStop_Click(object sender, EventArgs e)
    {
        lblProgramStatus.Text = "Durum: Zamanlayıcı durduruluyor";
        await _schedulerService.StopAsync();
        lblSchedulerStatus.Text = "Zamanlayıcı: Durduruldu";
        lblProgramStatus.Text = "Durum: Durduruldu";
        _isRunning = false;
        btnStart.Enabled = true;
        btnStop.Enabled = false;
        AppendLog("Zamanlayıcı durduruldu");
    }

    private async void btnTestCapture_Click(object sender, EventArgs e)
    {
        AppendLog("Manuel test çekimi başlıyor");
        lblProgramStatus.Text = "Durum: Test çekimi";
        try
        {
            var (success, path, error) = await _captureService.CaptureAndSaveSingleFrameAsync(_settings);
            if (success)
            {
                AppendLog($"Test çekimi başarılı: {path}");
                lblPhotoFolder.Text = $"Fotoğraf klasörü: {_settings.PhotosRootDirectory}";
            }
            else
            {
                AppendLog($"Test çekimi başarısız: {error}");
            }
        }
        catch (Exception ex)
        {
            AppendLog($"Test çekimi hatası: {ex.Message}");
        }
        finally
        {
            lblProgramStatus.Text = "Durum: Bekleniyor";
        }
    }

    private async void btnCreateVideo_Click(object sender, EventArgs e)
    {
        try
        {
            progressVideo.Value = 0;
            lblProgramStatus.Text = "Durum: Video oluşturuluyor";
            AppendLog("Time-lapse oluşturma başladı");
            var progress = new Progress<double>(p =>
            {
                var value = (int)Math.Round(p * 100);
                value = Math.Max(0, Math.Min(100, value));
                progressVideo.Value = value;
            });
            var fps = (int)numFps.Value;
            var width = (int)numWidth.Value;
            var height = (int)numHeight.Value;
            (bool success, string? outputPath, string? error) result;
            if (chkMultiCamInterleave.Checked && _settings.Cameras.Count > 0)
            {
                var selectedCameraIds = _settings.Cameras.Where(c => c.Enabled).Select(c => c.Id).ToArray();
                result = await _timelapseService.CreateTimelapseForCamerasAsync(_settings, selectedCameraIds, fps: fps, width: width, height: height, progress: progress);
            }
            else
            {
                result = await _timelapseService.CreateTimelapseAsync(_settings, fps: fps, width: width, height: height, progress: progress);
            }
            var (success, outputPath, error) = result;
            if (success)
            {
                AppendLog($"Video hazır: {outputPath}");
                lblProgramStatus.Text = "Durum: Video oluşturuldu";
            }
            else
            {
                AppendLog($"Video oluşturma hatası: {error}");
                lblProgramStatus.Text = "Durum: Hata";
            }
        }
        catch (Exception ex)
        {
            AppendLog($"Video oluşturma hatası: {ex.Message}");
            lblProgramStatus.Text = "Durum: Hata";
        }
        finally
        {
            progressVideo.Value = 0;
        }
    }

    private void btnSettings_Click(object sender, EventArgs e)
    {
        using var sf = new SettingsForm(_settings, _captureService, _settingsService);
        if (sf.ShowDialog(this) == DialogResult.OK)
        {
            _settings = _settingsService.Load();
            AppendLog("Ayarlar güncellendi");
        }
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        lblPhotoFolder.Text = $"Fotoğraf klasörü: {_settings.PhotosRootDirectory}";
        btnStop.Enabled = false;
        trayIcon.Icon = SystemIcons.Application;
        trayIcon.Visible = false;
    }

    private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        // Uygulamayı gerçekten kapat (yalnızca minimize tepsiye gider)
        trayIcon.Visible = false;
        await _schedulerService.StopAsync();
    }

    private void trayIcon_DoubleClick(object sender, EventArgs e)
    {
        Show();
        WindowState = FormWindowState.Normal;
        trayIcon.Visible = false;
    }

    private void Form1_Resize(object? sender, EventArgs e)
    {
        if (WindowState == FormWindowState.Minimized)
        {
            Hide();
            trayIcon.Visible = true;
            trayIcon.BalloonTipTitle = "Hikvision Time-Lapse";
            trayIcon.BalloonTipText = "Uygulama sistem tepsisine küçültüldü.";
            trayIcon.ShowBalloonTip(1500);
        }
    }

    private void btnCameras_Click(object sender, EventArgs e)
    {
        using var f = new CamerasForm(_settings, _settingsService, _captureService);
        if (f.ShowDialog(this) == DialogResult.OK)
        {
            _settings = _settingsService.Load();
            AppendLog("Kamera ve zaman ayarları güncellendi");
        }
    }

    private void btnAbout_Click(object? sender, EventArgs e)
    {
        var version = typeof(Form1).Assembly.GetName().Version?.ToString() ?? "1.0.0.0";
        var info =
            "Uygulama: Hikvision Time-Lapse\n" +
            "Geliştirici: aoaydin\n" +
            $"Sürüm: {version}\n" +
            "Açıklama: RTSP kameralarından zamanlanmış fotoğraf çekip time-lapse video üretir.";
        MessageBox.Show(this, info, "Hakkında", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}

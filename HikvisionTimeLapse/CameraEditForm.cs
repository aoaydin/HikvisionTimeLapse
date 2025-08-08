using HikvisionTimeLapse.Core.Models;
using HikvisionTimeLapse.Core.Services;
using OpenCvSharp;

namespace HikvisionTimeLapse;

public partial class CameraEditForm : Form
{
    private readonly ICaptureService _captureService;
    public CameraConfig Camera { get; private set; }

    public CameraEditForm(CameraConfig camera, ICaptureService captureService)
    {
        InitializeComponent();
        _captureService = captureService;
        Camera = camera;

        txtName.Text = Camera.Name;
        txtRtsp.Text = Camera.RtspUrl;
        txtUser.Text = Camera.Username;
        txtPass.Text = Camera.Password;
        chkEnabled.Checked = Camera.Enabled;
    }

    private async void btnTestConn_Click(object? sender, EventArgs e)
    {
        var temp = new CameraConfig
        {
            RtspUrl = txtRtsp.Text.Trim(),
            Username = txtUser.Text.Trim(),
            Password = txtPass.Text.Trim()
        };
        var ok = await _captureService.TestConnectionAsync(temp.BuildEffectiveRtsp(), 10000);
        MessageBox.Show(this, ok ? "Bağlantı başarılı" : "Bağlantı başarısız", "Test", MessageBoxButtons.OK,
            ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
    }

    private async void btnPreview_Click(object? sender, EventArgs e)
    {
        // Basit bir kare yakalayıp geçici klasöre yaz ve görüntüleyiciyle aç
        var tempCamera = new CameraConfig
        {
            RtspUrl = txtRtsp.Text.Trim(),
            Username = txtUser.Text.Trim(),
            Password = txtPass.Text.Trim(),
            Enabled = true
        };
        var appSettings = new AppSettings { PhotosRootDirectory = Path.Combine(Path.GetTempPath(), "tl_preview") };
        var (success, path, error) = await _captureService.CaptureAndSaveSingleFrameAsync(tempCamera, appSettings);
        if (success && File.Exists(path))
        {
            try { System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo { FileName = path, UseShellExecute = true }); }
            catch { /* ignore */ }
        }
        else
        {
            MessageBox.Show(this, "Önizleme alınamadı: " + error, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnOk_Click(object? sender, EventArgs e)
    {
        Camera.Name = txtName.Text.Trim();
        Camera.RtspUrl = txtRtsp.Text.Trim();
        Camera.Username = txtUser.Text.Trim();
        Camera.Password = txtPass.Text.Trim();
        Camera.Enabled = chkEnabled.Checked;
        DialogResult = DialogResult.OK;
    }

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
    }
}




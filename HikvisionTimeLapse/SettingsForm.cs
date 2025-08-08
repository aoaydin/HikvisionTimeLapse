using HikvisionTimeLapse.Core.Models;
using HikvisionTimeLapse.Core.Services;

namespace HikvisionTimeLapse;

public partial class SettingsForm : Form
{
    private readonly ISettingsService _settingsService;
    private readonly ICaptureService _captureService;
    private AppSettings _settings;

    public SettingsForm(AppSettings current, ICaptureService captureService, ISettingsService settingsService)
    {
        InitializeComponent();
        _settings = current;
        _captureService = captureService;
        _settingsService = settingsService;

        txtRtsp.Text = _settings.RtspUrl;
        txtUsername.Text = _settings.Username;
        txtPassword.Text = _settings.Password;
    }

    private async void btnTest_Click(object sender, EventArgs e)
    {
        // Formdaki değerlerle geçici ayar oluşturup, kimlik bilgilerini ve TCP zorlamasını URL'ye uygula
        var temp = new AppSettings
        {
            RtspUrl = txtRtsp.Text.Trim(),
            Username = txtUsername.Text.Trim(),
            Password = txtPassword.Text.Trim(),
            CaptureOpenTimeoutMs = _settings.CaptureOpenTimeoutMs
        };
        var effective = temp.BuildEffectiveRtsp();
        var ok = await _captureService.TestConnectionAsync(effective, _settings.CaptureOpenTimeoutMs);
        MessageBox.Show(this, ok ? "Bağlantı başarılı" : "Bağlantı başarısız", "Test", MessageBoxButtons.OK,
            ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        _settings.RtspUrl = txtRtsp.Text.Trim();
        _settings.Username = txtUsername.Text.Trim();
        _settings.Password = txtPassword.Text.Trim();
        _settingsService.Save(_settings);
        DialogResult = DialogResult.OK;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
    }
}



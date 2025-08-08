using System.Data;
using HikvisionTimeLapse.Core.Models;
using HikvisionTimeLapse.Core.Services;

namespace HikvisionTimeLapse;

public partial class CamerasForm : Form
{
    private readonly ISettingsService _settingsService;
    private readonly ICaptureService _captureService;
    private AppSettings _settings;

    public CamerasForm(AppSettings settings, ISettingsService settingsService, ICaptureService captureService)
    {
        InitializeComponent();
        _settings = settings;
        _settingsService = settingsService;
        _captureService = captureService;
    }

    private void CamerasForm_Load(object sender, EventArgs e)
    {
        BindGrid();
        BindTimes();
    }

    private void BindGrid()
    {
        gridCameras.Columns.Clear();
        gridCameras.AutoGenerateColumns = false;

        gridCameras.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ad", DataPropertyName = "Name" });
        gridCameras.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "RTSP", DataPropertyName = "RtspUrl" });
        gridCameras.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Kullanıcı", DataPropertyName = "Username" });
        gridCameras.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Şifre", DataPropertyName = "Password" });
        gridCameras.Columns.Add(new DataGridViewCheckBoxColumn { HeaderText = "Aktif", DataPropertyName = "Enabled" });

        var testButton = new DataGridViewButtonColumn { HeaderText = "Test", Text = "Test Et", UseColumnTextForButtonValue = true };
        gridCameras.Columns.Add(testButton);
        var editButton = new DataGridViewButtonColumn { HeaderText = "Düzenle", Text = "Düzenle", UseColumnTextForButtonValue = true };
        gridCameras.Columns.Add(editButton);

        gridCameras.DataSource = new BindingSource { DataSource = _settings.Cameras };
    }

    private void BindTimes()
    {
        listTimes.Items.Clear();
        // Normalize to HH:mm, de-duplicate, and sort
        var normalized = _settings.DailyCaptureTimes
            .Where(t => !string.IsNullOrWhiteSpace(t) && TimeSpan.TryParse(t, out _))
            .Select(t => TimeSpan.Parse(t).ToString(@"hh\:mm"))
            .Distinct()
            .OrderBy(t => t)
            .ToList();
        _settings.DailyCaptureTimes = normalized;
        foreach (var t in normalized)
            listTimes.Items.Add(t);
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        var newCam = new CameraConfig { Name = $"Kamera {_settings.Cameras.Count + 1}" };
        using var editor = new CameraEditForm(newCam, _captureService);
        if (editor.ShowDialog(this) == DialogResult.OK)
        {
            _settings.Cameras.Add(newCam);
            BindGrid();
        }
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
        if (gridCameras.CurrentRow?.DataBoundItem is CameraConfig cam)
        {
            _settings.Cameras.Remove(cam);
            BindGrid();
        }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        // Commit edited values
        gridCameras.EndEdit();
        var times = new List<string>();
        foreach (var item in listTimes.Items)
        {
            if (TimeSpan.TryParse(item?.ToString(), out var ts))
            {
                times.Add(ts.ToString(@"hh\:mm"));
            }
        }
        _settings.DailyCaptureTimes = times.Distinct().OrderBy(t => t).ToList();
        _settingsService.Save(_settings);
        DialogResult = DialogResult.OK;
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
    }

    private async void gridCameras_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (gridCameras.Rows[e.RowIndex].DataBoundItem is not CameraConfig cam) return;
        if (gridCameras.Columns[e.ColumnIndex] is not DataGridViewButtonColumn btnCol) return;
        if (btnCol.HeaderText == "Test")
        {
            var ok = await _captureService.TestConnectionAsync(cam.BuildEffectiveRtsp(), 8000);
            MessageBox.Show(this, ok ? "Bağlantı başarılı" : "Bağlantı başarısız", "Test", MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }
        else if (btnCol.HeaderText == "Düzenle")
        {
            var clone = new CameraConfig
            {
                Id = cam.Id,
                Name = cam.Name,
                RtspUrl = cam.RtspUrl,
                Username = cam.Username,
                Password = cam.Password,
                Enabled = cam.Enabled
            };
            using var editor = new CameraEditForm(clone, _captureService);
            if (editor.ShowDialog(this) == DialogResult.OK)
            {
                cam.Name = clone.Name;
                cam.RtspUrl = clone.RtspUrl;
                cam.Username = clone.Username;
                cam.Password = clone.Password;
                cam.Enabled = clone.Enabled;
                BindGrid();
            }
        }
    }

    private void btnAddTime_Click(object sender, EventArgs e)
    {
        var input = Microsoft.VisualBasic.Interaction.InputBox("Saat (HH:mm)", "Saat Ekle", "10:00");
        if (!string.IsNullOrWhiteSpace(input) && TimeSpan.TryParse(input, out var ts))
        {
            var norm = ts.ToString(@"hh\:mm");
            // prevent duplicates
            var exists = listTimes.Items.Cast<object>().Any(i => string.Equals(i?.ToString(), norm, StringComparison.OrdinalIgnoreCase));
            if (!exists)
            {
                listTimes.Items.Add(norm);
            }
            else
            {
                MessageBox.Show(this, "Bu saat zaten listede", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        else
        {
            MessageBox.Show(this, "Saat formatı geçersiz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void btnRemoveTime_Click(object sender, EventArgs e)
    {
        if (listTimes.SelectedItem != null)
        {
            listTimes.Items.Remove(listTimes.SelectedItem);
        }
    }
}



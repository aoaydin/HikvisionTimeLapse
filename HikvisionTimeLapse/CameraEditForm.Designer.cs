namespace HikvisionTimeLapse;

partial class CameraEditForm
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.TextBox txtName;
    private System.Windows.Forms.TextBox txtRtsp;
    private System.Windows.Forms.TextBox txtUser;
    private System.Windows.Forms.TextBox txtPass;
    private System.Windows.Forms.CheckBox chkEnabled;
    private System.Windows.Forms.Button btnTestConn;
    private System.Windows.Forms.Button btnPreview;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Label lblName;
    private System.Windows.Forms.Label lblRtsp;
    private System.Windows.Forms.Label lblUser;
    private System.Windows.Forms.Label lblPass;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        txtName = new System.Windows.Forms.TextBox();
        txtRtsp = new System.Windows.Forms.TextBox();
        txtUser = new System.Windows.Forms.TextBox();
        txtPass = new System.Windows.Forms.TextBox();
        chkEnabled = new System.Windows.Forms.CheckBox();
        btnTestConn = new System.Windows.Forms.Button();
        btnPreview = new System.Windows.Forms.Button();
        btnOk = new System.Windows.Forms.Button();
        btnCancel = new System.Windows.Forms.Button();
        lblName = new System.Windows.Forms.Label();
        lblRtsp = new System.Windows.Forms.Label();
        lblUser = new System.Windows.Forms.Label();
        lblPass = new System.Windows.Forms.Label();
        SuspendLayout();

        lblName.AutoSize = true; lblName.Text = "Ad"; lblName.Location = new System.Drawing.Point(16, 18);
        txtName.Location = new System.Drawing.Point(120, 15); txtName.Size = new System.Drawing.Size(300, 23);

        lblRtsp.AutoSize = true; lblRtsp.Text = "RTSP"; lblRtsp.Location = new System.Drawing.Point(16, 50);
        txtRtsp.Location = new System.Drawing.Point(120, 47); txtRtsp.Size = new System.Drawing.Size(300, 23);

        lblUser.AutoSize = true; lblUser.Text = "Kullanıcı"; lblUser.Location = new System.Drawing.Point(16, 82);
        txtUser.Location = new System.Drawing.Point(120, 79); txtUser.Size = new System.Drawing.Size(180, 23);

        lblPass.AutoSize = true; lblPass.Text = "Şifre"; lblPass.Location = new System.Drawing.Point(16, 114);
        txtPass.Location = new System.Drawing.Point(120, 111); txtPass.Size = new System.Drawing.Size(180, 23); txtPass.PasswordChar = '*';

        chkEnabled.Text = "Aktif"; chkEnabled.Location = new System.Drawing.Point(120, 144); chkEnabled.AutoSize = true;

        btnTestConn.Text = "Bağlantıyı Test Et"; btnTestConn.Location = new System.Drawing.Point(16, 180); btnTestConn.Size = new System.Drawing.Size(140, 30);
        btnTestConn.Click += btnTestConn_Click;

        btnPreview.Text = "Görüntü Önizleme"; btnPreview.Location = new System.Drawing.Point(162, 180); btnPreview.Size = new System.Drawing.Size(140, 30);
        btnPreview.Click += btnPreview_Click;

        btnOk.Text = "Kaydet"; btnOk.Location = new System.Drawing.Point(260, 220); btnOk.Size = new System.Drawing.Size(80, 30);
        btnOk.Click += btnOk_Click;

        btnCancel.Text = "İptal"; btnCancel.Location = new System.Drawing.Point(340, 220); btnCancel.Size = new System.Drawing.Size(80, 30);
        btnCancel.Click += btnCancel_Click;

        ClientSize = new System.Drawing.Size(420, 260);
        Controls.AddRange(new System.Windows.Forms.Control[] { txtName, txtRtsp, txtUser, txtPass, chkEnabled, btnTestConn, btnPreview, btnOk, btnCancel, lblName, lblRtsp, lblUser, lblPass });
        Name = "CameraEditForm";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        Text = "Kamera Düzenle";
        ResumeLayout(false);
        PerformLayout();
    }
}




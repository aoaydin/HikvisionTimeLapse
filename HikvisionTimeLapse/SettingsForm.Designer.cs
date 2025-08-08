namespace HikvisionTimeLapse;

partial class SettingsForm
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.TextBox txtRtsp;
    private System.Windows.Forms.TextBox txtUsername;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Button btnTest;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Label lblRtsp;
    private System.Windows.Forms.Label lblUser;
    private System.Windows.Forms.Label lblPass;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.txtRtsp = new System.Windows.Forms.TextBox();
        this.txtUsername = new System.Windows.Forms.TextBox();
        this.txtPassword = new System.Windows.Forms.TextBox();
        this.btnTest = new System.Windows.Forms.Button();
        this.btnSave = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.lblRtsp = new System.Windows.Forms.Label();
        this.lblUser = new System.Windows.Forms.Label();
        this.lblPass = new System.Windows.Forms.Label();
        this.SuspendLayout();

        this.lblRtsp.AutoSize = true;
        this.lblRtsp.Location = new System.Drawing.Point(20, 20);
        this.lblRtsp.Text = "RTSP URL";
        this.txtRtsp.Location = new System.Drawing.Point(120, 18);
        this.txtRtsp.Size = new System.Drawing.Size(340, 23);

        this.lblUser.AutoSize = true;
        this.lblUser.Location = new System.Drawing.Point(20, 55);
        this.lblUser.Text = "Kullanıcı Adı";
        this.txtUsername.Location = new System.Drawing.Point(120, 53);
        this.txtUsername.Size = new System.Drawing.Size(200, 23);

        this.lblPass.AutoSize = true;
        this.lblPass.Location = new System.Drawing.Point(20, 90);
        this.lblPass.Text = "Şifre";
        this.txtPassword.Location = new System.Drawing.Point(120, 88);
        this.txtPassword.Size = new System.Drawing.Size(200, 23);
        this.txtPassword.PasswordChar = '*';

        this.btnTest.Location = new System.Drawing.Point(120, 130);
        this.btnTest.Size = new System.Drawing.Size(100, 30);
        this.btnTest.Text = "Test Et";
        this.btnTest.Click += new System.EventHandler(this.btnTest_Click);

        this.btnSave.Location = new System.Drawing.Point(240, 130);
        this.btnSave.Size = new System.Drawing.Size(100, 30);
        this.btnSave.Text = "Kaydet";
        this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

        this.btnCancel.Location = new System.Drawing.Point(360, 130);
        this.btnCancel.Size = new System.Drawing.Size(100, 30);
        this.btnCancel.Text = "İptal";
        this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

        this.ClientSize = new System.Drawing.Size(480, 180);
        this.Controls.Add(this.lblRtsp);
        this.Controls.Add(this.txtRtsp);
        this.Controls.Add(this.lblUser);
        this.Controls.Add(this.txtUsername);
        this.Controls.Add(this.lblPass);
        this.Controls.Add(this.txtPassword);
        this.Controls.Add(this.btnTest);
        this.Controls.Add(this.btnSave);
        this.Controls.Add(this.btnCancel);
        this.Name = "SettingsForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Kamera Ayarları";
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}




namespace HikvisionTimeLapse;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    private System.Windows.Forms.Button btnStart;
    private System.Windows.Forms.Button btnStop;
    private System.Windows.Forms.Button btnCreateVideo;
    private System.Windows.Forms.Label lblSchedulerStatus;
    private System.Windows.Forms.Label lblProgramStatus;
    private System.Windows.Forms.TextBox txtLog;
    private System.Windows.Forms.ProgressBar progressVideo;
    private System.Windows.Forms.Label lblPhotoFolder;
    private System.Windows.Forms.NumericUpDown numFps;
    private System.Windows.Forms.NumericUpDown numWidth;
    private System.Windows.Forms.NumericUpDown numHeight;
    private System.Windows.Forms.Label lblFps;
    private System.Windows.Forms.Label lblWidth;
    private System.Windows.Forms.Label lblHeight;
    private System.Windows.Forms.Button btnCameras;
    private System.Windows.Forms.CheckBox chkMultiCamInterleave;
    private System.Windows.Forms.NotifyIcon trayIcon;
    private System.Windows.Forms.Button btnAbout;

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
        components = new System.ComponentModel.Container();
        btnStart = new Button();
        btnStop = new Button();
        btnCreateVideo = new Button();
        lblSchedulerStatus = new Label();
        lblProgramStatus = new Label();
        txtLog = new TextBox();
        progressVideo = new ProgressBar();
        lblPhotoFolder = new Label();
        numFps = new NumericUpDown();
        numWidth = new NumericUpDown();
        numHeight = new NumericUpDown();
        lblFps = new Label();
        lblWidth = new Label();
        lblHeight = new Label();
        btnCameras = new Button();
        chkMultiCamInterleave = new CheckBox();
        trayIcon = new NotifyIcon(components);
        btnAbout = new Button();
        ((System.ComponentModel.ISupportInitialize)numFps).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numWidth).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numHeight).BeginInit();
        SuspendLayout();
        // 
        // btnStart
        // 
        btnStart.Location = new Point(20, 20);
        btnStart.Name = "btnStart";
        btnStart.Size = new Size(100, 30);
        btnStart.TabIndex = 0;
        btnStart.Text = "Başlat";
        btnStart.UseVisualStyleBackColor = true;
        btnStart.Click += btnStart_Click;
        // 
        // btnStop
        // 
        btnStop.Location = new Point(130, 20);
        btnStop.Name = "btnStop";
        btnStop.Size = new Size(100, 30);
        btnStop.TabIndex = 1;
        btnStop.Text = "Durdur";
        btnStop.UseVisualStyleBackColor = true;
        btnStop.Click += btnStop_Click;
        // 
        // btnCreateVideo
        // 
        btnCreateVideo.Location = new Point(360, 18);
        btnCreateVideo.Name = "btnCreateVideo";
        btnCreateVideo.Size = new Size(160, 30);
        btnCreateVideo.TabIndex = 3;
        btnCreateVideo.Text = "Time-lapse Oluştur";
        btnCreateVideo.UseVisualStyleBackColor = true;
        btnCreateVideo.Click += btnCreateVideo_Click;
        // 
        // lblSchedulerStatus
        // 
        lblSchedulerStatus.AutoSize = true;
        lblSchedulerStatus.Location = new Point(20, 65);
        lblSchedulerStatus.Name = "lblSchedulerStatus";
        lblSchedulerStatus.Size = new Size(171, 20);
        lblSchedulerStatus.TabIndex = 4;
        lblSchedulerStatus.Text = "Zamanlayıcı: Durduruldu";
        // 
        // lblProgramStatus
        // 
        lblProgramStatus.AutoSize = true;
        lblProgramStatus.Location = new Point(20, 85);
        lblProgramStatus.Name = "lblProgramStatus";
        lblProgramStatus.Size = new Size(130, 20);
        lblProgramStatus.TabIndex = 5;
        lblProgramStatus.Text = "Durum: Bekleniyor";
        // 
        // txtLog
        // 
        txtLog.Location = new Point(20, 130);
        txtLog.Multiline = true;
        txtLog.Name = "txtLog";
        txtLog.ReadOnly = true;
        txtLog.ScrollBars = ScrollBars.Vertical;
        txtLog.Size = new Size(740, 250);
        txtLog.TabIndex = 7;
        // 
        // progressVideo
        // 
        progressVideo.Location = new Point(20, 390);
        progressVideo.Name = "progressVideo";
        progressVideo.Size = new Size(740, 20);
        progressVideo.TabIndex = 8;
        // 
        // lblPhotoFolder
        // 
        lblPhotoFolder.AutoSize = true;
        lblPhotoFolder.Location = new Point(20, 105);
        lblPhotoFolder.Name = "lblPhotoFolder";
        lblPhotoFolder.Size = new Size(130, 20);
        lblPhotoFolder.TabIndex = 6;
        lblPhotoFolder.Text = "Fotoğraf klasörü: -";
        // 
        // numFps
        // 
        numFps.Location = new Point(585, 21);
        numFps.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
        numFps.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numFps.Name = "numFps";
        numFps.Size = new Size(60, 27);
        numFps.TabIndex = 15;
        numFps.Value = new decimal(new int[] { 25, 0, 0, 0 });
        // 
        // numWidth
        // 
        numWidth.Location = new Point(670, 21);
        numWidth.Maximum = new decimal(new int[] { 7680, 0, 0, 0 });
        numWidth.Minimum = new decimal(new int[] { 160, 0, 0, 0 });
        numWidth.Name = "numWidth";
        numWidth.Size = new Size(70, 27);
        numWidth.TabIndex = 13;
        numWidth.Value = new decimal(new int[] { 1920, 0, 0, 0 });
        // 
        // numHeight
        // 
        numHeight.Location = new Point(670, 51);
        numHeight.Maximum = new decimal(new int[] { 4320, 0, 0, 0 });
        numHeight.Minimum = new decimal(new int[] { 120, 0, 0, 0 });
        numHeight.Name = "numHeight";
        numHeight.Size = new Size(70, 27);
        numHeight.TabIndex = 11;
        numHeight.Value = new decimal(new int[] { 1080, 0, 0, 0 });
        // 
        // lblFps
        // 
        lblFps.AutoSize = true;
        lblFps.Location = new Point(550, 25);
        lblFps.Name = "lblFps";
        lblFps.Size = new Size(32, 20);
        lblFps.TabIndex = 14;
        lblFps.Text = "FPS";
        // 
        // lblWidth
        // 
        lblWidth.AutoSize = true;
        lblWidth.Location = new Point(650, 25);
        lblWidth.Name = "lblWidth";
        lblWidth.Size = new Size(23, 20);
        lblWidth.TabIndex = 12;
        lblWidth.Text = "W";
        // 
        // lblHeight
        // 
        lblHeight.AutoSize = true;
        lblHeight.Location = new Point(650, 55);
        lblHeight.Name = "lblHeight";
        lblHeight.Size = new Size(20, 20);
        lblHeight.TabIndex = 10;
        lblHeight.Text = "H";
        // 
        // btnCameras
        // 
        btnCameras.Location = new Point(237, 19);
        btnCameras.Name = "btnCameras";
        btnCameras.Size = new Size(114, 30);
        btnCameras.TabIndex = 16;
        btnCameras.Text = "Kameralar...";
        btnCameras.UseVisualStyleBackColor = true;
        btnCameras.Click += btnCameras_Click;
        // 
        // chkMultiCamInterleave
        // 
        chkMultiCamInterleave.AutoSize = true;
        chkMultiCamInterleave.Location = new Point(356, 56);
        chkMultiCamInterleave.Name = "chkMultiCamInterleave";
        chkMultiCamInterleave.Size = new Size(226, 24);
        chkMultiCamInterleave.TabIndex = 17;
        chkMultiCamInterleave.Text = "Çoklu kamera (sırala/birleştir)";
        chkMultiCamInterleave.UseVisualStyleBackColor = true;
        // 
        // trayIcon
        // 
        trayIcon.Text = "Hikvision Time-Lapse";
        trayIcon.DoubleClick += trayIcon_DoubleClick;
        // 
        // btnAbout
        // 
        btnAbout.Location = new Point(653, 85);
        btnAbout.Name = "btnAbout";
        btnAbout.Size = new Size(87, 30);
        btnAbout.TabIndex = 18;
        btnAbout.Text = "Hakkında";
        btnAbout.UseVisualStyleBackColor = true;
        btnAbout.Click += btnAbout_Click;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(784, 431);
        Controls.Add(btnAbout);
        Controls.Add(btnCameras);
        Controls.Add(chkMultiCamInterleave);
        Controls.Add(lblHeight);
        Controls.Add(numHeight);
        Controls.Add(lblWidth);
        Controls.Add(numWidth);
        Controls.Add(lblFps);
        Controls.Add(numFps);
        Controls.Add(progressVideo);
        Controls.Add(txtLog);
        Controls.Add(lblPhotoFolder);
        Controls.Add(lblProgramStatus);
        Controls.Add(lblSchedulerStatus);
        Controls.Add(btnCreateVideo);
        Controls.Add(btnStop);
        Controls.Add(btnStart);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Hikvision Time-Lapse";
        FormClosing += Form1_FormClosing;
        Load += Form1_Load;
        Resize += Form1_Resize;
        ((System.ComponentModel.ISupportInitialize)numFps).EndInit();
        ((System.ComponentModel.ISupportInitialize)numWidth).EndInit();
        ((System.ComponentModel.ISupportInitialize)numHeight).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}

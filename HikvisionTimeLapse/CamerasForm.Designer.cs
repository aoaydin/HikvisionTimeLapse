namespace HikvisionTimeLapse;

partial class CamerasForm
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.DataGridView gridCameras;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.Button btnRemove;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.ListBox listTimes;
    private System.Windows.Forms.Button btnAddTime;
    private System.Windows.Forms.Button btnRemoveTime;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        gridCameras = new System.Windows.Forms.DataGridView();
        btnAdd = new System.Windows.Forms.Button();
        btnRemove = new System.Windows.Forms.Button();
        btnSave = new System.Windows.Forms.Button();
        btnClose = new System.Windows.Forms.Button();
        listTimes = new System.Windows.Forms.ListBox();
        btnAddTime = new System.Windows.Forms.Button();
        btnRemoveTime = new System.Windows.Forms.Button();
        ((System.ComponentModel.ISupportInitialize)gridCameras).BeginInit();
        SuspendLayout();

        gridCameras.AllowUserToAddRows = false;
        gridCameras.AllowUserToDeleteRows = false;
        gridCameras.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        gridCameras.Location = new System.Drawing.Point(12, 12);
        gridCameras.MultiSelect = false;
        gridCameras.Name = "gridCameras";
        gridCameras.RowHeadersVisible = false;
        gridCameras.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        gridCameras.Size = new System.Drawing.Size(620, 260);
        gridCameras.TabIndex = 0;
        gridCameras.CellContentClick += gridCameras_CellContentClick;

        btnAdd.Location = new System.Drawing.Point(12, 280);
        btnAdd.Size = new System.Drawing.Size(80, 30);
        btnAdd.Text = "Ekle";
        btnAdd.Click += btnAdd_Click;

        btnRemove.Location = new System.Drawing.Point(98, 280);
        btnRemove.Size = new System.Drawing.Size(80, 30);
        btnRemove.Text = "Sil";
        btnRemove.Click += btnRemove_Click;

        btnSave.Location = new System.Drawing.Point(452, 280);
        btnSave.Size = new System.Drawing.Size(90, 30);
        btnSave.Text = "Kaydet";
        btnSave.Click += btnSave_Click;

        btnClose.Location = new System.Drawing.Point(542, 280);
        btnClose.Size = new System.Drawing.Size(90, 30);
        btnClose.Text = "Kapat";
        btnClose.Click += btnClose_Click;

        listTimes.Location = new System.Drawing.Point(646, 12);
        listTimes.Size = new System.Drawing.Size(142, 224);

        btnAddTime.Location = new System.Drawing.Point(646, 244);
        btnAddTime.Size = new System.Drawing.Size(70, 30);
        btnAddTime.Text = "+ Saat";
        btnAddTime.Click += btnAddTime_Click;

        btnRemoveTime.Location = new System.Drawing.Point(718, 244);
        btnRemoveTime.Size = new System.Drawing.Size(70, 30);
        btnRemoveTime.Text = "- Saat";
        btnRemoveTime.Click += btnRemoveTime_Click;

        ClientSize = new System.Drawing.Size(800, 322);
        Controls.Add(btnRemoveTime);
        Controls.Add(btnAddTime);
        Controls.Add(listTimes);
        Controls.Add(btnClose);
        Controls.Add(btnSave);
        Controls.Add(btnRemove);
        Controls.Add(btnAdd);
        Controls.Add(gridCameras);
        Name = "CamerasForm";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        Text = "Kameralar ve Zamanlar";
        Load += CamerasForm_Load;
        ((System.ComponentModel.ISupportInitialize)gridCameras).EndInit();
        ResumeLayout(false);
    }
}




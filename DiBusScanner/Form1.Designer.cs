namespace DiBusScanner
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lvDevices = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDiBusAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderRadioChannel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderComment = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnSetDeviceRadioChannel = new System.Windows.Forms.Button();
            this.tbxDeviceNewRadioChannel = new System.Windows.Forms.TextBox();
            this.gbxSettings = new System.Windows.Forms.GroupBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxScanTo = new System.Windows.Forms.TextBox();
            this.lblRadioChannelsFrom = new System.Windows.Forms.Label();
            this.tbxScanFrom = new System.Windows.Forms.TextBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmItemSerialPortSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmItemLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmItemRus = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmItemEng = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnStopGetData = new System.Windows.Forms.Button();
            this.lblDevDevInfo = new System.Windows.Forms.Label();
            this.lblDevRadioChannel = new System.Windows.Forms.Label();
            this.lblDevDiBusAddress = new System.Windows.Forms.Label();
            this.btnGetDataFromDevice = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.lblLabelDevInfo = new System.Windows.Forms.Label();
            this.lblLabelRadioChannel = new System.Windows.Forms.Label();
            this.lblLabelDiBusAddress = new System.Windows.Forms.Label();
            this.lblDevice = new System.Windows.Forms.Label();
            this.pbxDevIcon = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelCounter = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabelInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.imageListLarge = new System.Windows.Forms.ImageList(this.components);
            this.gbxSettings.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxDevIcon)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvDevices
            // 
            this.lvDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvDevices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeaderDiBusAddress,
            this.columnHeaderRadioChannel,
            this.columnHeaderType,
            this.columnHeaderComment});
            this.lvDevices.FullRowSelect = true;
            this.lvDevices.HideSelection = false;
            this.lvDevices.LargeImageList = this.imageList1;
            this.lvDevices.Location = new System.Drawing.Point(0, 0);
            this.lvDevices.Name = "lvDevices";
            this.lvDevices.Size = new System.Drawing.Size(406, 473);
            this.lvDevices.SmallImageList = this.imageList1;
            this.lvDevices.TabIndex = 0;
            this.lvDevices.UseCompatibleStateImageBehavior = false;
            this.lvDevices.View = System.Windows.Forms.View.Details;
            this.lvDevices.SelectedIndexChanged += new System.EventHandler(this.lvDevices_SelectedIndexChanged);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "";
            // 
            // columnHeaderDiBusAddress
            // 
            this.columnHeaderDiBusAddress.Text = "DiBus адрес";
            this.columnHeaderDiBusAddress.Width = 80;
            // 
            // columnHeaderRadioChannel
            // 
            this.columnHeaderRadioChannel.Text = "Радиоканал";
            this.columnHeaderRadioChannel.Width = 80;
            // 
            // columnHeaderType
            // 
            this.columnHeaderType.Text = "Тип";
            this.columnHeaderType.Width = 75;
            // 
            // columnHeaderComment
            // 
            this.columnHeaderComment.Text = "";
            this.columnHeaderComment.Width = 100;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "MBS2_64.png");
            this.imageList1.Images.SetKeyName(1, "gamma_32.png");
            this.imageList1.Images.SetKeyName(2, "pult.png");
            this.imageList1.Images.SetKeyName(3, "MBS3.png");
            this.imageList1.Images.SetKeyName(4, "beta_35.png");
            this.imageList1.Images.SetKeyName(5, "alpha_32.png");
            this.imageList1.Images.SetKeyName(6, "newtron_32.png");
            this.imageList1.Images.SetKeyName(7, "Undefined_device_32.png");
            this.imageList1.Images.SetKeyName(8, "BDBeta32.png");
            this.imageList1.Images.SetKeyName(9, "БД_64.png");
            // 
            // btnSetDeviceRadioChannel
            // 
            this.btnSetDeviceRadioChannel.Enabled = false;
            this.btnSetDeviceRadioChannel.Location = new System.Drawing.Point(18, 112);
            this.btnSetDeviceRadioChannel.Name = "btnSetDeviceRadioChannel";
            this.btnSetDeviceRadioChannel.Size = new System.Drawing.Size(139, 25);
            this.btnSetDeviceRadioChannel.TabIndex = 11;
            this.btnSetDeviceRadioChannel.Text = "Set RadioChannel (0-255)";
            this.btnSetDeviceRadioChannel.UseVisualStyleBackColor = true;
            this.btnSetDeviceRadioChannel.Click += new System.EventHandler(this.btnSetDeviceRadioChannel_Click);
            // 
            // tbxDeviceNewRadioChannel
            // 
            this.tbxDeviceNewRadioChannel.Enabled = false;
            this.tbxDeviceNewRadioChannel.Location = new System.Drawing.Point(168, 115);
            this.tbxDeviceNewRadioChannel.Name = "tbxDeviceNewRadioChannel";
            this.tbxDeviceNewRadioChannel.Size = new System.Drawing.Size(45, 20);
            this.tbxDeviceNewRadioChannel.TabIndex = 11;
            this.tbxDeviceNewRadioChannel.Text = "0";
            this.tbxDeviceNewRadioChannel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbxDeviceNewRadioChannel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxScanFrom_KeyPress);
            // 
            // gbxSettings
            // 
            this.gbxSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gbxSettings.Controls.Add(this.btnFind);
            this.gbxSettings.Controls.Add(this.btnStop);
            this.gbxSettings.Controls.Add(this.label3);
            this.gbxSettings.Controls.Add(this.tbxScanTo);
            this.gbxSettings.Controls.Add(this.lblRadioChannelsFrom);
            this.gbxSettings.Controls.Add(this.tbxScanFrom);
            this.gbxSettings.Location = new System.Drawing.Point(7, 500);
            this.gbxSettings.Name = "gbxSettings";
            this.gbxSettings.Size = new System.Drawing.Size(309, 49);
            this.gbxSettings.TabIndex = 5;
            this.gbxSettings.TabStop = false;
            // 
            // btnFind
            // 
            this.btnFind.Image = ((System.Drawing.Image)(resources.GetObject("btnFind.Image")));
            this.btnFind.Location = new System.Drawing.Point(2, 8);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(38, 38);
            this.btnFind.TabIndex = 1;
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // btnStop
            // 
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.Location = new System.Drawing.Point(2, 8);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(38, 38);
            this.btnStop.TabIndex = 6;
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(242, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(10, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "-";
            // 
            // tbxScanTo
            // 
            this.tbxScanTo.Location = new System.Drawing.Point(258, 19);
            this.tbxScanTo.Name = "tbxScanTo";
            this.tbxScanTo.Size = new System.Drawing.Size(33, 20);
            this.tbxScanTo.TabIndex = 4;
            this.tbxScanTo.Text = "255";
            this.tbxScanTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbxScanTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxScanFrom_KeyPress);
            // 
            // lblRadioChannelsFrom
            // 
            this.lblRadioChannelsFrom.AutoSize = true;
            this.lblRadioChannelsFrom.Location = new System.Drawing.Point(121, 22);
            this.lblRadioChannelsFrom.Name = "lblRadioChannelsFrom";
            this.lblRadioChannelsFrom.Size = new System.Drawing.Size(76, 13);
            this.lblRadioChannelsFrom.TabIndex = 3;
            this.lblRadioChannelsFrom.Text = "Радиоканалы";
            // 
            // tbxScanFrom
            // 
            this.tbxScanFrom.Location = new System.Drawing.Point(203, 19);
            this.tbxScanFrom.Name = "tbxScanFrom";
            this.tbxScanFrom.Size = new System.Drawing.Size(33, 20);
            this.tbxScanFrom.TabIndex = 2;
            this.tbxScanFrom.Text = "0";
            this.tbxScanFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbxScanFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxScanFrom_KeyPress);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemEdit});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(803, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItemEdit
            // 
            this.toolStripMenuItemEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmItemSerialPortSettings,
            this.tsmItemLanguage,
            this.toolStripSeparator1,
            this.tsmItemExit});
            this.toolStripMenuItemEdit.Name = "toolStripMenuItemEdit";
            this.toolStripMenuItemEdit.Size = new System.Drawing.Size(83, 20);
            this.toolStripMenuItemEdit.Text = "Параметры";
            // 
            // tsmItemSerialPortSettings
            // 
            this.tsmItemSerialPortSettings.Name = "tsmItemSerialPortSettings";
            this.tsmItemSerialPortSettings.Size = new System.Drawing.Size(202, 22);
            this.tsmItemSerialPortSettings.Text = "Настройки СОМ-порта";
            this.tsmItemSerialPortSettings.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // tsmItemLanguage
            // 
            this.tsmItemLanguage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmItemRus,
            this.tsmItemEng});
            this.tsmItemLanguage.Name = "tsmItemLanguage";
            this.tsmItemLanguage.Size = new System.Drawing.Size(202, 22);
            this.tsmItemLanguage.Text = "Язык";
            // 
            // tsmItemRus
            // 
            this.tsmItemRus.Checked = true;
            this.tsmItemRus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsmItemRus.Image = ((System.Drawing.Image)(resources.GetObject("tsmItemRus.Image")));
            this.tsmItemRus.Name = "tsmItemRus";
            this.tsmItemRus.Size = new System.Drawing.Size(119, 22);
            this.tsmItemRus.Text = "Русский";
            this.tsmItemRus.Click += new System.EventHandler(this.tsmItemRus_Click);
            // 
            // tsmItemEng
            // 
            this.tsmItemEng.Image = ((System.Drawing.Image)(resources.GetObject("tsmItemEng.Image")));
            this.tsmItemEng.Name = "tsmItemEng";
            this.tsmItemEng.Size = new System.Drawing.Size(119, 22);
            this.tsmItemEng.Text = "English";
            this.tsmItemEng.Click += new System.EventHandler(this.tsmItemEng_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(199, 6);
            // 
            // tsmItemExit
            // 
            this.tsmItemExit.Name = "tsmItemExit";
            this.tsmItemExit.Size = new System.Drawing.Size(202, 22);
            this.tsmItemExit.Text = "Exit";
            this.tsmItemExit.Click += new System.EventHandler(this.tsmItemExit_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(7, 27);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvDevices);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnStopGetData);
            this.splitContainer1.Panel2.Controls.Add(this.lblDevDevInfo);
            this.splitContainer1.Panel2.Controls.Add(this.lblDevRadioChannel);
            this.splitContainer1.Panel2.Controls.Add(this.lblDevDiBusAddress);
            this.splitContainer1.Panel2.Controls.Add(this.btnGetDataFromDevice);
            this.splitContainer1.Panel2.Controls.Add(this.btnSetDeviceRadioChannel);
            this.splitContainer1.Panel2.Controls.Add(this.tbxDeviceNewRadioChannel);
            this.splitContainer1.Panel2.Controls.Add(this.listBox1);
            this.splitContainer1.Panel2.Controls.Add(this.lblLabelDevInfo);
            this.splitContainer1.Panel2.Controls.Add(this.lblLabelRadioChannel);
            this.splitContainer1.Panel2.Controls.Add(this.lblLabelDiBusAddress);
            this.splitContainer1.Panel2.Controls.Add(this.lblDevice);
            this.splitContainer1.Panel2.Controls.Add(this.pbxDevIcon);
            this.splitContainer1.Size = new System.Drawing.Size(792, 475);
            this.splitContainer1.SplitterDistance = 406;
            this.splitContainer1.TabIndex = 14;
            // 
            // btnStopGetData
            // 
            this.btnStopGetData.Location = new System.Drawing.Point(18, 143);
            this.btnStopGetData.Name = "btnStopGetData";
            this.btnStopGetData.Size = new System.Drawing.Size(139, 25);
            this.btnStopGetData.TabIndex = 19;
            this.btnStopGetData.Text = "Stop";
            this.btnStopGetData.UseVisualStyleBackColor = true;
            this.btnStopGetData.Visible = false;
            this.btnStopGetData.Click += new System.EventHandler(this.btnStopGetData_Click);
            // 
            // lblDevDevInfo
            // 
            this.lblDevDevInfo.Location = new System.Drawing.Point(165, 78);
            this.lblDevDevInfo.Name = "lblDevDevInfo";
            this.lblDevDevInfo.Size = new System.Drawing.Size(78, 34);
            this.lblDevDevInfo.TabIndex = 18;
            // 
            // lblDevRadioChannel
            // 
            this.lblDevRadioChannel.AutoSize = true;
            this.lblDevRadioChannel.Location = new System.Drawing.Point(165, 55);
            this.lblDevRadioChannel.Name = "lblDevRadioChannel";
            this.lblDevRadioChannel.Size = new System.Drawing.Size(19, 13);
            this.lblDevRadioChannel.TabIndex = 17;
            this.lblDevRadioChannel.Text = "__";
            // 
            // lblDevDiBusAddress
            // 
            this.lblDevDiBusAddress.AutoSize = true;
            this.lblDevDiBusAddress.Location = new System.Drawing.Point(165, 29);
            this.lblDevDiBusAddress.Name = "lblDevDiBusAddress";
            this.lblDevDiBusAddress.Size = new System.Drawing.Size(55, 13);
            this.lblDevDiBusAddress.TabIndex = 16;
            this.lblDevDiBusAddress.Text = "__.__.___";
            // 
            // btnGetDataFromDevice
            // 
            this.btnGetDataFromDevice.Enabled = false;
            this.btnGetDataFromDevice.Location = new System.Drawing.Point(18, 143);
            this.btnGetDataFromDevice.Name = "btnGetDataFromDevice";
            this.btnGetDataFromDevice.Size = new System.Drawing.Size(139, 25);
            this.btnGetDataFromDevice.TabIndex = 15;
            this.btnGetDataFromDevice.Text = "Get Data";
            this.btnGetDataFromDevice.UseVisualStyleBackColor = true;
            this.btnGetDataFromDevice.Click += new System.EventHandler(this.btnGetDataFromDevice_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(18, 172);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(347, 303);
            this.listBox1.TabIndex = 14;
            // 
            // lblLabelDevInfo
            // 
            this.lblLabelDevInfo.AutoSize = true;
            this.lblLabelDevInfo.Location = new System.Drawing.Point(38, 81);
            this.lblLabelDevInfo.Name = "lblLabelDevInfo";
            this.lblLabelDevInfo.Size = new System.Drawing.Size(107, 13);
            this.lblLabelDevInfo.TabIndex = 4;
            this.lblLabelDevInfo.Text = "Additional information";
            // 
            // lblLabelRadioChannel
            // 
            this.lblLabelRadioChannel.AutoSize = true;
            this.lblLabelRadioChannel.Location = new System.Drawing.Point(38, 55);
            this.lblLabelRadioChannel.Name = "lblLabelRadioChannel";
            this.lblLabelRadioChannel.Size = new System.Drawing.Size(74, 13);
            this.lblLabelRadioChannel.TabIndex = 3;
            this.lblLabelRadioChannel.Text = "RadioChannel";
            // 
            // lblLabelDiBusAddress
            // 
            this.lblLabelDiBusAddress.AutoSize = true;
            this.lblLabelDiBusAddress.Location = new System.Drawing.Point(38, 29);
            this.lblLabelDiBusAddress.Name = "lblLabelDiBusAddress";
            this.lblLabelDiBusAddress.Size = new System.Drawing.Size(82, 13);
            this.lblLabelDiBusAddress.TabIndex = 2;
            this.lblLabelDiBusAddress.Text = "DiBus Address: ";
            // 
            // lblDevice
            // 
            this.lblDevice.AutoSize = true;
            this.lblDevice.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblDevice.Location = new System.Drawing.Point(14, 3);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(67, 20);
            this.lblDevice.TabIndex = 1;
            this.lblDevice.Text = "Device";
            // 
            // pbxDevIcon
            // 
            this.pbxDevIcon.Location = new System.Drawing.Point(249, 8);
            this.pbxDevIcon.Name = "pbxDevIcon";
            this.pbxDevIcon.Size = new System.Drawing.Size(132, 132);
            this.pbxDevIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxDevIcon.TabIndex = 0;
            this.pbxDevIcon.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelCounter,
            this.toolStripProgressBar1,
            this.toolStripStatusLabelInfo});
            this.statusStrip1.Location = new System.Drawing.Point(0, 552);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(803, 22);
            this.statusStrip1.TabIndex = 15;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelCounter
            // 
            this.toolStripStatusLabelCounter.Name = "toolStripStatusLabelCounter";
            this.toolStripStatusLabelCounter.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.AutoSize = false;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(200, 16);
            this.toolStripProgressBar1.Visible = false;
            // 
            // toolStripStatusLabelInfo
            // 
            this.toolStripStatusLabelInfo.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripStatusLabelInfo.Name = "toolStripStatusLabelInfo";
            this.toolStripStatusLabelInfo.Size = new System.Drawing.Size(0, 17);
            this.toolStripStatusLabelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // imageListLarge
            // 
            this.imageListLarge.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListLarge.ImageStream")));
            this.imageListLarge.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListLarge.Images.SetKeyName(0, "MBS2_256.png");
            this.imageListLarge.Images.SetKeyName(1, "DBGK_R20D.png");
            this.imageListLarge.Images.SetKeyName(2, "UPI.png");
            this.imageListLarge.Images.SetKeyName(3, "MBS_3_256.png");
            this.imageListLarge.Images.SetKeyName(4, "BDBeta.png");
            this.imageListLarge.Images.SetKeyName(5, "BDAlpha.png");
            this.imageListLarge.Images.SetKeyName(6, "newtron_128.png");
            this.imageListLarge.Images.SetKeyName(7, "Undefined_Device_icon.png");
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 574);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.gbxSettings);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = " ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.gbxSettings.ResumeLayout(false);
            this.gbxSettings.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbxDevIcon)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        //private RoundedButton btnFind;
        private System.Windows.Forms.ListView lvDevices;
        private System.Windows.Forms.ImageList imageList1;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeaderDiBusAddress;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.ColumnHeader columnHeaderComment;
        private System.Windows.Forms.ColumnHeader columnHeaderRadioChannel;
        private System.Windows.Forms.GroupBox gbxSettings;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxScanTo;
        private System.Windows.Forms.Label lblRadioChannelsFrom;
        private System.Windows.Forms.TextBox tbxScanFrom;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnSetDeviceRadioChannel;
        private System.Windows.Forms.TextBox tbxDeviceNewRadioChannel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmItemSerialPortSettings;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmItemExit;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCounter;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelInfo;
        private System.Windows.Forms.Label lblLabelDevInfo;
        private System.Windows.Forms.Label lblLabelRadioChannel;
        private System.Windows.Forms.Label lblLabelDiBusAddress;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.PictureBox pbxDevIcon;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnGetDataFromDevice;
        private System.Windows.Forms.Label lblDevDiBusAddress;
        private System.Windows.Forms.Label lblDevRadioChannel;
        private System.Windows.Forms.Label lblDevDevInfo;
        private System.Windows.Forms.ImageList imageListLarge;
        private System.Windows.Forms.Button btnStopGetData;
        private System.Windows.Forms.ToolStripMenuItem tsmItemLanguage;
        private System.Windows.Forms.ToolStripMenuItem tsmItemRus;
        private System.Windows.Forms.ToolStripMenuItem tsmItemEng;
    }
}


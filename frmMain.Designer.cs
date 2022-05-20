namespace COMunicator
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.chbBaudTest = new System.Windows.Forms.CheckBox();
            this.btnBaudRate = new System.Windows.Forms.Button();
            this.CntMnu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CntText = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.CntCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.CntSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusImg = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.mnuShowType = new System.Windows.Forms.ToolStripMenuItem();
            this.chkString = new System.Windows.Forms.ToolStripMenuItem();
            this.chkLine = new System.Windows.Forms.ToolStripMenuItem();
            this.mnutxtLine = new System.Windows.Forms.ToolStripTextBox();
            this.chkByte = new System.Windows.Forms.ToolStripMenuItem();
            this.chkHex = new System.Windows.Forms.ToolStripMenuItem();
            this.chkFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.chkMarsA = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuShow = new System.Windows.Forms.ToolStripMenuItem();
            this.chkTime = new System.Windows.Forms.ToolStripMenuItem();
            this.chkBaudRate = new System.Windows.Forms.ToolStripMenuItem();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtEndCMD = new System.Windows.Forms.TextBox();
            this.chbClear = new System.Windows.Forms.CheckBox();
            this.chbAutoSend = new System.Windows.Forms.CheckBox();
            this.lblSPBaud = new System.Windows.Forms.Label();
            this.cbBaud = new System.Windows.Forms.ComboBox();
            this.chbAutoReply = new System.Windows.Forms.CheckBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.tbSend = new System.Windows.Forms.TextBox();
            this.CntSend = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuAddSend = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditSend = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSettings = new System.Windows.Forms.Button();
            this.cbbCOMPorts = new System.Windows.Forms.ComboBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblConnection = new System.Windows.Forms.Label();
            this.Timer2 = new System.Windows.Forms.Timer(this.components);
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.chbEndChar = new System.Windows.Forms.CheckBox();
            this.TimeOut = new System.Windows.Forms.Timer(this.components);
            this.lbLog = new System.Windows.Forms.ListBox();
            this.btnNetConn = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabCOM = new System.Windows.Forms.TabPage();
            this.btnSPRefresh = new System.Windows.Forms.Button();
            this.lblSPPort = new System.Windows.Forms.Label();
            this.tabTCPClient = new System.Windows.Forms.TabPage();
            this.btnSettings2 = new System.Windows.Forms.Button();
            this.lblLocalPort = new System.Windows.Forms.Label();
            this.txtLocalPort = new System.Windows.Forms.TextBox();
            this.lblProtocol = new System.Windows.Forms.Label();
            this.cbProtocol = new System.Windows.Forms.ComboBox();
            this.lblIP = new System.Windows.Forms.Label();
            this.cbIP = new System.Windows.Forms.ComboBox();
            this.cbPort = new System.Windows.Forms.ComboBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.tabTCPServer = new System.Windows.Forms.TabPage();
            this.btnSettings3 = new System.Windows.Forms.Button();
            this.cbSPort = new System.Windows.Forms.ComboBox();
            this.lblServerPort = new System.Windows.Forms.Label();
            this.btnNetSConn = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.chbSendFromFile = new System.Windows.Forms.CheckBox();
            this.tabsMessages = new System.Windows.Forms.TabControl();
            this.tabComm = new System.Windows.Forms.TabPage();
            this.olvPacket = new BrightIdeasSoftware.FastObjectListView();
            this.colTime = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colPacket = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tabProcessLog = new System.Windows.Forms.TabPage();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.CntMnu.SuspendLayout();
            this.StatusStrip1.SuspendLayout();
            this.CntSend.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabCOM.SuspendLayout();
            this.tabTCPClient.SuspendLayout();
            this.tabTCPServer.SuspendLayout();
            this.tabsMessages.SuspendLayout();
            this.tabComm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvPacket)).BeginInit();
            this.tabProcessLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // chbBaudTest
            // 
            this.chbBaudTest.AutoSize = true;
            this.chbBaudTest.Location = new System.Drawing.Point(242, 35);
            this.chbBaudTest.Name = "chbBaudTest";
            this.chbBaudTest.Size = new System.Drawing.Size(75, 17);
            this.chbBaudTest.TabIndex = 44;
            this.chbBaudTest.Text = "Baud Test";
            this.chbBaudTest.UseVisualStyleBackColor = true;
            // 
            // btnBaudRate
            // 
            this.btnBaudRate.Location = new System.Drawing.Point(201, 31);
            this.btnBaudRate.Name = "btnBaudRate";
            this.btnBaudRate.Size = new System.Drawing.Size(35, 23);
            this.btnBaudRate.TabIndex = 43;
            this.btnBaudRate.Text = "Set";
            this.btnBaudRate.UseVisualStyleBackColor = true;
            this.btnBaudRate.Click += new System.EventHandler(this.btnBaudRate_Click);
            // 
            // CntMnu
            // 
            this.CntMnu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.CntMnu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CntText,
            this.ToolStripMenuItem1,
            this.CntCopy,
            this.CntSaveAs,
            this.clearToolStripMenuItem});
            this.CntMnu.Name = "CntMnu";
            this.CntMnu.Size = new System.Drawing.Size(142, 98);
            // 
            // CntText
            // 
            this.CntText.Name = "CntText";
            this.CntText.Size = new System.Drawing.Size(141, 22);
            this.CntText.Text = "To Textbox";
            this.CntText.Click += new System.EventHandler(this.CntText_Click);
            // 
            // ToolStripMenuItem1
            // 
            this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
            this.ToolStripMenuItem1.Size = new System.Drawing.Size(138, 6);
            // 
            // CntCopy
            // 
            this.CntCopy.Name = "CntCopy";
            this.CntCopy.Size = new System.Drawing.Size(141, 22);
            this.CntCopy.Text = "Copy";
            this.CntCopy.Click += new System.EventHandler(this.CntCopy_Click);
            // 
            // CntSaveAs
            // 
            this.CntSaveAs.Name = "CntSaveAs";
            this.CntSaveAs.Size = new System.Drawing.Size(141, 22);
            this.CntSaveAs.Text = "Save log as...";
            this.CntSaveAs.Click += new System.EventHandler(this.CntSaveAs_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // StatusStrip1
            // 
            this.StatusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusImg,
            this.ToolStripDropDownButton1,
            this.lblStatus});
            this.StatusStrip1.Location = new System.Drawing.Point(0, 523);
            this.StatusStrip1.Name = "StatusStrip1";
            this.StatusStrip1.Size = new System.Drawing.Size(328, 26);
            this.StatusStrip1.TabIndex = 40;
            this.StatusStrip1.Text = "StatusStrip1";
            // 
            // statusImg
            // 
            this.statusImg.Image = global::COMunicator.Properties.Resources.circ_red24;
            this.statusImg.Name = "statusImg";
            this.statusImg.Size = new System.Drawing.Size(20, 21);
            // 
            // ToolStripDropDownButton1
            // 
            this.ToolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShowType,
            this.ToolStripMenuItem4,
            this.mnuShow});
            this.ToolStripDropDownButton1.Image = global::COMunicator.Properties.Resources.con24;
            this.ToolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripDropDownButton1.Name = "ToolStripDropDownButton1";
            this.ToolStripDropDownButton1.Size = new System.Drawing.Size(33, 24);
            this.ToolStripDropDownButton1.Text = "ToolStripDropDownButton1";
            // 
            // mnuShowType
            // 
            this.mnuShowType.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chkString,
            this.chkByte,
            this.chkHex,
            this.chkFormat,
            this.toolStripMenuItem5,
            this.chkMarsA});
            this.mnuShowType.Name = "mnuShowType";
            this.mnuShowType.Size = new System.Drawing.Size(130, 22);
            this.mnuShowType.Text = "Show Type";
            // 
            // chkString
            // 
            this.chkString.Checked = true;
            this.chkString.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkString.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chkLine});
            this.chkString.Name = "chkString";
            this.chkString.Size = new System.Drawing.Size(125, 22);
            this.chkString.Text = "String";
            this.chkString.Click += new System.EventHandler(this.chkString_Click);
            // 
            // chkLine
            // 
            this.chkLine.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnutxtLine});
            this.chkLine.Name = "chkLine";
            this.chkLine.Size = new System.Drawing.Size(148, 22);
            this.chkLine.Text = "Line separator";
            this.chkLine.Click += new System.EventHandler(this.chkTime_Click);
            // 
            // mnutxtLine
            // 
            this.mnutxtLine.Name = "mnutxtLine";
            this.mnutxtLine.Size = new System.Drawing.Size(100, 23);
            // 
            // chkByte
            // 
            this.chkByte.Name = "chkByte";
            this.chkByte.Size = new System.Drawing.Size(125, 22);
            this.chkByte.Text = "Byte";
            this.chkByte.Click += new System.EventHandler(this.chkString_Click);
            // 
            // chkHex
            // 
            this.chkHex.Name = "chkHex";
            this.chkHex.Size = new System.Drawing.Size(125, 22);
            this.chkHex.Text = "Hex Num";
            this.chkHex.Click += new System.EventHandler(this.chkString_Click);
            // 
            // chkFormat
            // 
            this.chkFormat.Name = "chkFormat";
            this.chkFormat.Size = new System.Drawing.Size(125, 22);
            this.chkFormat.Text = "Format";
            this.chkFormat.Click += new System.EventHandler(this.chkString_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(122, 6);
            // 
            // chkMarsA
            // 
            this.chkMarsA.Name = "chkMarsA";
            this.chkMarsA.Size = new System.Drawing.Size(125, 22);
            this.chkMarsA.Text = "MARS-A";
            this.chkMarsA.Click += new System.EventHandler(this.chkString_Click);
            // 
            // ToolStripMenuItem4
            // 
            this.ToolStripMenuItem4.Name = "ToolStripMenuItem4";
            this.ToolStripMenuItem4.Size = new System.Drawing.Size(127, 6);
            // 
            // mnuShow
            // 
            this.mnuShow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chkTime,
            this.chkBaudRate});
            this.mnuShow.Name = "mnuShow";
            this.mnuShow.Size = new System.Drawing.Size(130, 22);
            this.mnuShow.Text = "Show";
            // 
            // chkTime
            // 
            this.chkTime.Name = "chkTime";
            this.chkTime.Size = new System.Drawing.Size(124, 22);
            this.chkTime.Text = "Time";
            this.chkTime.Click += new System.EventHandler(this.chkTime_Click);
            // 
            // chkBaudRate
            // 
            this.chkBaudRate.Name = "chkBaudRate";
            this.chkBaudRate.Size = new System.Drawing.Size(124, 22);
            this.chkBaudRate.Text = "BaudRate";
            this.chkBaudRate.Click += new System.EventHandler(this.chkTime_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 21);
            // 
            // txtEndCMD
            // 
            this.txtEndCMD.Location = new System.Drawing.Point(201, 162);
            this.txtEndCMD.Name = "txtEndCMD";
            this.txtEndCMD.Size = new System.Drawing.Size(39, 20);
            this.txtEndCMD.TabIndex = 39;
            // 
            // chbClear
            // 
            this.chbClear.AutoSize = true;
            this.chbClear.Location = new System.Drawing.Point(11, 129);
            this.chbClear.Name = "chbClear";
            this.chbClear.Size = new System.Drawing.Size(66, 17);
            this.chbClear.TabIndex = 41;
            this.chbClear.Text = "No clear";
            this.chbClear.UseVisualStyleBackColor = true;
            // 
            // chbAutoSend
            // 
            this.chbAutoSend.AutoSize = true;
            this.chbAutoSend.Location = new System.Drawing.Point(11, 164);
            this.chbAutoSend.Name = "chbAutoSend";
            this.chbAutoSend.Size = new System.Drawing.Size(90, 17);
            this.chbAutoSend.TabIndex = 36;
            this.chbAutoSend.Text = "Auto Sending";
            this.chbAutoSend.UseVisualStyleBackColor = true;
            this.chbAutoSend.CheckedChanged += new System.EventHandler(this.chbAutoSend_CheckedChanged);
            // 
            // lblSPBaud
            // 
            this.lblSPBaud.AutoSize = true;
            this.lblSPBaud.Location = new System.Drawing.Point(6, 36);
            this.lblSPBaud.Name = "lblSPBaud";
            this.lblSPBaud.Size = new System.Drawing.Size(56, 13);
            this.lblSPBaud.TabIndex = 35;
            this.lblSPBaud.Text = "Baud rate:";
            // 
            // cbBaud
            // 
            this.cbBaud.FormattingEnabled = true;
            this.cbBaud.Items.AddRange(new object[] {
            "2400",
            "3600",
            "4800",
            "9600",
            "14400",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cbBaud.Location = new System.Drawing.Point(68, 33);
            this.cbBaud.Name = "cbBaud";
            this.cbBaud.Size = new System.Drawing.Size(127, 21);
            this.cbBaud.TabIndex = 34;
            // 
            // chbAutoReply
            // 
            this.chbAutoReply.AutoSize = true;
            this.chbAutoReply.Location = new System.Drawing.Point(11, 147);
            this.chbAutoReply.Name = "chbAutoReply";
            this.chbAutoReply.Size = new System.Drawing.Size(78, 17);
            this.chbAutoReply.TabIndex = 32;
            this.chbAutoReply.Text = "Auto Reply";
            this.chbAutoReply.UseVisualStyleBackColor = true;
            this.chbAutoReply.CheckedChanged += new System.EventHandler(this.chbAutoReply_CheckedChanged);
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(246, 186);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 21);
            this.btnSend.TabIndex = 30;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // tbSend
            // 
            this.tbSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSend.ContextMenuStrip = this.CntSend;
            this.tbSend.Location = new System.Drawing.Point(8, 187);
            this.tbSend.Name = "tbSend";
            this.tbSend.Size = new System.Drawing.Size(232, 20);
            this.tbSend.TabIndex = 29;
            this.tbSend.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSend_KeyDown);
            // 
            // CntSend
            // 
            this.CntSend.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.CntSend.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAddSend,
            this.mnuEditSend,
            this.toolStripMenuItem3});
            this.CntSend.Name = "CntMnu";
            this.CntSend.Size = new System.Drawing.Size(115, 54);
            // 
            // mnuAddSend
            // 
            this.mnuAddSend.Name = "mnuAddSend";
            this.mnuAddSend.Size = new System.Drawing.Size(114, 22);
            this.mnuAddSend.Text = "Přidat...";
            this.mnuAddSend.Click += new System.EventHandler(this.mnuAddSend_Click);
            // 
            // mnuEditSend
            // 
            this.mnuEditSend.Name = "mnuEditSend";
            this.mnuEditSend.Size = new System.Drawing.Size(114, 22);
            this.mnuEditSend.Text = "Edit";
            this.mnuEditSend.Click += new System.EventHandler(this.mnuEditSend_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(111, 6);
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(156, 60);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(82, 23);
            this.btnSettings.TabIndex = 28;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // cbbCOMPorts
            // 
            this.cbbCOMPorts.FormattingEnabled = true;
            this.cbbCOMPorts.Location = new System.Drawing.Point(68, 6);
            this.cbbCOMPorts.Name = "cbbCOMPorts";
            this.cbbCOMPorts.Size = new System.Drawing.Size(127, 21);
            this.cbbCOMPorts.TabIndex = 26;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(68, 60);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(82, 23);
            this.btnConnect.TabIndex = 25;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lblConnection
            // 
            this.lblConnection.AutoSize = true;
            this.lblConnection.Location = new System.Drawing.Point(302, 40);
            this.lblConnection.Name = "lblConnection";
            this.lblConnection.Size = new System.Drawing.Size(0, 13);
            this.lblConnection.TabIndex = 27;
            // 
            // Timer2
            // 
            this.Timer2.Interval = 2000;
            this.Timer2.Tick += new System.EventHandler(this.Timer2_Tick);
            // 
            // Timer1
            // 
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // chbEndChar
            // 
            this.chbEndChar.AutoSize = true;
            this.chbEndChar.Location = new System.Drawing.Point(107, 164);
            this.chbEndChar.Name = "chbEndChar";
            this.chbEndChar.Size = new System.Drawing.Size(72, 17);
            this.chbEndChar.TabIndex = 46;
            this.chbEndChar.Text = "End char:";
            this.chbEndChar.UseVisualStyleBackColor = true;
            // 
            // TimeOut
            // 
            this.TimeOut.Interval = 20;
            this.TimeOut.Tick += new System.EventHandler(this.TimeOut_Tick);
            // 
            // lbLog
            // 
            this.lbLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLog.ContextMenuStrip = this.CntMnu;
            this.lbLog.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbLog.FormattingEnabled = true;
            this.lbLog.ItemHeight = 14;
            this.lbLog.Location = new System.Drawing.Point(0, 495);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(328, 32);
            this.lbLog.TabIndex = 42;
            // 
            // btnNetConn
            // 
            this.btnNetConn.Location = new System.Drawing.Point(68, 60);
            this.btnNetConn.Name = "btnNetConn";
            this.btnNetConn.Size = new System.Drawing.Size(82, 23);
            this.btnNetConn.TabIndex = 50;
            this.btnNetConn.Text = "Connect";
            this.btnNetConn.UseVisualStyleBackColor = true;
            this.btnNetConn.Click += new System.EventHandler(this.btnNetConn_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabCOM);
            this.tabControl1.Controls.Add(this.tabTCPClient);
            this.tabControl1.Controls.Add(this.tabTCPServer);
            this.tabControl1.Location = new System.Drawing.Point(0, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(328, 116);
            this.tabControl1.TabIndex = 51;
            // 
            // tabCOM
            // 
            this.tabCOM.Controls.Add(this.btnSPRefresh);
            this.tabCOM.Controls.Add(this.lblSPPort);
            this.tabCOM.Controls.Add(this.cbbCOMPorts);
            this.tabCOM.Controls.Add(this.lblConnection);
            this.tabCOM.Controls.Add(this.btnConnect);
            this.tabCOM.Controls.Add(this.btnSettings);
            this.tabCOM.Controls.Add(this.cbBaud);
            this.tabCOM.Controls.Add(this.lblSPBaud);
            this.tabCOM.Controls.Add(this.chbBaudTest);
            this.tabCOM.Controls.Add(this.btnBaudRate);
            this.tabCOM.Location = new System.Drawing.Point(4, 22);
            this.tabCOM.Name = "tabCOM";
            this.tabCOM.Padding = new System.Windows.Forms.Padding(3);
            this.tabCOM.Size = new System.Drawing.Size(320, 90);
            this.tabCOM.TabIndex = 0;
            this.tabCOM.Text = "COM";
            this.tabCOM.UseVisualStyleBackColor = true;
            // 
            // btnSPRefresh
            // 
            this.btnSPRefresh.Image = global::COMunicator.Properties.Resources.Refresh16;
            this.btnSPRefresh.Location = new System.Drawing.Point(201, 4);
            this.btnSPRefresh.Name = "btnSPRefresh";
            this.btnSPRefresh.Size = new System.Drawing.Size(35, 23);
            this.btnSPRefresh.TabIndex = 46;
            this.btnSPRefresh.UseVisualStyleBackColor = true;
            this.btnSPRefresh.Click += new System.EventHandler(this.btnSPRefresh_Click);
            // 
            // lblSPPort
            // 
            this.lblSPPort.AutoSize = true;
            this.lblSPPort.Location = new System.Drawing.Point(6, 9);
            this.lblSPPort.Name = "lblSPPort";
            this.lblSPPort.Size = new System.Drawing.Size(29, 13);
            this.lblSPPort.TabIndex = 45;
            this.lblSPPort.Text = "Port:";
            // 
            // tabTCPClient
            // 
            this.tabTCPClient.Controls.Add(this.btnSettings2);
            this.tabTCPClient.Controls.Add(this.lblLocalPort);
            this.tabTCPClient.Controls.Add(this.txtLocalPort);
            this.tabTCPClient.Controls.Add(this.lblProtocol);
            this.tabTCPClient.Controls.Add(this.cbProtocol);
            this.tabTCPClient.Controls.Add(this.lblIP);
            this.tabTCPClient.Controls.Add(this.cbIP);
            this.tabTCPClient.Controls.Add(this.cbPort);
            this.tabTCPClient.Controls.Add(this.lblPort);
            this.tabTCPClient.Controls.Add(this.btnNetConn);
            this.tabTCPClient.Location = new System.Drawing.Point(4, 22);
            this.tabTCPClient.Name = "tabTCPClient";
            this.tabTCPClient.Padding = new System.Windows.Forms.Padding(3);
            this.tabTCPClient.Size = new System.Drawing.Size(320, 90);
            this.tabTCPClient.TabIndex = 1;
            this.tabTCPClient.Text = "Net Client";
            this.tabTCPClient.UseVisualStyleBackColor = true;
            // 
            // btnSettings2
            // 
            this.btnSettings2.Location = new System.Drawing.Point(156, 60);
            this.btnSettings2.Name = "btnSettings2";
            this.btnSettings2.Size = new System.Drawing.Size(82, 23);
            this.btnSettings2.TabIndex = 59;
            this.btnSettings2.Text = "Settings";
            this.btnSettings2.UseVisualStyleBackColor = true;
            this.btnSettings2.Click += new System.EventHandler(this.btnSettings2_Click);
            // 
            // lblLocalPort
            // 
            this.lblLocalPort.AutoSize = true;
            this.lblLocalPort.Location = new System.Drawing.Point(201, 36);
            this.lblLocalPort.Name = "lblLocalPort";
            this.lblLocalPort.Size = new System.Drawing.Size(57, 13);
            this.lblLocalPort.TabIndex = 58;
            this.lblLocalPort.Text = "Local port:";
            // 
            // txtLocalPort
            // 
            this.txtLocalPort.Location = new System.Drawing.Point(264, 33);
            this.txtLocalPort.Name = "txtLocalPort";
            this.txtLocalPort.Size = new System.Drawing.Size(53, 20);
            this.txtLocalPort.TabIndex = 57;
            // 
            // lblProtocol
            // 
            this.lblProtocol.AutoSize = true;
            this.lblProtocol.Location = new System.Drawing.Point(201, 9);
            this.lblProtocol.Name = "lblProtocol";
            this.lblProtocol.Size = new System.Drawing.Size(49, 13);
            this.lblProtocol.TabIndex = 56;
            this.lblProtocol.Text = "Protocol:";
            // 
            // cbProtocol
            // 
            this.cbProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProtocol.FormattingEnabled = true;
            this.cbProtocol.Items.AddRange(new object[] {
            "TCP/IP",
            "UDP"});
            this.cbProtocol.Location = new System.Drawing.Point(256, 6);
            this.cbProtocol.Name = "cbProtocol";
            this.cbProtocol.Size = new System.Drawing.Size(61, 21);
            this.cbProtocol.TabIndex = 55;
            this.cbProtocol.SelectedIndexChanged += new System.EventHandler(this.cbProtocol_SelectedIndexChanged);
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(6, 9);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(20, 13);
            this.lblIP.TabIndex = 54;
            this.lblIP.Text = "IP:";
            // 
            // cbIP
            // 
            this.cbIP.FormattingEnabled = true;
            this.cbIP.Location = new System.Drawing.Point(68, 6);
            this.cbIP.Name = "cbIP";
            this.cbIP.Size = new System.Drawing.Size(127, 21);
            this.cbIP.TabIndex = 51;
            // 
            // cbPort
            // 
            this.cbPort.FormattingEnabled = true;
            this.cbPort.Items.AddRange(new object[] {
            "2400",
            "3600",
            "4800",
            "9600",
            "14400",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cbPort.Location = new System.Drawing.Point(68, 33);
            this.cbPort.Name = "cbPort";
            this.cbPort.Size = new System.Drawing.Size(127, 21);
            this.cbPort.TabIndex = 52;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(6, 36);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(29, 13);
            this.lblPort.TabIndex = 53;
            this.lblPort.Text = "Port:";
            // 
            // tabTCPServer
            // 
            this.tabTCPServer.Controls.Add(this.btnSettings3);
            this.tabTCPServer.Controls.Add(this.cbSPort);
            this.tabTCPServer.Controls.Add(this.lblServerPort);
            this.tabTCPServer.Controls.Add(this.btnNetSConn);
            this.tabTCPServer.Location = new System.Drawing.Point(4, 22);
            this.tabTCPServer.Name = "tabTCPServer";
            this.tabTCPServer.Size = new System.Drawing.Size(320, 90);
            this.tabTCPServer.TabIndex = 2;
            this.tabTCPServer.Text = "Net Server";
            this.tabTCPServer.UseVisualStyleBackColor = true;
            // 
            // btnSettings3
            // 
            this.btnSettings3.Location = new System.Drawing.Point(156, 60);
            this.btnSettings3.Name = "btnSettings3";
            this.btnSettings3.Size = new System.Drawing.Size(82, 23);
            this.btnSettings3.TabIndex = 59;
            this.btnSettings3.Text = "Settings";
            this.btnSettings3.UseVisualStyleBackColor = true;
            this.btnSettings3.Click += new System.EventHandler(this.btnSettings2_Click);
            // 
            // cbSPort
            // 
            this.cbSPort.FormattingEnabled = true;
            this.cbSPort.Items.AddRange(new object[] {
            "2400",
            "3600",
            "4800",
            "9600",
            "14400",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cbSPort.Location = new System.Drawing.Point(68, 33);
            this.cbSPort.Name = "cbSPort";
            this.cbSPort.Size = new System.Drawing.Size(127, 21);
            this.cbSPort.TabIndex = 57;
            // 
            // lblServerPort
            // 
            this.lblServerPort.AutoSize = true;
            this.lblServerPort.Location = new System.Drawing.Point(6, 36);
            this.lblServerPort.Name = "lblServerPort";
            this.lblServerPort.Size = new System.Drawing.Size(29, 13);
            this.lblServerPort.TabIndex = 58;
            this.lblServerPort.Text = "Port:";
            // 
            // btnNetSConn
            // 
            this.btnNetSConn.Location = new System.Drawing.Point(68, 60);
            this.btnNetSConn.Name = "btnNetSConn";
            this.btnNetSConn.Size = new System.Drawing.Size(82, 23);
            this.btnNetSConn.TabIndex = 55;
            this.btnNetSConn.Text = "Connect";
            this.btnNetSConn.UseVisualStyleBackColor = true;
            this.btnNetSConn.Click += new System.EventHandler(this.btnNetSConn_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(253, 141);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 52;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // chbSendFromFile
            // 
            this.chbSendFromFile.AutoSize = true;
            this.chbSendFromFile.Location = new System.Drawing.Point(107, 129);
            this.chbSendFromFile.Name = "chbSendFromFile";
            this.chbSendFromFile.Size = new System.Drawing.Size(104, 17);
            this.chbSendFromFile.TabIndex = 53;
            this.chbSendFromFile.Text = "Sending from file";
            this.chbSendFromFile.UseVisualStyleBackColor = true;
            this.chbSendFromFile.CheckedChanged += new System.EventHandler(this.chbSendFromFile_CheckedChanged);
            // 
            // tabsMessages
            // 
            this.tabsMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabsMessages.Controls.Add(this.tabComm);
            this.tabsMessages.Controls.Add(this.tabProcessLog);
            this.tabsMessages.Location = new System.Drawing.Point(8, 213);
            this.tabsMessages.Name = "tabsMessages";
            this.tabsMessages.SelectedIndex = 0;
            this.tabsMessages.Size = new System.Drawing.Size(313, 276);
            this.tabsMessages.TabIndex = 54;
            // 
            // tabComm
            // 
            this.tabComm.Controls.Add(this.olvPacket);
            this.tabComm.Location = new System.Drawing.Point(4, 22);
            this.tabComm.Name = "tabComm";
            this.tabComm.Padding = new System.Windows.Forms.Padding(3);
            this.tabComm.Size = new System.Drawing.Size(305, 250);
            this.tabComm.TabIndex = 0;
            this.tabComm.Text = "Packets";
            this.tabComm.UseVisualStyleBackColor = true;
            // 
            // olvPacket
            // 
            this.olvPacket.AllColumns.Add(this.colTime);
            this.olvPacket.AllColumns.Add(this.colPacket);
            this.olvPacket.CellEditUseWholeCell = false;
            this.olvPacket.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTime,
            this.colPacket});
            this.olvPacket.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvPacket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.olvPacket.HideSelection = false;
            this.olvPacket.Location = new System.Drawing.Point(3, 3);
            this.olvPacket.Name = "olvPacket";
            this.olvPacket.ShowGroups = false;
            this.olvPacket.Size = new System.Drawing.Size(299, 244);
            this.olvPacket.TabIndex = 50;
            this.olvPacket.UseCompatibleStateImageBehavior = false;
            this.olvPacket.View = System.Windows.Forms.View.Details;
            this.olvPacket.VirtualMode = true;
            this.olvPacket.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.olvPacket_FormatRow);
            this.olvPacket.Resize += new System.EventHandler(this.olvPacket_Resize);
            // 
            // colTime
            // 
            this.colTime.Text = "Time";
            this.colTime.Width = 76;
            // 
            // colPacket
            // 
            this.colPacket.Text = "Packet";
            this.colPacket.Width = 214;
            // 
            // tabProcessLog
            // 
            this.tabProcessLog.Controls.Add(this.txtLog);
            this.tabProcessLog.Location = new System.Drawing.Point(4, 22);
            this.tabProcessLog.Name = "tabProcessLog";
            this.tabProcessLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabProcessLog.Size = new System.Drawing.Size(305, 250);
            this.tabProcessLog.TabIndex = 1;
            this.tabProcessLog.Text = "Log";
            this.tabProcessLog.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.SystemColors.Window;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txtLog.Location = new System.Drawing.Point(3, 3);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(299, 244);
            this.txtLog.TabIndex = 50;
            this.txtLog.Text = "";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 549);
            this.Controls.Add(this.tabsMessages);
            this.Controls.Add(this.chbSendFromFile);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.chbEndChar);
            this.Controls.Add(this.lbLog);
            this.Controls.Add(this.StatusStrip1);
            this.Controls.Add(this.txtEndCMD);
            this.Controls.Add(this.chbClear);
            this.Controls.Add(this.chbAutoSend);
            this.Controls.Add(this.chbAutoReply);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.tbSend);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(344, 344);
            this.Name = "frmMain";
            this.Text = "COMunicator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.CntMnu.ResumeLayout(false);
            this.StatusStrip1.ResumeLayout(false);
            this.StatusStrip1.PerformLayout();
            this.CntSend.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabCOM.ResumeLayout(false);
            this.tabCOM.PerformLayout();
            this.tabTCPClient.ResumeLayout(false);
            this.tabTCPClient.PerformLayout();
            this.tabTCPServer.ResumeLayout(false);
            this.tabTCPServer.PerformLayout();
            this.tabsMessages.ResumeLayout(false);
            this.tabComm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvPacket)).EndInit();
            this.tabProcessLog.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.CheckBox chbBaudTest;
        internal System.Windows.Forms.Button btnBaudRate;
        internal System.Windows.Forms.StatusStrip StatusStrip1;
        internal System.Windows.Forms.ToolStripDropDownButton ToolStripDropDownButton1;
        internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem4;
        internal System.Windows.Forms.TextBox txtEndCMD;
        internal System.Windows.Forms.CheckBox chbClear;
        internal System.Windows.Forms.CheckBox chbAutoSend;
        internal System.Windows.Forms.Label lblSPBaud;
        internal System.Windows.Forms.ComboBox cbBaud;
        internal System.Windows.Forms.CheckBox chbAutoReply;
        internal System.Windows.Forms.Button btnSend;
        internal System.Windows.Forms.TextBox tbSend;
        internal System.Windows.Forms.Button btnSettings;
        internal System.Windows.Forms.ComboBox cbbCOMPorts;
        internal System.Windows.Forms.Button btnConnect;
        internal System.Windows.Forms.Label lblConnection;
        internal System.Windows.Forms.ContextMenuStrip CntMnu;
        internal System.Windows.Forms.ToolStripMenuItem CntText;
        internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripMenuItem CntCopy;
        internal System.Windows.Forms.ToolStripMenuItem CntSaveAs;
        internal System.Windows.Forms.ContextMenuStrip CntSend;
        internal System.Windows.Forms.Timer Timer2;
        internal System.Windows.Forms.Timer Timer1;
        private System.Windows.Forms.CheckBox chbEndChar;
        internal System.Windows.Forms.Timer TimeOut;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuAddSend;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        internal System.Windows.Forms.ListBox lbLog;
        private System.Windows.Forms.Button btnNetConn;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabCOM;
        private System.Windows.Forms.TabPage tabTCPClient;
        internal System.Windows.Forms.Label lblIP;
        internal System.Windows.Forms.ComboBox cbIP;
        internal System.Windows.Forms.ComboBox cbPort;
        internal System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        internal System.Windows.Forms.Label lblSPPort;
        internal System.Windows.Forms.Button btnSPRefresh;
        private System.Windows.Forms.TabPage tabTCPServer;
        internal System.Windows.Forms.ComboBox cbSPort;
        internal System.Windows.Forms.Label lblServerPort;
        private System.Windows.Forms.Button btnNetSConn;
        private System.Windows.Forms.ComboBox cbProtocol;
        internal System.Windows.Forms.Label lblProtocol;
        internal System.Windows.Forms.TextBox txtLocalPort;
        internal System.Windows.Forms.Label lblLocalPort;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStripMenuItem mnuShowType;
        private System.Windows.Forms.ToolStripMenuItem chkString;
        private System.Windows.Forms.ToolStripMenuItem chkLine;
        private System.Windows.Forms.ToolStripTextBox mnutxtLine;
        private System.Windows.Forms.ToolStripMenuItem chkByte;
        private System.Windows.Forms.ToolStripMenuItem chkHex;
        private System.Windows.Forms.ToolStripMenuItem chkFormat;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem chkMarsA;
        private System.Windows.Forms.ToolStripMenuItem mnuShow;
        private System.Windows.Forms.ToolStripMenuItem chkTime;
        private System.Windows.Forms.ToolStripMenuItem chkBaudRate;
        private System.Windows.Forms.ToolStripStatusLabel statusImg;
        internal System.Windows.Forms.Button btnSettings2;
        internal System.Windows.Forms.Button btnSettings3;
        internal System.Windows.Forms.CheckBox chbSendFromFile;
        private System.Windows.Forms.ToolStripMenuItem mnuEditSend;
        private System.Windows.Forms.TabControl tabsMessages;
        private System.Windows.Forms.TabPage tabComm;
        private System.Windows.Forms.TabPage tabProcessLog;
        private System.Windows.Forms.RichTextBox txtLog;
        private BrightIdeasSoftware.FastObjectListView olvPacket;
        private BrightIdeasSoftware.OLVColumn colTime;
        private BrightIdeasSoftware.OLVColumn colPacket;
    }
}
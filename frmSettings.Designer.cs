namespace COMunicator
{
    partial class frmSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbParity = new System.Windows.Forms.ComboBox();
            this.cbDataBits = new System.Windows.Forms.ComboBox();
            this.chbDTR = new System.Windows.Forms.CheckBox();
            this.chbRTS = new System.Windows.Forms.CheckBox();
            this.lblParity = new System.Windows.Forms.Label();
            this.lblDataBits = new System.Windows.Forms.Label();
            this.lblStopBits = new System.Windows.Forms.Label();
            this.cbStopBits = new System.Windows.Forms.ComboBox();
            this.txtAutoSendDelay = new System.Windows.Forms.TextBox();
            this.lblAutosendDelay = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabSerial = new System.Windows.Forms.TabPage();
            this.tabSending = new System.Windows.Forms.TabPage();
            this.gbReply = new System.Windows.Forms.GroupBox();
            this.btnReplyFile = new System.Windows.Forms.Button();
            this.chbEnableReply = new System.Windows.Forms.CheckBox();
            this.txtReplyFile = new System.Windows.Forms.TextBox();
            this.gbAutosending = new System.Windows.Forms.GroupBox();
            this.gbSendingFromFile = new System.Windows.Forms.GroupBox();
            this.chbEoF = new System.Windows.Forms.CheckBox();
            this.btnSendingFile = new System.Windows.Forms.Button();
            this.chbEnableSendFromFile = new System.Windows.Forms.CheckBox();
            this.txtSendingFile = new System.Windows.Forms.TextBox();
            this.tabPaths = new System.Windows.Forms.TabPage();
            this.gbDataFiles = new System.Windows.Forms.GroupBox();
            this.lblDataFolder = new System.Windows.Forms.Label();
            this.btnDataFolder = new System.Windows.Forms.Button();
            this.txtDataFolder = new System.Windows.Forms.TextBox();
            this.gbLogFile = new System.Windows.Forms.GroupBox();
            this.chbNewLog = new System.Windows.Forms.CheckBox();
            this.btnLogFile = new System.Windows.Forms.Button();
            this.chbEnableLog = new System.Windows.Forms.CheckBox();
            this.txtLogFile = new System.Windows.Forms.TextBox();
            this.tabOther = new System.Windows.Forms.TabPage();
            this.gbCoding = new System.Windows.Forms.GroupBox();
            this.cbCoding = new System.Windows.Forms.ComboBox();
            this.lblCoding = new System.Windows.Forms.Label();
            this.chbEnableReplyWait = new System.Windows.Forms.CheckBox();
            this.txtReplyTimeout = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabSerial.SuspendLayout();
            this.tabSending.SuspendLayout();
            this.gbReply.SuspendLayout();
            this.gbAutosending.SuspendLayout();
            this.gbSendingFromFile.SuspendLayout();
            this.tabPaths.SuspendLayout();
            this.gbDataFiles.SuspendLayout();
            this.gbLogFile.SuspendLayout();
            this.tabOther.SuspendLayout();
            this.gbCoding.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOk.Location = new System.Drawing.Point(12, 306);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(93, 306);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbParity
            // 
            this.cbParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbParity.FormattingEnabled = true;
            this.cbParity.Location = new System.Drawing.Point(7, 22);
            this.cbParity.Name = "cbParity";
            this.cbParity.Size = new System.Drawing.Size(75, 21);
            this.cbParity.TabIndex = 2;
            // 
            // cbDataBits
            // 
            this.cbDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataBits.FormattingEnabled = true;
            this.cbDataBits.Location = new System.Drawing.Point(7, 62);
            this.cbDataBits.Name = "cbDataBits";
            this.cbDataBits.Size = new System.Drawing.Size(75, 21);
            this.cbDataBits.TabIndex = 3;
            // 
            // chbDTR
            // 
            this.chbDTR.AutoSize = true;
            this.chbDTR.Location = new System.Drawing.Point(10, 89);
            this.chbDTR.Name = "chbDTR";
            this.chbDTR.Size = new System.Drawing.Size(49, 17);
            this.chbDTR.TabIndex = 4;
            this.chbDTR.Text = "DTR";
            this.chbDTR.UseVisualStyleBackColor = true;
            // 
            // chbRTS
            // 
            this.chbRTS.AutoSize = true;
            this.chbRTS.Location = new System.Drawing.Point(10, 112);
            this.chbRTS.Name = "chbRTS";
            this.chbRTS.Size = new System.Drawing.Size(48, 17);
            this.chbRTS.TabIndex = 5;
            this.chbRTS.Text = "RTS";
            this.chbRTS.UseVisualStyleBackColor = true;
            // 
            // lblParity
            // 
            this.lblParity.AutoSize = true;
            this.lblParity.Location = new System.Drawing.Point(4, 6);
            this.lblParity.Name = "lblParity";
            this.lblParity.Size = new System.Drawing.Size(36, 13);
            this.lblParity.TabIndex = 6;
            this.lblParity.Text = "Parity:";
            // 
            // lblDataBits
            // 
            this.lblDataBits.AutoSize = true;
            this.lblDataBits.Location = new System.Drawing.Point(7, 46);
            this.lblDataBits.Name = "lblDataBits";
            this.lblDataBits.Size = new System.Drawing.Size(52, 13);
            this.lblDataBits.TabIndex = 7;
            this.lblDataBits.Text = "Data bits:";
            // 
            // lblStopBits
            // 
            this.lblStopBits.AutoSize = true;
            this.lblStopBits.Location = new System.Drawing.Point(94, 46);
            this.lblStopBits.Name = "lblStopBits";
            this.lblStopBits.Size = new System.Drawing.Size(51, 13);
            this.lblStopBits.TabIndex = 8;
            this.lblStopBits.Text = "Stop bits:";
            // 
            // cbStopBits
            // 
            this.cbStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStopBits.FormattingEnabled = true;
            this.cbStopBits.Location = new System.Drawing.Point(88, 62);
            this.cbStopBits.Name = "cbStopBits";
            this.cbStopBits.Size = new System.Drawing.Size(75, 21);
            this.cbStopBits.TabIndex = 9;
            // 
            // txtAutoSendDelay
            // 
            this.txtAutoSendDelay.Location = new System.Drawing.Point(9, 32);
            this.txtAutoSendDelay.Name = "txtAutoSendDelay";
            this.txtAutoSendDelay.Size = new System.Drawing.Size(100, 20);
            this.txtAutoSendDelay.TabIndex = 10;
            // 
            // lblAutosendDelay
            // 
            this.lblAutosendDelay.AutoSize = true;
            this.lblAutosendDelay.Location = new System.Drawing.Point(6, 16);
            this.lblAutosendDelay.Name = "lblAutosendDelay";
            this.lblAutosendDelay.Size = new System.Drawing.Size(109, 13);
            this.lblAutosendDelay.TabIndex = 11;
            this.lblAutosendDelay.Text = "AutoSend Delay [ms]:";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabSerial);
            this.tabControl1.Controls.Add(this.tabSending);
            this.tabControl1.Controls.Add(this.tabPaths);
            this.tabControl1.Controls.Add(this.tabOther);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(317, 288);
            this.tabControl1.TabIndex = 14;
            // 
            // tabSerial
            // 
            this.tabSerial.Controls.Add(this.cbParity);
            this.tabSerial.Controls.Add(this.cbDataBits);
            this.tabSerial.Controls.Add(this.chbDTR);
            this.tabSerial.Controls.Add(this.chbRTS);
            this.tabSerial.Controls.Add(this.cbStopBits);
            this.tabSerial.Controls.Add(this.lblParity);
            this.tabSerial.Controls.Add(this.lblStopBits);
            this.tabSerial.Controls.Add(this.lblDataBits);
            this.tabSerial.Location = new System.Drawing.Point(4, 22);
            this.tabSerial.Name = "tabSerial";
            this.tabSerial.Padding = new System.Windows.Forms.Padding(3);
            this.tabSerial.Size = new System.Drawing.Size(309, 205);
            this.tabSerial.TabIndex = 0;
            this.tabSerial.Text = "Serial COM";
            this.tabSerial.UseVisualStyleBackColor = true;
            // 
            // tabSending
            // 
            this.tabSending.Controls.Add(this.gbReply);
            this.tabSending.Controls.Add(this.gbAutosending);
            this.tabSending.Location = new System.Drawing.Point(4, 22);
            this.tabSending.Name = "tabSending";
            this.tabSending.Padding = new System.Windows.Forms.Padding(3);
            this.tabSending.Size = new System.Drawing.Size(309, 262);
            this.tabSending.TabIndex = 1;
            this.tabSending.Text = "Sending";
            this.tabSending.UseVisualStyleBackColor = true;
            // 
            // gbReply
            // 
            this.gbReply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbReply.Controls.Add(this.btnReplyFile);
            this.gbReply.Controls.Add(this.chbEnableReply);
            this.gbReply.Controls.Add(this.txtReplyFile);
            this.gbReply.Location = new System.Drawing.Point(9, 205);
            this.gbReply.Name = "gbReply";
            this.gbReply.Size = new System.Drawing.Size(297, 51);
            this.gbReply.TabIndex = 14;
            this.gbReply.TabStop = false;
            this.gbReply.Text = "Reply from template file";
            // 
            // btnReplyFile
            // 
            this.btnReplyFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReplyFile.Location = new System.Drawing.Point(262, 15);
            this.btnReplyFile.Name = "btnReplyFile";
            this.btnReplyFile.Size = new System.Drawing.Size(29, 23);
            this.btnReplyFile.TabIndex = 14;
            this.btnReplyFile.Text = "...";
            this.btnReplyFile.UseVisualStyleBackColor = true;
            this.btnReplyFile.Click += new System.EventHandler(this.btnReplyFile_Click);
            // 
            // chbEnableReply
            // 
            this.chbEnableReply.AutoSize = true;
            this.chbEnableReply.Location = new System.Drawing.Point(9, 19);
            this.chbEnableReply.Name = "chbEnableReply";
            this.chbEnableReply.Size = new System.Drawing.Size(140, 17);
            this.chbEnableReply.TabIndex = 14;
            this.chbEnableReply.Text = "Enable replying from file:";
            this.chbEnableReply.UseVisualStyleBackColor = true;
            // 
            // txtReplyFile
            // 
            this.txtReplyFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReplyFile.Location = new System.Drawing.Point(155, 17);
            this.txtReplyFile.Name = "txtReplyFile";
            this.txtReplyFile.Size = new System.Drawing.Size(110, 20);
            this.txtReplyFile.TabIndex = 10;
            // 
            // gbAutosending
            // 
            this.gbAutosending.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbAutosending.Controls.Add(this.txtReplyTimeout);
            this.gbAutosending.Controls.Add(this.chbEnableReplyWait);
            this.gbAutosending.Controls.Add(this.gbSendingFromFile);
            this.gbAutosending.Controls.Add(this.lblAutosendDelay);
            this.gbAutosending.Controls.Add(this.txtAutoSendDelay);
            this.gbAutosending.Location = new System.Drawing.Point(6, 6);
            this.gbAutosending.Name = "gbAutosending";
            this.gbAutosending.Size = new System.Drawing.Size(297, 193);
            this.gbAutosending.TabIndex = 12;
            this.gbAutosending.TabStop = false;
            this.gbAutosending.Text = "Autosending";
            // 
            // gbSendingFromFile
            // 
            this.gbSendingFromFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSendingFromFile.Controls.Add(this.chbEoF);
            this.gbSendingFromFile.Controls.Add(this.btnSendingFile);
            this.gbSendingFromFile.Controls.Add(this.chbEnableSendFromFile);
            this.gbSendingFromFile.Controls.Add(this.txtSendingFile);
            this.gbSendingFromFile.Location = new System.Drawing.Point(6, 119);
            this.gbSendingFromFile.Name = "gbSendingFromFile";
            this.gbSendingFromFile.Size = new System.Drawing.Size(281, 68);
            this.gbSendingFromFile.TabIndex = 13;
            this.gbSendingFromFile.TabStop = false;
            this.gbSendingFromFile.Text = "Sending from file";
            // 
            // chbEoF
            // 
            this.chbEoF.AutoSize = true;
            this.chbEoF.Location = new System.Drawing.Point(9, 42);
            this.chbEoF.Name = "chbEoF";
            this.chbEoF.Size = new System.Drawing.Size(164, 17);
            this.chbEoF.TabIndex = 15;
            this.chbEoF.Text = "On end of file start from begin";
            this.chbEoF.UseVisualStyleBackColor = true;
            // 
            // btnSendingFile
            // 
            this.btnSendingFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendingFile.Location = new System.Drawing.Point(246, 15);
            this.btnSendingFile.Name = "btnSendingFile";
            this.btnSendingFile.Size = new System.Drawing.Size(29, 23);
            this.btnSendingFile.TabIndex = 14;
            this.btnSendingFile.Text = "...";
            this.btnSendingFile.UseVisualStyleBackColor = true;
            this.btnSendingFile.Click += new System.EventHandler(this.btnSendingFile_Click);
            // 
            // chbEnableSendFromFile
            // 
            this.chbEnableSendFromFile.AutoSize = true;
            this.chbEnableSendFromFile.Location = new System.Drawing.Point(9, 19);
            this.chbEnableSendFromFile.Name = "chbEnableSendFromFile";
            this.chbEnableSendFromFile.Size = new System.Drawing.Size(141, 17);
            this.chbEnableSendFromFile.TabIndex = 14;
            this.chbEnableSendFromFile.Text = "Enable sending from file:";
            this.chbEnableSendFromFile.UseVisualStyleBackColor = true;
            // 
            // txtSendingFile
            // 
            this.txtSendingFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSendingFile.Location = new System.Drawing.Point(155, 17);
            this.txtSendingFile.Name = "txtSendingFile";
            this.txtSendingFile.Size = new System.Drawing.Size(94, 20);
            this.txtSendingFile.TabIndex = 10;
            // 
            // tabPaths
            // 
            this.tabPaths.Controls.Add(this.gbDataFiles);
            this.tabPaths.Controls.Add(this.gbLogFile);
            this.tabPaths.Location = new System.Drawing.Point(4, 22);
            this.tabPaths.Name = "tabPaths";
            this.tabPaths.Padding = new System.Windows.Forms.Padding(3);
            this.tabPaths.Size = new System.Drawing.Size(309, 205);
            this.tabPaths.TabIndex = 2;
            this.tabPaths.Text = "Paths";
            this.tabPaths.UseVisualStyleBackColor = true;
            // 
            // gbDataFiles
            // 
            this.gbDataFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDataFiles.Controls.Add(this.lblDataFolder);
            this.gbDataFiles.Controls.Add(this.btnDataFolder);
            this.gbDataFiles.Controls.Add(this.txtDataFolder);
            this.gbDataFiles.Location = new System.Drawing.Point(6, 89);
            this.gbDataFiles.Name = "gbDataFiles";
            this.gbDataFiles.Size = new System.Drawing.Size(297, 53);
            this.gbDataFiles.TabIndex = 16;
            this.gbDataFiles.TabStop = false;
            this.gbDataFiles.Text = "Data files";
            // 
            // lblDataFolder
            // 
            this.lblDataFolder.AutoSize = true;
            this.lblDataFolder.Location = new System.Drawing.Point(6, 20);
            this.lblDataFolder.Name = "lblDataFolder";
            this.lblDataFolder.Size = new System.Drawing.Size(62, 13);
            this.lblDataFolder.TabIndex = 17;
            this.lblDataFolder.Text = "Data folder:";
            // 
            // btnDataFolder
            // 
            this.btnDataFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDataFolder.Location = new System.Drawing.Point(262, 15);
            this.btnDataFolder.Name = "btnDataFolder";
            this.btnDataFolder.Size = new System.Drawing.Size(29, 23);
            this.btnDataFolder.TabIndex = 14;
            this.btnDataFolder.Text = "...";
            this.btnDataFolder.UseVisualStyleBackColor = true;
            this.btnDataFolder.Click += new System.EventHandler(this.btnDataFolder_Click);
            // 
            // txtDataFolder
            // 
            this.txtDataFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDataFolder.Location = new System.Drawing.Point(110, 17);
            this.txtDataFolder.Name = "txtDataFolder";
            this.txtDataFolder.Size = new System.Drawing.Size(155, 20);
            this.txtDataFolder.TabIndex = 10;
            // 
            // gbLogFile
            // 
            this.gbLogFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbLogFile.Controls.Add(this.chbNewLog);
            this.gbLogFile.Controls.Add(this.btnLogFile);
            this.gbLogFile.Controls.Add(this.chbEnableLog);
            this.gbLogFile.Controls.Add(this.txtLogFile);
            this.gbLogFile.Location = new System.Drawing.Point(6, 6);
            this.gbLogFile.Name = "gbLogFile";
            this.gbLogFile.Size = new System.Drawing.Size(297, 77);
            this.gbLogFile.TabIndex = 15;
            this.gbLogFile.TabStop = false;
            this.gbLogFile.Text = "Log file";
            // 
            // chbNewLog
            // 
            this.chbNewLog.AutoSize = true;
            this.chbNewLog.Location = new System.Drawing.Point(9, 42);
            this.chbNewLog.Name = "chbNewLog";
            this.chbNewLog.Size = new System.Drawing.Size(244, 17);
            this.chbNewLog.TabIndex = 15;
            this.chbNewLog.Text = "Creating new log file for every new connection";
            this.chbNewLog.UseVisualStyleBackColor = true;
            // 
            // btnLogFile
            // 
            this.btnLogFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogFile.Location = new System.Drawing.Point(262, 15);
            this.btnLogFile.Name = "btnLogFile";
            this.btnLogFile.Size = new System.Drawing.Size(29, 23);
            this.btnLogFile.TabIndex = 14;
            this.btnLogFile.Text = "...";
            this.btnLogFile.UseVisualStyleBackColor = true;
            this.btnLogFile.Click += new System.EventHandler(this.btnLogFile_Click);
            // 
            // chbEnableLog
            // 
            this.chbEnableLog.AutoSize = true;
            this.chbEnableLog.Location = new System.Drawing.Point(9, 19);
            this.chbEnableLog.Name = "chbEnableLog";
            this.chbEnableLog.Size = new System.Drawing.Size(95, 17);
            this.chbEnableLog.TabIndex = 14;
            this.chbEnableLog.Text = "Enable log file:";
            this.chbEnableLog.UseVisualStyleBackColor = true;
            // 
            // txtLogFile
            // 
            this.txtLogFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLogFile.Location = new System.Drawing.Point(110, 17);
            this.txtLogFile.Name = "txtLogFile";
            this.txtLogFile.Size = new System.Drawing.Size(155, 20);
            this.txtLogFile.TabIndex = 10;
            // 
            // tabOther
            // 
            this.tabOther.Controls.Add(this.gbCoding);
            this.tabOther.Location = new System.Drawing.Point(4, 22);
            this.tabOther.Name = "tabOther";
            this.tabOther.Size = new System.Drawing.Size(309, 205);
            this.tabOther.TabIndex = 3;
            this.tabOther.Text = "Other";
            this.tabOther.UseVisualStyleBackColor = true;
            // 
            // gbCoding
            // 
            this.gbCoding.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbCoding.Controls.Add(this.cbCoding);
            this.gbCoding.Controls.Add(this.lblCoding);
            this.gbCoding.Location = new System.Drawing.Point(3, 3);
            this.gbCoding.Name = "gbCoding";
            this.gbCoding.Size = new System.Drawing.Size(297, 53);
            this.gbCoding.TabIndex = 17;
            this.gbCoding.TabStop = false;
            this.gbCoding.Text = "Coding";
            // 
            // cbCoding
            // 
            this.cbCoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCoding.FormattingEnabled = true;
            this.cbCoding.Location = new System.Drawing.Point(86, 17);
            this.cbCoding.Name = "cbCoding";
            this.cbCoding.Size = new System.Drawing.Size(205, 21);
            this.cbCoding.TabIndex = 18;
            // 
            // lblCoding
            // 
            this.lblCoding.AutoSize = true;
            this.lblCoding.Location = new System.Drawing.Point(6, 20);
            this.lblCoding.Name = "lblCoding";
            this.lblCoding.Size = new System.Drawing.Size(43, 13);
            this.lblCoding.TabIndex = 17;
            this.lblCoding.Text = "Coding:";
            // 
            // chbEnableReplyWait
            // 
            this.chbEnableReplyWait.AutoSize = true;
            this.chbEnableReplyWait.Location = new System.Drawing.Point(9, 58);
            this.chbEnableReplyWait.Name = "chbEnableReplyWait";
            this.chbEnableReplyWait.Size = new System.Drawing.Size(219, 17);
            this.chbEnableReplyWait.TabIndex = 15;
            this.chbEnableReplyWait.Text = "Enable waiting for reply with timeout [ms]:";
            this.chbEnableReplyWait.UseVisualStyleBackColor = true;
            // 
            // txtReplyTimeout
            // 
            this.txtReplyTimeout.Location = new System.Drawing.Point(236, 55);
            this.txtReplyTimeout.Name = "txtReplyTimeout";
            this.txtReplyTimeout.Size = new System.Drawing.Size(55, 20);
            this.txtReplyTimeout.TabIndex = 16;
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(341, 337);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabSerial.ResumeLayout(false);
            this.tabSerial.PerformLayout();
            this.tabSending.ResumeLayout(false);
            this.gbReply.ResumeLayout(false);
            this.gbReply.PerformLayout();
            this.gbAutosending.ResumeLayout(false);
            this.gbAutosending.PerformLayout();
            this.gbSendingFromFile.ResumeLayout(false);
            this.gbSendingFromFile.PerformLayout();
            this.tabPaths.ResumeLayout(false);
            this.gbDataFiles.ResumeLayout(false);
            this.gbDataFiles.PerformLayout();
            this.gbLogFile.ResumeLayout(false);
            this.gbLogFile.PerformLayout();
            this.tabOther.ResumeLayout(false);
            this.gbCoding.ResumeLayout(false);
            this.gbCoding.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cbParity;
        private System.Windows.Forms.ComboBox cbDataBits;
        private System.Windows.Forms.CheckBox chbDTR;
        private System.Windows.Forms.CheckBox chbRTS;
        private System.Windows.Forms.Label lblParity;
        private System.Windows.Forms.Label lblDataBits;
        private System.Windows.Forms.Label lblStopBits;
        private System.Windows.Forms.ComboBox cbStopBits;
        private System.Windows.Forms.TextBox txtAutoSendDelay;
        private System.Windows.Forms.Label lblAutosendDelay;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabSerial;
        private System.Windows.Forms.TabPage tabSending;
        private System.Windows.Forms.TabPage tabPaths;
        private System.Windows.Forms.GroupBox gbAutosending;
        private System.Windows.Forms.GroupBox gbSendingFromFile;
        private System.Windows.Forms.CheckBox chbEoF;
        private System.Windows.Forms.Button btnSendingFile;
        private System.Windows.Forms.CheckBox chbEnableSendFromFile;
        private System.Windows.Forms.TextBox txtSendingFile;
        private System.Windows.Forms.GroupBox gbReply;
        private System.Windows.Forms.Button btnReplyFile;
        private System.Windows.Forms.CheckBox chbEnableReply;
        private System.Windows.Forms.TextBox txtReplyFile;
        private System.Windows.Forms.GroupBox gbLogFile;
        private System.Windows.Forms.Button btnLogFile;
        private System.Windows.Forms.CheckBox chbEnableLog;
        private System.Windows.Forms.TextBox txtLogFile;
        private System.Windows.Forms.CheckBox chbNewLog;
        private System.Windows.Forms.GroupBox gbDataFiles;
        private System.Windows.Forms.Label lblDataFolder;
        private System.Windows.Forms.Button btnDataFolder;
        private System.Windows.Forms.TextBox txtDataFolder;
        private System.Windows.Forms.TabPage tabOther;
        private System.Windows.Forms.GroupBox gbCoding;
        private System.Windows.Forms.ComboBox cbCoding;
        private System.Windows.Forms.Label lblCoding;
        private System.Windows.Forms.TextBox txtReplyTimeout;
        private System.Windows.Forms.CheckBox chbEnableReplyWait;
    }
}
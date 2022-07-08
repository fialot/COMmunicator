using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using Fx.IO.Protocol;
using GlobalClasses;

using System.Net.Sockets;
using System.Threading;
using Fx.Logging;
using AppSettings;
using Fx.IO;
using Fx.Conversion;
using System.Reflection;
using Fx.Plugins;

namespace COMunicator
{
    public partial class frmMain : Form
    {
        


        static frmSettings formSet;

        //string[] history;
        //int historyIndex;

        public delegate void MyDelegate(comStatus status);
        public delegate void NewLogDelegate(LogRecord record);

        //public Comm com;

        Sender conn = new Sender();


        public History history;
        public History historyIP;
        public History historyPort;
        public History historyPortServer;

        #region Connection Changes

        /// <summary>
        /// Incomming connection change
        /// </summary>
        /// <param name="state">State change</param>
        private void ConnChangedState(StateChange state)
        {
            Invoke(new Action(() =>
            {
                switch (state)
                {
                    case StateChange.Connected:
                        Connected();
                        break;
                    case StateChange.Disconnected:
                        Disconnected();
                        break;
                    case StateChange.ConnectionError:
                        Disconnected();
                        MessageBox.Show("Connection error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case StateChange.Started:
                        Started();
                        break;
                    case StateChange.Stopped:
                        Stopped();
                        break;
                    case StateChange.NewData:

                        break;
                }
            }));
        }

        /// <summary>
        /// Procedure if connected
        /// </summary>
        private void Connected()
        {
            statusImg.Image = Properties.Resources.circ_green24;

            switch (Settings.Connection.Type)
            {
                case ConnectionType.Serial:
                    btnSend.Enabled = true;
                    btnConnect.Enabled = true;
                    btnNetConn.Enabled = false;
                    btnNetSConn.Enabled = false;
                    btnConnect.Text = "Disconnect";
                    lblStatus.Text = "Connected to: " + Settings.Connection.SerialPort;
                    break;

                case ConnectionType.TCP:
                case ConnectionType.UDP:
                    btnSend.Enabled = true;
                    btnConnect.Enabled = false;
                    btnNetConn.Enabled = true;
                    btnNetSConn.Enabled = false;
                    btnNetConn.Text = "Disconnect";
                    lblStatus.Text = "Connected to: " + Settings.Connection.IP + ":" + Settings.Connection.Port.ToString();
                    historyIP.Add(cbIP.Text);
                    historyPort.Add(cbPort.Text);
                    RefreshHistNet();
                    Global.LogPacket.SaveHeader("Net " + Settings.Connection.IP + ":" + Settings.Connection.Port.ToString());
                    break;

                case ConnectionType.TCPServer:
                    btnSend.Enabled = true;
                    lblStatus.Text = "Connected client.";
                    break;
            }

            Global.LogPacket.SaveHeader(cbbCOMPorts.Text);
        }

        /// <summary>
        /// Procedure if disconnected
        /// </summary>
        private void Disconnected()
        {
            btnSend.Enabled = false;
            btnConnect.Enabled = true;
            btnNetConn.Enabled = true;
            btnNetSConn.Enabled = true;
            btnConnect.Text = "Connect";
            btnNetConn.Text = "Connect";
            if (Settings.Connection.Type == ConnectionType.TCPServer)
            {
                lblStatus.Text = "Created server on port: " + Settings.Connection.LocalPort.ToString();
            }
            else
            {
                lblStatus.Text = "Disconnected.";
                statusImg.Image = Properties.Resources.circ_red24;
            }
            
            
        }

        private void Started()
        {
            statusImg.Image = Properties.Resources.circ_green24;

            switch (Settings.Connection.Type)
            {
                case ConnectionType.TCPServer:
                    btnConnect.Enabled = false;
                    btnNetConn.Enabled = false;
                    btnNetSConn.Enabled = true;
                    btnNetSConn.Text = "Stop";
                    lblStatus.Text = "Created server on port: " + Settings.Connection.LocalPort.ToString();
                    historyPortServer.Add(cbSPort.Text);
                    RefreshHistNet();
                    Global.LogPacket.SaveHeader("TCP Server port:" + Settings.Connection.LocalPort.ToString());
                    break;
            }
        }

        private void Stopped()
        {
            statusImg.Image = Properties.Resources.circ_red24;

            switch (Settings.Connection.Type)
            {
                case ConnectionType.TCPServer:
                    btnConnect.Enabled = true;
                    btnNetConn.Enabled = true;
                    btnNetSConn.Enabled = true;
                    btnNetSConn.Text = "Start";
                    lblStatus.Text = "Disconnected.";
                    break;
            }
        }

        #endregion

        public void ShowLog(LogRecord record)
        {
            try
            {
                if (tabsMessages.SelectedIndex == 0)
                {
                    //olvPacket.SetObjects(Global.LogPacket.Recods);
                    olvPacket.BeginUpdate();
                    olvPacket.AddObject(record);
                    olvPacket.EndUpdate();

                    if (btnAutoScroll.Checked)
                    {
                        if (olvPacket.GetItemCount() > 0)
                        {
                            olvPacket.EnsureVisible(olvPacket.GetItemCount() - 1);
                        }
                    }
                    
                }
                else if (tabsMessages.SelectedIndex == 1)
                {
                    // ----- Add to rtf text -----
                    txtLog.Select(txtLog.TextLength, 0);
                    txtLog.SelectionColor = record.color;
                    string appText = record.text + Environment.NewLine;
                    txtLog.AppendText(appText);

                    if (txtLog.Text.Length > 10000)
                    {
                        txtLog.Select(0, appText.Length);
                        txtLog.SelectedText = " ";
                    }

                    /*if (txtLog.Lines.Length > 100)
                    {
                        int index = txtLog.GetFirstCharIndexFromLine(txtLog.Lines.Length - 100);
                        txtLog.Select(0, index);
                        txtLog.SelectedText = " ";
                    }*/

                    if (btnAutoScroll.Checked)
                    {
                        txtLog.SelectionStart = txtLog.Text.Length;
                        txtLog.ScrollToCaret();
                    }

                    /*new System.Threading.Thread(() =>
                {
                    RichTextBox rtb = new RichTextBox();
                    rtb.Rtf = txtLog.Rtf;
                    rtb.Select(rtb.TextLength, 0);
                    rtb.SelectionColor = record.color;
                    rtb.AppendText(record.text + Environment.NewLine);

                    Invoke((Action)(() =>
                    {
                        txtLog.Rtf = rtb.Rtf;
                        txtLog.SelectionStart = txtLog.Text.Length;
                        txtLog.ScrollToCaret();
                    }));
                }).Start();*/
                }
                else if (tabsMessages.SelectedIndex == 2)
                {
                    var elapsed = DateTime.Now - conn.Statistic.StartTime;
                    string stat = "Elapsed time: " + elapsed.Days.ToString() + "d " + elapsed.Hours.ToString() + ":" + elapsed.Minutes.ToString("00") + ":" + elapsed.Seconds.ToString("00") + Environment.NewLine;
                    stat += "Sended messages: " + conn.Statistic.RequestCounter.ToString() + Environment.NewLine;
                    stat += "Incomming messages: " + conn.Statistic.ReplyCounter.All.ToString() + Environment.NewLine;
                    stat += "   - Over 100ms: " + conn.Statistic.ReplyCounter.Over100ms.ToString() + Environment.NewLine;
                    stat += "   - Over 250ms: " + conn.Statistic.ReplyCounter.Over250ms.ToString() + Environment.NewLine;
                    stat += "   - Over 500ms: " + conn.Statistic.ReplyCounter.Over500ms.ToString() + Environment.NewLine;
                    stat += "   - Over 1s: " + conn.Statistic.ReplyCounter.Over1s.ToString() + Environment.NewLine;
                    stat += "   - Over 2s: " + conn.Statistic.ReplyCounter.Over2s.ToString() + Environment.NewLine;
                    stat += "Incomming time outs: " + conn.Statistic.ReplyCounter.TimeOut.ToString() + Environment.NewLine;

                    txtStatistic.Text = stat;
                }
            }
            catch { }
            
        }
        
        #region Form

        public frmMain()
        {
            InitializeComponent();
        }

        /*private void DataReceive(object source, comStatus status)
        {
            olvPacket.Invoke(new MyDelegate(updateLog), new Object[] { status }); //BeginInvoke
            
        }*/

        private void NewLogRecord(LogRecord record)
        {
            try
            {
                olvPacket.Invoke(new NewLogDelegate(ShowLog), new Object[] { record }); //BeginInvoke
            }
            catch { }
        }

        #region LoadForm

        private void frmMain_Load(object sender, EventArgs e)
        {

            conn.ChangedState += new ChangedStateEventHandler(ConnChangedState);

            // ----- GET APPLICATION VERSION -> TO CAPTION -----
            this.Text = this.Text + " v" + Application.ProductVersion.Substring(0,Application.ProductVersion.Length-2);

            // ----- LOAD SETTINGS -----
            Settings.LoadXml();

            // ----- Load plug-ins -----
            Global.PL.LoadPlugins();
            ProtocolFormat.SetPlugins(Global.PL.PluginsProtocol);
            LoadPlugins();

            // ----- Creating data folder -----
            if (!Directory.Exists(Paths.GetFullPath(Settings.App.DataFolder))) Directory.CreateDirectory(Paths.GetFullPath(Settings.App.DataFolder));

            Global.LogPacket.LogFileDirectory = Paths.GetFullPath(Settings.Messages.LogFileDirectory);
            Global.LogPacket.SaveToFile = Settings.Messages.SaveToFile;
;
            // ----- APPLY SETTINGS TO FORM -----
            ApplySettingsToForm();

            // ----- READ SENDING HISTORY -----
            ReadHistory();

            // ----- LOAD COMMAND MENU -----
            LoadMenu();

            
            
            



            formSet = new frmSettings();

            olvPacket.UpdateObjects(Global.LogPacket.Recods); //SetObjects

            colTime.AspectGetter = delegate (object x) {
                if (x == null) return "";
                return ((LogRecord)x).time.ToString("hh:mm:ss.fff");
            };
            colDelay.AspectGetter = delegate (object x) {
                if (x == null) return "";
                return ((LogRecord)x).delay.TotalSeconds.ToString("0.000");
            };
            colLength.AspectGetter = delegate (object x) {
                if (x == null) return ""; return ((LogRecord)x).data.Length;
            };
            colIO.AspectGetter = delegate (object x) {
                if (x == null) return "";
                if (((LogRecord)x).input)
                    return ">>";
                else
                    return "<<";
            };
            colPacket.AspectGetter = delegate (object x) {
                if (x == null) return "";
                return ((LogRecord)x).dataText;
            };

        }

        

        void ReadHistory()
        {
            history = new History(Settings.App.DataFolder + Path.DirectorySeparatorChar + "history_cmd.txt");
            tbSend.Text = history.Get();
            historyIP = new History(Settings.App.DataFolder + Path.DirectorySeparatorChar + "history_IP.txt", 5);
            historyPort = new History(Settings.App.DataFolder + Path.DirectorySeparatorChar + "history_port.txt", 5);
            historyPortServer = new History(Settings.App.DataFolder + Path.DirectorySeparatorChar + "history_port_server.txt", 5);
            RefreshHistNet();
            
        }

        void RefreshHistNet()
        {
            string[] items = historyIP.GetList();
            cbIP.Items.Clear();
            for (int i = 1; i < items.Length; i++) cbIP.Items.Add(items[i]);
            items = historyPort.GetList();
            cbPort.Items.Clear();
            for (int i = 1; i < items.Length; i++) cbPort.Items.Add(items[i]);
            items = historyPortServer.GetList();
            cbSPort.Items.Clear();
            for (int i = 1; i < items.Length; i++) cbSPort.Items.Add(items[i]);
        }

        void RefreshCOMPorts()
        {
            string COM = cbbCOMPorts.Text;
            cbbCOMPorts.Items.Clear();
            string[] ports = SerialPort.GetPortNames();
            for (int i = 0; i < ports.Length; i++)
            {
                this.cbbCOMPorts.Items.Add(ports[i]);
            }

            cbbCOMPorts.Text = COM;
        }

        void ApplySettingsToForm()
        {
            // ----- Read COM port list -----
            RefreshCOMPorts();
            cbbCOMPorts.Text = Settings.Connection.SerialPort;
            if ((cbbCOMPorts.Items.Count > 0) & cbbCOMPorts.Text == "")
            {
                cbbCOMPorts.Text = cbbCOMPorts.Items[0].ToString();
            }

            cbBaud.Text = Settings.Connection.BaudRate.ToString();
            cbIP.Text = Settings.Connection.IP;
            cbPort.Text = Settings.Connection.Port.ToString();
            txtLocalPort.Text = Settings.Connection.LocalPort.ToString();
            cbSPort.Text = Settings.Connection.LocalPort.ToString();
            cbProtocol.SelectedIndex = 0;
            if (Settings.Connection.Type == ConnectionType.UDP) cbProtocol.SelectedIndex = 1;

            if (Settings.GUI.TabIndex >= tabControl1.TabPages.Count) Settings.GUI.TabIndex = 0;
            tabControl1.SelectedIndex = Settings.GUI.TabIndex;

            switch (Settings.Messages.PacketView)
            {
                case ePacketView.Bytes:
                    Global.LogPacket.SetPacketView(ePacketView.Bytes);
                    chkByte.Checked = true;
                    break;
                case ePacketView.Hex:
                    Global.LogPacket.SetPacketView(ePacketView.Hex);
                    chkHex.Checked = true;
                    break;
                case ePacketView.String:
                case ePacketView.StringReplaceCommandChars:
                    Global.LogPacket.SetPacketView(ePacketView.StringReplaceCommandChars);
                    chkString.Checked = true;
                    break;
                case ePacketView.Custom:
                    foreach(var item in mnuShowType.DropDownItems)
                    {
                        if (item.GetType() == typeof(ToolStripMenuItem))
                        {
                            if (((ToolStripMenuItem)item).Text == Settings.Messages.PacketViewPlugin)
                            {
                                Global.LogPacket.SetCustomView((IPluginProtocol)Global.PL.GetPlugin(Settings.Messages.PacketViewPlugin));
                                Global.LogPacket.SetPacketView(ePacketView.Custom);
                                ((ToolStripMenuItem)item).Checked = true;
                            }
                        }
                    }
                    break;
            }
            
            Global.LogPacket.NewRecord += new NewRecordEventHandler(NewLogRecord);

            chkLine.Checked = Settings.Messages.UseLineSeparatingChar;
            mnutxtLine.Text = Settings.Messages.LineSeparatingChar;

            chkTime.Checked = Settings.Messages.ShowTime;
            chkBaudRate.Checked = Settings.Messages.ShowBaudRate;

            btnNoClear.Checked = Settings.Messages.ClearEditbox;
            btnAutoScroll.Checked = Settings.GUI.AutoScroll;
            btnEnableAutoReply.Checked = Settings.Messages.EnableReplyFile;
            btnEnableAutoSending.Checked = Settings.Messages.EnableAutoSending;
            btnUseEndChar.Checked = Settings.Messages.AddEndChar;
            txtEndCMD.Text = Settings.Messages.EndChar;
        }

        #endregion

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Connection.SerialPort = cbbCOMPorts.Text;
            Settings.Connection.BaudRate = Conv.ToInt(cbBaud.Text, 115200);

            Settings.Connection.IP = cbIP.Text;
            Settings.Connection.Port = Conv.ToInt(cbPort.Text, 17000);
            if (Settings.Connection.Type == ConnectionType.UDP)
                Settings.Connection.LocalPort = Conv.ToInt(txtLocalPort.Text, 17001);
            else
                Settings.Connection.LocalPort = Conv.ToInt(cbSPort.Text,17000);
            if (cbProtocol.SelectedIndex == 1) Settings.Connection.Type = ConnectionType.UDP;
            else Settings.Connection.Type = ConnectionType.Serial;

            Settings.GUI.TabIndex = tabControl1.SelectedIndex;

            //settings.SP.bits = SP.DataBits;
            //settings.SP.parity = SP.Parity.ToString();

            //settings.SP.stopbits = SP.StopBits.ToString();
            //settings.SP.DTR = SP.DtrEnable;
            //settings.SP.RTS = SP.RtsEnable;


            if (chkByte.Checked)
                Settings.Messages.PacketView = ePacketView.Bytes;
            else if (chkHex.Checked)
                Settings.Messages.PacketView = ePacketView.Hex;
            else if (chkString.Checked)
                Settings.Messages.PacketView = ePacketView.StringReplaceCommandChars;
            else
            {
                Settings.Messages.PacketView = ePacketView.Custom;

                foreach (var item in mnuShowType.DropDownItems)
                {
                    if (item.GetType() == typeof(ToolStripMenuItem))
                    {
                        if (((ToolStripMenuItem)item).Checked)
                            Settings.Messages.PacketViewPlugin = ((ToolStripMenuItem)item).Text;
                    }
                }
            }

            Settings.Messages.UseLineSeparatingChar = chkLine.Checked;
            Settings.Messages.LineSeparatingChar = mnutxtLine.Text;


            Settings.Messages.ShowTime = chkTime.Checked;
            Settings.Messages.ShowBaudRate = chkBaudRate.Checked;



            Settings.Messages.ClearEditbox = btnNoClear.Checked;
            Settings.GUI.AutoScroll = btnAutoScroll.Checked;
            Settings.Messages.EnableReplyFile = btnEnableAutoReply.Checked;
            Settings.Messages.EnableAutoSending = btnEnableAutoSending.Checked;
            Settings.Messages.AddEndChar = btnUseEndChar.Checked;
            Settings.Messages.EndChar = txtEndCMD.Text;

            Settings.SaveXml();

            // ----- SAVE COMMANDS HISTORY -----
            history.SetTemporary(tbSend.Text);
            history.Save();
            historyIP.Save();
            historyPort.Save();
            historyPortServer.Save();

            conn.Disconnect();
            conn = null;

            //if (btnConnect.Tag == "1") btnConnect_Click(sender, e);

        }

        private void LoadPlugins()
        {
            foreach (var plugin in Global.PL.PluginsProtocol)
            {
                var item = new ToolStripMenuItem(plugin.GetName(), null, mnuProtocolPlugin_Click);
                mnuShowType.DropDownItems.Add(item);
            }

        }

        private void mnuProtocolPlugin_Click(object sender, EventArgs e)
        {
            foreach (var item in mnuShowType.DropDownItems)
            {
                if (item.GetType() == typeof(ToolStripMenuItem))
                {
                    ((ToolStripMenuItem)item).Checked = false;
                }
            }

            ((ToolStripMenuItem)sender).Checked = true;

            RefreshMessages(((ToolStripMenuItem)sender).Text);
        }

        private void LoadMenu()
        {
            // ----- MENU -> ADD ITEM -----
            CntSend.Items.Clear();
            CntSend.Items.Add("Add...");
            CntSend.Items[0].Click += new EventHandler(mnuAddSend_Click);
            CntSend.Items.Add("Edit");
            CntSend.Items[1].Click += new EventHandler(mnuEditSend_Click);
            CntSend.Items.Add("-");

            // ----- MENU -> OTHER ITEMS -----
            

            // ----- Add default items -----
            

            string[] items = Files.ReadLines(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar + "data" + Path.DirectorySeparatorChar + "menu_items.txt", true);
            AddMenuItems(items);

            // ----- Add User defined items -----
            items = Files.ReadLines(Paths.GetFullPath(Settings.App.DataFolder + Path.DirectorySeparatorChar + "menu_items.txt"), true);
            AddMenuItems(items);


        }

        

        void AddMenuItems(string[] items)
        {
            string[] CntItem;

            for (int i = 0; i < items.Length; i++)
            {
                try
                {
                    CntItem = items[i].Split(new string[] { @"\:" }, StringSplitOptions.RemoveEmptyEntries);
                    if (CntItem.Length > 1)
                    {
                        if (CntItem.Length > 2)
                        {
                            int index = -1;
                            for (int j = 3; j < CntSend.Items.Count; j++)
                            {
                                if (CntSend.Items[j].Text == CntItem[0])
                                {
                                    index = j;
                                    break;
                                }
                            }
                            if (index < 0)
                            {
                                CntSend.Items.Add(CntItem[0]);
                                index = CntSend.Items.Count - 1;
                            }

                            if (CntItem.Length > 3)
                            {
                                int index2 = -1;
                                for (int j = 0; j < ((ToolStripMenuItem)CntSend.Items[index]).DropDownItems.Count; j++)
                                {
                                    if (((ToolStripMenuItem)CntSend.Items[index]).DropDownItems[j].Text == CntItem[1])
                                    {
                                        index2 = j;
                                        break;
                                    }
                                }
                                if (index2 < 0)
                                {
                                    ((ToolStripMenuItem)CntSend.Items[index]).DropDownItems.Add(CntItem[1]);
                                    index2 = ((ToolStripMenuItem)CntSend.Items[index]).DropDownItems.Count - 1;
                                }

                                ToolStripMenuItem dpItem = ((ToolStripMenuItem)((ToolStripMenuItem)CntSend.Items[index]).DropDownItems[index2]);

                                dpItem.DropDownItems.Add(CntItem[2]);
                                dpItem.DropDownItems[dpItem.DropDownItems.Count - 1].Tag = CntItem[CntItem.Length - 1];
                                dpItem.DropDownItems[dpItem.DropDownItems.Count - 1].Click += new EventHandler(CntSend_Click);
                            }
                            else
                            {
                                ToolStripMenuItem dpItem = ((ToolStripMenuItem)CntSend.Items[index]);
                                dpItem.DropDownItems.Add(CntItem[1]);
                                dpItem.DropDownItems[dpItem.DropDownItems.Count - 1].Tag = CntItem[CntItem.Length - 1];
                                dpItem.DropDownItems[dpItem.DropDownItems.Count - 1].Click += new EventHandler(CntSend_Click);
                            }
                        }
                        else
                        {
                            CntSend.Items.Add(CntItem[0]);
                            CntSend.Items[CntSend.Items.Count - 1].Tag = CntItem[CntItem.Length - 1];
                            CntSend.Items[CntSend.Items.Count - 1].Click += new EventHandler(CntSend_Click);
                            //CntSend.Items[i + 2].Tag = CntItem[1];
                            //CntSend.Items[i + 2].Click += new EventHandler(CntSend_Click);
                        }

                    }

                }
                catch { }
            }
        }

        #endregion

        #region Buttons


        private void btnConnect_Click(object sender, EventArgs e)
        {

            if (!conn.Status.IsConnected)
            {
                btnConnect.Enabled = false;
                btnNetConn.Enabled = false;
                btnNetSConn.Enabled = false;
                Settings.Connection.Type = ConnectionType.Serial;
                Settings.Connection.SerialPort = cbbCOMPorts.Text;
                Settings.Connection.BaudRate = Conv.ToInt(cbBaud.Text, 115200);

                conn.Connect(Settings.Connection);
            }
            else
            {
                conn.Disconnect();
            }
        }


        private void btnBaudRate_Click(object sender, EventArgs e)
        {
            conn.SetBaudRate(Conv.ToInt(cbBaud.Text, 115200));
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string text = tbSend.Text;
            if (btnUseEndChar.Checked) text += txtEndCMD.Text;
            if (conn.Status.IsConnected && (tbSend.Text != ""))
            {
                conn.Send(text);
                history.Add(tbSend.Text);
                if (btnNoClear.Checked == false) { tbSend.Text = ""; };
                history.SetTemporary(tbSend.Text);
            }
            tbSend.Focus();
        }


        #endregion

        #region Communication


        #endregion

        #region Files

        private string[] LoadFile(string filename, bool remove = true)
        {
            string str = "";

            try
            {
                StreamReader objReader = new StreamReader(filename);
                str = objReader.ReadToEnd();
                objReader.Close();
                objReader.Dispose();
            }
            catch
            {

            }

            if (str.Length > 0)
                if (remove)
                    return str.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                else
                    return str.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
            else return new string[0];
        }

        private void AddToFile(string filename, string line)
        {
            try
            {
                StreamWriter objWriter = new StreamWriter(filename,true); // append
                objWriter.Write("\n" + line);
                objWriter.Close();
                objWriter.Dispose();
            }
            catch
            {

            }
        }

        private void WriteToFile(string filename, string[] lines)
        {
            string folder = Path.GetDirectoryName(filename);
            bool existDir = Directory.Exists(folder);
            if (!existDir) Directory.CreateDirectory(folder);
            try
            {
                StreamWriter objWriter = new StreamWriter(filename); // append
                for (int i = 0; i < lines.Length; i++)
                {
                    objWriter.Write(lines[i] + "\n");
                }
                objWriter.Close();
                objWriter.Dispose();
            }
            catch
            {

            }
        }

        #endregion


        void CntSend_Click(object sender, EventArgs e)
        {
            if ((string)((ToolStripMenuItem)sender).Tag != "")
            {
                tbSend.Text = (string)((ToolStripMenuItem)sender).Tag;
            }
        }

        

        private void CntText_Click(object sender, EventArgs e)
        {
            if (olvPacket.SelectedIndex >= 0)
            {
                tbSend.Text = "\\x" + BitConverter.ToString(((LogRecord)olvPacket.SelectedItem.RowObject).data).Replace("-", "");
            }
        }

        private void CntCopy_Click(object sender, EventArgs e)
        {
            if (olvPacket.SelectedIndex >= 0)
            {
                System.Windows.Forms.Clipboard.SetText(((LogRecord)olvPacket.SelectedItem.RowObject).dataText);
            }
        }

        private void CntSaveAs_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Log File *.log|*.log|All Files (*.*)|*.*";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Files.Save(dialog.FileName, Global.LogPacket.Text());
                }
            }
            catch (Exception err)
            {
                Dialogs.ShowError(err.Message, "Error");
            }
        }

        private void TimeOut_Tick(object sender, EventArgs e)
        {
            /*TimeOut.Enabled = false;

            string dataS = ShowData(comBuffer, true);

            if (chbAutoReply.Checked)
            {
                Random rnd = new Random();

                if (dataS == "v") Send("2.1");
                else if (dataS == "s") Send(@"\i" + rnd.Next(0, 100).ToString());
                else if (dataS == "r") Send(@"\i1");
                else if (dataS == "z") Send(@"\i" + rnd.Next(0, 100).ToString());

                string key = byteToFormat(comBuffer);
                if (ReplyData.ContainsKey(key))
                {
                    string value = ReplyData[key];
                    Send(value);
                }
            }*/


            //lbLog.SelectedIndex = lbLog.Items.Count - 1;
            //lbLog.Invoke(new MyDelegate(updateLog), new Object[] { });
            //txt = "";
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Global.Log.ClearLog();
            Global.LogPacket.ClearLog();
            olvPacket.SetObjects(Global.LogPacket.Recods);
            txtLog.Text = "";
            conn.Clear();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            ChangeSettings(0);
        }

        private void ChangeSettings(int tab) 
        {
            Settings.Messages.EnableReplyFile = btnEnableAutoReply.Checked;
            Settings.Messages.EnableSendingFile = btnSendFromFile.Checked;
            formSet.ShowDialog(tab);

            btnEnableAutoReply.Checked = Settings.Messages.EnableReplyFile;
            btnSendFromFile.Checked = Settings.Messages.EnableSendingFile;

            conn.RefreshAutoSend();
        }

        private Parity ToParity(string name)
        {
            switch (name)
            {
                case "Even":
                    return Parity.Even;
                case "Mark":
                    return Parity.Mark;
                case "None":
                    return Parity.None;
                case "Odd":
                    return Parity.Odd;
                case "Space":
                    return Parity.Space;
                default:
                    return Parity.None;
            }
        }

        private StopBits ToStopBits(string name)
        {
            switch (name)
            {
                case "None":
                    return StopBits.None;
                case "One":
                    return StopBits.One;
                case "OnePointFive":
                    return StopBits.OnePointFive;
                case "Two":
                    return StopBits.Two;
                default:
                    return StopBits.One;
            }
        }

        private void mnuAddSend_Click(object sender, EventArgs e)
        {
            string txt = "New";
            
            if (Dialogs.InputBox("Add packet to list", "Name of packet:", ref txt) == DialogResult.OK)
            {
                Files.Save(Paths.GetFullPath(Settings.App.DataFolder + Path.DirectorySeparatorChar + "menu_items.txt"), "\n" + txt + @"\:" + tbSend.Text, true);
                //AddToFile("polozky.txt", txt + @"\:" + tbSend.Text);
                LoadMenu();
            }
        }


        

        private void tbSend_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Up))
            {

                string text = history.GetPrevious(tbSend.Text);
                if (text != null) tbSend.Text = text;
            }
            else if (e.KeyCode.Equals(Keys.Down))
            {
                tbSend.Text = history.GetNext();
            }
            else if (e.KeyCode.Equals(Keys.Enter))
            {
                btnSend_Click(sender, null);
            }

        }


        private void btnNetConn_Click(object sender, EventArgs e)
        {
            if (!conn.Status.IsConnected)
            {
                try
                {
                    btnConnect.Enabled = false;
                    btnNetConn.Enabled = false;
                    btnNetSConn.Enabled = false;
                    if (cbProtocol.SelectedIndex == 1)
                        Settings.Connection.Type = ConnectionType.UDP;
                    else
                        Settings.Connection.Type = ConnectionType.TCP;
                    Settings.Connection.IP = cbIP.Text;
                    Settings.Connection.Port = Convert.ToInt32(cbPort.Text);
                    Settings.Connection.LocalPort = Convert.ToInt32(txtLocalPort.Text);

                    conn.Connect(Settings.Connection);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                conn.Disconnect();
            }
        }


        private void btnSPRefresh_Click(object sender, EventArgs e)
        {
            RefreshCOMPorts();
        }

        private void btnNetSConn_Click(object sender, EventArgs e)
        {
            if (!conn.Status.IsConnected)
            {
                try
                {
                    btnConnect.Enabled = false;
                    btnNetConn.Enabled = false;
                    btnNetSConn.Enabled = false;
                    Settings.Connection.Type = ConnectionType.TCPServer;
                    Settings.Connection.LocalPort = Convert.ToInt32(cbSPort.Text);

                    conn.Connect(Settings.Connection);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                conn.Disconnect();
            }
        }


        private void cbProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == 1)
            {
                lblLocalPort.Visible = true;
                txtLocalPort.Visible = true;
            }
            else
            {
                lblLocalPort.Visible = false;
                txtLocalPort.Visible = false;   
            }

        }


        private void btnSettings2_Click(object sender, EventArgs e)
        {
            ChangeSettings(1);
        }


        private void mnuEditSend_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Paths.GetFullPath(Settings.App.DataFolder + Path.DirectorySeparatorChar + "menu_items.txt"));
        }

        private void chkTime_Click(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked;
        }

        private void olvPacket_Resize(object sender, EventArgs e)
        {
            colPacket.Width = olvPacket.Width - colTime.Width - colLength.Width;
        }

        private void olvPacket_FormatRow(object sender, BrightIdeasSoftware.FormatRowEventArgs e)
        {
            if (e.Item.RowObject == null) return;

            if (((LogRecord)e.Item.RowObject).input)
            {
                e.Item.ForeColor = Color.Blue;
            }
            else
            {
                e.Item.ForeColor = Color.Black;
            }
        }

        private void tabsMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshMessages();
        }

        private void RefreshMessages(string viewName = "")
        {
            if (chkString.Checked)
            {
                Global.LogPacket.SetPacketView(ePacketView.StringReplaceCommandChars);
            }
            else if (chkByte.Checked)
            {
                Global.LogPacket.SetPacketView(ePacketView.Bytes);
            }
            else if (chkHex.Checked)
            {
                Global.LogPacket.SetPacketView(ePacketView.Hex);
            }
            else if (viewName != "")
            {
                Global.LogPacket.SetCustomView((IPluginProtocol)Global.PL.GetPlugin(viewName));
                Global.LogPacket.SetPacketView(ePacketView.Custom);
            }

            /*txtPackets.Rtf = Global.LogPacket.TextRTF();
            txtPackets.SelectionStart = txtPackets.Text.Length;
            txtPackets.ScrollToCaret();*/

            olvPacket.SetObjects(Global.LogPacket.Recods);
            txtLog.Rtf = Global.LogPacket.TextRTF();

            var elapsed = DateTime.Now - conn.Statistic.StartTime;
            string stat = "Elapsed time: " + elapsed.Days.ToString() + "d " + elapsed.Hours.ToString() + ":" + elapsed.Minutes.ToString("00") + ":" + elapsed.Seconds.ToString("00") + Environment.NewLine;
            stat += "Sended messages: " + conn.Statistic.RequestCounter.ToString() + Environment.NewLine;
            stat += "Incomming messages: " + conn.Statistic.ReplyCounter.All.ToString() + Environment.NewLine;
            stat += "   - Over 100ms: " + conn.Statistic.ReplyCounter.Over100ms.ToString() + Environment.NewLine;
            stat += "   - Over 250ms: " + conn.Statistic.ReplyCounter.Over250ms.ToString() + Environment.NewLine;
            stat += "   - Over 500ms: " + conn.Statistic.ReplyCounter.Over500ms.ToString() + Environment.NewLine;
            stat += "   - Over 1s: " + conn.Statistic.ReplyCounter.Over1s.ToString() + Environment.NewLine;
            stat += "   - Over 2s: " + conn.Statistic.ReplyCounter.Over2s.ToString() + Environment.NewLine;
            stat += "Incomming time outs: " + conn.Statistic.ReplyCounter.TimeOut.ToString() + Environment.NewLine;

            txtStatistic.Text = stat;
        }

        private void btnBoolan_Click(object sender, EventArgs e)
        {
            ((ToolStripButton)sender).Checked = !((ToolStripButton)sender).Checked;
        }

        private void btnEnableAutoSending_Click(object sender, EventArgs e)
        {
            btnEnableAutoSending.Checked = !btnEnableAutoSending.Checked;
            Settings.Messages.EnableAutoSending = btnEnableAutoSending.Checked;
            if (btnEnableAutoSending.Checked)
                conn.Send(tbSend.Text);
            conn.RefreshAutoSend();
        }

        private void btnSendFromFile_Click(object sender, EventArgs e)
        {
            btnSendFromFile.Checked = !btnSendFromFile.Checked;
            Settings.Messages.EnableSendingFile = btnSendFromFile.Checked;
            conn.RefreshAutoSend();
        }

        private void btnEnableAutoReply_Click(object sender, EventArgs e)
        {
            btnEnableAutoReply.Checked = !btnEnableAutoReply.Checked;
            Settings.Messages.EnableReplyFile = btnEnableAutoReply.Checked;
            conn.RefreshReply();
        }
    }

}

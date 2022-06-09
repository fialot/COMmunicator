﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using TCPClient;
using myFunctions;
using COMunicator.Protocol;
using GlobalClasses;

using System.Net.Sockets;
using System.Threading;
using Fx.Connection;
using Fx.Logging;
using AppSettings;
using Fx.IO;

namespace COMunicator
{
    public partial class frmMain : Form
    {
        // ----- Reply data -----
        static Dictionary<string, string> ReplyData = new Dictionary<string, string>();

        // ----- Auto send from file data ---
        string[] AutoSendData;
        int AutoSendDataIndex;
        bool ReplyCome = true;


        static frmSettings formSet;

        byte[] PrevData;

        
        byte[] comBuffer;

        //string[] history;
        //int historyIndex;

        public delegate void MyDelegate(comStatus status);
        public delegate void NewLogDelegate(LogRecord record);

        public Comm com;
        public History history;
        public History historyIP;
        public History historyPort;
        public History historyPortServer;

        public void ShowLog(LogRecord record)
        {
            olvPacket.SetObjects(Global.LogPacket.Recods);

            if (olvPacket.GetItemCount() > 0)
            {
                olvPacket.EnsureVisible(olvPacket.GetItemCount() - 1);
            }

            // ----- Add to rtf text -----
            txtLog.Select(txtLog.TextLength, 0);
            txtLog.SelectionColor = record.color;
            txtLog.AppendText(record.text + Environment.NewLine);

            if (txtLog.Lines.Length > 100)
            {
                int index = txtLog.GetFirstCharIndexFromLine(txtLog.Lines.Length - 100);
                txtLog.Select(0, index);
                txtLog.SelectedText = " ";
            }

            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }

        public void updateLog(comStatus status)
        {
            if (comBuffer == null) comBuffer = new byte[0];
            if (status == comStatus.Close)
            {
                if (com.OpenInterface == Comm.interfaces.TCPClient || com.OpenInterface == Comm.interfaces.None)
                    btnNetConn_Click(new object(), new EventArgs());
                else if (com.OpenInterface == Comm.interfaces.TCPServer) 
                {
                    lblStatus.Text = "Server: Client disconnected";
                    //Global.Log.Add("Client disconnected.");
                } else if (com.OpenInterface == Comm.interfaces.COM)
                {
                    btnConnect_Click(new object(), new EventArgs());
                }

            }
            else if (status == comStatus.OK)
            {

                TimeOut.Enabled = false;
                TimeOut.Enabled = true;

                byte[] newBytes = com.Read();
                if (newBytes.Length > 0)
                {
                    PrevData = com.AddArray(PrevData, newBytes);
                    comBuffer = PrevData;
                }
            }
            else if (status == comStatus.Open)
            {
                if (com.OpenInterface == Comm.interfaces.TCPClient)
                    NetConnected();
                else if (com.OpenInterface == Comm.interfaces.TCPServer)
                {
                    lblStatus.Text = "Server: Client connected";
                    //Global.Log.Add("Client connected.");
                }
                
            }
            else if (status == comStatus.OpenError)
            {
                Dialogs.ShowErr("Connection Timeout","Error");
            }
        }

        #region Form

        public frmMain()
        {
            InitializeComponent();
        }

        private void DataReceive(object source, comStatus status)
        {
            olvPacket.Invoke(new MyDelegate(updateLog), new Object[] { status }); //BeginInvoke
            
        }

        private void NewLogRecord(LogRecord record)
        {
            olvPacket.Invoke(new NewLogDelegate(ShowLog), new Object[] { record }); //BeginInvoke

        }

        #region LoadForm

        private void frmMain_Load(object sender, EventArgs e)
        {
            // ----- GET APPLICATION VERSION -> TO CAPTION -----
            this.Text = this.Text + " v" + Application.ProductVersion.Substring(0,Application.ProductVersion.Length-2);

            // ----- LOAD SETTINGS -----
            Settings.LoadXml();
            settings.LoadSettings();


            // ----- Creating data folder -----
            if (!Directory.Exists(Files.ReplaceVarPaths(settings.Paths.dataFolder))) Directory.CreateDirectory(Files.ReplaceVarPaths(settings.Paths.dataFolder));

            Global.LogPacket.LogFileDirectory = Files.ReplaceVarPaths(Settings.Messages.LogFileDirectory);
            Global.LogPacket.SaveToFile = Settings.Messages.SaveToFile;
;
            Encoding enc = Settings.Messages.UsedEncoding;

            // ----- CREATE NEW COMMUNICATION INSTANCE -----
            com = new Comm(enc);
            com.SetParamsSP(Settings.Connection.Parity, Settings.Connection.DataBits, Settings.Connection.StopBits, Settings.Connection.DTR, Settings.Connection.RTS);
            com.ReceivedData += new ReceivedEventHandler(DataReceive);


            // ----- APPLY SETTINGS TO FORM -----
            ApplySettingsToForm();

            // ----- READ SENDING HISTORY -----
            ReadHistory();

            // ----- LOAD COMMAND MENU -----
            LoadMenu();

            // ----- READ REPLY FILE -----
            ReplyData = LoadReplyFile(Files.ReplaceVarPaths(Settings.Messages.ReplyFile));

            // ----- READ AUTO SEND FILE -----
            LoadAutoSendData();

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
            else if (chkMarsA.Checked)
            {
                Global.LogPacket.SetPacketView(ePacketView.MARS_A);
            }
            Global.LogPacket.NewRecord += new NewRecordEventHandler(NewLogRecord);



            formSet = new frmSettings();

            PrevData = new byte[0];

            AutoSendDataIndex = -1;

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
            colPacket.AspectGetter = delegate (object x) {
                if (x == null) return "";
                return ((LogRecord)x).text;
            };

        }

        void LoadAutoSendData()
        {
            AutoSendData = Files.LoadFileLines(Files.ReplaceVarPaths(Settings.Messages.SendingFile), true);
            AutoSendDataIndex = 0;
        }

        Dictionary<string,string> LoadReplyFile(string fileName)
        {
            Dictionary<string, string> dict = new Dictionary<string,string>();
            string[] lines = Files.LoadFileLines(fileName, true);
            string[] item;
            for (int i = 0; i < lines.Length; i++)
            {
                item = lines[i].Split(new string[] { @"\->" }, StringSplitOptions.RemoveEmptyEntries);
                if (item.Length == 2)
                    dict.Add(byteToFormat(com.FormatMsg(item[0])), item[1]);
            }

            return dict;
        }

        void ReadHistory()
        {
            history = new History(settings.Paths.dataFolder + Path.DirectorySeparatorChar + "history_cmd.txt");
            tbSend.Text = history.Get();
            historyIP = new History(settings.Paths.dataFolder + Path.DirectorySeparatorChar + "history_IP.txt", 5);
            historyPort = new History(settings.Paths.dataFolder + Path.DirectorySeparatorChar + "history_port.txt", 5);
            historyPortServer = new History(settings.Paths.dataFolder + Path.DirectorySeparatorChar + "history_port_server.txt", 5);
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

            Timer1.Interval = Settings.Messages.AutoSendingPeriod;                   // auto sending interval

            cbBaud.Text = Settings.Connection.BaudRate.ToString();
            cbIP.Text = Settings.Connection.IP;
            cbPort.Text = Settings.Connection.Port.ToString();
            txtLocalPort.Text = Settings.Connection.LocalPort.ToString();
            cbSPort.Text = Settings.Connection.LocalPort.ToString();
            cbProtocol.SelectedIndex = 0;
            if (Settings.Connection.Type == ConnectionType.UDP) cbProtocol.SelectedIndex = 1;

            if (settings.tab >= tabControl1.TabPages.Count) settings.tab = 0;
            tabControl1.SelectedIndex = settings.tab;

            switch (Settings.Messages.PacketView)
            {
                case ePacketView.Bytes:
                    chkByte.Checked = true; break;
                case ePacketView.Hex:
                    chkHex.Checked = true; break;
                case ePacketView.String:
                case ePacketView.StringReplaceCommandChars:
                    chkString.Checked = true; break;
                case ePacketView.MARS_A:
                    chkMarsA.Checked = true; break;
            }
            

            chkLine.Checked = Settings.Messages.UseLineSeparatingChar;
            mnutxtLine.Text = Settings.Messages.LineSeparatingChar;

            chkTime.Checked = Settings.Messages.ShowTime;
            chkBaudRate.Checked = Settings.Messages.ShowBaudRate;

            chbClear.Checked = Settings.Messages.ClearEditbox;
            chbAutoReply.Checked = Settings.Messages.EnableReplyFile;
            chbAutoSend.Checked = Settings.Messages.EnableAutoSending;
            chbEndChar.Checked = Settings.Messages.AddEndChar;
            txtEndCMD.Text = Settings.Messages.EndChar;
        }

        #endregion

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Connection.SerialPort = cbbCOMPorts.Text;
            Settings.Connection.BaudRate = Conv.ToIntDef(cbBaud.Text, 115200);

            Settings.Connection.IP = cbIP.Text;
            Settings.Connection.Port = Conv.ToIntDef(cbPort.Text, 17000);
            if (Settings.Connection.Type == ConnectionType.UDP)
                Settings.Connection.LocalPort = Conv.ToIntDef(txtLocalPort.Text, 17001);
            else
                Settings.Connection.LocalPort = Conv.ToIntDef(cbSPort.Text,17000);
            if (cbProtocol.SelectedIndex == 1) Settings.Connection.Type = ConnectionType.UDP;
            else Settings.Connection.Type = ConnectionType.Serial;

            settings.tab = tabControl1.SelectedIndex;

            //settings.SP.bits = SP.DataBits;
            //settings.SP.parity = SP.Parity.ToString();

            //settings.SP.stopbits = SP.StopBits.ToString();
            //settings.SP.DTR = SP.DtrEnable;
            //settings.SP.RTS = SP.RtsEnable;


            if (chkByte.Checked)
                Settings.Messages.PacketView = ePacketView.Bytes;
            if (chkHex.Checked)
                Settings.Messages.PacketView = ePacketView.Hex;
            if (chkString.Checked)
                Settings.Messages.PacketView = ePacketView.StringReplaceCommandChars;
            if (chkMarsA.Checked)
                Settings.Messages.PacketView = ePacketView.MARS_A;

            Settings.Messages.UseLineSeparatingChar = chkLine.Checked;
            Settings.Messages.LineSeparatingChar = mnutxtLine.Text;


            Settings.Messages.ShowTime = chkTime.Checked;
            Settings.Messages.ShowBaudRate = chkBaudRate.Checked;



            Settings.Messages.ClearEditbox = chbClear.Checked;
            Settings.Messages.EnableReplyFile = chbAutoReply.Checked;
            Settings.Messages.EnableAutoSending = chbAutoSend.Checked;
            Settings.Messages.AddEndChar = chbEndChar.Checked;
            Settings.Messages.EndChar = txtEndCMD.Text;
            settings.SaveSettings();

            Settings.SaveXml();

            // ----- SAVE COMMANDS HISTORY -----
            history.SetTemporary(tbSend.Text);
            history.Save();
            historyIP.Save();
            historyPort.Save();
            historyPortServer.Save();

            if (com.IsOpen()) com.Close();
            com = null;

            //if (btnConnect.Tag == "1") btnConnect_Click(sender, e);

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
            

            string[] items = Files.LoadFileLines(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar + "menu_items.txt", true);
            AddMenuItems(items);

            // ----- Add User defined items -----
            items = Files.LoadFileLines(Files.ReplaceVarPaths(settings.Paths.dataFolder + Path.DirectorySeparatorChar + "menu_items.txt"), true);
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
            if (!(btnConnect.Tag == "1"))
            {

                if (com.IsOpen()) com.Close();

                try
                {
                    com.Open(cbbCOMPorts.Text, Convert.ToInt32(cbBaud.Text));
                    

                    lblStatus.Text = "Connected to: " + cbbCOMPorts.Text;

                    btnConnect.Tag = "1";
                    btnConnect.Text = "Disconnect";
                    statusImg.Image = COMunicator.Properties.Resources.circ_green24;
                    btnNetConn.Enabled = false;
                    btnNetSConn.Enabled = false;
                    Global.LogPacket.SaveHeader(cbbCOMPorts.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                btnConnect.Tag = "0";
                btnConnect.Text = "Connect";
                try
                {
                    com.Close();
                    lblStatus.Text = "Disconnected.";
                    btnNetConn.Enabled = true;
                    btnNetSConn.Enabled = true;
                    statusImg.Image = COMunicator.Properties.Resources.circ_red24;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnBaudRate_Click(object sender, EventArgs e)
        {
            com.SetBaudRate(Convert.ToInt32(cbBaud.Text));
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string text = tbSend.Text;
            if (chbEndChar.Checked) text += txtEndCMD.Text;
            if (com.IsOpen())
            {
                Send(text);
                if (tbSend.Text != "") history.Add(tbSend.Text);
                if (chbClear.Checked == false) { tbSend.Text = ""; };
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

        
        
        void Send(string msg)
        {
            byte[] b = com.FormatMsg(msg);
            byte[] chr = new byte[1];

            if (com.IsOpen() & b.Length > 0)
            {
                com.Send(b);

                ShowData(b, false);

            }
        }

        private string ShowData(byte[] message, bool input)
        {
            string description = "";

            // ----- Status log packets -----
            if (input) {
                //var delay = DateTime.Now - Global.Log.LastLogTime;
                //Global.Log.Add("Input message length: " + message.Length + ", delay: " + delay.TotalSeconds, Color.Blue);
                ReplyCome = true;
            }
            else
            {
                //Global.Log.Add("Send message length: " + message.Length.ToString(), Color.Black, true, false);
                ReplyCome = false;
            }

             if (chkMarsA.Checked)
             {
                if (message.Length > 2)
                {
                    int frameNum = 0;
                    try
                    {
                        int frame = Conv.SwapBytes(BitConverter.ToUInt16(message, 0));
                        int length = (frame & 2047) - 6; // dala length
                        frameNum = (frame & 12288) + 6 + (128 << 8) + +(1 << 8);
                    }
                    catch (Exception){}

                    Send(@"\s" + frameNum.ToString());
                }
            }

            var endChar = Settings.Messages.UsedEncoding.GetString(com.FormatMsg(mnutxtLine.Text));

            if (chkLine.Checked && endChar != "")
            {
                Global.LogPacket.LineSeparatingChar = endChar;
            }
            else
            {
                Global.LogPacket.LineSeparatingChar = "";
            }


            if (chkBaudRate.Checked)
            {
                description = cbBaud.Text + "[Bd]";
            }

            string text = "";
            if (input)
                text = Global.LogPacket.Add(description, message, Color.Blue, chkTime.Checked, input);
            else
                text = Global.LogPacket.Add(description, message, Color.Black, chkTime.Checked, input);


            /*if (text.Length > 0)
            {
                lbLog.Items.Add(text);
            }

            if (lbLog.Items.Count > 100)
                lbLog.Items.RemoveAt(0);

            
            lbLog.SelectedIndex = lbLog.Items.Count - 1;*/

            // ----- Show Packet list -----
            /*txtPackets.Rtf = Global.LogPacket.TextRTF();
            txtPackets.SelectionStart = txtPackets.Text.Length;
            txtPackets.ScrollToCaret();*/

            // ----- Show communication log -----
            /*txtLog.Rtf = Global.LogPacket.TextRTF();
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();*/

            //olvPacket.UpdateObjects(Global.LogPacket.Recods);
            /*if (olvPacket.GetItemCount() > 0)
                olvPacket.EnsureVisible(olvPacket.GetItemCount() - 1);*/

            return text;
        }

        


        private void SP_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            olvPacket.Invoke(new MyDelegate(updateLog), new Object[] { }); //BeginInvoke
            //TimeOut.Enabled = true;
            //TimeOut_Tick(sender, e);
    
        }


        private void chkString_Click(object sender, EventArgs e)
        {
            chkFormat.Checked = false;
            chkByte.Checked = false;
            chkHex.Checked = false;
            chkString.Checked = false;
            chkMarsA.Checked = false;
            ((ToolStripMenuItem)sender).Checked = true;

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
            else if (chkMarsA.Checked)
            {
                Global.LogPacket.SetPacketView(ePacketView.MARS_A);
            }

            /*txtPackets.Rtf = Global.LogPacket.TextRTF();
            txtPackets.SelectionStart = txtPackets.Text.Length;
            txtPackets.ScrollToCaret();*/

            olvPacket.SetObjects(Global.LogPacket.Recods);
            txtLog.Rtf = Global.LogPacket.TextRTF();

            if (chkMarsA.Checked) TimeOut.Interval = 80;
            else TimeOut.Interval = 20;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            string text;
            if (com.IsOpen())
            {
                if (chbSendFromFile.Checked)
                {
                    if (AutoSendDataIndex >= 0 && AutoSendData.Length > 0)
                    {
                        tbSend.Text = AutoSendData[AutoSendDataIndex];
                        AutoSendDataIndex += 1;
                        if (AutoSendDataIndex == AutoSendData.Length)
                        {
                            if (Settings.Messages.SendingFileRepeating)
                                AutoSendDataIndex = 0;
                            else
                                AutoSendDataIndex--;
                        }
                    }
                }
                if (chbBaudTest.Checked)
                {
                    int baud = Conv.ToIntDef(cbBaud.Text, 2350) + 50;
                    if (baud < 1200) baud = 1200;
                    if (baud > 115200) baud = 115200;
                    cbBaud.Text = baud.ToString();

                    btnBaudRate_Click(sender, e);
                }
                text = tbSend.Text;
                if (chbEndChar.Checked) text += txtEndCMD.Text;

                if (text != "")
                {
                    if (!Settings.Messages.WaitForReply || (Settings.Messages.WaitForReply && ReplyCome))
                    Send(text);
                }
            }
            
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            cbBaud.Text = cbBaud.Text + 50;
            btnBaudRate_Click(sender, e);
        }

        private void chbAutoSend_CheckedChanged(object sender, EventArgs e)
        {
            if (chbAutoSend.Checked)
                Timer1.Enabled = true;
            else
                Timer1.Enabled = false;
        
        }

        private void CntText_Click(object sender, EventArgs e)
        {
            if (olvPacket.SelectedIndex >= 0)
            {
                tbSend.Text = ((LogRecord)olvPacket.SelectedItem.RowObject).text;
            }
        }

        private void CntCopy_Click(object sender, EventArgs e)
        {
            if (olvPacket.SelectedIndex >= 0)
            {
                System.Windows.Forms.Clipboard.SetText(((LogRecord)olvPacket.SelectedItem.RowObject).text);
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
                    //Files.SaveFile(dialog.FileName, txtPackets.Text);
                }
            }
            catch (Exception err)
            {
                Dialogs.ShowErr(err.Message, "Error");
            }
        }

        private void TimeOut_Tick(object sender, EventArgs e)
        {
            TimeOut.Enabled = false;

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
            }


            //lbLog.SelectedIndex = lbLog.Items.Count - 1;
            //lbLog.Invoke(new MyDelegate(updateLog), new Object[] { });
            //txt = "";
            PrevData = new byte[0];
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Global.Log.ClearLog();
            Global.LogPacket.ClearLog();
            olvPacket.SetObjects(Global.LogPacket.Recods);
            txtLog.Text = "";

        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            ChangeSettings(0);
        }

        private void ChangeSettings(int tab) 
        {
            Settings.Messages.EnableReplyFile = chbAutoReply.Checked;
            Settings.Messages.EnableSendingFile = chbSendFromFile.Checked;
            formSet.ShowDialog(tab);

            try
            {
                if (com != null)
                {
                    com.SetParamsSP(Settings.Connection.Parity, Settings.Connection.DataBits, Settings.Connection.StopBits, Settings.Connection.DTR, Settings.Connection.RTS);
                    com.SetEncoding(Settings.Messages.UsedEncoding);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Timer1.Interval = Settings.Messages.AutoSendingPeriod;
            chbAutoReply.Checked = Settings.Messages.EnableReplyFile;
            chbSendFromFile.Checked = Settings.Messages.EnableSendingFile;
            LoadAutoSendData();
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
                Files.SaveFile(Files.ReplaceVarPaths(settings.Paths.dataFolder + Path.DirectorySeparatorChar + "menu_items.txt"), "\n" + txt + @"\:" + tbSend.Text, true);
                //AddToFile("polozky.txt", txt + @"\:" + tbSend.Text);
                LoadMenu();
            }
        }


        private string byteToFormat(byte[] bytes)
        {
            string msg = "";

            for (int i = 0; i < bytes.Length; i++)
            {
                msg += '\\' + bytes[i].ToString();
            }

            return msg;
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

        private void chbAutoReply_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void btnNetConn_Click(object sender, EventArgs e)
        {
            if (btnNetConn.Tag != "run")
            {
                try
                {
                    ProtocolType protocol = ProtocolType.Tcp;
                    if (cbProtocol.SelectedIndex == 1) protocol = ProtocolType.Udp;
                    if (protocol == ProtocolType.Tcp)
                        com.ConnectTcp(cbIP.Text, Convert.ToInt32(cbPort.Text));
                    else
                    {
                        int localPort = Conv.ToIntDef(txtLocalPort.Text, -1);
                        com.ConnectUdp(cbIP.Text, Convert.ToInt32(cbPort.Text), localPort);
                        NetConnected();
                    }
                        

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                try
                {
                    com.Close();
                }
                catch (Exception)
                {

                }

                btnNetConn.Tag = "stop";
                btnNetConn.Text = "Connect";
                lblStatus.Text = "Disconnected";
                btnConnect.Enabled = true;
                btnNetSConn.Enabled = true;
                statusImg.Image = COMunicator.Properties.Resources.circ_red24;
            }
        }

        private void NetConnected()
        {
            btnNetConn.Tag = "run";
            btnNetConn.Text = "Disconnect";
            lblStatus.Text = "Connected to: " + cbIP.Text + ":" + cbPort.Text;
            statusImg.Image = COMunicator.Properties.Resources.circ_green24;
            btnConnect.Enabled = false;
            btnNetSConn.Enabled = false;
            historyIP.Add(cbIP.Text);
            historyPort.Add(cbPort.Text);
            RefreshHistNet();
            Global.LogPacket.SaveHeader("Net " + cbIP.Text + ":" + cbPort.Text);
        }

        private void btnSPRefresh_Click(object sender, EventArgs e)
        {
            RefreshCOMPorts();
        }

        private void btnNetSConn_Click(object sender, EventArgs e)
        {
            if (btnNetSConn.Tag != "run")
            {
                try
                {
                    com.CreateTCPServer(Convert.ToInt32(cbSPort.Text));
                    serverConnected();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                try
                {
                    com.Close();
                }
                catch (Exception)
                {

                }

                btnNetSConn.Tag = "stop";
                btnNetSConn.Text = "Connect";
                lblStatus.Text = "Disconnected";
                btnConnect.Enabled = true;
                btnNetConn.Enabled = true;
                statusImg.Image = COMunicator.Properties.Resources.circ_red24;
            }
        }

        private void serverConnected()
        {
            btnNetSConn.Tag = "run";
            btnNetSConn.Text = "Disconnect";
            lblStatus.Text = "Server start on port: " + cbSPort.Text;
            statusImg.Image = COMunicator.Properties.Resources.circ_green24;
            btnConnect.Enabled = false;
            btnNetConn.Enabled = false;
            historyPortServer.Add(cbSPort.Text);
            RefreshHistNet();
            Global.LogPacket.SaveHeader("Net TCP Server port: " + cbSPort.Text);
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

        private void button2_Click(object sender, EventArgs e)
        {
            //string x = Protocol.Protocol.MarsA(Conv.HexToUInt("6AEB012A"), com.FormatMsg( "bb"));
        }

        private void btnSettings2_Click(object sender, EventArgs e)
        {
            ChangeSettings(1);
        }

        private void chbSendFromFile_CheckedChanged(object sender, EventArgs e)
        {
            if (chbSendFromFile.Checked)
            {
                LoadAutoSendData();
            }
        }

        private void mnuEditSend_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Files.ReplaceVarPaths(settings.Paths.dataFolder + Path.DirectorySeparatorChar + "menu_items.txt"));
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
    }

}

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
using LOG;
using TCPClient;
using myFunctions;
using COMunicator.Protocol;

using System.Net.Sockets;
using System.Threading;

namespace COMunicator
{
    public partial class frmMain : Form
    {
        // ----- Reply data -----
        static Dictionary<string, string> ReplyData = new Dictionary<string, string>();

        // ----- Auto send from file data ---
        string[] AutoSendData;
        int AutoSendDataIndex; 


        static frmSettings formSet;

        byte[] PrevData;

        
        byte[] comBuffer;

        //string[] history;
        //int historyIndex;

        public delegate void MyDelegate(comStatus status);

        public Comm com;
        public History history;
        public History historyIP;
        public History historyPort;
        public History historyPortServer;

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
                    Log.add("Client disconnected");
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
                    Log.add("Client connected");
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
            lbLog.Invoke(new MyDelegate(updateLog), new Object[] { status }); //BeginInvoke
            
        }

        #region LoadForm

        private void frmMain_Load(object sender, EventArgs e)
        {
            // ----- GET APPLICATION VERSION -> TO CAPTION -----
            this.Text = this.Text + " v" + Application.ProductVersion.Substring(0,Application.ProductVersion.Length-2);

            // ----- LOAD SETTINGS -----
            settings.LoadSettings();
            // ----- Creating data folder -----
            if (!Directory.Exists(Files.ReplaceVarPaths(settings.Paths.dataFolder))) Directory.CreateDirectory(Files.ReplaceVarPaths(settings.Paths.dataFolder));
;
            Encoding enc = settings.encoding;

            // ----- CREATE NEW COMMUNICATION INSTANCE -----
            com = new Comm(enc);
            com.SetParamsSP(ToParity(settings.SP.parity), settings.SP.bits, ToStopBits(settings.SP.stopbits), settings.SP.DTR, settings.SP.RTS);
            com.ReceivedData += new ReceivedEventHandler(DataReceive);

            // ----- APPLY SETTINGS TO FORM -----
            ApplySettingsToForm();

            // ----- READ SENDING HISTORY -----
            ReadHistory();

            // ----- LOAD COMMAND MENU -----
            LoadMenu();

            // ----- READ REPLY FILE -----
            ReplyData = LoadReplyFile(Files.ReplaceVarPaths(settings.Paths.ReplyFile));

            // ----- READ AUTO SEND FILE -----
            LoadAutoSendData();

            

            formSet = new frmSettings();

            PrevData = new byte[0];

            AutoSendDataIndex = -1;

        }

        void LoadAutoSendData()
        {
            AutoSendData = Files.LoadFileLines(Files.ReplaceVarPaths(settings.Paths.SendingFile), true);
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
            cbbCOMPorts.Text = settings.SP.port;
            if ((cbbCOMPorts.Items.Count > 0) & cbbCOMPorts.Text == "")
            {
                cbbCOMPorts.Text = cbbCOMPorts.Items[0].ToString();
            }

            Timer1.Interval = settings.Fun.AutoSendDelay;                   // auto sending interval

            cbBaud.Text = settings.SP.baudrate.ToString();
            cbIP.Text = settings.TCPClient.IP;
            cbPort.Text = settings.TCPClient.port.ToString();
            txtLocalPort.Text = settings.TCPClient.localPort.ToString();
            cbSPort.Text = settings.TCPClient.serverPort.ToString();
            cbProtocol.SelectedIndex = 0;
            if (settings.TCPClient.UDP) cbProtocol.SelectedIndex = 1;

            if (settings.tab >= tabControl1.TabPages.Count) settings.tab = 0;
            tabControl1.SelectedIndex = settings.tab;

            chkString.Checked = settings.Show.String;
            chkByte.Checked = settings.Show.Byte;
            chkHex.Checked = settings.Show.HexNum;
            chkFormat.Checked = settings.Show.Format;
            chkMarsA.Checked = settings.Show.MarsA;

            chkLine.Checked = settings.Show.Line;
            mnutxtLine.Text = settings.Show.LineNum;
            mnuShowCommandChars.Checked = settings.Show.ShowCommandChars;
            //mnuFormatString.Text = settings.Log.FormatString;

            chkTime.Checked = settings.Show.Time;
            chkBaudRate.Checked = settings.Show.BaudRate;

            chbClear.Checked = settings.Fun.NoClear;
            chbAutoReply.Checked = settings.Fun.AutoReply;
            chbAutoSend.Checked = settings.Fun.AutoSend;
            chbEndChar.Checked = settings.Fun.IsEndChar;
            txtEndCMD.Text = settings.Fun.EndChar;
        }

        #endregion

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            settings.SP.port = cbbCOMPorts.Text;
            settings.SP.baudrate = Conv.ToIntDef(cbBaud.Text, 115200);

            settings.TCPClient.IP = cbIP.Text;
            settings.TCPClient.port = Conv.ToIntDef(cbPort.Text, 17000);
            settings.TCPClient.localPort = Conv.ToIntDef(txtLocalPort.Text, 17001);
            settings.TCPClient.serverPort = Conv.ToIntDef(cbSPort.Text,17000);
            if (cbProtocol.SelectedIndex == 1) settings.TCPClient.UDP = true;
            else settings.TCPClient.UDP = false;

            settings.tab = tabControl1.SelectedIndex;

            //settings.SP.bits = SP.DataBits;
            //settings.SP.parity = SP.Parity.ToString();

            //settings.SP.stopbits = SP.StopBits.ToString();
            //settings.SP.DTR = SP.DtrEnable;
            //settings.SP.RTS = SP.RtsEnable;



            settings.Show.String = chkString.Checked;
            settings.Show.Byte = chkByte.Checked;
            settings.Show.HexNum = chkHex.Checked;
            settings.Show.Format = chkFormat.Checked;
            settings.Show.MarsA = chkMarsA.Checked;

            settings.Show.Line = chkLine.Checked;
            settings.Show.LineNum = mnutxtLine.Text;
            settings.Show.ShowCommandChars = mnuShowCommandChars.Checked;
            //settings.Log.FormatString = mnuFormatString.Text;

            settings.Show.Time = chkTime.Checked;
            settings.Show.BaudRate = chkBaudRate.Checked;
            

            settings.Fun.NoClear = chbClear.Checked;
            settings.Fun.AutoReply = chbAutoReply.Checked;
            settings.Fun.AutoSend = chbAutoSend.Checked;
            settings.Fun.IsEndChar = chbEndChar.Checked;
            settings.Fun.EndChar = txtEndCMD.Text;
            settings.SaveSettings();

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
            string[] CntItem;
            string[] items = Files.LoadFileLines(Files.ReplaceVarPaths(settings.Paths.dataFolder + Path.DirectorySeparatorChar + "menu_items.txt"), true);

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
                            ((ToolStripMenuItem)CntSend.Items[index]).DropDownItems.Add(CntItem[1]);
                            ((ToolStripMenuItem)CntSend.Items[index]).DropDownItems[((ToolStripMenuItem)CntSend.Items[index]).DropDownItems.Count - 1].Tag = CntItem[CntItem.Length - 1];
                            ((ToolStripMenuItem)CntSend.Items[index]).DropDownItems[((ToolStripMenuItem)CntSend.Items[index]).DropDownItems.Count - 1].Click += new EventHandler(CntSend_Click);
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
                    Log.init(Files.ReplaceVarPaths(settings.Paths.logFile), cbbCOMPorts.Text);
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
            uint data;
            string dataS = "", dataBS = "", dataHex = "";
            string txt = "", encTxt = "";
            bool add = false;
            byte[] EndChar;

            string arrows;
            if (input) arrows = ">>";
            else arrows = "<<";


            try
            {
                EndChar = com.FormatMsg(mnutxtLine.Text);
                if (EndChar.Length == 0) EndChar = new byte[1];
            }
            catch (Exception)
            {
                EndChar = new byte[1];
                EndChar[0] = (byte)'\n';
            }

            

            //dataS = SP.ReadExisting
            if (message.Length <= 4)
            {
                //dataS = ByteToHex(message);
                //data = CInt("&H" & dataS);
                data = 0;
            }
            else
            {
                data = 0;
            }
            List<byte> bList = message.ToList();
            List<int> nullList = new List<int>();

            // ----- Replace null bytes -----
            dataS = settings.encoding.GetString(bList.ToArray());
            if (dataS.Length == bList.Count)    // if not unicode
            {
                for (int i = bList.Count - 1; i >= 0; i--)
                    if (bList[i] == 0)
                    {
                        nullList.Add(i);
                        bList.RemoveAt(i);
                        bList.Insert(i, 1);
                    }
                dataS = settings.encoding.GetString(bList.ToArray());
            }

            byte[] byteMsg = Encoding.Default.GetBytes(dataS);

            // Replace special chars
            for (int i = byteMsg.Length - 1; i >= 0; i--)
            {
                if (mnuShowCommandChars.Checked && byteMsg[i] < 32)
                {
                    dataS = dataS.Remove(i, 1);
                    if (nullList.Contains(i))
                    {
                        dataS = dataS.Insert(i, "{0}");
                    }
                    else
                    {
                        dataS = dataS.Insert(i, "{" + byteMsg[i].ToString() + "}");
                    }

                }
            }

            byte[] test = new byte[1];

            for (int i = 0; i < message.Length; i++)
            {
                dataBS += @"\" + message[i].ToString();
                dataHex += message[i].ToString("X2") + " ";
            }

            //if (dataBS.Length > 0) dataBS = dataBS.Remove(dataBS.Length - 1);



            if (chkString.Checked)
            {
                if (add) txt += " <-> ";
                add = true;
                txt += dataS;
                //txt = txt + '"' + dataS + '"';
            }

            //If chkLine.Checked Then txt = txt & """" & dataS & """ <-> "

            if (chkByte.Checked)
            {
                if (add) txt += " <-> ";
                txt += dataBS;
                add = true;
            }

            /*if (chkNumber.Checked)
            {
                if (add) txt += " <-> ";
                txt = txt + data.ToString();
                add = true;
            }*/

            if (chkHex.Checked)
            {
                if (add) txt += " <-> ";
                txt += dataHex;
                add = true;
            }

            if (chkMarsA.Checked)
            {
                if (add) txt += " <-> ";
                string dataMars = "";

                if (message.Length > 2)
                {
                    int frameNum = 0;
                    try
                    {
                        int frame = Conv.SwapBytes(BitConverter.ToUInt16(message, 0));
                        int length = (frame & 2047) - 6; // dala length
                        frameNum = (frame & 12288) + 6 + (128 << 8) + +(1 << 8);
                        for (int i = 8; i < 8 + length; i++)
                        {
                            test[0] = message[i];
                            encTxt = settings.encoding.GetString(test);
                            if (!mnuShowCommandChars.Checked || message[i] > 31)
                            {
                                if (message[i] != 0) dataMars += encTxt;
                            }
                            else dataMars += "{" + message[i].ToString() + "}";

                        }
                    }
                    catch (Exception err)
                    {

                    }
                    Send(@"\s" + frameNum.ToString());
                }

                txt += dataMars;
                add = true;
            }

            /*if (chkFixedPoint.Checked)
            {
                if (add) txt += " <-> ";
                if (IsNumeric(mnutxtFixPoint.Text))
                {
                    int num = CInt(mnutxtFixPoint.Text);
                    if ((num <= 32) & (num > 0))
                    {
                        txt = txt + CFixToFloat(data, num);
                    }
                }
                else txt = txt + CFixToFloat(data);
                add = true;
            }*/

            if (chkBaudRate.Checked)
            {
                if (txt.Length > 0)
                    txt = cbBaud.Text + " " + arrows + " " + txt;
            }

            if (chkTime.Checked)
            {
                if (txt.Length > 0)
                    txt = DateTime.Now.ToString("HH:mm:ss.fff") + " " + arrows + " " + txt;
            }


            if (chkLine.Checked & !(message[message.Length - 1] == EndChar[0]))
            {
                txt = txt.Replace((char)EndChar[0], '\n');
            }



            if (txt.Length > 0)
            {
                lbLog.Items.Add(arrows + "  " + txt);
            }


            if (txtLog.Text.Length > 100000)
            {
                int startIndex = txt.Length;
                if (startIndex >= txtLog.Text.Length) txtLog.Text = "";
                else
                {
                    txtLog.Select(0, startIndex);
                    txtLog.SelectionColor = Color.Red;
                    txtLog.SelectedText = " ";          // Empty not worked!!! <<<

                    //txtLog.Text = txtLog.Text.Remove(txtLog.SelectionStart, txtLog.SelectionLength);

                    //txtLog.Text = txtLog.Text.Substring(startIndex);
                    //var regExp = new System.Text.RegularExpressions.Regex(@"\bis\b");
                    //txtLog.Rtf = regExp.sub(txtLog.Rtf, "are");

                    //txtLog.Rtf = txtLog.Rtf.Substring(startIndex);
                }

            }
                
            if (lbLog.Items.Count > 100)
                lbLog.Items.RemoveAt(0);


            txtLog.SelectionStart = txtLog.Text.Length;
       
            
            if (input)
                txtLog.SelectionColor = Color.Blue;
            else
                txtLog.SelectionColor = Color.Black;

            txt = txt.Replace("{13}{10}", "\n");
            txt = txt.Replace("{10}", "\n");
            //str.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
            if (txt.Length > 0 && txt[0] == '\n') txt = txt.Remove(0,1);
            txtLog.AppendText(txt);
            if (txt.Length > 0)
            {
                Log.add(arrows + "  " + txt);
            }
            if (txt.Length > 0 && !(txt[txt.Length - 1] == '\n' || txt[txt.Length - 1] == '\r')) txtLog.AppendText("\n");
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();

            lbLog.SelectedIndex = lbLog.Items.Count - 1;


            return dataS;
        }

        


        private void SP_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            lbLog.Invoke(new MyDelegate(updateLog), new Object[] { }); //BeginInvoke
            //TimeOut.Enabled = true;
            //TimeOut_Tick(sender, e);
    
        }


        private void chkString_Click(object sender, EventArgs e)
        {
            if (((ToolStripMenuItem)sender).Checked) ((ToolStripMenuItem)sender).Checked = false; else ((ToolStripMenuItem)sender).Checked = true;
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
                            if (settings.Paths.BeginAfterEoF)
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
                if (text != "") Send(text);
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
            tbSend.Text = lbLog.Items[lbLog.SelectedIndex].ToString().Remove(0, 4);
        }

        private void CntCopy_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(lbLog.Items[lbLog.SelectedIndex].ToString().Remove(0, 4));
        }

        private void CntSaveAs_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Log File *.log|*.log|All Files (*.*)|*.*";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Files.SaveFile(dialog.FileName, txtLog.Text);
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


            lbLog.SelectedIndex = lbLog.Items.Count - 1;
            //lbLog.Invoke(new MyDelegate(updateLog), new Object[] { });
            //txt = "";
            PrevData = new byte[0];
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lbLog.Items.Clear();
            txtLog.Clear();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            ChangeSettings(0);
        }

        private void ChangeSettings(int tab) 
        {
            settings.Fun.AutoReply = chbAutoReply.Checked;
            settings.Paths.EnableSendingFile = chbSendFromFile.Checked;
            formSet.ShowDialog(tab);

            try
            {
                if (com != null)
                {
                    com.SetParamsSP(ToParity(settings.SP.parity), settings.SP.bits, ToStopBits(settings.SP.stopbits), settings.SP.DTR, settings.SP.RTS);
                    com.SetEncoding(settings.encoding);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Timer1.Interval = settings.Fun.AutoSendDelay;
            chbAutoReply.Checked = settings.Fun.AutoReply;
            chbSendFromFile.Checked = settings.Paths.EnableSendingFile;
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
            Log.init(Files.ReplaceVarPaths(settings.Paths.logFile), "Net " + cbIP.Text + ":" + cbPort.Text);
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
            Log.init(Files.ReplaceVarPaths(settings.Paths.logFile), "Net TCP Server port: " + cbSPort.Text);
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
    }

}

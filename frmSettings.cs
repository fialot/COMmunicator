using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using myFunctions;

namespace COMunicator
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(int tab)
        {
            try { this.tabControl1.SelectedIndex = tab; }
            catch { }
            return base.ShowDialog();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            settings.SP.bits = Convert.ToInt32(cbDataBits.Text);
            settings.SP.parity = cbParity.Text;
            settings.SP.DTR = chbDTR.Checked;
            settings.SP.RTS = chbRTS.Checked;
            settings.SP.stopbits = cbStopBits.Text;

            try
            {
                settings.Fun.AutoSendDelay = Convert.ToInt32(txtAutoSendDelay.Text);
            } catch (Exception) {}

            settings.Paths.SendingFile = txtSendingFile.Text;
            settings.Paths.EnableSendingFile = chbEnableSendFromFile.Checked;
            settings.Paths.BeginAfterEoF = chbEoF.Checked;

            settings.Paths.ReplyFile = txtReplyFile.Text;
            settings.Fun.AutoReply = chbEnableReply.Checked;
            settings.Fun.WaitForReply = chbEnableReplyWait.Checked;
            settings.Fun.ReplyTimeout = Conv.ToIntDef(txtReplyTimeout.Text, 1000);

            settings.Paths.logFile = txtLogFile.Text;
            settings.Paths.logEnable = chbEnableLog.Checked;
            settings.Paths.logNewFile = chbNewLog.Checked;

            settings.Paths.dataFolder = txtDataFolder.Text;

            if (cbCoding.Text != "")
            {
                try
                {
                    EncodingInfo[] EncIfo = Encoding.GetEncodings();
                    for (int i = 0; i < EncIfo.Length; i++)
                    {
                        if (EncIfo[i].DisplayName == cbCoding.Text)
                        {
                            settings.encoding = Encoding.GetEncoding(EncIfo[i].CodePage);
                            break;
                        }
                            
                    }
                }
                catch (Exception)
                {
                    settings.encoding = System.Text.Encoding.Default;
                }
                
            }
            

            this.Close();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            EncodingInfo[] EncIfo = Encoding.GetEncodings();

            cbCoding.Items.Clear();
            for (int i = 0; i < EncIfo.Length; i++)
            {
                cbCoding.Items.Add(EncIfo[i].DisplayName);
            }
            cbCoding.Text = settings.encoding.EncodingName;

            cbParity.Items.Clear();
            cbParity.Items.Add(Parity.None.ToString());
            cbParity.Items.Add(Parity.Even.ToString());
            cbParity.Items.Add(Parity.Odd.ToString());
            cbParity.Items.Add(Parity.Mark.ToString());
            cbParity.Items.Add(Parity.Space.ToString());

            cbDataBits.Items.Clear();
            for (int i = 5; i <= 8; i++)
                cbDataBits.Items.Add(i.ToString());

            cbStopBits.Items.Clear();

            cbStopBits.Items.Add(StopBits.One.ToString());
            cbStopBits.Items.Add(StopBits.OnePointFive.ToString());
            cbStopBits.Items.Add(StopBits.Two.ToString());
            cbStopBits.Items.Add(StopBits.None.ToString());

            cbDataBits.Text = settings.SP.bits.ToString();
            cbStopBits.Text = settings.SP.stopbits.ToString();
            cbParity.Text = settings.SP.parity;
            chbDTR.Checked = settings.SP.DTR;
            chbRTS.Checked = settings.SP.RTS;

            txtAutoSendDelay.Text = settings.Fun.AutoSendDelay.ToString();
            chbEnableReplyWait.Checked = settings.Fun.WaitForReply;
            txtReplyTimeout.Text = settings.Fun.ReplyTimeout.ToString();
            txtSendingFile.Text = settings.Paths.SendingFile;
            chbEnableSendFromFile.Checked = settings.Paths.EnableSendingFile;
            chbEoF.Checked = settings.Paths.BeginAfterEoF;

            txtReplyFile.Text = settings.Paths.ReplyFile;
            chbEnableReply.Checked = settings.Fun.AutoReply;

            txtLogFile.Text = settings.Paths.logFile;
            chbEnableLog.Checked = settings.Paths.logEnable;
            chbNewLog.Checked = settings.Paths.logNewFile;

            txtDataFolder.Text = settings.Paths.dataFolder;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSendingFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = System.IO.Path.GetDirectoryName(Files.ReplaceVarPaths(txtSendingFile.Text));
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtSendingFile.Text = dialog.FileName;
            }
        }

        private void btnReplyFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = System.IO.Path.GetDirectoryName(Files.ReplaceVarPaths(txtReplyFile.Text));
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtReplyFile.Text = dialog.FileName;
            }
        }

        private void btnLogFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = System.IO.Path.GetDirectoryName(Files.ReplaceVarPaths(txtLogFile.Text));
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtLogFile.Text = dialog.FileName;
            }
        }

        private void btnDataFolder_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = System.IO.Path.GetDirectoryName(Files.ReplaceVarPaths(txtDataFolder.Text));
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtDataFolder.Text = dialog.FileName;
            }
        }


    }
}

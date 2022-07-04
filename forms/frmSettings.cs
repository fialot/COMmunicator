using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using AppSettings;
using Fx.IO;

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

            Settings.Connection.DataBits = Convert.ToInt32(cbDataBits.Text);
            Settings.Connection.Parity = Conv.ToEnum<Parity>(cbParity.Text, Parity.None);
            Settings.Connection.DTR = chbDTR.Checked;
            Settings.Connection.RTS = chbRTS.Checked;
            Settings.Connection.StopBits = Conv.ToEnum<StopBits>(cbStopBits.Text, StopBits.One);

            try
            {
                Settings.Messages.AutoSendingPeriod = Convert.ToInt32(txtAutoSendDelay.Text);
            } catch (Exception) {}

            Settings.Messages.SendingFile = txtSendingFile.Text;
            Settings.Messages.EnableSendingFile = chbEnableSendFromFile.Checked;
            Settings.Messages.SendingFileRepeating = chbEoF.Checked;

            Settings.Messages.ReplyFile = txtReplyFile.Text;
            Settings.Messages.EnableReplyFile = chbEnableReply.Checked;
            Settings.Messages.WaitForReply = chbEnableReplyWait.Checked;
            Settings.Messages.WaitForReplyTimeout = Conv.ToIntDef(txtReplyTimeout.Text, 1000);

            Settings.Messages.LogFileDirectory = txtLogFile.Text;
            Settings.Messages.SaveToFile = chbEnableLog.Checked;

            Settings.App.DataFolder = txtDataFolder.Text;

            if (cbCoding.Text != "")
            {
                try
                {
                    EncodingInfo[] EncIfo = Encoding.GetEncodings();
                    for (int i = 0; i < EncIfo.Length; i++)
                    {
                        if (EncIfo[i].DisplayName == cbCoding.Text)
                        {
                            Settings.Connection.UsedEncoding = Encoding.GetEncoding(EncIfo[i].CodePage);
                            break;
                        }
                            
                    }
                }
                catch (Exception)
                {
                    Settings.Connection.UsedEncoding = System.Text.Encoding.Default;
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
            cbCoding.Text = Settings.Connection.UsedEncoding.EncodingName;

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

            cbDataBits.Text = Settings.Connection.DataBits.ToString();
            cbStopBits.Text = Settings.Connection.StopBits.ToString();
            cbParity.Text = Settings.Connection.Parity.ToString();
            chbDTR.Checked = Settings.Connection.DTR;
            chbRTS.Checked = Settings.Connection.RTS;

            

            txtAutoSendDelay.Text = Settings.Messages.AutoSendingPeriod.ToString();
            chbEnableReplyWait.Checked = Settings.Messages.WaitForReply;
            txtReplyTimeout.Text = Settings.Messages.WaitForReplyTimeout.ToString();
            txtSendingFile.Text = Settings.Messages.SendingFile;
            chbEnableSendFromFile.Checked = Settings.Messages.EnableSendingFile;
            chbEoF.Checked = Settings.Messages.SendingFileRepeating;

            txtReplyFile.Text = Settings.Messages.ReplyFile;
            chbEnableReply.Checked = Settings.Messages.EnableReplyFile;

            txtLogFile.Text = Settings.Messages.LogFileDirectory;
            chbEnableLog.Checked = Settings.Messages.SaveToFile;

            txtDataFolder.Text = Settings.App.DataFolder;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSendingFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            try
            {
                dialog.InitialDirectory = System.IO.Path.GetDirectoryName(Paths.GetFullPath(txtSendingFile.Text));
            }
            catch { }
            
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtSendingFile.Text = dialog.FileName;
            }
        }

        private void btnReplyFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            try
            {
                dialog.InitialDirectory = System.IO.Path.GetDirectoryName(Paths.GetFullPath(txtReplyFile.Text));
            }
            catch { }

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtReplyFile.Text = dialog.FileName;
            }
        }

        private void btnLogFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            try
            {
                dialog.InitialDirectory = System.IO.Path.GetDirectoryName(Paths.GetFullPath(txtLogFile.Text));
            }
            catch { }

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtLogFile.Text = dialog.FileName;
            }
        }

        private void btnDataFolder_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            try
            {
                dialog.InitialDirectory = System.IO.Path.GetDirectoryName(Paths.GetFullPath(txtDataFolder.Text));
            }
            catch { }

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtDataFolder.Text = dialog.FileName;
            }
        }


    }
}

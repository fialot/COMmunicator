using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using AppSettings;
using Fx.IO;
using Fx.IO.Protocol;
using GlobalClasses;
using myFunctions;
using TCPClient;

namespace COMunicator
{
    public class SenderStatus
    {
        public bool IsConnected = false;
    }

    public enum StateChange
    {
        Connected, Disconnected, ConnectionError, NewData, Started, Stopped
    }

    public delegate void ChangedStateEventHandler(StateChange state);

    public class Sender
    {
        public SenderStatus Status { get; private set; } = new SenderStatus();


        public event ChangedStateEventHandler ChangedState;

        Communication conn = new Communication();
        byte[] comBuffer = new byte[0];

        // ----- Reply data -----
        static Dictionary<string, string> ReplyData = new Dictionary<string, string>();

        // ----- Auto send from file data ---
        string[] AutoSendData = new string[0];
        int AutoSendDataIndex = -1;
        byte[] LastSendedMessage = new byte[0];

        bool ReplyCome = true;
        Timer autoSendingTimer = new Timer();
        Timer replyTimer = new Timer();
        bool AutoSendingLock = false;


        public Sender()
        {
            init();
        }

        public bool Connect(ConnectionSetting settings)
        {
            conn.LastCharInterval = Settings.Messages.LastCharTimeout;
            if (conn.IsOpen()) conn.Close();

            try
            {
                conn.Connect(settings);

                RefreshAutoSend();
                RefreshReply();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Disconnect()
        {
            conn.Close();

            autoSendingTimer.Enabled = false;

            return true;
        }

        public bool Send(string message)
        {
            var byteMsg = ProtocolCom.FormatMsg(message, Settings.Connection.UsedEncoding);
            return Send(byteMsg);
        }

        public bool Send(byte[] message)
        {
            if (message.Length == 0) return false;

            conn.Send(message);

            ReplyCome = false;
            LastSendedMessage = message;

            ProcessData(message, false);

            if (Settings.Messages.EnableAutoSending && Settings.Messages.WaitForReply)
            {
                replyTimer.Interval = Settings.Messages.WaitForReplyTimeout;
                replyTimer.Enabled = true;
            }
            return true;
        }

        public void SetBaudRate(int baud)
        {
            conn.SetSPBaudRate(baud);
        }

        public void RefreshAutoSend()
        {
            if (Settings.Messages.EnableAutoSending)
            {
                if (Settings.Messages.EnableSendingFile)
                {
                    LoadAutoSendData();
                }
                autoSendingTimer.Interval = Settings.Messages.AutoSendingPeriod;
                autoSendingTimer.Enabled = true;
            }
            else
            {
                autoSendingTimer.Enabled = false;
            }
        }


        public void RefreshReply()
        {
            // ----- READ REPLY FILE -----
            ReplyData = LoadReplyFile(Files.ReplaceVarPaths(Settings.Messages.ReplyFile));


        }

        Dictionary<string, string> LoadReplyFile(string fileName)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string[] lines = Files.LoadFileLines(fileName, true);
            string[] item;
            for (int i = 0; i < lines.Length; i++)
            {
                item = lines[i].Split(new string[] { @"\->" }, StringSplitOptions.RemoveEmptyEntries);
                if (item.Length == 2)
                    dict.Add(BitConverter.ToString(ProtocolCom.FormatMsg(item[0], Settings.Connection.UsedEncoding)), item[1]);
            }

            return dict;
        }



        private void init()
        {
            conn.ReceivedData += new ReceivedEventHandler(DataReceive);
            autoSendingTimer.Elapsed += new ElapsedEventHandler(AutoSendingEvent);
            replyTimer.Elapsed += new ElapsedEventHandler(ReplyTimeOutEvent);
        }

        private void DataReceive(object source, comStatus status)
        {

            switch(status)
            {
                case comStatus.Close:
                    if (Settings.Connection.Type != ConnectionType.TCPServer)
                        Status.IsConnected = false;
                    if (ChangedState != null)
                    {
                        ChangedState(StateChange.Disconnected);
                    }
                    break;
                case comStatus.OK:
                    ReplyCome = true;
                    ProcessData(conn.Read(), true);

                    break;
                case comStatus.Open:
                    Status.IsConnected = true;
                    if (ChangedState != null)
                    {
                        ChangedState(StateChange.Connected);
                    }
                    break;
                case comStatus.OpenError:
                    Status.IsConnected = false;
                    if (ChangedState != null)
                    {
                        ChangedState(StateChange.ConnectionError);
                    }
                    break;
                case comStatus.Started:
                    Status.IsConnected = true;
                    if (ChangedState != null)
                    {
                        ChangedState(StateChange.Started);
                    }
                    break;
                case comStatus.Stopped:
                    Status.IsConnected = false;
                    if (ChangedState != null)
                    {
                        ChangedState(StateChange.Stopped);
                    }
                    break;

            }
        }

        private string ProcessData(byte[] message, bool input)
        {
            string description = "";
            
            if (Settings.Messages.PacketView == Fx.Logging.ePacketView.MARS_A)
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
                    catch (Exception) { }

                    Send(@"\s" + frameNum.ToString());
                }
            }

            var endChar = Settings.Connection.UsedEncoding.GetString(ProtocolCom.FormatMsg(Settings.Messages.LineSeparatingChar, Settings.Connection.UsedEncoding));

            if (Settings.Messages.UseLineSeparatingChar && endChar != "")
            {
                Global.LogPacket.LineSeparatingChar = endChar;
            }
            else
            {
                Global.LogPacket.LineSeparatingChar = "";
            }


            if (Settings.Messages.ShowBaudRate)
            {
                description = Settings.Connection.BaudRate + "[Bd]";
            }

            string text = "";
            if (input)
            {
                text = Global.LogPacket.Add(description, message, Color.Blue, Settings.Messages.ShowTime, input);

                // ----- Send auto-reply -----
                if (Settings.Messages.EnableReplyFile)
                {
                    string key = BitConverter.ToString(message);
                    if (ReplyData.ContainsKey(key))
                    {
                        string value = ReplyData[key];
                        Send(value);
                    }
                }
            }
            else
                text = Global.LogPacket.Add(description, message, Color.Black, Settings.Messages.ShowTime, input);

            return text;
        }

        void LoadAutoSendData()
        {
            AutoSendData = Files.LoadFileLines(Files.ReplaceVarPaths(Settings.Messages.SendingFile), true);
            AutoSendDataIndex = 0;
        }

        private void AutoSendingEvent(object sender, ElapsedEventArgs e)
        {
            if (!AutoSendingLock)
            {
                AutoSendingLock = true;

                if (Status.IsConnected)
                {
                    if (!Settings.Messages.WaitForReply || (Settings.Messages.WaitForReply && ReplyCome))
                    {
                        if (Settings.Messages.EnableSendingFile)
                        {
                            if (AutoSendDataIndex >= 0 && AutoSendData.Length > 0)
                            {
                                string text = AutoSendData[AutoSendDataIndex];
                                AutoSendDataIndex += 1;
                                if (AutoSendDataIndex == AutoSendData.Length)
                                {
                                    if (Settings.Messages.SendingFileRepeating)
                                        AutoSendDataIndex = 0;
                                    else
                                        AutoSendDataIndex--;
                                }

                                if (text != "")
                                {
                                    Send(text);
                                    if (Settings.Messages.WaitForReply) replyTimer.Enabled = true;
                                }
                            }
                        }
                        else
                        {
                            Send(LastSendedMessage);
                            if (Settings.Messages.WaitForReply) replyTimer.Enabled = true;
                        }
                    }
                }

                AutoSendingLock = false;
            }
            
        }

        private void ReplyTimeOutEvent(object sender, ElapsedEventArgs e)
        {
            replyTimer.Enabled = false;
            ReplyCome = true;
        }


    }
}

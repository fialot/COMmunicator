using Fx.IO;
using Fx.Plugins;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fx.Logging
{
    public enum ePacketView
    {
        Custom,
        String,
        StringReplaceCommandChars,
        Bytes,
        Hex,
        MARS_A,
    }

    public delegate void NewRecordEventHandler(LogRecord record);

    public class LogRecord
    {
        public DateTime time = DateTime.MinValue;
        public TimeSpan delay;
        public string description = "";
        public string text = "";
        public byte[] data = new byte[0];
        public Color color = Color.Black;
        public bool withTime = true;
        public bool input = true;


        public LogRecord(DateTime time, string text, Color color)
        {
            this.time = time;
            this.description = text;
            this.text = text;
            this.color = color;

        }

        public LogRecord(DateTime time, byte[] bytes, Color color)
        {
            this.time = time;
            this.description = "";
            this.data = bytes;
            this.text = Encoding.UTF8.GetString(bytes);
            this.color = color;
        }

        public LogRecord(DateTime time, string description, string text, byte[] data, Color color, bool withTime, bool input, TimeSpan delay)
        {
            this.color = color;
            this.description = description;
            this.text = text;
            this.time = time;
            this.data = data;
            this.withTime = withTime;
            this.input = input;
            this.delay = delay;
        }
    }

    public class ProcessLog
    {

        public event NewRecordEventHandler NewRecord;

        public string Name { get; private set; } = "Log";
        
        public int Progress { get; private set; } = 0;
        public List<LogRecord> Recods { get; private set; } = new List<LogRecord>();
        public DateTime LastLogTime { get; private set; } = DateTime.MinValue;

        // ----- Settings -----
        public bool ShowTimeMs { get; set; } = true;
        public bool ShowTimeDate { get; set; } = true;
        public bool ShowDirection { get; set; } = true;
        public int MaxRecords { get; set; } = 100;
        public bool SaveToFile { get; set; } = false;
        public string LogFileDirectory { get; set; } = "";
        public Encoding UsedEncoding { get; set; } = Encoding.UTF8;
        public string LineSeparatingChar { get; set; } = "";


        private IPluginProtocol CustomView = null;
        private ePacketView PacketView = ePacketView.StringReplaceCommandChars;
        

        //public bool EnableSeparateLineWithChar { get; set; } = false;
        

        public ProcessLog(int maxRecords = 100, bool showDate = true, bool showTimeMs = true, bool showDirection = true)
        {
            this.ShowTimeMs = showTimeMs;
            this.ShowTimeDate = showDate;
            this.ShowDirection = showDirection;
            this.MaxRecords = maxRecords;
        }

        /// <summary>
        /// Set name
        /// </summary>
        /// <param name="name"></param>
        public void SetName(string name)
        {
            Name = name;
        }

        public void SetPacketView(ePacketView packetView)
        {
            if (PacketView != packetView)
            {
                PacketView = packetView;

                for (int i = 0; i < Recods.Count; i++)
                {
                    var item = Recods[i];
                    item.text = ConvertToText(item.data, !item.input);
                    item.text = CreateLogText(item.description, item.text, item.withTime, item.input);
                    Recods.RemoveAt(i);
                    Recods.Insert(i, item);
                }
            }
        }

        public void SetCustomView(IPluginProtocol customProtocol)
        {
            CustomView = customProtocol;
        }



        /// <summary>
        /// Clear message
        /// </summary>
        public void ClearLog()
        {
            Recods.Clear();
        }

        /// <summary>
        /// Add Message
        /// </summary>
        /// <param name="Message">Message to show</param>
        public string Add(string description, byte[] data, Color color = default(Color), bool withTime = true, bool input = true)
        {
            string text = "";

            var now = DateTime.Now;

            
            var delay = now - LastLogTime;
            if (LastLogTime == DateTime.MinValue)
                delay = new TimeSpan();

            LastLogTime = now;
            
            text = ConvertToText(data, !input);
            text = CreateLogText(description, text, withTime, input);

            // ----- Add message -----
            var record = new LogRecord(LastLogTime, description, text, data, color, withTime, input, delay);
            Recods.Add(record);

            // ----- Event -----
            if (NewRecord != null) NewRecord(record);

            // ----- Save log to file -----
            if (SaveToFile)
            {
                Save(text);
            }

            // ----- Add rich text -----
           /* rtf.Select(rtf.TextLength, 0);
            rtf.SelectionColor = color;
            rtf.AppendText(text + Environment.NewLine);*/

            // ----- Max records -----
            if (MaxRecords > 0)
            {
                if (Recods.Count >= MaxRecords)
                {
                    var RecordLength = Recods[0].text.Length;
                    Recods.RemoveAt(0);

                    /*rtf.Select(0, RecordLength + 1);
                    rtf.SelectedText = "";*/
                }
            }

            return text;
        }


        /// <summary>
        /// Add Message
        /// </summary>
        /// <param name="Message">Message to show</param>
        public string Add(string description, Color color = default(Color),  bool withTime = true, bool input = true)
        {
            return Add(description, new byte[0], color, withTime, input);
        }

        private string CreateLogText(string description, string data, bool withTime, bool input)
        {
            string text = "";

            // ----- Show time -----
            if (withTime)
            {
                if (ShowTimeDate)
                    text = LastLogTime.ToShortDateString() + "  ";
                if (ShowTimeMs)
                    text += LastLogTime.ToString("HH:mm:ss.fff") + "  ";
                else
                    text += LastLogTime.ToString("HH:mm:ss") + "  ";
            }

            // ----- Show direction array -----
            if (ShowDirection)
            {
                if (input)
                    text += ">>  ";
                else
                    text += "<<  ";
            }

            // ----- Add description ------
            if (description != "")
            {
                text += description;
                if (data != "")
                    text += ": ";
            }
            
            // ----- Add data ------
            text += data;

            return text;
        }

        private string ConvertToText(byte[] message, bool request)
        {
            string text = "";

            switch (PacketView)
            {
                case ePacketView.String:
                    // ----- Remove zero chars -----
                    var messageList = message.ToList();
                    for (int i = messageList.Count - 1; i >= 0; i--)
                    {
                        if (messageList[i] == 0) messageList.RemoveAt(i);
                    }
                    var newMessage = messageList.ToArray();
                    text = UsedEncoding.GetString(newMessage);

                    // ----- Split message if defined separator char -----
                    if (LineSeparatingChar != "")
                    {
                        text = text.Replace(LineSeparatingChar, Environment.NewLine);
                    }

                    text = text.Trim();

                    break;

                case ePacketView.StringReplaceCommandChars:
                    // ----- Create zero mask -----
                    byte[] newMsg = new byte[message.Length];
                    Array.Copy(message, newMsg, message.Length);

                    // ----- Replace zero chars -----
                    for (int i = 0; i < newMsg.Length; i++)
                    {
                        if (newMsg[i] == 0) newMsg[i] = 1;
                    }
                    text = UsedEncoding.GetString(newMsg);
                    var byteText = UsedEncoding.GetBytes(text);

                    if (text.Length == newMsg.Length)
                    {
                        // ----- Replace special chars -----
                        for (int i = text.Length - 1; i >= 0; i--)
                        {
                            // ----- If command char ----
                            if (byteText[i] < 32)
                            {
                                // ----- And not endline or tabulator -----
                                if (byteText[i] != 10 && byteText[i] != 10 && byteText[i] != 13)
                                {
                                    text = text.Remove(i, 1);
                                    if (message[i] == 0)
                                    {
                                        text = text.Insert(i, "{0}");
                                    }
                                    else
                                    {
                                        text = text.Insert(i, "{" + byteText[i].ToString() + "}");
                                    }
                                }
                            }
                        }
                    }

                    // ----- Split message if defined separator char -----
                    if (LineSeparatingChar != "")
                    {
                        text = text.Replace(LineSeparatingChar, Environment.NewLine);
                    }

                    text = text.Trim();

                    break;

                case ePacketView.Bytes:
                    for (int i = 0; i < message.Length; i++)
                    {
                        text += @"\" + message[i].ToString();
                    }
                    break;

                case ePacketView.Hex:
                    for (int i = 0; i < message.Length; i++)
                    {
                        if (text != "") text += " ";
                        text += message[i].ToString("X2");
                    }
                    break;

                case ePacketView.MARS_A:
                    string dataMars = "";

                    if (message.Length > 2)
                    {
                        int frameNum = 0;
                        try
                        {
                            byte[] test = new byte[1];
                            int frame = Conv.SwapBytes(BitConverter.ToUInt16(message, 0));
                            int length = (frame & 2047) - 6; // dala length
                            frameNum = (frame & 12288) + 6 + (128 << 8) + +(1 << 8);
                            for (int i = 8; i < 8 + length; i++)
                            {
                                test[0] = message[i];
                                var encTxt = UsedEncoding.GetString(test);
                                if (message[i] > 31)
                                {
                                    if (message[i] != 0) dataMars += encTxt;
                                }
                                else dataMars += "{" + message[i].ToString() + "}";

                            }
                        }
                        catch (Exception err)
                        {

                        }
                    }
                    text += dataMars;

                    break;

                case ePacketView.Custom:
                    if (CustomView != null)
                    {
                        text = CustomView.ParsePacket(message, request);
                    }
                    break;
            }


            return text;
        }

        private bool Save(string text)
        {
            try
            {
                string path = "";
                if (LogFileDirectory != "")
                {
                    path = LogFileDirectory + Path.DirectorySeparatorChar + "log" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                } else
                {
                    path = "log" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                }

                bool newFile = !File.Exists(path);

                var LogFile = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.None);
                var FileWriter = new StreamWriter(LogFile);


                if (newFile)
                {
                    // ----- IF NEW DAY -> NEW HEAD -----
                    FileWriter.WriteLine("");
                    FileWriter.WriteLine("---------------------------------------------------------");
                    FileWriter.WriteLine("   LOG " + DateTime.Now.ToString("YYYY-MM-dd"));
                    FileWriter.WriteLine("---------------------------------------------------------");
                }

                FileWriter.WriteLine(text);
                FileWriter.Close();
                LogFile.Close();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SaveHeader(string text)
        {
            if (SaveToFile)
            {
                try
                {
                    string path = "";
                    if (LogFileDirectory != "")
                    {
                        path = LogFileDirectory + Path.DirectorySeparatorChar + "log" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                    }
                    else
                    {
                        path = "log" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                    }

                    var LogFile = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.None);
                    var FileWriter = new StreamWriter(LogFile);

                    FileWriter.WriteLine("");
                    FileWriter.WriteLine("---------------------------------------------------------");
                    FileWriter.WriteLine("   " + text);
                    FileWriter.WriteLine("---------------------------------------------------------");
                    FileWriter.Close();
                    LogFile.Close();

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
                return true;
        }



        /// <summary>
        /// Set progress 
        /// </summary>
        /// <param name="progress">Progress (0 - 100) [%]</param>
        public void SetProgress(int progress)
        {
            if (progress < 0) progress = 0;
            if (progress > 100) progress = 100;
            Progress = progress;
        }

        public string Text()
        {
            string text = "";

            foreach(var item in Recods)
            {
                text += item.text + Environment.NewLine;
            }

            return text;
        }

        public string TextRTF()
        {
            RichTextBox rtf = new RichTextBox();

            try
            {
                foreach (var item in Recods)
                {
                    rtf.Select(rtf.TextLength, 0);
                    rtf.SelectionColor = item.color;
                    rtf.AppendText(item.text + Environment.NewLine);
                }
            }
            catch { }

            return rtf.Rtf;
        }
    }
}

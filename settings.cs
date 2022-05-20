using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using Ini;

namespace COMunicator
{
    static class settings
    {
        public struct TSerial
        {
            public string port;
            public int baudrate;
            public string parity;
            public int bits;
            public string stopbits;
            public bool DTR;
            public bool RTS;
        }

        public struct TTCPClient
        {
            public string IP;
            public int port;
            public int localPort;
            public int serverPort;
            public bool UDP;
        }


        public struct TShow
        {
            public bool String;
            public bool Byte;
            public bool HexNum;
            public bool Format;
            public bool MarsA;

            public bool Line;
            public string LineNum;
            public bool ShowCommandChars;
            public string FormatString;
            
            public bool Time;
            public bool BaudRate;
        }

        public struct TFun
        {
            public bool NoClear;
            public bool AutoReply;
            public bool AutoSend;
            public int AutoSendDelay;
            public bool IsEndChar;
            public string EndChar;

            public bool WaitForReply;
            public int ReplyTimeout;
        }

        public struct TPaths
        {
            public string logFile;
            public bool logEnable;
            public bool logNewFile;
            public string dataFolder;

            public string SendingFile;
            public bool EnableSendingFile;
            public bool BeginAfterEoF;

            public string ReplyFile;
        }

        public static TSerial SP;
        public static TTCPClient TCPClient;
        public static TShow Show;
        public static TFun Fun;
        public static TPaths Paths;

        public static Encoding encoding = Encoding.Default;
        
        public static int tab = 0;


        public static void LoadSettings()
        {
            IniParser ini;
            ini = new IniParser("./config.ini");

            // ----- Serial Port -----
            SP.port = ini.Read("Serial", "Com", "");
            SP.baudrate = ini.ReadInt("Serial", "Baud", 115200);
            SP.parity = ini.Read("Serial", "Parity", "None");
            SP.bits = ini.ReadInt("Serial", "DataBits", 8);
            SP.stopbits = ini.Read("Serial", "StopBits", "One");
            SP.DTR = ini.ReadBool("Serial", "DTR", false);
            SP.RTS = ini.ReadBool("Serial", "RTS", false);

            // ----- TCP/UDP -----
            TCPClient.IP = ini.Read("TCPClient", "IP", "192.168.1.1");
            TCPClient.port = ini.ReadInt("TCPClient", "Port", 17000);
            TCPClient.localPort = ini.ReadInt("TCPClient", "localPort", 17001);
            TCPClient.serverPort = ini.ReadInt("TCPClient", "serverPort", 17000);
            TCPClient.UDP = ini.ReadBool("TCPClient", "UDP", false);


            // ----- SHOW -----
            Show.String = ini.ReadBool("Show", "String", true);
            Show.Byte = ini.ReadBool("Show", "Byte", false);
            Show.HexNum = ini.ReadBool("Show", "Hex", false);
            Show.Format = ini.ReadBool("Show", "Format", false);
            Show.MarsA = ini.ReadBool("Show", "MarsA", false);
            Show.Line = ini.ReadBool("Show", "Line", false);
            Show.LineNum = ini.Read("Show", "LineNum", "");
            Show.ShowCommandChars = ini.ReadBool("Show", "ShowCommandChars", false);
            Show.FormatString = ini.Read("Show", "FormatString", "");

            Show.Time = ini.ReadBool("Show", "Time", false);
            Show.BaudRate = ini.ReadBool("Show", "Baud", false);

            // ----- PATHS -----
            Paths.logFile = ini.Read("Path", "LogFile", @"%AppPath%");
            Paths.logEnable = ini.ReadBool("Path", "LogEnable", true);
            Paths.logNewFile = ini.ReadBool("Path", "LogNewFile" ,false);
            Paths.dataFolder = ini.Read("Path", "dataFolder", @"%AppData%" + System.IO.Path.DirectorySeparatorChar + "COMunicator");
            Paths.SendingFile = ini.Read("Path", "SendingFile", @"%AppPath%" + System.IO.Path.DirectorySeparatorChar + "sending.txt");
            Paths.EnableSendingFile = ini.ReadBool("Path", "SendingFileEnable", true);
            Paths.BeginAfterEoF = ini.ReadBool("Path", "SendingBeginAfterEoF", true);
            Paths.ReplyFile = ini.Read("Path", "ReplyFile", @"%AppPath%" + System.IO.Path.DirectorySeparatorChar + "reply.txt");

            // ----- Sending -----
            Fun.NoClear = ini.ReadBool("Send", "NoClear", false);
            Fun.AutoReply = ini.ReadBool("Send", "AutoReply", false);
            Fun.AutoSend = ini.ReadBool("Send", "AutoSend", false);
            Fun.AutoSendDelay = ini.ReadInt("Send", "AutoSendDelay", 1000);
            Fun.IsEndChar = ini.ReadBool("Send", "IsEndChar", false);
            Fun.EndChar = ini.Read("Send", "EndChar", "");
            Fun.WaitForReply = ini.ReadBool("Send", "WaitForReply", false);


            tab = ini.ReadInt("GUI", "Tab", 0);
            
            string enc = ini.Read("Send", "Encoding", Encoding.Default.HeaderName);

            EncodingInfo[] x = Encoding.GetEncodings();     // načíst dostupné kódování

            bool encExist = false;
            for (int i = 0; i < x.GetLength(0); i++)        // kontrola je-li existující kódování
            {
                if (x[i].Name == enc)                  //  kontrola podle jména
                {
                    encExist = true;
                    try
                    {
                        encoding = Encoding.GetEncoding(enc);
                    }
                    catch (Exception)
                    {
                        encoding = System.Text.Encoding.Default;
                    }
                    break;
                }
            }

            if (!encExist)                                  // pokud neexistuje -> default
            {
                enc = Encoding.Default.HeaderName;
                encoding = System.Text.Encoding.Default;
            }
        }

        public static void SaveSettings()
        {
            IniParser ini;
            ini = new IniParser("./config.ini");

            ini.Write("Serial", "Com", SP.port);
            ini.Write("Serial", "Baud", SP.baudrate);
            ini.Write("Serial", "Parity", SP.parity);
            ini.Write("Serial", "DataBits", SP.bits);
            ini.Write("Serial", "StopBits", SP.stopbits);
            ini.Write("Serial", "DTR", SP.DTR);
            ini.Write("Serial", "RTS", SP.RTS);

            ini.Write("TCPClient", "IP", TCPClient.IP);
            ini.Write("TCPClient", "Port", TCPClient.port);
            ini.Write("TCPClient", "localPort", TCPClient.localPort);
            ini.Write("TCPClient", "serverPort", TCPClient.serverPort);
            ini.Write("TCPClient", "UDP", TCPClient.UDP);

            ini.Write("Show", "String", Show.String);
            ini.Write("Show", "Byte", Show.Byte);
            ini.Write("Show", "Hex", Show.HexNum);
            ini.Write("Show", "Format", Show.Format);
            ini.Write("Show", "MarsA", Show.MarsA);

            ini.Write("Show", "Line", Show.Line);
            ini.Write("Show", "LineNum", Show.LineNum);
            ini.Write("Show", "ShowCommandChars", Show.ShowCommandChars);
            ini.Write("Show", "FormatString", Show.FormatString);

            ini.Write("Show", "Time", Show.Time);
            ini.Write("Show", "Baud", Show.BaudRate);

            // ----- PATHS -----
            ini.Write("Path", "LogFile", Paths.logFile);
            ini.Write("Path", "LogEnable", Paths.logEnable);
            ini.Write("Path", "LogNewFile", Paths.logNewFile);
            ini.Write("Path", "dataFolder", Paths.dataFolder);
            ini.Write("Path", "SendingFile", Paths.SendingFile);
            ini.Write("Path", "SendingFileEnable", Paths.EnableSendingFile);
            ini.Write("Path", "SendingBeginAfterEoF", Paths.BeginAfterEoF);
            ini.Write("Path", "ReplyFile", Paths.ReplyFile);

            

            ini.Write("Send", "NoClear", Fun.NoClear);
            ini.Write("Send", "AutoReply", Fun.AutoReply);
            ini.Write("Send", "AutoSend", Fun.AutoSend);
            ini.Write("Send", "AutoSendDelay", Fun.AutoSendDelay);
            ini.Write("Send", "IsEndChar", Fun.IsEndChar);
            ini.Write("Send", "EndChar", Fun.EndChar);
            ini.Write("Send", "WaitForReply", Fun.WaitForReply);

            ini.Write("Send", "Encoding", encoding.HeaderName);


            ini.Write("GUI", "Tab", tab);

            ini.SaveFile();

        }
    }
}

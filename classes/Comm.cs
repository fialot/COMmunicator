using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;
//using System.Timers;
using System.Net.Sockets;
using TCPClient;
using myFunctions;
using COMunicator.Protocol;


namespace COMunicator
{
    /// <summary>
    /// Namespace for Serial port / TCP/IP communication class
    /// </summary>
    static class NamespaceDoc { }

    struct msgFunction
    {
        public string name;
        public int length;
        public string[] arguments;
    }

    /// <summary>
    /// Received data handler delegate
    /// </summary>
    /// <param name="source"></param>
    /// <param name="status">Communication status (received commands: close connection)</param>
    public delegate void ReceivedEventHandler(object source, comStatus status);

    /// <summary>
    /// Serial port / TCP/IP communication class
    /// </summary>
    public class Comm
    {
        /// <summary>
        /// Type of communication intarface
        /// </summary>
        public enum interfaces { None, COM, TCPClient, TCPServer, Udp };

        /// <summary>
        /// Serial port class
        /// </summary>
        private SerialPort SP;
        //private System.Timers.Timer TimeOut;

        /// <summary>
        /// Selected communication intarface
        /// </summary>
        public interfaces OpenInterface;

        /// <summary>
        /// Communication encoding
        /// </summary>
        private Encoding encoding;

        /// <summary>
        /// Communication buffer
        /// </summary>
        private byte[] comBuffer;

        /// <summary>
        /// Last received data
        /// </summary>
        private byte[] PrevData;

        /// <summary>
        /// Received data handler
        /// </summary>
        public event ReceivedEventHandler ReceivedData;

        private bool asynch;


        #region Constructor

        /// <summary>
        /// Communication class contructor
        /// </summary>
        public Comm()
        {
            init();
            encoding = System.Text.Encoding.Default;
        }

        /// <summary>
        /// Communication class contructor
        /// </summary>
        /// <param name="encoding">Used communication encoding</param>
        public Comm(Encoding encoding)
        {
            init();
            this.encoding = encoding;
        }


        /// <summary>
        /// Class initialization
        /// </summary>
        private void init()
        {
            OpenInterface = interfaces.None;
            SP = new SerialPort();
            SP.DataReceived += new SerialDataReceivedEventHandler(SP_Received);
            SocketClient.ReceivedData += new NetReceivedEventHandler(TCP_Received);
            asynch = false;

            //SocketClient.AsyncReceive += new AsyncReceivedEventHandlerTCP(TCP_Received);
            //TimeOut = new System.Timers.Timer(20);
            //TimeOut.Elapsed += new ElapsedEventHandler(TimeOut_Elapsed);
        }

        #endregion

        public void SetEncoding(Encoding encoding)
        {
            this.encoding = encoding;
        }

        public void SP_Received(object source, SerialDataReceivedEventArgs e)
        {
            int bytes = SP.BytesToRead;

            if (bytes > 0)
            {
                comBuffer = new byte[bytes];
                SP.Read(comBuffer, 0, bytes);
                PrevData = AddArray(PrevData, comBuffer);
                comBuffer = PrevData;
                if (ReceivedData != null)
                    ReceivedData(this, comStatus.OK);
            }
        }

        public void TCP_Received(byte[] data, comStatus status)
        {
            if (status == comStatus.OK)
            {
                comBuffer = data;
                PrevData = AddArray(PrevData, comBuffer);
                comBuffer = PrevData;
                ReceivedData(this, comStatus.OK);
            } 
            else if (status == comStatus.Close)
            {
                if (OpenInterface != interfaces.TCPServer)
                    Close();
                ReceivedData(this, comStatus.Close);
            }
            else
            {
                ReceivedData(this, status);
            }

        }

        #region Open

        /// <summary>
        /// Open Serial Port
        /// </summary>
        /// <param name="COM">Name of serial port</param>
        /// <param name="baud">Baud rate</param>
        /// <param name="parity">Parity</param>
        /// <param name="databits">Data bits</param>
        /// <param name="stopbits">Stop bits</param>
        /// <exception cref="System.ArgumentException">Thrown when...</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when...</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when...</exception>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when...</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when...</exception>
        /// <exception cref="System.IO.IOException">Thrown when...</exception>
        public void Open(string COM)
        {
            SP.PortName = COM;

            SP.Open();
            OpenInterface = interfaces.COM;
            comBuffer = new byte[0];
            PrevData = new byte[0];
            if (ReceivedData == null) asynch = false;
            else asynch = true;

        }

        /// <summary>
        /// Open Serial Port
        /// </summary>
        /// <param name="COM">Name of serial port</param>
        /// <param name="baud">Baud rate</param>
        /// <param name="parity">Parity</param>
        /// <param name="databits">Data bits</param>
        /// <param name="stopbits">Stop bits</param>
        /// <exception cref="System.ArgumentException">Thrown when...</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when...</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when...</exception>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when...</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when...</exception>
        /// <exception cref="System.IO.IOException">Thrown when...</exception>
        public void Open(string COM, int baud)
        {
            SP.BaudRate = baud;      
                    
            Open(COM);
        }

        /// <summary>
        /// Open Serial Port
        /// </summary>
        /// <param name="COM">Name of serial port</param>
        /// <param name="baud">Baud rate</param>
        /// <param name="parity">Parity</param>
        /// <param name="databits">Data bits</param>
        /// <param name="stopbits">Stop bits</param>
        /// <exception cref="System.ArgumentException">Thrown when...</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when...</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when...</exception>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when...</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when...</exception>
        /// <exception cref="System.IO.IOException">Thrown when...</exception>
        public void Open(string COM, int baud, Parity parity, int databits, StopBits stopbits)
        {
            SP.BaudRate = baud;
            SP.Parity = parity;
            SP.DataBits = databits;
            SP.StopBits = stopbits;

            Open(COM);
        }

        /// <summary>
        /// Connect to Udp
        /// </summary>
        /// <param name="lisenPort">Lissen port number</param>
        /// <exception cref="System.IO.IOException">Thrown when...</exception>
        public void ConnectUdp(int lisenPort)
        {
            if (ReceivedData == null) asynch = false;
            else asynch = true;
            bool res = SocketClient.ConnectUdp(lisenPort, asynch);
            if (res == true)
            {
                OpenInterface = interfaces.Udp;
                comBuffer = new byte[0];
                PrevData = new byte[0];
            }
            else
                throw new System.IO.IOException("Connect Error");
        }

        /// <summary>
        /// Connect to Udp
        /// </summary>
        /// <param name="hostName">Udp server host name</param>
        /// <param name="portNumber">Port number</param>
        /// <exception cref="System.IO.IOException">Thrown when...</exception>
        public void ConnectUdp(string hostName, int hostPort, int lisenPort = -1)
        {
            if (ReceivedData == null) asynch = false;
            else asynch = true;
            bool res = SocketClient.ConnectUdp(hostName, hostPort,lisenPort, asynch);
            if (res == true)
            {
                OpenInterface = interfaces.Udp;
                comBuffer = new byte[0];
                PrevData = new byte[0];
            }
            else
                throw new System.IO.IOException("Connect Error");
        }

        /// <summary>
        /// Connect to TCP/IP server
        /// </summary>
        /// <param name="hostName">TCP/IP server host name</param>
        /// <param name="portNumber">Port number</param>
        /// <exception cref="System.IO.IOException">Thrown when...</exception>
        public void ConnectTcp(string hostName, int portNumber)
        {
            if (ReceivedData == null) asynch = false;
            else asynch = true;
            bool res = SocketClient.ConnectTcp(hostName, portNumber, asynch);
            if (res == true)
            {
                OpenInterface = interfaces.TCPClient;
                comBuffer = new byte[0];
                PrevData = new byte[0];
            }
            else 
                throw new System.IO.IOException("Connect Error");
        }

        public void CreateTCPServer(int portNumber)
        {
            if (ReceivedData == null) asynch = false;
            else asynch = true;
            bool res = SocketClient.CreateTcp(portNumber,asynch);
            if (res == true)
            {
                OpenInterface = interfaces.TCPServer;
                comBuffer = new byte[0];
                PrevData = new byte[0];
            }
            else
                throw new System.IO.IOException("Connect Error");
        }

        #endregion


        /// <summary>
        /// Set Serial Port baudrate
        /// </summary>
        /// <param name="baud">Baudrate</param>
        public void SetBaudRate(int baud)
        {
            SP.BaudRate = baud;
        }

        public void SetParamsSP(Parity parity = Parity.None, int databits = 8, StopBits stopbits = StopBits.One, bool DTR = false, bool RTS = false)
        {
            //SP.PortName = COM;
            //SP.BaudRate = baud;
            SP.DataBits = databits;
            SP.Parity = parity;
            SP.StopBits = stopbits;
            SP.RtsEnable = RTS;
            SP.DtrEnable = DTR;
        }

        /// <summary>
        /// Read received data from buffer
        /// </summary>
        /// <returns>Data</returns>
        public byte[] Read()
        {
            byte[] res = ReadData();
            return res;
        }

        /// <summary>
        /// Read received data from buffer
        /// </summary>
        /// <returns>Data</returns>
        public string ReadString()
        {
            byte[] res = ReadData();
            return encoding.GetString(res);
        }

        /// <summary>
        /// Read received data from buffer
        /// </summary>
        /// <returns>Data</returns>
        public string ReadString(int timeout)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            byte[] res = ReadData(timeout);
            
            while ((res.Length == 0) && (stopwatch.ElapsedMilliseconds < timeout))
            {
                System.Windows.Forms.Application.DoEvents();
                System.Threading.Thread.Sleep(50);
                res = ReadData(timeout);
            }
            comBuffer = new byte[0];
            PrevData = new byte[0];
            return encoding.GetString(res);



        }

        private byte[] ReadData(int timeout = -1)
        {
            byte[] res = new byte[0];
            if (asynch)
            {
                res = comBuffer;
                comBuffer = new byte[0];
                PrevData = new byte[0];
            }
            else
            {
                if (OpenInterface == interfaces.TCPClient || OpenInterface == interfaces.TCPServer || OpenInterface == interfaces.Udp) res = SocketClient.Receive(timeout);
                else if (OpenInterface == interfaces.COM)
                {
                    int bytes = SP.BytesToRead;

                    if (bytes > 0)
                    {
                        res = new byte[bytes];
                        SP.Read(res, 0, bytes);
                    }
                }
            }
            return res;
        }

        public void ClearInput()
        {
            byte[] x = ReadData(1);
        }
            

        /// <summary>
        /// Close connection
        /// </summary>
        public void Close()
        {
            try
            {
                switch (OpenInterface)
                {
                    case interfaces.COM:
                        SP.Close();
                        break;
                    case interfaces.TCPClient:
                    case interfaces.TCPServer:
                    case interfaces.Udp:
                        SocketClient.Close();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception err)
            {

            }
            OpenInterface = interfaces.None;
        }

        /// <summary>
        /// Get connection status
        /// </summary>
        /// <returns>Return True if connection is open</returns>
        public bool IsOpen()
        {
            if (OpenInterface == interfaces.None) return false;
            else return true;
        }

        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="data">Data</param>
        public void Send(byte[] data)
        {
            switch (OpenInterface)
            {
                case interfaces.COM:
                    try
                    {
                        SP.WriteTimeout = 1000;
                        SP.Write(data, 0, data.Length);
                    }
                    catch(Exception)
                    {
                        ReceivedData(this, comStatus.Close);
                        Close();
                    }
                    break;
                case interfaces.TCPClient:
                case interfaces.TCPServer:
                case interfaces.Udp:
                    SocketClient.Send(data);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="data">Data</param>
        public void Send(string data)
        {
            switch (OpenInterface)
            {
                case interfaces.COM:
                    try
                    {
                        SP.WriteTimeout = 1000;
                        SP.Write(data);
                    }
                    catch (Exception)
                    {
                        ReceivedData(this, comStatus.Close);
                        Close();
                    }
                    break;
                case interfaces.TCPClient:
                case interfaces.TCPServer:
                case interfaces.Udp:
                    SocketClient.Send(encoding.GetBytes(data));
                    break;
                default:
                    break;
            }
        }

        #region Functions

        /// <summary>
        /// Format Message
        /// </summary>
        /// <param name="msg">Message string</param>
        /// <returns>Data to send</returns>
        public byte[] FormatMsg(string msg)
        {
            int pos, lastpos, count = 0;
            string cmd, ins, prefix, prefix1;
            byte num;
            int inum;
            ushort snum;
            float fnum;
            byte[] result;
            byte[] insBytes;

            List<byte> byteList = new List<byte>();
            List<bool> boolList = new List<bool>();
            for (int i = 0; i < msg.Length; i++)
            {
                byteList.Add(encoding.GetBytes(msg.Substring(i,1))[0]);
                boolList.Add(false);
            }
            result = byteList.ToArray();

            if (msg.Contains(@"\"))
            {
                pos = msg.IndexOf(@"\", 0);
                lastpos = pos;

                while (pos >= 0)
                {
                    cmd = FindCmd(msg, pos, ref count);
                    prefix = "";
                    prefix1 = "";
                    if (cmd.Length >= 2) prefix = cmd.Substring(0, 2);
                    if (cmd.Length >= 1) prefix1 = cmd.Substring(0, 1);
                    msgFunction fun = isFunction(msg, pos);

                    if (cmd == @"\")// lastpos++;
                    {
                        remove(ref msg, ref byteList, ref boolList, pos, 2);
                        insBytes = encoding.GetBytes("\\");
                        insert(ref msg, ref byteList, ref boolList, pos, insBytes);
                    }
                    else if (fun.name != null && fun.name != "")
                    {
                        remove(ref msg, ref byteList, ref boolList, pos, fun.length + 1);

                        insBytes = new byte[0];
                        if (fun.name == "file")
                        {
                            ins = "";
                            if (fun.arguments.Length > 0)
                                ins = Files.LoadFile(fun.arguments[0]);
                            insBytes = encoding.GetBytes(ins);
                        }
                        else if (fun.name == "marsa")
                        {
                            if (fun.arguments.Length >= 2)
                            {
                                insBytes = Protocol.Protocol.MarsA(Conv.HexToUInt(fun.arguments[0]), FormatMsg(fun.arguments[1]));
                                lastpos = -1;
                            }
                        }
                        else if (fun.name == "nuvia")
                        {
                            if (fun.arguments.Length >= 1)
                            {
                                if (fun.arguments.Length >= 3)
                                    insBytes = Protocol.Protocol.Nuvia(Conv.ToIntDef(fun.arguments[0], 0), Conv.HexToByte(fun.arguments[1]), FormatMsg(fun.arguments[2]));
                                else if (fun.arguments.Length >= 2)
                                    insBytes = Protocol.Protocol.Nuvia(0, Conv.HexToByte(fun.arguments[0]), FormatMsg(fun.arguments[1]));
                                else if (fun.arguments.Length >= 1)
                                    insBytes = Protocol.Protocol.Nuvia(0, Conv.HexToByte(fun.arguments[0]), new byte[0]);
                                lastpos = -1;
                            }
                        }
                        insert(ref msg, ref byteList, ref boolList, pos, insBytes);
                    }
                    else if (prefix1 == "x" || prefix1 == "$")
                    {
                        insBytes = Conv.HexToBytes(cmd.Remove(0, 1).Trim());
                        remove(ref msg, ref byteList, ref boolList, pos, count + 1);
                        insert(ref msg, ref byteList, ref boolList, pos, insBytes);
                    } // hex
                    else if ((prefix1 == "i") && (int.TryParse(cmd.Remove(0, 1).Trim(), out inum)))
                    {
                        insBytes = BitConverter.GetBytes(inum);
                        //Array.Reverse(insBytes);
                        remove(ref msg, ref byteList, ref boolList, pos, count + 1);
                        insert(ref msg, ref byteList, ref boolList, pos, insBytes);
                    } // hex
                    else if ((prefix1 == "s") && (ushort.TryParse(cmd.Remove(0, 1).Trim(), out snum)))
                    {

                        insBytes = BitConverter.GetBytes(snum);
                        //Array.Reverse(insBytes);
                        remove(ref msg, ref byteList, ref boolList, pos, count + 1);
                        insert(ref msg, ref byteList, ref boolList, pos, insBytes);
                    } 
                    else if ((prefix1 == "f") && (float.TryParse(cmd.Remove(0, 1).Trim(), out fnum)))
                    {

                        insBytes = BitConverter.GetBytes(fnum);
                        //Array.Reverse(insBytes);
                        remove(ref msg, ref byteList, ref boolList, pos, count + 1);
                        insert(ref msg, ref byteList, ref boolList, pos, insBytes);
                    } // \n -> <10>
                    else if ((prefix1 == "\'") || (prefix1 == "\"") || (prefix1 == "\\") || (prefix1 == "a") || (prefix1 == "b") || (prefix1 == "f") || (prefix1 == "n") || (prefix1 == "r") || (prefix1 == "t") || (prefix1 == "v"))
                    {
                        switch (prefix1)
                        {
                            case "'":
                                ins = "\'";
                                break;
                            case "\"":
                                ins = "\"";
                                break;
                            case "\\":
                                ins = "\\";
                                break;
                            case "a":
                                ins = "\a";
                                break;
                            case "b":
                                ins = "\b";
                                break;
                            case "f":
                                ins = "\f";
                                break;
                            case "n":
                                ins = "\n";
                                break;
                            case "r":
                                ins = "\r";
                                break;
                            case "t":
                                ins = "\t";
                                break;
                            case "v":
                                ins = "\v";
                                break;
                            default:
                                ins = "\n";
                                break;
                        }
                        //ins = "\n";
                        if (cmd.Length >= 2)
                        {
                            if (cmd[1] == ' ') remove(ref msg, ref byteList, ref boolList, pos, 3);
                            else remove(ref msg, ref byteList, ref boolList, pos, 2);
                        }
                        else remove(ref msg, ref byteList, ref boolList, pos, 2);
                        insBytes = encoding.GetBytes(ins);
                        insert(ref msg, ref byteList, ref boolList, pos, insBytes);
                    } // hex
                    else if (byte.TryParse(cmd, out num))
                    {
                        insBytes = new byte[1];
                        insBytes[0] = num;
                        remove(ref msg, ref byteList, ref boolList, pos, count + 1);
                        insert(ref msg, ref byteList, ref boolList, pos, insBytes);
                    }
                    else
                    {
                        remove(ref msg, ref byteList, ref boolList, pos, 1);
                    }

                    if (msg.Length > lastpos + 1)
                        pos = msg.IndexOf(@"\", lastpos + 1);
                    else pos = -1;
                }
                result = byteList.ToArray();
                for (int i = 0; i < result.Length; i++)
                {
                    if (boolList[i] == false)
                    {
                        insBytes = encoding.GetBytes(msg.Substring(i, 1));
                        remove(ref msg, ref byteList, ref boolList, i, 1);
                        insert(ref msg, ref byteList, ref boolList, i, insBytes);
                    }
                }
            }
            return result;
        }

        private string FindCmd(string msg, int pos, ref int count)
        {
            int len = msg.Length - pos - 1;
            int p, p1, p2;
            if (len > 0)
            {
                if (msg[pos + 1] == '\\')
                {
                    count = 1;
                    return "\\";
                }
                else
                {
                    p1 = msg.IndexOf('\\', pos + 1);
                    p2 = msg.IndexOf(' ', pos + 1);
                    if (p1 < 0) p1 = int.MaxValue;
                    if (p2 < 0) p2 = int.MaxValue;

                    if (p1 < p2) p = p1;
                    else p = p2;

                    if (p < int.MaxValue)
                    {
                        if (p == p1)
                        {
                            count = p - pos - 1;
                            return msg.Substring(pos + 1, count);
                        }
                        else
                        {
                            count = p - pos;
                            return msg.Substring(pos + 1, count);
                        }
                    }
                    else
                    {
                        count = len;
                        return msg.Substring(pos + 1, count);
                    }
                }
            }
            else
            {
                count = 0;
                return "";
            }
        }

        private void remove(ref string msg, ref List<byte> byteList, ref List<bool> boolList, int pos, int len)
        {
            msg = msg.Remove(pos, len);
            for (int i = 0; i < len; i++)
            {
                byteList.RemoveAt(pos);
                boolList.RemoveAt(pos);
            }
        }

        private void insert(ref string msg, ref List<byte> byteList, ref List<bool> boolList, int pos, byte[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                msg = msg.Insert(pos, "-");
                byteList.Insert(pos + i, array[i]);
                boolList.Insert(pos + i, true);
            }
        }

        private msgFunction isFunction(string msg, int position)
        {
            msgFunction fun = new msgFunction();
            int p1 = msg.IndexOf('(', position);
            int p2 = msg.IndexOf(')', position + 1);
            if (p1 >= 0 && p2 >= 0)
            {
                fun.name = msg.Substring(position + 1, p1 - 1 - position);
                if (fun.name.Contains("\\") || fun.name.Contains(" "))
                {
                    fun.name = "";
                    return fun;
                }

                fun.arguments = msg.Substring(p1 + 1, p2 - (p1 + 1)).Split(new string[] { ";" }, StringSplitOptions.None);
                for (int i = 0; i < fun.arguments.Length; i++)
                {
                    fun.arguments[i] = fun.arguments[i].Trim();
                    if (fun.arguments.Length > 0)
                    {
                        if (fun.arguments[i].Length > 0)
                        {
                            if (fun.arguments[i][0] == '\"' && fun.arguments[i][fun.arguments[i].Length - 1] == '\"')
                                fun.arguments[i] = fun.arguments[i].Substring(1, fun.arguments[i].Length - 2);
                        }
                    }
                }
                fun.length = p2 - position;
                if (msg.Length > p2 + 1)
                {
                    if (msg[p2 + 1] == ' ') fun.length += 1;
                }
            }
            return fun;
        }

        public byte[] AddArray(byte[] Data1, byte[] Data2)
        {
            byte[] X;
            if (Data1 == null) Data1 = new byte[0];
            if (Data2 == null) Data2 = new byte[0];

            int n = Data1.Length;
            X = new byte[n + Data2.Length];

            for (int i = 0; i < n; i++)
            {
                X[i] = Data1[i];
            }
            for (int i = 0; i < Data2.Length; i++)
            {
                X[i + n] = Data2[i];
            }

            return X;
        }

        #endregion
    }
}

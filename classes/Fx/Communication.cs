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
using System.Timers;

namespace Fx.IO
{
    /// <summary>
    /// Namespace for Serial port / TCP/IP communication class
    /// </summary>
    static class NamespaceDoc { }

    /// <summary>
    /// Communication Exception (with sended packets)
    /// </summary>
    public class CommException : Exception
    {
        public string SendedString { get; }
        public string ReceivedString { get; }
        public byte[] SendedData { get; }
        public byte[] ReceivedData { get; }

        public CommException() : base() { }
        public CommException(string message) : base(message) { }
        public CommException(string message, Exception innerException) : base(message, innerException) { }
        public CommException(string message, string sended, string received) : base(message)
        {
            this.SendedString = sended;
            this.ReceivedString = received;
        }
        public CommException(string message, byte[] SendedData, byte[] ReceivedData) : base(message)
        {
            this.SendedData = SendedData;
            this.ReceivedData = ReceivedData;
        }
    }


    struct msgFunction
    {
        public string name;
        public int length;
        public string[] arguments;
    }

    /// <summary>
    /// Type of communication intarface
    /// </summary>
    public enum interfaces { None, COM, TCPClient, TCPServer, Udp };

    /// <summary>
    /// Received data handler delegate
    /// </summary>
    /// <param name="source"></param>
    /// <param name="status">Communication status (received commands: close connection)</param>
    public delegate void ReceivedEventHandler(object source, comStatus status);

    /// <summary>
    /// Serial port / TCP/IP communication class
    /// </summary>
    public class Communication
    {
        public int LastCharInterval { get; set; } = 20;

        /// <summary>
        /// Serial port class
        /// </summary>
        private SerialPort SP;
        //private System.Timers.Timer TimeOut;

        /// <summary>
        /// Selected communication intarface
        /// </summary>
        public interfaces OpenedInterface;

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
        private byte[] RecData;

        /// <summary>
        /// Received data handler
        /// </summary>
        public event ReceivedEventHandler ReceivedData;

        private bool asynch = false;
        private Timer lastCharTimer = new Timer();


        #region Constructor

        /// <summary>
        /// Communication class contructor
        /// </summary>
        public Communication()
        {
            init();
            encoding = System.Text.Encoding.Default;
        }

        /// <summary>
        /// Communication class contructor
        /// </summary>
        /// <param name="encoding">Used communication encoding</param>
        public Communication(Encoding encoding)
        {
            init();
            this.encoding = encoding;
        }


        /// <summary>
        /// Class initialization
        /// </summary>
        private void init()
        {
            OpenedInterface = interfaces.None;
            SP = new SerialPort();
            SP.WriteBufferSize = 8192;
            SP.DataReceived += new SerialDataReceivedEventHandler(SP_Received);
            SocketClient.ReceivedData += new NetReceivedEventHandler(TCP_Received);
            lastCharTimer.Elapsed += new ElapsedEventHandler(Timer_Received);

            //SocketClient.AsyncReceive += new AsyncReceivedEventHandlerTCP(TCP_Received);
            //TimeOut = new System.Timers.Timer(20);
            //TimeOut.Elapsed += new ElapsedEventHandler(TimeOut_Elapsed);
        }

        #endregion

        public void SetEncoding(Encoding encoding)
        {
            this.encoding = encoding;
        }

        public void Timer_Received(object sender, ElapsedEventArgs e)
        {
            lastCharTimer.Enabled = false;

            if (ReceivedData != null)
                ReceivedData(this, comStatus.OK);
        }

        public void SP_Received(object source, SerialDataReceivedEventArgs e)
        {
            if (asynch)
            {
                int bytes = SP.BytesToRead;

                if (bytes > 0)
                {
                    comBuffer = new byte[bytes];
                    SP.Read(comBuffer, 0, bytes);
                    RecData = AddArray(RecData, comBuffer);
                    if (LastCharInterval > 0)
                    {
                        lastCharTimer.Interval = LastCharInterval;
                        lastCharTimer.Enabled = true;
                    }
                    else
                    {
                        if (ReceivedData != null)
                            ReceivedData(this, comStatus.OK);
                    }
                }
            }
        }

        public void TCP_Received(byte[] data, comStatus status)
        {
            if (status == comStatus.OK)
            {
                comBuffer = data;
                RecData = AddArray(RecData, comBuffer);
                comBuffer = RecData;
                if (LastCharInterval > 0)
                {
                    lastCharTimer.Interval = LastCharInterval;
                    lastCharTimer.Enabled = true;
                }
                else
                {
                    if (ReceivedData != null)
                        ReceivedData(this, comStatus.OK);
                }
            }
            else if (status == comStatus.Close)
            {
                if (OpenedInterface != interfaces.TCPServer)
                    Close();
                if (ReceivedData != null)
                    ReceivedData(this, comStatus.Close);
            }
            else
            {
                if (ReceivedData != null)
                    ReceivedData(this, status);
            }

        }

        #region Connect

        public void Connect(ConnectionSetting settings)
        {
            encoding = settings.UsedEncoding;

            switch (settings.Type)
            {
                case ConnectionType.Serial:
                    SetSPParams(settings.BaudRate, settings.Parity, settings.DataBits, settings.StopBits, settings.DTR, settings.RTS);
                    ConnectSP(settings.SerialPort);
                    break;
                case ConnectionType.TCP:
                    ConnectTcp(settings.IP, settings.Port);
                    break;
                case ConnectionType.UDP:
                    ConnectUdp(settings.IP, settings.Port, settings.LocalPort);
                    break;
                case ConnectionType.TCPServer:
                    CreateTCPServer(settings.LocalPort);
                    break;
            }
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
        public void ConnectSP(string COM)
        {
            SP.PortName = COM;
            try
            {
                SP.Open();

                if (ReceivedData != null)
                    ReceivedData(this, comStatus.Open);
            }
            catch (Exception)
            {
                if (ReceivedData != null)
                    ReceivedData(this, comStatus.OpenError);
                throw new System.IO.IOException("Connect Error");
            }

            OpenedInterface = interfaces.COM;
            comBuffer = new byte[0];
            RecData = new byte[0];
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
        public void ConnectSP(string COM, int baud)
        {
            SP.BaudRate = baud;

            ConnectSP(COM);
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
        public void ConnectSP(string COM, int baud, Parity parity, int databits, StopBits stopbits)
        {
            SP.BaudRate = baud;
            SP.Parity = parity;
            SP.DataBits = databits;
            SP.StopBits = stopbits;

            ConnectSP(COM);
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
                OpenedInterface = interfaces.Udp;
                comBuffer = new byte[0];
                RecData = new byte[0];

                if (ReceivedData != null)
                    ReceivedData(this, comStatus.Open);
            }
            else
            {
                if (ReceivedData != null)
                    ReceivedData(this, comStatus.OpenError);
                throw new System.IO.IOException("Connect Error");
            }
                
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
            bool res = SocketClient.ConnectUdp(hostName, hostPort, lisenPort, asynch);
            if (res == true)
            {
                OpenedInterface = interfaces.Udp;
                comBuffer = new byte[0];
                RecData = new byte[0];
                if (ReceivedData != null)
                    ReceivedData(this, comStatus.Open);
            }
            else
            {
                if (ReceivedData != null)
                    ReceivedData(this, comStatus.OpenError);
                throw new System.IO.IOException("Connect Error");
            }
                
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
                OpenedInterface = interfaces.TCPClient;
                comBuffer = new byte[0];
                RecData = new byte[0];
            }
            else
                throw new System.IO.IOException("Connect Error");
        }

        /// <summary>
        /// Create TCP/IP server
        /// </summary>
        /// <param name="portNumber">Lisen port number</param>
        public void CreateTCPServer(int portNumber)
        {
            if (ReceivedData == null) asynch = false;
            else asynch = true;
            bool res = SocketClient.CreateTcp(portNumber, asynch);
            if (res == true)
            {
                OpenedInterface = interfaces.TCPServer;
                comBuffer = new byte[0];
                RecData = new byte[0];
                if (ReceivedData != null)
                    ReceivedData(this, comStatus.Started);
            }
            else
            {
                if (ReceivedData != null)
                    ReceivedData(this, comStatus.OpenError);
                throw new System.IO.IOException("Connect Error");
            }
                
        }

        #endregion

        #region Serial Port

        /// <summary>
        /// Set Serial Port baudrate
        /// </summary>
        /// <param name="baud">Baudrate</param>
        public void SetSPBaudRate(int baud)
        {
            SP.BaudRate = baud;
        }

        /// <summary>
        /// Set Serial Port parameters
        /// </summary>
        /// <param name="baud">Baud Rate</param>
        /// <param name="parity">Parity</param>
        /// <param name="databits">Data bits</param>
        /// <param name="stopbits">Stop Bits</param>
        /// <param name="DTR">DTR</param>
        /// <param name="RTS">RTS</param>
        public void SetSPParams(int baud, Parity parity = Parity.None, int databits = 8, StopBits stopbits = StopBits.One, bool DTR = false, bool RTS = false)
        {
            //SP.PortName = COM;
            SP.BaudRate = baud;
            SP.DataBits = databits;
            SP.Parity = parity;
            SP.StopBits = stopbits;
            SP.RtsEnable = RTS;
            SP.DtrEnable = DTR;
        }

        #endregion

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
        public byte[] Read(int timeout, int nextCharTimeOut = 0)
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

            if (nextCharTimeOut > 0)
            {
                byte[] res2;
                stopwatch.Restart();
                while ((res.Length > 0) && (stopwatch.ElapsedMilliseconds < nextCharTimeOut))
                {
                    //System.Windows.Forms.Application.DoEvents();
                    System.Threading.Thread.Sleep(5);
                    res2 = ReadData(nextCharTimeOut);
                    if (res2.Length > 0)
                    {
                        res = AddArray(res, res2);
                        stopwatch.Restart();
                    }
                }
            }


            comBuffer = new byte[0];
            RecData = new byte[0];
            return res;
        }

        /// <summary>
        /// Read received data from buffer
        /// </summary>
        /// <returns>Data</returns>
        public string ReadString(int timeout, int nextCharTimeOut = 0)
        {
            byte[] res = Read(timeout, nextCharTimeOut);
            return encoding.GetString(res);
        }

        private byte[] ReadData(int timeout = -1)
        {
            byte[] res = new byte[0];
            if (asynch)
            {
                res = RecData;
                RecData = new byte[0];
            }
            else
            {
                if (OpenedInterface == interfaces.TCPClient || OpenedInterface == interfaces.TCPServer || OpenedInterface == interfaces.Udp) res = SocketClient.Receive(timeout);
                else if (OpenedInterface == interfaces.COM)
                {
                    int bytes = 0;
                    try
                    {
                        bytes = SP.BytesToRead;
                    }
                    catch { }


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
                switch (OpenedInterface)
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
            catch (Exception)
            {

            }
            
            if (OpenedInterface == interfaces.TCPServer)
            {
                if (ReceivedData != null)
                    ReceivedData(this, comStatus.Stopped);
            }
            else
            {
                if (ReceivedData != null)
                    ReceivedData(this, comStatus.Close);
            }
            OpenedInterface = interfaces.None;
        }

        /// <summary>
        /// Get connection status
        /// </summary>
        /// <returns>Return True if connection is open</returns>
        public bool IsOpen()
        {
            if (OpenedInterface == interfaces.None) return false;
            else return true;
        }

        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="data">Data</param>
        public void Send(byte[] data)
        {
            switch (OpenedInterface)
            {
                case interfaces.COM:
                    try
                    {
                        SP.WriteTimeout = 5000;
                        SP.Write(data, 0, data.Length);
                    }
                    catch (Exception err)
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
            switch (OpenedInterface)
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

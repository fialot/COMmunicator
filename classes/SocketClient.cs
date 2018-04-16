using System;
using System.Net;
using System.Windows;
using System.Windows.Forms;

using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.IO;

// Version: 1.1

namespace TCPClient
{

    /// <summary>
    /// Communication status (commands in TCP/IP received data - Close connection)
    /// </summary>
    public enum comStatus { OK, Open, Close, OpenError };

    /// <summary>
    /// Received data handler delegate
    /// </summary>
    /// <param name="data">Received data</param>
    /// <param name="status">Communication status (received commands: open/close connection...)</param>
    public delegate void NetReceivedEventHandler(byte[] data, comStatus status);


    public static class SocketClient
    {
        enum ConnType
        {
            TcpClient,
            TcpServer,
            Udp
        }

        // Cached Socket object that will be used by each call for the lifetime of this class
        static Socket _socket = null;

        static TcpListener _tcpServer = null;
        static TcpClient _tcpClient = null;

        static UdpClient _udpClientLisener = null;
        static UdpClient _udpClientSender = null;

        static ConnType _connType = ConnType.TcpClient;
        static int _timeOutMs = 5000;
        static byte[] _buffer = new byte[0];

        static string _hostIP = "";
        static int _hostPort = 9000;
        static int _localPort = 9000;



        /// <summary>
        /// Received data handler
        /// </summary>
        static public event NetReceivedEventHandler ReceivedData;


        // Define a timeout in milliseconds for each asynchronous call. If a response is not received within this 
        // timeout period, the call is aborted.
        const int TIMEOUT_MILLISECONDS = 5000;

        // The maximum size of the data buffer to use with the asynchronous socket methods
        const int MAX_BUFFER_SIZE = 2048;

        /// <summary>
        /// Attempt a UDP listening
        /// </summary>
        /// <param name="listenPort"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        public static bool ConnectUdp(int listenPort, bool async = true)
        {
            return ConnectUdp("", -1, listenPort, async);
        }

        /// <summary>
        /// Attempt a UDP socket connection to the given host over the given port
        /// </summary>
        /// <param name="hostName">The name of the host</param>
        /// <param name="hostPort">The port number to connect</param>
        /// <param name="listenPort">The port number to lissen</param>
        /// <param name="async"></param>
        /// <returns></returns>
        public static bool ConnectUdp(string hostName, int hostPort, int listenPort, bool async = true)
        {

            _hostIP = hostName;
            _hostPort = hostPort;
            _localPort = listenPort;

            string result = string.Empty;

            _connType = ConnType.Udp;
            if (listenPort >= 0)
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Any, listenPort);
                _udpClientLisener = new UdpClient(ep);

                _udpClientLisener.BeginReceive(new AsyncCallback(ReadCallbackUdp), ep);
            }
            else
            {
                _udpClientLisener = new UdpClient();
            }

            if (hostPort >= 0 && hostName != "")
            {
                _udpClientSender = new UdpClient();
                _udpClientSender.Connect(hostName, hostPort);
            }

            return true;
        }

        private static void ReadCallbackUdp(IAsyncResult result)
        {
            if (_udpClientLisener != null)
            {
                try
                {
                    IPEndPoint ep = (IPEndPoint)(result.AsyncState);
                    byte[] receiveBytes = _udpClientLisener.EndReceive(result, ref ep);

                    ReceivedData(receiveBytes, comStatus.OK);
                    _udpClientLisener.BeginReceive(new AsyncCallback(ReadCallbackUdp), ep);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    Close();
                    ConnectUdp(_hostIP, _hostPort, _localPort, true);

                    //ReceivedData(new byte[0], comStatus.Close);
                }

                
            }
        }

        /// <summary>
        /// Attempt a TCP socket connection to the given host over the given port
        /// </summary>
        /// <param name="hostName">The name of the host</param>
        /// <param name="portNumber">The port number to connect</param>
        public static bool ConnectTcp(string hostName, int portNumber, bool async = true)
        {
            string result = string.Empty;

            _connType = ConnType.TcpClient;
            _tcpClient = new TcpClient();
            if (async)
            {
                _tcpClient.BeginConnect(hostName, portNumber, new AsyncCallback(ConnectCallback), _tcpClient);
            }
            else
            {
                _tcpClient.Connect(hostName, portNumber);
            }
            
            return true;
        }

        private static void ConnectCallback(IAsyncResult result)
        {
            try
            {
                if (_tcpClient.Connected)
                {
                    byte[] data = new byte[0];
                    ReceivedData(data, comStatus.Open);

                    //We are connected successfully.
                    NetworkStream networkStream = _tcpClient.GetStream();

                    byte[] buffer = new byte[_tcpClient.ReceiveBufferSize];

                    //Now we are connected start asyn read operation.
                    //networkStream.by
                    networkStream.BeginRead(buffer, 0, 0, ReadCallback, buffer);
                } else
                {
                    byte[] data = new byte[0];
                    ReceivedData(data, comStatus.OpenError);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private static void ReadCallback(IAsyncResult result)
        {
            if (_tcpClient != null)
            {
                NetworkStream networkStream = _tcpClient.GetStream();

                byte[] buffer = new byte[_tcpClient.ReceiveBufferSize];
                try
                {
                    int length = networkStream.Read(buffer, 0, buffer.Length);

                    if (length > 0)
                    {
                        byte[] buffer2 = new byte[length];
                        for (int i = 0; i < buffer2.Length; i++) { buffer2[i] = buffer[i]; }
                        ReceivedData(buffer2, comStatus.OK);
                        networkStream.BeginRead(buffer, 0, 0, ReadCallback, buffer);

                    }
                    else
                    {
                        buffer = new byte[0];
                        ReceivedData(buffer, comStatus.Close);
                        if (_connType == ConnType.TcpServer)
                            _tcpServer.BeginAcceptTcpClient(new AsyncCallback(ConnectCallbackServer), _tcpServer);
                    }
                }
                catch (Exception)
                {
                    buffer = new byte[0];
                    ReceivedData(buffer, comStatus.Close);
                    if (_connType == ConnType.TcpServer)
                        _tcpServer.BeginAcceptTcpClient(new AsyncCallback(ConnectCallbackServer), _tcpServer);
                }
                
            }
        }

        /// <summary>
        /// Attempt a TCP socket connection to the given host over the given port
        /// </summary>
        /// <param name="portNumber">The port number to connect</param>
        public static bool CreateTcp(int portNumber, bool async = true)
        {
            string result = string.Empty;

            _connType = ConnType.TcpServer;
            _tcpServer = new TcpListener(portNumber);
            if (async)
            {
                _tcpServer.Start(1);        // Max connections
                _tcpServer.BeginAcceptTcpClient(new AsyncCallback(ConnectCallbackServer), _tcpServer);
            }
            else
            {
                _tcpServer.AcceptTcpClient();
            }

            return true;
        }

        private static void ConnectCallbackServer(IAsyncResult result)
        {
            try
            {
                if (_tcpServer != null)
                {
                    byte[] data = new byte[0];
                    ReceivedData(data, comStatus.Open);
                    _tcpClient = _tcpServer.EndAcceptTcpClient(result);

                    //We are connected successfully.
                    NetworkStream networkStream = _tcpClient.GetStream();

                    byte[] buffer = new byte[_tcpClient.ReceiveBufferSize];

                    //Now we are connected start asyn read operation.
                    //networkStream.by
                    networkStream.BeginRead(buffer, 0, 0, ReadCallback, buffer);
                }
                else
                {
                    byte[] data = new byte[0];
                    //ReceivedData(data, comStatus.OpenError);
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Send the given data to the server using the established connection
        /// </summary>
        /// <param name="data">The data to send to the server</param>
        /// <returns>The result of the Send request</returns>
        public static string Send(byte[] data)
        {
            if (_connType == ConnType.TcpClient || _connType == ConnType.TcpServer)
            {
                if (_tcpClient != null && _tcpClient.Connected)
                {
                    NetworkStream stream = _tcpClient.GetStream();
                    stream.WriteTimeout = _timeOutMs;
                    stream.Write(data, 0, data.Length);
                }
            }
            else if (_connType == ConnType.Udp)
            {
                if (_udpClientSender != null)
                {
                    _udpClientSender.Send(data, data.Length);
                }
            }


            string response = "Operation Timeout";
            return response;
        }


        /// <summary>
        /// Receive data from the server using the established socket connection
        /// </summary>
        /// <returns>The data received from the server</returns>
        public static byte[] Receive(int timeOut = -1)
        {
            if (_connType == ConnType.TcpClient)
            {
                if (_tcpClient != null && _tcpClient.Connected)
                {
                    NetworkStream networkStream = _tcpClient.GetStream();
                    networkStream.ReadTimeout = timeOut;

                    byte[] buffer = new byte[_tcpClient.ReceiveBufferSize];
                    int length = 0;
                    try
                    {
                        length = networkStream.Read(buffer, 0, buffer.Length);
                    }
                    catch (Exception)
                    {
                        return new byte[0];
                    }
                    

                    if (length > 0)
                    {
                        byte[] buffer2 = new byte[length];
                        for (int i = 0; i < buffer2.Length; i++) { buffer2[i] = buffer[i]; }
                        return buffer2;

                    }
                    else
                    {
                        return new byte[0];
                    }
                }
            }

            return new byte[0];
        }

        /// <summary>
        /// Closes the Socket connection and releases all associated resources
        /// </summary>
        public static void Close()
        {
            if (_socket != null)
            {
                _socket.Close();
            }

            if (_tcpClient != null)
            {
                _tcpClient.Close();
                _tcpClient = null;
            }

            if (_udpClientLisener != null)
            {
                _udpClientLisener.Close();
                _udpClientLisener = null;
            }

            if (_udpClientSender != null)
            {
                _udpClientSender.Close();
                _udpClientSender = null;
            }

            if (_tcpServer != null)
            {
                _tcpServer.Stop();
                _tcpServer = null;
            }


            //if (_clientDone != null) _clientDone.Close();
        }
    }
}

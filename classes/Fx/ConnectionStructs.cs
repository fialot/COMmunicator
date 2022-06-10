using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Fx.IO;
using Fx.Security;

namespace Fx.IO
{
    public enum ConnectionType
    {
        Serial = 0, TCP = 1, UDP = 2, TCPServer = 3, SSH = 4
    }

    public enum DeviceType
    {
        General = 0
    }

    public enum ComProtocolType
    {
        General = 0
    }

    public class ConnectionSetting
    {
        // ----- Description -----
        public string Name { get; set; }  = "";
        public string Group { get; set; } = "";

        // ----- Device -----
        public DeviceType Device { get; set; } = DeviceType.General;
        public ComProtocolType Protocol { get; set; } = ComProtocolType.General;
        public int Address { get; set; } = 0;

        // ----- Connection type -----
        public ConnectionType Type { get; set; } = ConnectionType.Serial;

        // ----- Serial port -----
        public string SerialPort { get; set; } = "COM1";
        public int BaudRate { get; set; } = 15200;
        public Parity Parity { get; set; } = Parity.None;
        public int DataBits { get; set; } = 8;
        public StopBits StopBits { get; set; } = StopBits.One;
        public bool DTR { get; set; } = false;
        public bool RTS { get; set; } = false;

        // ----- Ethernet -----
        public string IP { get; set; } = "";
        public int Port { get; set; } = 0;
        public int LocalPort { get; set; } = 0;

        // ----- SSH Login -----
        public string Login { get; set; } = "";
        public string Password { get; set; } = "";
        public string PrivateKeyPath { get; set; } = "";

        // ----- Encoding -----
        public Encoding UsedEncoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ConnectionSetting() {}

        /// <summary>
        /// Serial port constructor
        /// </summary>
        /// <param name="serialPort">Serial port name</param>
        /// <param name="baudRate">Baud rate</param>
        /// <param name="parity">Parity</param>
        /// <param name="dataBits">Data bits</param>
        /// <param name="stopBits">Stop bits</param>
        public ConnectionSetting(string serialPort, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            Type = ConnectionType.Serial;
            SerialPort = serialPort;
            BaudRate = baudRate;
            Parity = parity;
            DataBits = dataBits;
            StopBits = stopBits;
        }

        /// <summary>
        /// TCP constructor
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">Port</param>
        public ConnectionSetting(string ip, int port)
        {
            Type = ConnectionType.TCP;
            IP = ip;
        }

        /// <summary>
        /// UDP constructor
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">Port</param>
        /// <param name="localPortUDP">Local port</param>
        public ConnectionSetting(string ip, int port, int localPortUDP)
        {
            Type = ConnectionType.UDP;
            IP = ip;
            Port = port;
            LocalPort = localPortUDP;
        }
                
        /// <summary>
        /// Constructor with load settings from XML element
        /// </summary>
        /// <param name="xml">Connection XML element</param>
        public ConnectionSetting(XElement xml)
        {
            Load(xml);
        }

        /// <summary>
        /// Loading setting from XML element
        /// </summary>
        /// <param name="xml">XML element</param>
        /// <returns>Indication on decrypted password (need save setting)</returns>
        public bool Load(XElement xml)
        {
            XElement group;
            XElement element;
            bool needSave = false;

            // ----- Name -----
            element = xml.Element("name");
            if (element != null)
            {
                this.Name = element.Value;
            }

            // ----- Group -----
            element = xml.Element("group");
            if (element != null)
            {
                this.Group = element.Value;
            }

            // ----- Address -----
            element = xml.Element("address");
            if (element != null)
            {
                this.Address = Conv.ToIntDef(element.Value, 0);
            }

            // ----- Device -----
            element = xml.Element("dev_type");
            if (element != null)
            {
                this.Device = Conv.ToEnum<DeviceType>(element.Value, DeviceType.General);
            }

            // ----- Protocol -----
            element = xml.Element("protocol");
            if (element != null)
            {
                this.Protocol = Conv.ToEnum<ComProtocolType>(element.Value, ComProtocolType.General);
            }

            // ----- Connection type -----
            element = xml.Element("conn_type");
            if (element != null)
            {
                this.Type = Conv.ToEnum<ConnectionType>(element.Value, ConnectionType.Serial);
            }

            // ----- Encoding -----
            element = xml.Element("encoding");
            if (element != null)
            {
                this.UsedEncoding = Conv.ToEncoding(element.Value, Encoding.UTF8);
            }

            // ----- Serial port settings -----
            group = xml.Element("serial");
            if (group != null)
            {
                // ----- Port -----
                element = group.Element("port");
                if (element != null)
                {
                    this.SerialPort = element.Value;
                }

                // ----- Baud rate -----
                element = group.Element("baud");
                if (element != null)
                {
                    this.BaudRate = Conv.ToIntDef(element.Value, 115200);
                }

                // ----- Parity -----
                element = group.Element("parity");
                if (element != null)
                {
                    this.Parity = Conv.ToEnum<System.IO.Ports.Parity>(element.Value, System.IO.Ports.Parity.None);
                }

                // ----- Data bits -----
                element = group.Element("data_bits");
                if (element != null)
                {
                    this.DataBits = Conv.ToIntDef(element.Value, 8);
                }

                // ----- Stop bits -----
                element = group.Element("stop_bits");
                if (element != null)
                {
                    this.StopBits = Conv.ToEnum<System.IO.Ports.StopBits>(element.Value, System.IO.Ports.StopBits.One);
                }

                // ----- DTR -----
                element = group.Element("DTR");
                if (element != null)
                {
                    this.DTR = Conv.ToBoolDef(element.Value, false);
                }

                // ----- RTS -----
                element = group.Element("RTS");
                if (element != null)
                {
                    this.RTS = Conv.ToBoolDef(element.Value, false);
                }
            }

            // ----- Ethernet settings -----
            group = xml.Element("net");
            if (group != null)
            {
                // ----- IP address -----
                element = group.Element("IP");
                if (element != null)
                {
                    this.IP = element.Value;
                }

                // ----- Port -----
                element = group.Element("port");
                if (element != null)
                {
                    this.Port = Conv.ToIntDef(element.Value, 1000);
                }

                // ----- UDP local port -----
                element = group.Element("local_port");
                if (element != null)
                {
                    this.LocalPort = Conv.ToIntDef(element.Value, 1000);
                }
            }

            // ----- Login settings (for SSH) -----
            group = xml.Element("login");
            if (group != null)
            {
                // ----- IP address -----
                element = group.Element("user_name");
                if (element != null)
                {
                    this.Login = element.Value;
                }

                // ----- Port -----
                element = group.Element("password");
                if (element != null)
                {
                    this.Password = element.Value;

                    // ----- Decrypt pass -----
                    Password pass = new Password();

                    this.Password = pass.Decrypt(this.Password);

                    // ----- Check if password encrypted -----
                    if (element.Value == this.Password)
                        needSave = true;
                }

                // ----- Private key -----
                element = group.Element("private_key");
                if (element != null)
                {
                    this.PrivateKeyPath = element.Value;
                }
            }

            return needSave;
        }

        /// <summary>
        /// Create XML element from settings
        /// </summary>
        /// <returns>XML element</returns>
        public XElement GetXmlElement()
        {
            // ----- Encoding password -----
            Password pass = new Password();
            string password = this.Password;
            if (password != "") password = pass.Encrypt(this.Password);

            // ----- Write device settings -----
            var connElement = new XElement("connection");

            connElement.Add(new XElement("name", this.Name));
            connElement.Add(new XElement("group", this.Group));
            connElement.Add(new XElement("address", this.Address));

            connElement.Add(new XElement("dev_type", this.Device.ToString()));
            connElement.Add(new XElement("protocol", this.Protocol.ToString()));
            connElement.Add(new XElement("conn_type", (int)this.Type));
            connElement.Add(new XElement("encoding", this.UsedEncoding.HeaderName));

            bool saveSerial = (this.Device == DeviceType.General);
            if (this.Type == ConnectionType.Serial) saveSerial = true;

            bool saveNet = (this.Device == DeviceType.General);
            if (this.Type != ConnectionType.Serial) saveNet = true;


            if (saveSerial)
            {
                var serialElement = new XElement("serial");
                serialElement.Add(new XElement("port", this.SerialPort));
                serialElement.Add(new XElement("baud", this.BaudRate));
                serialElement.Add(new XElement("parity", this.Parity));
                serialElement.Add(new XElement("data_bits", this.DataBits));
                serialElement.Add(new XElement("stop_bits", this.StopBits));
                if (this.DTR)
                    serialElement.Add(new XElement("DTR", this.DTR));
                if (this.RTS)
                    serialElement.Add(new XElement("RTS", this.RTS));
                connElement.Add(serialElement);
            }

            if (saveNet)
            {
                var netElement = new XElement("net");
                netElement.Add(new XElement("IP", this.IP));
                netElement.Add(new XElement("port", this.Port));
                if (this.LocalPort > 0)
                    netElement.Add(new XElement("local_port", this.LocalPort));
                connElement.Add(netElement);

                if (this.Login != "")
                {
                    var loginElement = new XElement("login");
                    loginElement.Add(new XElement("user_name", this.Login));
                    if (password != "")
                        loginElement.Add(new XElement("password", password));
                    if (this.PrivateKeyPath != "")
                        loginElement.Add(new XElement("private_key", this.PrivateKeyPath));
                    connElement.Add(loginElement);
                }
            }

            return connElement;
        }
    }
}

using Fx.Conversion;
using Fx.IO.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Plugins
{
    public class ProtocolMODBUS: IPlugin, IPluginProtocol
    {
        #region Public

        public string GetName()
        {
            return "MODBUS RTU protocol";
        }

        public string GetDescription()
        {
            return "MODBUS RTU protocol";
        }

        /// <summary>
        /// Get plugin version
        /// </summary>
        /// <returns></returns>
        public string GetVersion()
        {
            return "v1.0";
        }

        public EPluginType GetPluginType()
        {
            return EPluginType.Protocol;
        }

        /// <summary>
        /// Get plugin type
        /// </summary>
        /// <returns></returns>
        public string GetPluginTypeString()
        {
            return "protocol";
        }

        public string GetFunctionName()
        {
            return "mb";
        }

        public byte[] CreatePacket(string message)
        {
            return processInput(message);
        }

        public string ParsePacket(byte[] message)
        {
            return parsePacket(message, false);
        }

        public string ParsePacket(byte[] message, bool request)
        {
            return parsePacket(message, request);
        }

        public byte[] AcknowledgeReception(byte[] message)
        {
            return new byte[0];
        }

        #endregion

        /// <summary>
        /// Process input data
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Packet bytes</returns>
        private byte[] processInput(string input)
        {
            var arguments = input.Split(new string[] { ";" }, StringSplitOptions.None);

            if (arguments.Length >= 3)
                return newPacket(Conv.ToInt(arguments[0], 0), Conv.ToByte(arguments[1]), ProtocolFormat.Format(arguments[2], Encoding.UTF8));
            else if (arguments.Length >= 2)
                return newPacket(0, Conv.ToByte(arguments[0]), ProtocolFormat.Format(arguments[1], Encoding.UTF8));
            else if (arguments.Length >= 1)
                return newPacket(0, Conv.ToByte(arguments[0]), new byte[0]);
            else return new byte[0];
        }

        /// <summary>
        /// Create packet from arguments
        /// </summary>
        /// <param name="address">Device address</param>
        /// <param name="command">Command</param>
        /// <param name="data">Data</param>
        /// <returns>Packet bytes</returns>
        private byte[] newPacket(int address, byte command, byte[] data)
        {


            int size = data.Length + 4;
            byte[] packet = new byte[size];

            // ----- check correct data -----
            if (address > 255) address = 0;
            if (address < 0) address = 0;

            // ----- Header -----
            packet[0] = (byte)address;
            packet[1] = (byte)command;

            // ----- Data -----
            Array.Copy(data, 0, packet, 2, data.Length);

            // ----- Checksum -----
            ushort check = checksum(packet, size - 2);
            packet[size - 2] = (byte)(check & 0xFF);
            packet[size - 1] = (byte)((check & 0xFF00) >> 8);

            return packet;
        }

        private string parsePacket(byte[] packet, bool request)
        {
            // ----- Check packet length -----
            if (packet.Length < 4)
                return "Error - Too short packet: " + BitConverter.ToString(packet).Replace("-", "");
           
            // ----- Check CheckSum -----
            ushort CRC1 = (ushort)((packet[packet.Length - 1] * 256) + packet[packet.Length - 2]);
            ushort CRC2 = checksum(packet, packet.Length - 2);

            if (CRC1 != CRC2)
                return "Error - Invalid checksum: " + BitConverter.ToString(packet).Replace("-", "");

            byte[] data = new byte[packet.Length - 4];
            Array.Copy(packet, 2, data, 0, data.Length);

            string text = "";

            text = BitConverter.ToString(packet);//.Replace("-", "");


            return text;
        }

        /// <summary>
        /// Compute the MODBUS RTU CRC
        /// </summary>
        /// <param name="buffer">Buffer to compute CRC</param>
        /// <param name="length">Buffer length</param>
        /// <returns></returns>
        private ushort checksum(byte[] buffer, int length)
        {
            ushort crc = 0xFFFF;

            for (int pos = 0; pos < length; pos++)
            {
                crc ^= (ushort)buffer[pos];          // XOR byte into least sig. byte of crc

                for (int i = 8; i != 0; i--)
                {    // Loop over each bit
                    if ((crc & 0x0001) != 0)
                    {      // If the LSB is set
                        crc >>= 1;                    // Shift right and XOR 0xA001
                        crc ^= 0xA001;
                    }
                    else                            // Else LSB is not set
                        crc >>= 1;                    // Just shift right
                }
            }
            return crc;
        }


    }
}

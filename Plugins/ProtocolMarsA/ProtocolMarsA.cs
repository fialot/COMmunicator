using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fx.Conversion;
using Fx.IO.Protocol;

namespace Fx.Plugins
{
    public class ProtocolMarsA: IPlugin, IPluginProtocol
    {
        #region Public

        public string GetName()
        {
            return "MARS-A protocol";
        }

        public string GetDescription()
        {
            return "MARS-A protocol";
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
            return "marsa";
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

                return ProtocolFormat.Format(@"\s" + frameNum.ToString(), Encoding.UTF8);
            }
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

            if (arguments.Length >= 2)
                return newPacket(Conv.ToInt(arguments[0], 0), ProtocolFormat.Format(arguments[1], Encoding.UTF8));
            else if (arguments.Length >= 1)
                return newPacket(0, ProtocolFormat.Format(arguments[0], Encoding.UTF8));
            else return new byte[0];
        }

        /// <summary>
        /// Create packet from arguments
        /// </summary>
        /// <param name="address">Device address</param>
        /// <param name="command">Command</param>
        /// <param name="data">Data</param>
        /// <returns>Packet bytes</returns>
        private byte[] newPacket(int address, byte[] data)
        {
            string res = "";
            byte[] resByte = new byte[0];
            //if (address.Length > 0)                     // check correct address
            {
                ushort frameType = (ushort)(49152 + data.Length + 6);
                if (data.Length > 1626) return new byte[0];      // check max. data length
                ushort packetType = (ushort)(2432);

                byte[] array = BitConverter.GetBytes(address);

                ushort[] addr = Conv.ToUInt16Array(array);

                array = data; //Encoding.Default.GetBytes(data);
                Array.Reverse(array);
                ushort[] shortData = Conv.ToUInt16Array(array);


                List<ushort> items = new List<ushort>();
                items.Add(frameType);
                items.Add(packetType);
                items.Add(addr[1]);
                items.Add(addr[0]);
                for (int i = shortData.Length - 1; i >= 0; i--)
                    items.Add(shortData[i]);

                ushort crc = 0;
                for (int i = 0; i < items.Count; i++) crc ^= items[i];
                items.Add(crc);

                res = "\\x";
                for (int i = 0; i < items.Count; i++) res += items[i].ToString("X4");
                resByte = new byte[items.Count * 2];
                byte[] numberBytes;
                for (int i = 0; i < items.Count; i++)
                {
                    numberBytes = BitConverter.GetBytes(items[i]);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(numberBytes);
                    for (int j = 0; j < numberBytes.Length; j++)
                    {
                        resByte[2 * i + j] = numberBytes[j];
                    }
                }
                //string hexData = data.ToCharArray()
                //ushort[] shortData = Conv.HexToUShorts(hexData);

            }
            return resByte;

        }

        private string parsePacket(byte[] packet, bool request)
        {
            string text = "";
            string dataMars = "";

            if (packet.Length > 2)
            {
                int frameNum = 0;
                try
                {
                    byte[] test = new byte[1];
                    int frame = Conv.SwapBytes(BitConverter.ToUInt16(packet, 0));
                    int length = (frame & 2047) - 6; // dala length
                    frameNum = (frame & 12288) + 6 + (128 << 8) + +(1 << 8);
                    for (int i = 8; i < 8 + length; i++)
                    {
                        test[0] = packet[i];
                        var encTxt = Encoding.UTF8.GetString(test);
                        if (packet[i] > 31)
                        {
                            if (packet[i] != 0) dataMars += encTxt;
                        }
                        else dataMars += "{" + packet[i].ToString() + "}";

                    }
                }
                catch (Exception err)
                {

                }
            }
            text += dataMars;

            return text;
            
        }

    }
}

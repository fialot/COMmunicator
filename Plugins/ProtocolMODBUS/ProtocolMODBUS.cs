using Fx.Conversion;
using Fx.IO.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Plugins
{

    enum mbFunctions
    {
        ReadCoils = 0x01,
        ReadDiscreteInputs = 0x02,
        ReadHoldingRegisters = 0x03,
        ReadInputRegisters = 0x04,
        WriteSigleCoil = 0x05,
        WriteSingleRegister = 0x06,
        WriteMultipleCoils = 0x0F,
        WriteMultipleRegisters = 0x10,

        ReadHoldingRegistersExt = 0x41,
        ReadInputRegistersExt = 0x42,
    }

    class mbArguments
    {
        public byte Address = 0;
        public byte Function = 0;
        public string[] Argument = new string[0];
    }

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
            var args = GetArguments(input);

            return newPacket(args);
        }

        private mbArguments GetArguments(string input)
        {
            mbArguments args = new mbArguments();
            var arguments = input.Split(new string[] { ";" }, StringSplitOptions.None);
            
            if (arguments.Length == 1)
            {
                args.Address = 0;
                
                args.Function = Conv.ToByte(arguments[0]);
                if (args.Function == 0)
                {
                    args.Function = (byte)Conv.ToEnum<mbFunctions>(arguments[0], mbFunctions.ReadCoils);
                }
            }
            else if (arguments.Length == 2)
            {
                args.Address = Conv.ToByte(arguments[0]);
                args.Function = Conv.ToByte(arguments[1]);
                if (args.Function == 0)
                {
                    args.Function = (byte)Conv.ToEnum<mbFunctions>(arguments[1], mbFunctions.ReadCoils);
                }
            }
            else
            {
                args.Address = Conv.ToByte(arguments[0]);
                args.Function = Conv.ToByte(arguments[1]);
                if (args.Function == 0)
                {
                    args.Function = (byte)Conv.ToEnum<mbFunctions>(arguments[1], mbFunctions.ReadCoils);
                }
                var list = arguments.ToList();
                list.RemoveRange(0, 2);
                args.Argument = list.ToArray();
            }

            return args;
        }

        /// <summary>
        /// Create packet from arguments
        /// </summary>
        /// <param name="address">Device address</param>
        /// <param name="command">Command</param>
        /// <param name="data">Data</param>
        /// <returns>Packet bytes</returns>
        private byte[] newPacket(mbArguments arg)
        {
            byte[] data = new byte[0];


            switch((mbFunctions)arg.Function)
            {
                case mbFunctions.ReadCoils:
                case mbFunctions.ReadDiscreteInputs:
                case mbFunctions.ReadHoldingRegisters:
                case mbFunctions.ReadInputRegisters:
                case mbFunctions.ReadHoldingRegistersExt:
                case mbFunctions.ReadInputRegistersExt:
                    data = new byte[4];
                    if (arg.Argument.Length >= 2)
                    {
                        ushort regAddress = Conv.ToUShort(arg.Argument[0]);
                        data[0] = (byte)((regAddress >> 8) & 0xFF);
                        data[1] = (byte)(regAddress & 0xFF);

                        ushort regLength = Conv.ToUShort(arg.Argument[1]);
                        data[2] = (byte)((regLength >> 8) & 0xFF);
                        data[3] = (byte)(regLength & 0xFF);
                    }
                    break;
                case mbFunctions.WriteSigleCoil:
                    data = new byte[4];
                    if (arg.Argument.Length >= 2)
                    {
                        ushort regAddress = Conv.ToUShort(arg.Argument[0]);
                        data[0] = (byte)((regAddress >> 8) & 0xFF);
                        data[1] = (byte)(regAddress & 0xFF);

                        bool regVal = Conv.ToBool(arg.Argument[1]);
                        if (regVal)
                            data[2] = 0xFF;
                        else
                            data[2] = 0x00;
                        data[3] = 0;
                    }
                    break;
                case mbFunctions.WriteSingleRegister:
                    data = new byte[4];
                    if (arg.Argument.Length >= 2)
                    {
                        ushort regAddress = Conv.ToUShort(arg.Argument[0]);
                        data[0] = (byte)((regAddress >> 8) & 0xFF);
                        data[1] = (byte)(regAddress & 0xFF);

                        var values = ProtocolFormat.Format(arg.Argument[1], Encoding.UTF8);
                        if (values.Length >= 2)
                        {
                            data[2] = values[1];
                            data[3] = values[0];
                        }
                    }
                    break;
                case mbFunctions.WriteMultipleCoils:
                case mbFunctions.WriteMultipleRegisters:
                    if (arg.Argument.Length >= 3)
                    {
                        var values = ProtocolFormat.Format(arg.Argument[2], Encoding.UTF8);
                        data = new byte[5 + values.Length];

                        ushort regAddress = Conv.ToUShort(arg.Argument[0]);
                        data[0] = (byte)((regAddress >> 8) & 0xFF);
                        data[1] = (byte)(regAddress & 0xFF);

                        ushort regLength = Conv.ToUShort(arg.Argument[1]);
                        data[2] = (byte)((regLength >> 8) & 0xFF);
                        data[3] = (byte)(regLength & 0xFF);

                        data[4] = (byte)values.Length;
                        Array.Copy(values, 0, data, 5, values.Length);
                    }
                    break;
                default:
                    data = ProtocolFormat.Format(Conv.ToString(arg.Argument), Encoding.UTF8);
                    break;

            }

            int size = data.Length + 4;
            byte[] packet = new byte[size];

            // ----- Header -----
            packet[0] = arg.Address;
            packet[1] = arg.Function;

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

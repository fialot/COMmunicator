﻿using Fx.Conversion;
using Fx.IO;
using Fx.IO.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Plugins
{

    enum NuviaCmd
    {
        Link = 0x01,
        GetXml = 0xD2,
        GetParam = 0xC2,
        SetParam = 0xC3,
        SetParamTemporary = 0xC4,
        GetSpectrumParam = 0x04,
        GetConfig = 0xCA,
        SetConfig = 0xCB,
        LoadFactoryConfig = 0xCC,
        SaveFactoryConfig = 0xCD,
        ResetConfig = 0xCE,
        ResetAndStayInBld = 0xD7,
        Login = 0xE1,
    }

    public class ProtocolNuvia: IPlugin, IPluginProtocol
    {
        #region Public

        public string GetName()
        {
            return "Nuvia protocol";
        }

        public string GetDescription()
        {
            return "Nuvia protocol";
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
            return "nuvia";
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
                return newPacket(Conv.ToIntDef(arguments[0], 0), Conv.HexToByte(arguments[1]), ProtocolFormat.Format(arguments[2], Encoding.UTF8));
            else if (arguments.Length >= 2)
                return newPacket(0, Conv.HexToByte(arguments[0]), ProtocolFormat.Format(arguments[1], Encoding.UTF8));
            else if (arguments.Length >= 1)
                return newPacket(0, Conv.HexToByte(arguments[0]), new byte[0]);
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
            int size = 3 + data.Length;
            byte[] packet = new byte[size + 3];

            // ----- check correct data -----
            if (address > 255) address = 0;
            if (address < 0) address = 0;

            // ----- Header -----
            packet[0] = (byte)((size >> 16) & 0xFF);
            packet[1] = (byte)((size >> 8) & 0xFF);
            packet[2] = (byte)(size & 0xFF);
            packet[3] = (byte)address;
            packet[4] = command;

            // ----- Data -----
            Array.Copy(data, 0, packet, 5, data.Length);

            
            // ----- Checksum -----
            packet[packet.Length - 1] = checksum(packet, packet.Length - 1);

            return packet;
        }

        private string parsePacket(byte[] packet, bool request)
        {
            // ----- Check packet length -----
            if (packet.Length < 6)
                return "Error - Too short packet: " + BitConverter.ToString(packet).Replace("-", "");
            int length = (packet[0] << 16) + (packet[1] << 8) + packet[2];
            if (length != packet.Length - 3)
                return "Error - Corrupted packet: " + BitConverter.ToString(packet).Replace("-", "");

            // ----- Check CheckSum -----
            byte CRC1 = packet[packet.Length - 1];
            byte CRC2 = checksum(packet, packet.Length - 1);

            if (CRC1 != CRC2)
                return "Error - Invalid checksum: " + BitConverter.ToString(packet).Replace("-", "");

            byte[] data = new byte[packet.Length - 6];
            Array.Copy(packet, 5, data, 0, data.Length);

            string text = "";
            if (request)
            {
                text = ((NuviaCmd)packet[4]).ToString() + " ";
            }

            switch ((NuviaCmd)packet[4])
            {
                case NuviaCmd.Link:
                case NuviaCmd.GetXml:
                case NuviaCmd.GetParam:
                case NuviaCmd.SetParam:
                case NuviaCmd.SetParamTemporary:
                    text += Encoding.UTF8.GetString(ArrayConv.RemoveValues(data, 0));
                    break;
                default:
                    text += BitConverter.ToString(packet).Replace("-", "");
                    break;
            }
            return text;
        }

        private byte checksum(byte[] packet, int length)
        {
            // ----- Check correct length -----
            if (length > packet.Length) length = packet.Length;

            // ----- Checksum -----
            uint checksum = 0;
            for (int i = 0; i < length; i++)
            {
                checksum += packet[i];
            }
            checksum = ~(checksum & 0xFF);
            checksum += 1;
            return (byte)(checksum & 0xFF);
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fx.IO;
using myFunctions;

namespace COMunicator.Protocol
{
    static class Protocol
    {

        public static byte[] MarsA(uint address, byte[] data)
        {
            string res = "";
            byte[] resByte = new byte[0];
            //if (address.Length > 0)                     // check correct address
            {
                ushort frameType = (ushort)(49152 + data.Length + 6);
                if (data.Length > 1626) return new byte[0];      // check max. data length
                ushort packetType = (ushort)(2432);

                byte[] array = BitConverter.GetBytes(address);

                ushort[] addr = Conv.ToUShort(array);

                array = data; //Encoding.Default.GetBytes(data);
                Array.Reverse(array);
                ushort[] shortData = Conv.ToUShort(array);

                


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
                resByte = new byte[items.Count*2];
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

        public static byte[] Nuvia(int address, byte command, byte[] data)
        {
            int size = 3 + data.Length;
            byte[] resByte = new byte[size + 3];
            if (address > 255) address = 0;
            if (address < 0) address = 0;

            resByte[0] = (byte)((size >> 16) & 0xFF);
            resByte[1] = (byte)((size >> 8) & 0xFF);
            resByte[2] = (byte)(size & 0xFF);
            resByte[3] = (byte)address;
            resByte[4] = command;
            Array.Copy(data, 0, resByte, 5, data.Length);
            uint checksum = 0;
            for (int i = 0; i < resByte.Length - 1; i++)
            {
                checksum += resByte[i];
            }
            checksum = ~(checksum & 0xFF);
            checksum += 1;

            resByte[resByte.Length - 1] = (byte)(checksum & 0xFF);
            return resByte;
        }
    }
}

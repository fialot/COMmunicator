using myFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fx.Plugins;

namespace Fx.IO.Protocol
{
    public static class ProtocolCom
    {
        public static List<IPluginProtocol> Plugins = null;

        /// <summary>
        /// Format Message
        /// </summary>
        /// <param name="msg">Message string</param>
        /// <returns>Data to send</returns>
        static public byte[] FormatMsg(string msg, Encoding encoding)
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
                byteList.Add(encoding.GetBytes(msg.Substring(i, 1))[0]);
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
                                insBytes = Protocol.MarsA(Conv.HexToUInt(fun.arguments[0]), FormatMsg(fun.arguments[1], encoding));
                                lastpos = -1;
                            }
                        }
                        /*else if (fun.name == "nuvia")
                        {
                            if (fun.arguments.Length >= 1)
                            {
                                if (fun.arguments.Length >= 3)
                                    insBytes = Protocol.Nuvia(Conv.ToIntDef(fun.arguments[0], 0), Conv.HexToByte(fun.arguments[1]), FormatMsg(fun.arguments[2], encoding));
                                else if (fun.arguments.Length >= 2)
                                    insBytes = Protocol.Nuvia(0, Conv.HexToByte(fun.arguments[0]), FormatMsg(fun.arguments[1], encoding));
                                else if (fun.arguments.Length >= 1)
                                    insBytes = Protocol.Nuvia(0, Conv.HexToByte(fun.arguments[0]), new byte[0]);
                                lastpos = -1;
                            }
                        }*/
                        else
                        {
                            foreach(var item in Plugins)
                            {
                                if (fun.name == item.GetFunctionName())
                                {
                                    insBytes = item.CreatePacket(fun.argumentString);
                                    lastpos = -1;
                                }
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
                        Array.Reverse(insBytes);
                        remove(ref msg, ref byteList, ref boolList, pos, count + 1);
                        insert(ref msg, ref byteList, ref boolList, pos, insBytes);
                    } // hex
                    else if ((prefix1 == "s") && (ushort.TryParse(cmd.Remove(0, 1).Trim(), out snum)))
                    {

                        insBytes = BitConverter.GetBytes(snum);
                        Array.Reverse(insBytes);
                        remove(ref msg, ref byteList, ref boolList, pos, count + 1);
                        insert(ref msg, ref byteList, ref boolList, pos, insBytes);
                    }
                    else if ((prefix1 == "f") && (float.TryParse(cmd.Remove(0, 1).Trim(), out fnum)))
                    {

                        insBytes = BitConverter.GetBytes(fnum);
                        Array.Reverse(insBytes);
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


        static private string FindCmd(string msg, int pos, ref int count)
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

        static private void remove(ref string msg, ref List<byte> byteList, ref List<bool> boolList, int pos, int len)
        {
            msg = msg.Remove(pos, len);
            for (int i = 0; i < len; i++)
            {
                byteList.RemoveAt(pos);
                boolList.RemoveAt(pos);
            }
        }

        static private void insert(ref string msg, ref List<byte> byteList, ref List<bool> boolList, int pos, byte[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                msg = msg.Insert(pos, "-");
                byteList.Insert(pos + i, array[i]);
                boolList.Insert(pos + i, true);
            }
        }

        static private msgFunction isFunction(string msg, int position)
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

                fun.argumentString = msg.Substring(p1 + 1, p2 - (p1 + 1));
                fun.arguments = fun.argumentString.Split(new string[] { ";" }, StringSplitOptions.None);
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
    }
}

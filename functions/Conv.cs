using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace myFunctions
{
	/// <summary>
	/// Conversion
	/// Version:    1.1
	/// Date:       2015-10-02   
	/// </summary>
    static class Conv
	{
		#region Testing number

		/// <summary>
		/// Check if string is Short
		/// </summary>
		/// <param name="text">Number string</param>
		/// <returns>Return True if string is Short</returns>
        public static bool IsShort(string text)
		{
			short value;
			return short.TryParse(text, out value);
		}

		/// <summary>
		/// Check if string is positive Short
		/// </summary>
		/// <param name="text">Number string</param>
		/// <returns>Return True if string is positive short</returns>
        public static bool IsPositiveShort(string text)
		{
			short value;
			bool res = short.TryParse(text, out value);
			if (res)
				if (value < 0) res = false;
			return res;
		}

		/// <summary>
		/// Check if string is Integer
		/// </summary>
		/// <param name="text">Number string</param>
		/// <returns>Return True if string is integer</returns>
        public static bool IsInt(string text)
		{
			int value;
			return int.TryParse(text, out value);
		}

		/// <summary>
		/// Check if string is positive Int
		/// </summary>
		/// <param name="text">Number string</param>
		/// <returns>Return True if string is positive integer</returns>
        public static bool IsPositiveInt(string text)
		{
			int value;
			bool res = int.TryParse(text, out value);
			if (res)
				if (value < 0) res = false;
			return res;
		}

		/// <summary>
		/// Check if string is Float
		/// </summary>
		/// <param name="text">Number string</param>
		/// <returns>Return True if string is float</returns>
        public static bool IsFloat(string text)
		{
			float value;
			return float.TryParse(text, out value);
		}

		/// <summary>
		/// Check if string is positive Float
		/// </summary>
		/// <param name="text">Number string</param>
		/// <returns>Return True if string is positive float</returns>
        public static bool IsPositiveFloat(string text)
		{
			float value;
			bool res = float.TryParse(text, out value);
			if (res)
				if (value < 0) res = false;
			return res;
		}

		/// <summary>
		/// Check if string is DateTime
		/// </summary>
		/// <param name="text">String</param>
		/// <returns>Return True if string is DateTime</returns>
        public static bool IsDate(string text)
		{
			DateTime value;
			return DateTime.TryParse(text, out value);
		}

		/// <summary>
		/// Check if string is number
		/// </summary>
		/// <param name="Expression">Number string</param>
		/// <returns>Return True if string is number</returns>
        public static bool IsNumeric(object Expression)
		{
			bool isNum;
			double retNum;

			isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
			return isNum;
		}

		/// <summary>
		/// Check if string is integer (INT32)
		/// </summary>
		/// <param name="Expression">Number string</param>
		/// <returns>Return True if string is integer</returns>
        public static bool IsInteger(object Expression)
		{
			bool isNum;
			int retNum;

			isNum = Int32.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
			return isNum;
		}

		#endregion

        #region To Number with Default number
		
        /// <summary>
        /// Convert String to Bool with default value on convert Error
        /// </summary>
        /// <param name="text">Number string</param>
        /// <param name="def">Default value</param>
        /// <returns>Short number</returns>
        public static bool ToBoolDef(string text, bool def)
        {
            bool value;
            if (bool.TryParse(text, out value))
                return value;
            else
            {
                if (text == "1")
                {
                    return true;
                }
                else if (text == "0")
                {
                    return false;
                }
                return def;
            }
                
        }

        /// <summary>
        /// Convert String to Short with default value on convert Error
        /// </summary>
        /// <param name="text">Number string</param>
        /// <param name="def">Default value</param>
        /// <returns>Short number</returns>
        public static short ToShortDef(string text, short def)
        {
            short value;
            if (short.TryParse(text, out value))
                return value;
            else
                return def;
        }

        /// <summary>
        /// Convert String to Integer with default value on convert Error
        /// </summary>
        /// <param name="text">Number string</param>
        /// <param name="def">Default value</param>
        /// <returns>Integer number</returns>
        public static int ToIntDef(string text, int def)
        {
            int value;
            if (int.TryParse(text, out value))
                return value;
            else
                return def;
        }

        /// <summary>
        /// Convert String to Int32 with default value on convert Error
        /// </summary>
        /// <param name="text">Number string</param>
        /// <param name="def">Default value</param>
        /// <returns>Integer number</returns>
        public static int ToInt32Def(string text, int def)
        {
            return ToIntDef(text, def);
        }

        /// <summary>
        /// Convert String to Int64 with default value on convert Error
        /// </summary>
        /// <param name="text">Number string</param>
        /// <param name="def">Default value</param>
        /// <returns>Long number</returns>
        public static long ToLongDef(string text, int def)
        {
            long value;
            if (long.TryParse(text, out value))
                return value;
            else
                return def;
        }

        /// <summary>
        /// Convert String to Float with default value on convert Error
        /// </summary>
        /// <param name="text">Number string</param>
        /// <param name="def">Default value</param>
        /// <returns>Float number</returns>
        public static float ToFloatDef(string text, float def)
        {
            float value;
            if (float.TryParse(text, out value))
                return value;
            else
                return def;
        }

        /// <summary>
        /// Convert String to Float with default value on convert Error
        /// </summary>
        /// <param name="text">Number string</param>
        /// <param name="def">Default value</param>
        /// <returns>Float number</returns>
        public static float ToFloatDefI(string text, float def)
        {
            try
            {
                return Convert.ToSingle(text, NumberFormatInfo.InvariantInfo);
            }
            catch (Exception)
            {
                return def;
            }
        }

        /// <summary>
        /// Convert String to Double with default value on convert Error
        /// </summary>
        /// <param name="text">Number string</param>
        /// <param name="def">Default value</param>
        /// <returns>Double number</returns>
        public static double ToDoubleDef(string text, double def)
        {
            double value;
            if (double.TryParse(text, out value))
                return value;
            else
                return def;
        }

        /// <summary>
        /// Convert String to Double with default value on convert Error
        /// </summary>
        /// <param name="text">Number string</param>
        /// <param name="def">Default value</param>
        /// <returns>Double number</returns>
        public static double ToDoubleDef(string text, double def, NumberFormatInfo format)
        {
            if (IsNumeric(text))
                return Convert.ToDouble(text, format);
            else return def;
        }

        /// <summary>
        /// Convert String (Invariant number format - '.')  to Double with default value on convert Error
        /// </summary>
        /// <param name="text">Number string</param>
        /// <param name="def">Default value</param>
        /// <returns>Double number</returns>
        public static double ToDoubleDefI(string text, double def)
        {
            if (IsNumeric(text))
                return Convert.ToDouble(text, NumberFormatInfo.InvariantInfo);
            else return def;
        }

        #endregion
        /// <summary>
        /// Return bool value like "0" or "1"
        /// </summary>
        /// <param name="value">Bool value</param>
        /// <returns>String representation of bool</returns>
        public static string ToString(bool value)
        {
            if (value == true)
                return "1";
            else
                return "0";
        }

        /// <summary>
        /// Return Net Mask from int value
        /// </summary>
        /// <param name="value">Integer value from mask</param>
        /// <returns></returns>
        public static string ToIPMask(int value)
        {
            string res = "";
            uint ires = 0;
            if (value < 0 || value > 32) return "";

            for (int i = 0; i < value; i++ )
            {
                ires <<= 1;
                ires += 1;
            }
            for (int i = 0; i < 32 - value; i++)
            {
                ires <<= 1;
            }
            //res = ires.ToString("x");

            byte[] x = BitConverter.GetBytes(ires);
            res = "";
            for (int i = x.Length-1; i >= 0; i--)
            {
                if (res.Length > 0) res += ".";
                res += x[i].ToString();
            }

            return res;
        }
        #region Array

        /// <summary>
        /// Array to string separated by defined char
        /// </summary>
        /// <param name="Expression">Array</param>
        /// <param name="separator">Separator</param>
        /// <returns>String</returns>
        public static string ArrToStr(object Expression, string separator = "; ")
        {
            string res = "";
            if (Expression.GetType() == typeof(ushort[]))
            {
                ushort[] exp = (ushort[])Expression;
                if (exp.Length > 0) res = exp[0].ToString();
                for (int i = 1; i < exp.Length; i++) res += separator + exp[i].ToString();
            }
            else if (Expression.GetType() == typeof(uint[]))
            {
                uint[] exp = (uint[])Expression;
                if (exp.Length > 0) res = exp[0].ToString();
                for (int i = 1; i < exp.Length; i++) res += separator + exp[i].ToString();
            }
            else if (Expression.GetType() == typeof(int[]))
            {
                int[] exp = (int[])Expression;
                if (exp.Length > 0) res = exp[0].ToString();
                for (int i = 1; i < exp.Length; i++) res += separator + exp[i].ToString();
            }
            else if (Expression.GetType() == typeof(float[]))
            {
                float[] exp = (float[])Expression;
                if (exp.Length > 0) res = exp[0].ToString();
                for (int i = 1; i < exp.Length; i++) res += separator + exp[i].ToString();
            }
            else if (Expression.GetType() == typeof(bool[]))
            {
                string boolStr;
                bool[] exp = (bool[])Expression;
                if (exp.Length > 0)
                {
                    if (exp[0] == true)
                        boolStr = "1";
                    else
                        boolStr = "0";
                    res = boolStr;
                }
                for (int i = 1; i < exp.Length; i++)
                {
                    if (exp[i] == true)
                        boolStr = "1";
                    else
                        boolStr = "0";
                    res += separator + boolStr;
                }
            }
            else
                res = Expression.ToString();
            return res;
        }
        
        /// <summary>
        /// String to Short array
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="separator">Separator</param>
        /// <returns>Short array</returns>
        public static short[] ToInt16Arr(string value, string separator = ";")
        {
            string[] separate = value.Split(new string[] { separator }, StringSplitOptions.None);
            short[] res = new short[separate.Length];
            try
            {
                for (int i = 0; i < separate.Length; i++)
                {
                    res[i] = Convert.ToInt16(separate[i]);
                }
            }
            catch
            {
                res = new short[0];
            }

            return res;
        }

        /// <summary>
        /// String to UShort array
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="separator">Separator</param>
        /// <returns>UShort array</returns>
        public static ushort[] ToUInt16Arr(string value, string separator = ";")
        {
            string[] separate = value.Split(new string[] { separator }, StringSplitOptions.None);
            ushort[] res = new ushort[separate.Length];
            try
            {
                for (int i = 0; i < separate.Length; i++)
                {
                    res[i] = Convert.ToUInt16(separate[i]);
                }
            }
            catch
            {
                res = new ushort[0];
            }

            return res;
        }

        /// <summary>
        /// String to Int array
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="separator">Separator</param>
        /// <returns>Int array</returns>
        public static int[] ToInt32Arr(string value, string separator = ";")
        {
            string[] separate = value.Split(new string[] { separator }, StringSplitOptions.None);
            int[] res = new int[separate.Length];
            try
            {
                for (int i = 0; i < separate.Length; i++)
                {
                    res[i] = Convert.ToInt32(separate[i]);
                }
            }
            catch
            {
                res = new int[0];
            }

            return res;
        }

        /// <summary>
        /// String to UInt array
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="separator">Separator</param>
        /// <returns>UInt array</returns>
        public static uint[] ToUInt32Arr(string value, string separator = ";")
        {
            string[] separate = value.Split(new string[] { separator }, StringSplitOptions.None);
            uint[] res = new uint[separate.Length];
            try
            {
                for (int i = 0; i < separate.Length; i++)
                {
                    res[i] = Convert.ToUInt32(separate[i]);
                }
            }
            catch
            {
                res = new uint[0];
            }

            return res;
        }

        /// <summary>
        /// String to Float array
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="separator">Separator</param>
        /// <returns>Float array</returns>
        public static float[] ToFloatArr(string value, string separator = ";")
        {
            string[] separate = value.Split(new string[] { separator }, StringSplitOptions.None);
            float[] res = new float[separate.Length];
            try
            {
                for (int i = 0; i < separate.Length; i++)
                {
                    res[i] = Convert.ToSingle(separate[i]);
                }
            }
            catch
            {
                res = new float[0];
            }

            return res;
        }

        /// <summary>
        /// String to Bool array
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="separator">Separator</param>
        /// <returns>Bool array</returns>
        public static bool[] StrToBool(string value, string separator = ";")
        {
            string[] separate = value.Split(new string[] { separator }, StringSplitOptions.None);
            bool[] res = new bool[separate.Length];
            try
            {
                for (int i = 0; i < separate.Length; i++)
                {
                    if (separate[i].ToLower().Trim() == "true" || separate[i].Trim() == "1")
                        res[i] = true;
                    else
                        res[i] = false;
                }
            }
            catch
            {
                res = new bool[0];
            }

            return res;
        }

        #endregion

        /// <summary>
        /// Convert byte array to ushort array
        /// </summary>
        /// <param name="byteArr">Byte array</param>
        /// <returns>UShort array</returns>
        public static ushort[] ToUShort(byte[] byteArr)
        {
            try
            {
                if (byteArr.Length % 2 == 1)
                {
                    Array.Resize(ref byteArr, byteArr.Length + 1);
                    for (int i = byteArr.Length - 1; i > 0; i--)
                    {
                        byteArr[i] = byteArr[i - 1];
                    }
                    byteArr[0] = 0;
                }
                ushort[] res = new ushort[byteArr.Length / 2];

                for (int i = 0; i < byteArr.Length / 2; i++)
                {
                    res[i] = BitConverter.ToUInt16(byteArr, i * 2);
                }
                return res;
            }
            catch
            {
                return new ushort[0];
            }

        }

        #region Swap

        /// <summary>
        /// Swaping bytes in ushort
        /// </summary>
        /// <param name="x">UShort number</param>
        /// <returns></returns>
        public static ushort SwapBytes(ushort x)
        {
            return (ushort)((ushort)((x & 0xff) << 8) | ((x >> 8) & 0xff));
        }

        /// <summary>
        /// Swaping bytes in uint
        /// </summary>
        /// <param name="x">Number</param>
        /// <returns></returns>
        public static uint SwapBytes(uint x)
        {
            // swap adjacent 16-bit blocks
            x = (x >> 16) | (x << 16);
            // swap adjacent 8-bit blocks
            return ((x & 0xFF00FF00) >> 8) | ((x & 0x00FF00FF) << 8);
        }

        /// <summary>
        /// Swaping bytes in ulong
        /// </summary>
        /// <param name="x">Number</param>
        /// <returns></returns>
        public static ulong SwapBytes(ulong x)
        {
            // swap adjacent 32-bit blocks
            x = (x >> 32) | (x << 32);
            // swap adjacent 16-bit blocks
            x = ((x & 0xFFFF0000FFFF0000) >> 16) | ((x & 0x0000FFFF0000FFFF) << 16);
            // swap adjacent 8-bit blocks
            return ((x & 0xFF00FF00FF00FF00) >> 8) | ((x & 0x00FF00FF00FF00FF) << 8);
        }

        #endregion

        #region From Hex convert

        /// <summary>
        /// Convert Hex to Uint with default value
        /// </summary>
        /// <param name="hex">Hex number</param>
        /// <param name="def">Dafault value if convert error</param>
        /// <returns>UInt value</returns>
        public static uint HexToUInt(string hex, uint def = 0)
        {
            if (hex.Length > 8) return def;
            while (hex.Length % 8 > 0) hex = '0' + hex;

            try
            {
                return uint.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            } 
            catch (Exception err)
            {
                return def;
            }
        }

        /// <summary>
        /// Convert Hex to byte array
        /// </summary>
        /// <param name="hex">Hex number</param>
        /// <returns>Byte array</returns>
        public static byte[] HexToBytes(string hex)
        {
            /*return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();*/
            if (hex.Length % 2 == 1) hex = '0' + hex;
            try
            {
                return Enumerable.Range(0, hex.Length / 2).Select(x => Byte.Parse(hex.Substring(2 * x, 2), System.Globalization.NumberStyles.HexNumber)).ToArray();
            } catch (Exception)
            {
                return new byte[0];
            }
            
        }

        /// <summary>
        /// Convert Hex to byte array
        /// </summary>
        /// <param name="hex">Hex number</param>
        /// <returns>Byte array</returns>
        public static byte HexToByte(string hex)
        {
            /*return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();*/
            if (hex.Length % 2 == 1) hex = '0' + hex;
            try
            {
                byte[] res = Enumerable.Range(0, hex.Length / 2).Select(x => Byte.Parse(hex.Substring(2 * x, 2), System.Globalization.NumberStyles.HexNumber)).ToArray();
                if (res.Length > 0)
                    return res[0];
                else
                    return 0;
            }
            catch (Exception)
            {
                return 0;
            }

        }

        /// <summary>
        /// Convert Hex to ushort array
        /// </summary>
        /// <param name="hex">Hex number</param>
        /// <returns>UShort array</returns>
        public static ushort[] HexToUShorts(string hex)
        {
            while (hex.Length % 4 > 0) hex = '0' + hex;
            
            return Enumerable.Range(0, hex.Length / 4).Select(x => ushort.Parse(hex.Substring(4 * x, 4), System.Globalization.NumberStyles.HexNumber)).ToArray();
        }

        #endregion
    }
}

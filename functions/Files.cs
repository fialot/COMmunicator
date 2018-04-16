using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;

namespace myFunctions
{
    public struct CSVfile
    {
        public string[] head;
        public List<string[]> data;
    }
    /// <summary>
    /// Files functions
    /// Version:    1.1
    /// Date:       2015-10-02    
    /// </summary>
    static class Files
    {

        #region Load Files

        /// <summary>
        /// Load text file
        /// </summary>
        /// <param name="fileName">Filename</param>
        /// <returns>Return text string</returns>
        public static string LoadFile(string fileName)
        {
            string res = "";

            try
            {
                using (StreamReader reader = new StreamReader(fileName, true))
                {
                    res = reader.ReadToEnd();
                }
            }
            catch
            {
            }
            return res;
        }

        /// <summary>
        /// Load text file
        /// </summary>
        /// <param name="fileName">Filename</param>
        /// <param name="enc">Encoding of file</param>
        /// <returns>Return text string</returns>
        public static string LoadFile(string fileName, Encoding enc)
        {
            string res = "";

            try
            {
                using (StreamReader reader = new StreamReader(fileName, enc, true))
                {
                    res = reader.ReadToEnd();
                }
            }
            catch
            {
            }
            return res;
        }

        /// <summary>
        /// Load text file
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <returns>Return text string</returns>
        public static string LoadFile(Stream stream)
        {
            string res = "";

            try
            {
                using (StreamReader reader = new StreamReader(stream, true))
                {
                    res = reader.ReadToEnd();
                }
            }
            catch
            {
            }
            return res;
        }

        /// <summary>
        /// Load text file
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="enc">Encoding of file</param>
        /// <returns>Return text string</returns>
        public static string LoadFile(Stream stream, Encoding enc)
        {
            string res = "";

            try
            {
                using (StreamReader reader = new StreamReader(stream, enc, true))
                {
                    res = reader.ReadToEnd();
                }
            }
            catch
            {
            }
            return res;
        }

        /// <summary>
        /// Load text file lines
        /// </summary>
        /// <param name="fileName">Filename</param>
        /// <returns>Return text string[]</returns>
        public static string[] LoadFileLines(string fileName, bool removeEmptyLines = false)
        {
            string res = LoadFile(fileName);

            if (res.Length > 0)
                if (removeEmptyLines)
                    return res.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                else
                    return res.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
            else return new string[0];
        }

        /// <summary>
        /// Parsing CSV files to CSV structure
        /// </summary>
        /// <param name="txt">Text to parsing</param>
        /// <returns>CSVfile structure</returns>
        public static CSVfile ParseCSV(string txt, string separator = ";")
        {
            CSVfile table;
            table.head = new string[0];
            table.data = new List<string[]>();


            string[] lines = txt.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                if (i == 0)
                    table.head = lines[i].Split(new string[] { separator }, StringSplitOptions.None);
                else
                {
                    string[] values = lines[i].Split(new string[] { separator }, StringSplitOptions.None);
                    table.data.Add(values);
                }
            }
            return table;
        }

        /// <summary>
        /// Load CSV string to string table
        /// </summary>
        /// <param name="txt">CSV string</param>
        /// <returns></returns>
        public static string[,] LoadCSV(string txt)
        {
            string[] lines = txt.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            string[] radek = lines[0].Split(new string[] { ";" }, StringSplitOptions.None);
            string[,] data = new string[radek.Length, lines.Length];

            int n = radek.Length;

            for (int i = 0; i < lines.Length; i++)
            {
                radek = lines[i].Split(new string[] { ";" }, StringSplitOptions.None);
                for (int j = 0; j < n; j++)
                {
                    if (j < radek.Length) data[j, i] = radek[j];
                }
            }
            return data;
        }

        #endregion

        #region Save Files

        /// <summary>
        /// Save text file
        /// </summary>
        /// <param name="filename">Filename</param>
        /// <param name="text">Text to saving</param>
        /// <param name="append">Append to file</param>
        /// <returns>Return True if save Ok</returns>
        public static bool SaveFile(string filename, string text, bool append = false)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filename, append))
                {
                    writer.Write(text);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Save text file
        /// </summary>
        /// <param name="filename">Filename</param>
        /// <param name="text">Text to saving</param>
        /// <param name="append">Append to file</param>
        /// <returns>Return True if save Ok</returns>
        public static bool SaveFile(string filename, string[] text, bool append = false)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filename, append))
                {
                    for (int i = 0; i < text.Length; i++)
                    {
                        if (text[i] != null) 
                        {
                            if (i > 0) writer.Write("\n");
                            writer.Write(text[i]);
                        }
                        
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Save text file
        /// </summary>
        /// <param name="filename">Filename</param>
        /// <param name="text">Text to saving</param>
        /// <param name="enc">Encoding</param>
        /// <param name="append">Append to file</param>
        /// <returns>Return True if save Ok</returns>
        public static bool SaveFile(string filename, string text, Encoding enc, bool append = false)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filename, append, enc))
                {
                    writer.Write(text);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Save text file
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="text">Text to saving</param>
        /// <returns>Return True if save Ok</returns>
        public static bool SaveFile(Stream stream, string text)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(text);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Save text file
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="text">Text to saving</param>
        /// <param name="enc">Encoding</param>
        /// <param name="append">Append to file</param>
        /// <returns>Return True if save Ok</returns>
        public static bool SaveFile(Stream stream, string text, Encoding enc)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(stream, enc))
                {
                    writer.Write(text);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion


        // ----- SERIALIZE OBJECT -----
        /// <summary>
        /// Serialize object
        /// </summary>
        /// <param name="fileName">Save to file</param>
        /// <param name="item">Object to serialize</param>
        /// <returns>Return true if saving OK</returns>
        public static bool ExportXml(string fileName, object item)
        {
            DataContractSerializer serializer = new DataContractSerializer(item.GetType());

            // ----- DELETE PREVIOUSLY FILE -----
            try
            {
                if (File.Exists(fileName)) File.Delete(fileName);
            }
            catch (Exception) { }

            // ----- CREATE NEW FILE -----
            try
            {
                using (FileStream stream = File.OpenWrite(fileName))
                {
                    serializer.WriteObject(stream, item);
                }
                return true;
            }
            catch (Exception)
            {
                //Dialogs.ShowErr(err.Message, "Error");
                return false;
            }
        }

        // ----- DESERIALIZE TO OBJECT -----
        /// <summary>
        /// Deserialize object
        /// </summary>
        /// <param name="fileName">Load from file</param>
        /// <param name="item">Deserialize to object</param>
        /// <returns>Deserialize object (return null if deserialize error)</returns>
        public static object ImportXml(string fileName, object item)
        {
            DataContractSerializer serializer = new DataContractSerializer(item.GetType());
            try
            {
                if (File.Exists(fileName))
                {
                    using (FileStream stream = File.OpenRead(fileName))
                    {
                        item = serializer.ReadObject(stream);
                    }
                }
            }
            catch (Exception)
            {
                item = null;
            }
            return item;
        }

        public static string ReplaceVarPaths(string path)
        {
            path = path.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            path = path.Replace("%AppPath%", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            return path;
        }
    }

}

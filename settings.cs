using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using Ini;

namespace COMunicator
{
    static class settings
    {
        

        public struct TPaths
        {
            public string dataFolder;
        }

        public static TPaths Paths;
                
        public static int tab = 0;


        public static void LoadSettings()
        {
            IniParser ini;
            ini = new IniParser("./config.ini");



            // ----- PATHS -----
            Paths.dataFolder = ini.Read("Path", "dataFolder", @"%AppData%" + System.IO.Path.DirectorySeparatorChar + "COMunicator");
            

            tab = ini.ReadInt("GUI", "Tab", 0);
            
            /*string enc = ini.Read("Send", "Encoding", Encoding.Default.HeaderName);

            EncodingInfo[] x = Encoding.GetEncodings();     // načíst dostupné kódování

            bool encExist = false;
            for (int i = 0; i < x.GetLength(0); i++)        // kontrola je-li existující kódování
            {
                if (x[i].Name == enc)                  //  kontrola podle jména
                {
                    encExist = true;
                    try
                    {
                        encoding = Encoding.GetEncoding(enc);
                    }
                    catch (Exception)
                    {
                        encoding = System.Text.Encoding.Default;
                    }
                    break;
                }
            }

            if (!encExist)                                  // pokud neexistuje -> default
            {
                enc = Encoding.Default.HeaderName;
                encoding = System.Text.Encoding.Default;
            }*/
        }

        public static void SaveSettings()
        {
            IniParser ini;
            ini = new IniParser("./config.ini");


            // ----- PATHS -----
            ini.Write("Path", "dataFolder", Paths.dataFolder);



            ini.Write("GUI", "Tab", tab);

            ini.SaveFile();

        }
    }
}

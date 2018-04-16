using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LOG
{
    static class Log
    {
        static FileStream LogFile;
        static StreamWriter FileWriter;
        static string fileName;
        static DateTime lastDate;
        static bool newDayFile;

        public static bool init(string logFileName, string title, bool newDayFile = false)
        {
            fileName = logFileName;
            Log.newDayFile = newDayFile;
            lastDate = DateTime.Now;
            try
            {
                LogFile = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None);
                FileWriter = new StreamWriter(LogFile);

                FileWriter.WriteLine("");
                FileWriter.WriteLine("---------------------------------------------------------");
                FileWriter.WriteLine("   " + title + "   " + DateTime.Now.ToString());
                FileWriter.WriteLine("---------------------------------------------------------");

                FileWriter.Close();
                LogFile.Close();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void add(string txt)
        {
            try
            {

                LogFile = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None);
                FileWriter = new StreamWriter(LogFile);

                if (lastDate.Day != DateTime.Now.Day || lastDate.Month != DateTime.Now.Month)
                {
                    // ----- IF NEW LOG PER DAY -----
                    if (newDayFile)
                    {

                    }

                    // ----- IF NEW DAY -> NEW HEAD -----
                    FileWriter.WriteLine("");
                    FileWriter.WriteLine("---------------------------------------------------------");
                    FileWriter.WriteLine("   " + DateTime.Now.ToString());
                    FileWriter.WriteLine("---------------------------------------------------------");
                }
                lastDate = DateTime.Now;

                //FileWriter.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " " + txt);
                FileWriter.WriteLine(txt);
                FileWriter.Close();
                LogFile.Close();
            }
            catch (Exception)
            {

            }
        }

    }
}

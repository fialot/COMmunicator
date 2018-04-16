using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace myFunctions
{
    /// <summary>
    /// Device functions
    /// Version:    1.0
    /// Date:       2015-06-26    
    /// </summary>
    static class DevFun
    {
        /// <summary>
        /// Get last meas date from RAMS XML file
        /// </summary>
        /// <param name="text">XML file string</param>
        /// <returns>Date string</returns>
        public static string getLastMeas(string text)
        {
            const string begin = "<lastmeasuring>";
            const string end = "</lastmeasuring>";

            int p = text.IndexOf(begin);
            if (p >= 0)
            {
                int p2 = text.IndexOf(end, p);
                if (p2 >= 0)
                {
                    string res = text.Substring(p + begin.Length, p2 - (p + begin.Length));
                    if ((res.Length > 0) && (res[res.Length - 1] == 'Z'))
                        res = res.Remove(res.Length - 1);
                    res = res.Replace('T', ' ');
                    return res;
                }
            }
            return "";

        }
    }
}

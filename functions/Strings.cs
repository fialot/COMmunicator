using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace myFunctions
{
    /// <summary>
    /// Strings functions
    /// Version:    1.0
    /// Date:       2015-06-26    
    /// </summary>
    static class Strings
    {
        /// <summary>
        /// Convert String to Stream
        /// </summary>
        /// <param name="s">Text string</param>
        /// <returns>Stream</returns>
        public static Stream GetStream(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static Stream getStream(string s)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(s ?? ""));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logging;

namespace GlobalClasses
{
    public static class Global
    {
        public static ProcessLog Log = new ProcessLog(showDate: false);
        public static ProcessLog LogPacket = new ProcessLog(showDate: false);
    }
}

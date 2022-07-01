using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Fx.IO;
using Fx.Logging;
using Fx.Plugins;

namespace GlobalClasses
{

    public static class Global
    {
        //public static ProcessLog Log = new ProcessLog(showDate: false);
        public static ProcessLog LogPacket = new ProcessLog(showDate: false);

        public static PluginLoader PL = new PluginLoader();
    }

}

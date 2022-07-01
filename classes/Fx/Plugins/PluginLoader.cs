using Fx.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Plugins
{
    public class PluginLoader
    {
        public static List<IPlugin> Plugins { get; set; } = new List<IPlugin>();
        public static List<IPluginProtocol> PluginsProtocol { get; set; } = new List<IPluginProtocol>();

        public void LoadPlugins()
        {
            var pluginList = Directory.GetFiles(Paths.GetAppPath() + "plugins", "*.dll");

            foreach (var path in pluginList)
            {
                try
                {
                    var DLL = Assembly.LoadFile(path);

                    Type pluginType = typeof(IPlugin);
                    Type[] types = DLL.GetExportedTypes().Where(p => pluginType.IsAssignableFrom(p) && p.IsClass).ToArray();

                    foreach (Type type in DLL.GetExportedTypes())
                    {
                        var x = typeof(IPlugin).IsAssignableFrom(type);

                        if (!type.IsAbstract && type.IsClass)
                        {

                            var plugin = Activator.CreateInstance(type);
                            /*Plugins.Add(plugin);

                            if (plugin.GetPluginType() == EPluginType.Protocol)
                            {
                                PluginsProtocol.Add((IPluginProtocol)plugin);
                            }*/
                        }
                    }
                }
                catch ( Exception err)
                {
                    int x = 1;
                }
            }
        }

    }
}

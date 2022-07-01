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
        /// <summary>
        /// List of all plugins
        /// </summary>
        public List<IPlugin> Plugins { get; set; } = new List<IPlugin>();

        /// <summary>
        /// List of Protocol plugins
        /// </summary>
        public List<IPluginProtocol> PluginsProtocol { get; set; } = new List<IPluginProtocol>();

        /// <summary>
        /// Load plugins
        /// </summary>
        public void LoadPlugins()
        {
            // ----- Clear lists -----
            Plugins.Clear();
            PluginsProtocol.Clear();


            // ----- Search plugins in folder -----
            var pluginList = Directory.GetFiles(Paths.GetAppPath() + "plugins", "*.dll");

            // ----- Check each file -----
            foreach (var path in pluginList)
            {
                try
                {
                    // ----- Load dll -----
                    var DLL = Assembly.LoadFile(path);

                    // ----- Check if plugin -----
                    Type pluginType = typeof(IPlugin);
                    Type[] types = DLL.GetExportedTypes().Where(p => pluginType.IsAssignableFrom(p) && p.IsClass).ToArray();

                    // ----- Add plugin -----
                    foreach (Type type in types)
                    {
                        // ----- Add to list -----
                        var plugin = (IPlugin)Activator.CreateInstance(type);

                        Plugins.Add(plugin);

                        // ----- Create Protocol plugin list -----
                        if (plugin.GetPluginType() == EPluginType.Protocol)
                        {
                            PluginsProtocol.Add((IPluginProtocol)plugin);
                        }
                    }
                }
                catch {}
            }
        }

        public IPlugin GetPlugin(string name)
        {
            foreach(var item in Plugins)
            {
                if (item.GetName() == name)
                    return item;
            }
            return null;
        }
    }
}

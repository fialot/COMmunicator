using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Plugins
{
    public enum EPluginType
    {
        General = 1, Protocol = 2
    }

    public interface IPlugin
    {
        string GetName();
        string GetDescription();
        string GetVersion();
        EPluginType GetPluginType();
        string GetPluginTypeString();
    }

    public interface IPluginProtocol
    {
        string GetName();
        string GetDescription();
        string GetVersion();

        string GetFunctionName();
        byte[] CreatePacket(string message);
        string ParsePacket(byte[] message);
    }
}

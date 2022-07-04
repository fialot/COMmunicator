using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Plugins
{
    /// <summary>
    /// Plugin types
    /// </summary>
    public enum EPluginType
    {
        General = 1, Protocol = 2
    }

    /// <summary>
    /// Global plugin interface
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Get plugin name
        /// </summary>
        /// <returns>Plugin name</returns>
        string GetName();

        /// <summary>
        /// Get Plugin description
        /// </summary>
        /// <returns>Plugin description</returns>
        string GetDescription();

        /// <summary>
        /// Get plugin version
        /// </summary>
        /// <returns>Plugin version</returns>
        string GetVersion();

        /// <summary>
        /// Get plugin type
        /// </summary>
        /// <returns>Plugin type number</returns>
        EPluginType GetPluginType();

        /// <summary>
        /// Get plugin type string
        /// </summary>
        /// <returns>Plugin type</returns>
        string GetPluginTypeString();
    }

    /// <summary>
    /// Interface Plugin - Protocol
    /// </summary>
    public interface IPluginProtocol
    {
        /// <summary>
        /// Get plugin name
        /// </summary>
        /// <returns>Plugin name</returns>
        string GetName();

        /// <summary>
        /// Get Plugin description
        /// </summary>
        /// <returns>Plugin description</returns>
        string GetDescription();

        /// <summary>
        /// Get plugin version
        /// </summary>
        /// <returns>Plugin version</returns>
        string GetVersion();

        /// <summary>
        /// Get function name
        /// </summary>
        /// <returns>Function name</returns>
        string GetFunctionName();

        /// <summary>
        /// Create packet from text
        /// </summary>
        /// <param name="message">Text</param>
        /// <returns>Packet bytes</returns>
        byte[] CreatePacket(string message);

        /// <summary>
        /// Parse packet
        /// </summary>
        /// <param name="message">Bytes</param>
        /// <returns>Text output</returns>
        string ParsePacket(byte[] message);

        /// <summary>
        /// Parse packet
        /// </summary>
        /// <param name="message">Bytes</param>
        /// <param name="request">Is request or reply</param>
        /// <returns>Text output</returns>
        string ParsePacket(byte[] message, bool request);

        /// <summary>
        /// Acknowledge of data reception
        /// </summary>
        /// <param name="message">Incomming data</param>
        /// <returns>Acknowledge packet</returns>
        byte[] AcknowledgeReception(byte[] message);
    }
}

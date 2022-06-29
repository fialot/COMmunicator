using Fx.IO;
using Fx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AppSettings
{

    /// <summary>
    /// File settings class
    /// </summary>
    public class AppSettings
    {
        public string DataFolder { get; set; } = @"%AppData%" + System.IO.Path.DirectorySeparatorChar + "COMunicator";


        /// <summary>
        /// Default constructor
        /// </summary>
        public AppSettings()
        {

        }


        /// <summary>
        /// Constructor with load settings from XML element
        /// </summary>
        /// <param name="xml">Connection XML element</param>
        public AppSettings(XElement xml)
        {
            Load(xml);
        }

        /// <summary>
        /// Loading setting from XML element
        /// </summary>
        /// <param name="xml">XML element</param>
        /// <returns>Indication on decrypted password (need save setting)</returns>
        public bool Load(XElement xml)
        {
            //XElement mainGroup;
            //XElement group;
            XElement element;
            //XAttribute attribute;

            bool needSave = false;

            // ----- Tabs -----
            element = xml.Element("dataFolder");
            if (element != null)
            {
                this.DataFolder = element.Value;
            }


            return needSave;
        }

        /// <summary>
        /// Create XML element from settings
        /// </summary>
        /// <returns>XML element</returns>
        public XElement GetXmlElement()
        {
            // ----- Write GUI settings -----
            var connElement = new XElement("app");

            connElement.Add(new XElement("dataFolder", this.DataFolder));

            return connElement;
        }
    }

}

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
    public class GuiSettings
    {
        public int TabIndex { get; set; } = 0;



        /// <summary>
        /// Default constructor
        /// </summary>
        public GuiSettings()
        {

        }


        /// <summary>
        /// Constructor with load settings from XML element
        /// </summary>
        /// <param name="xml">Connection XML element</param>
        public GuiSettings(XElement xml)
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

            // ----- Encoding -----
            element = xml.Element("tab");
            if (element != null)
            {
                this.TabIndex = Conv.ToIntDef(element.Value, 0);
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
            var connElement = new XElement("GUI");

            connElement.Add(new XElement("tab", this.TabIndex.ToString()));

            return connElement;
        }
    }

}

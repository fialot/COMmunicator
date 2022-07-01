using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Fx.IO;
using Fx.Logging;

namespace AppSettings
{
    public static class Settings
    {
        
        /// <summary>
        /// Connection settings
        /// </summary>
        public static ConnectionSetting Connection = new ConnectionSetting();
        public static MsgSettings Messages = new MsgSettings();
        public static GuiSettings GUI = new GuiSettings();
        public static AppSettings App = new AppSettings();

        /// <summary>
        /// Path to xml settings file
        /// </summary>
        static string xmlPath = "";


        /// <summary>
        /// Load settings from XML file
        /// </summary>
        public static void LoadXml()
        {
            // ----- Get xml file path -----
            if (xmlPath == "")
                xmlPath = GetXmlPath();

            // ----- Load settings from XML -----
            LoadXml(xmlPath);
        }

        /// <summary>
        /// Load settings from XML file
        /// </summary>
        /// <param name="path">Xml file path</param>
        public static void LoadXml(string path)
        {
            string XMLtext = Files.LoadFile(path);

            try
            {
                bool save = false;
                // ----- Parse XML to Structure -----
                var xml = XDocument.Parse(XMLtext);
                XElement settings;
                XElement mainGroup;

                settings = xml.Element("settings");

                // ----- Connection section -----
                mainGroup = settings.Element("connection");
                if (mainGroup != null)
                {
                    if (Connection.Load(mainGroup))
                    {
                        mainGroup = Connection.GetXmlElement();
                        save = true;
                    }    
                }

                // ----- Messages section -----
                mainGroup = settings.Element("messages");
                if (mainGroup != null)
                {
                    Messages.Load(mainGroup);
                }

                // ----- GUI section -----
                mainGroup = settings.Element("GUI");
                if (mainGroup != null)
                {
                    GUI.Load(mainGroup);
                }

                // ----- GUI section -----
                mainGroup = settings.Element("app");
                if (mainGroup != null)
                {
                    App.Load(mainGroup);
                }


                if (save)
                {
                    xml.Save(path);
                }

            }
            catch (Exception)
            {

            }

        }

        /// <summary>
        /// Save settings to XML
        /// </summary>
        /// <returns>Return true if succesfully saved</returns>
        public static bool SaveXml()
        {
            // ----- Get xml file path -----
            if (xmlPath == "")
                xmlPath = GetXmlPath();

            // ----- Save XML -----
            return SaveXml(xmlPath);
        }

        /// <summary>
        /// Save settings to XML
        /// </summary>
        /// <param name="path">XML file path</param>
        /// <returns>Return true if succesfully saved</returns>
        public static bool SaveXml(string path)
        {
            try
            {
                // ----- Parse XML to Structure -----
                var xml = new XDocument(new XDeclaration("1.0", "utf-8", null));
                var mainElement = new XElement("settings");
                xml.Add(mainElement);



                // ----- Write connection settings -----
                var element = Connection.GetXmlElement();
                mainElement.Add(element);

                // ----- Write messages settings -----
                element = Messages.GetXmlElement();
                mainElement.Add(element);

                // ----- Write GUI settings -----
                element = GUI.GetXmlElement();
                mainElement.Add(element);

                // ----- Write App settings -----
                element = App.GetXmlElement();
                mainElement.Add(element);

                // ---- Save to XML -----
                xml.Save(path);

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get default XML file path
        /// </summary>
        /// <returns>XML file path</returns>
        static string GetXmlPath()
        {
            string xmlPath = Paths.GetAppPath() + "config" + Path.DirectorySeparatorChar + "settings.xml";

            // ----- If not local file -----
            if (!File.Exists(xmlPath))
            {
                // ----- Change to AppData folder -----
                xmlPath = Paths.GetAppDataPath() + "COMunicator";
                // ----- Create AppData folder -----
                try
                {
                    if (!Directory.Exists(xmlPath))
                        Directory.CreateDirectory(xmlPath);
                }
                catch { }

                xmlPath = xmlPath + Path.DirectorySeparatorChar + "settings.xml";
            }

            return xmlPath;
        }
    }

}

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
    public class MsgSettings
    {
        
        public ePacketView PacketView { get; set; } = ePacketView.StringReplaceCommandChars;
        public bool UseLineSeparatingChar { get; set; } = false;
        public string LineSeparatingChar { get; set; } = "";
        public bool ShowDirection { get; set; } = true;
        public bool ShowTime { get; set; } = true;
        public bool ShowBaudRate { get; set; } = true;
        public int MaxRecords { get; set; } = 100;


        public bool SaveToFile { get; set; } = false;
        public string LogFileDirectory { get; set; } = "";

        public bool ClearEditbox { get; set; } = false;
        public bool AddEndChar { get; set; } = false;
        public string EndChar { get; set; } = "";

        public int LastCharTimeout { get; set; } = 10;
        public bool EnableAutoSending { get; set; } = false;
        public int AutoSendingPeriod { get; set; } = 1000;
        public bool WaitForReply { get; set; } = true;
        public int WaitForReplyTimeout { get; set; } = 1000;
        public bool EnableSendingFile { get; set; } = false;
        public string SendingFile { get; set; } = "";
        public bool SendingFileRepeating { get; set; } = true;

        public bool EnableReplyFile { get; set; } = false;
        public string ReplyFile { get; set; } = "";



        /// <summary>
        /// Default constructor
        /// </summary>
        public MsgSettings()
        {

        }


        /// <summary>
        /// Constructor with load settings from XML element
        /// </summary>
        /// <param name="xml">Connection XML element</param>
        public MsgSettings(XElement xml)
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
            XElement mainGroup;
            XElement group;
            XElement element;
            XAttribute attribute;

            bool needSave = false;


            // ----- Format settings -----
            group = xml.Element("format");
            if (group != null)
            {
                // ----- View type -----
                element = group.Element("view_type");
                if (element != null)
                {
                    this.PacketView = Conv.ToEnum<ePacketView>(element.Value, ePacketView.StringReplaceCommandChars);
                }

                // ----- Line separator -----
                element = group.Element("line_separator");
                if (element != null)
                {
                    this.LineSeparatingChar = element.Value;

                    attribute = element.Attribute("enable");
                    if (attribute != null)
                    {
                        this.UseLineSeparatingChar = Conv.ToBoolDef(attribute.Value, false);
                    }
                }

                // ----- Show -----
                element = group.Element("show");
                if (element != null)
                {
                    this.MaxRecords = Conv.ToIntDef(element.Value, 100);

                    attribute = element.Attribute("direction");
                    if (attribute != null)
                    {
                        this.ShowDirection = Conv.ToBoolDef(attribute.Value, false);
                    }

                    attribute = element.Attribute("time");
                    if (attribute != null)
                    {
                        this.ShowTime = Conv.ToBoolDef(attribute.Value, false);
                    }

                    attribute = element.Attribute("baudrate");
                    if (attribute != null)
                    {
                        this.ShowBaudRate = Conv.ToBoolDef(attribute.Value, false);
                    }
                }
            }


            // ----- Sending settings -----
            mainGroup = xml.Element("sending");
            if (mainGroup != null)
            {
                // ----- EditBox -----
                group = mainGroup.Element("edit_box");
                if (group != null)
                {
                    attribute = group.Attribute("clear");
                    if (attribute != null)
                    {
                        this.ClearEditbox = Conv.ToBoolDef(attribute.Value, false);
                    }

                    // ----- End char -----
                    element = group.Element("end_char");
                    if (element != null)
                    {
                        attribute = element.Attribute("enable");
                        if (attribute != null)
                        {
                            this.AddEndChar = Conv.ToBoolDef(attribute.Value, false);
                        }

                        this.EndChar = element.Value;
                    }
                }

                // ----- Auto-sendings -----
                group = mainGroup.Element("auto_sending");
                if (group != null)
                {
                    attribute = group.Attribute("enable");
                    if (attribute != null)
                    {
                        this.EnableAutoSending = Conv.ToBoolDef(attribute.Value, false);
                    }

                    // ----- Period -----
                    element = group.Element("period");
                    if (element != null)
                    {
                        this.AutoSendingPeriod = Conv.ToIntDef(element.Value, 1000);
                    }

                    // ----- Wait for reply -----
                    element = group.Element("wait_for_reply");
                    if (element != null)
                    {
                        attribute = element.Attribute("enable");
                        if (attribute != null)
                        {
                            this.WaitForReply = Conv.ToBoolDef(attribute.Value, false);
                        }

                        this.WaitForReplyTimeout = Conv.ToIntDef(element.Value, 1000);
                    }

                    // ----- Autosending from file -----
                    element = group.Element("from_file");
                    if (element != null)
                    {
                        attribute = element.Attribute("enable");
                        if (attribute != null)
                        {
                            this.EnableSendingFile = Conv.ToBoolDef(attribute.Value, false);
                        }

                        attribute = element.Attribute("repeat");
                        if (attribute != null)
                        {
                            this.SendingFileRepeating = Conv.ToBoolDef(attribute.Value, false);
                        }

                        this.SendingFile = element.Value;
                    }

                }

                // ----- Data reception -----
                group = mainGroup.Element("reception");
                if (group != null)
                {
                    // ----- Reply file -----
                    element = group.Element("lastCharTimeout");
                    if (element != null)
                    {
                        this.LastCharTimeout = Conv.ToIntDef(element.Value, 15);
                    }

                    // ----- Reply file -----
                    element = group.Element("reply_file");
                    if (element != null)
                    {
                        attribute = element.Attribute("enable");
                        if (attribute != null)
                        {
                            this.EnableReplyFile = Conv.ToBoolDef(attribute.Value, false);
                        }

                        this.ReplyFile = element.Value;
                    }
                }   
            }


            // ----- File settings -----
            group = xml.Element("file");
            if (group != null)
            {
                attribute = group.Attribute("enable");
                if (attribute != null)
                {
                    this.SaveToFile = Conv.ToBoolDef(attribute.Value, false);
                }

                // ----- IP address -----
                element = group.Element("path");
                if (element != null)
                {
                    this.LogFileDirectory = element.Value;
                }
            }

            return needSave;
        }

        /// <summary>
        /// Create XML element from settings
        /// </summary>
        /// <returns>XML element</returns>
        public XElement GetXmlElement()
        {


            // ----- Write device settings -----
            var msgElement = new XElement("messages");
            
            var formatElement = new XElement("format");
            formatElement.Add(new XElement("view_type", this.PacketView.ToString()));
            var lineSepChar = new XElement("line_separator", this.LineSeparatingChar);
            lineSepChar.Add(new XAttribute("enable", Conv.ToString(this.UseLineSeparatingChar)));
            formatElement.Add(lineSepChar);
            var show = new XElement("show", this.MaxRecords.ToString());
            show.Add(new XAttribute("direction", Conv.ToString(this.ShowDirection)));
            show.Add(new XAttribute("time", Conv.ToString(this.ShowTime)));
            show.Add(new XAttribute("baudrate", Conv.ToString(this.ShowBaudRate)));
            formatElement.Add(show);
            msgElement.Add(formatElement);


            var sendElement = new XElement("sending");
            var editBox = new XElement("edit_box");
            editBox.Add(new XAttribute("clear", Conv.ToString(this.ClearEditbox)));
            var endChar = new XElement("end_char");
            endChar.Add(new XAttribute("enable", Conv.ToString(this.AddEndChar)));
            editBox.Add(endChar);
            sendElement.Add(editBox);

            var autoSending = new XElement("auto_sending");
            autoSending.Add(new XAttribute("enable", Conv.ToString(this.EnableAutoSending)));
            autoSending.Add(new XElement("period", this.AutoSendingPeriod.ToString()));

            var waitReply = new XElement("wait_for_reply", this.WaitForReplyTimeout.ToString());
            waitReply.Add(new XAttribute("enable", Conv.ToString(this.WaitForReply)));
            autoSending.Add(waitReply);

            var fromFile = new XElement("from_file", this.SendingFile);
            fromFile.Add(new XAttribute("enable", Conv.ToString(this.EnableSendingFile)));
            fromFile.Add(new XAttribute("repeat", Conv.ToString(this.SendingFileRepeating)));
            autoSending.Add(fromFile);

            sendElement.Add(autoSending);

            var reception = new XElement("reception");
            reception.Add(new XElement("lastCharTimeout", this.LastCharTimeout));
            var replyFile = new XElement("reply_file", this.ReplyFile);
            replyFile.Add(new XAttribute("enable", Conv.ToString(this.EnableReplyFile)));
            reception.Add(replyFile);
            sendElement.Add(reception);

            msgElement.Add(sendElement);


            var fileElement = new XElement("file");
            fileElement.Add(new XAttribute("enable", Conv.ToString(this.SaveToFile)));
            fileElement.Add(new XElement("path", this.LogFileDirectory));
            msgElement.Add(fileElement);

            return msgElement;
        }
    }

}

using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Ini
{
    /// <summary>
    /// Namespace for Ini Parser
    /// </summary>
    static class NamespaceDoc { }

    /// <summary>
    /// Create a New INI file to store or load data
    /// Version:    1.0.0
    /// Date:       2015-06-26
    /// </summary>
    public class IniParser
    {
        
        #region Structures

        /// <summary>
        /// Key Item structure
        /// </summary>
        public struct item_t
        {
            /// <summary>
            /// Item Key
            /// </summary>
            public string key;
            /// <summary>
            /// Item Value
            /// </summary>
            public string value;
            /// <summary>
            /// Item Comment
            /// </summary>
            public string comment;
        }

        /// <summary>
        /// Section Item structure
        /// </summary>
        public struct section_t
        {
            /// <summary>
            /// List of Key Items
            /// </summary>
            public List<item_t> item;
            /// <summary>
            /// Section name
            /// </summary>
            public string name;
            /// <summary>
            /// Section comment
            /// </summary>
            public string comment;
        }

        #endregion

        #region Variables

        private List<section_t> section;
        private string endComment;
        private string path;

        #endregion

        #region Private functions

        /// <summary>
        /// IniParser Constructor
        /// </summary>
        /// <param name="filename">INI Filename</param>
        public IniParser(string filename = "")
        {
            path = filename;
            section = new List<section_t>();
            endComment = "";
            if (filename != "") LoadFile(filename);
        }

        /// <summary>
        /// Function isComment
        /// </summary>
        /// <param name="text">Line of INI file</param>
        /// <returns>Returns true if line is comment</returns>
        private bool isComment(ref string text)
        {
            if (text.Length == 0) return true;
            if (text[0] == '#' || text[0] == ';')
            {
                text = text.Remove(0, 1);
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Function isComment
        /// </summary>
        /// <param name="text">Line of INI file</param>
        /// <returns>Returns name if it is section, otherwire return ""</returns>
        private string isSection(string text)
        {
            int pos1 = text.IndexOf("[");
            int pos2 = text.IndexOf("]");
            if ((pos1 >= 0) && (pos2 >= 0))
            {
                if (pos2 > pos1)
                {
                    return text.Substring(pos1 + 1, pos2 - pos1-1);
                }
            }
            return "";
        }

        /// <summary>
        /// Function isKey
        /// </summary>
        /// <param name="text">Line of INI file</param>
        /// <returns>Returns key item with key name if it is key, otherwire return item.key = ""</returns>
        private item_t isKey(string text)
        {
            item_t item;
            item.comment = "";
            item.key = "";
            item.value = "";

            string[] sItem = text.Split(new string[] { "=" }, StringSplitOptions.None);
            if (sItem.Length == 2)
            {
                item.key = sItem[0].Trim();
                item.value = sItem[1].Trim();
            }
            else if (sItem.Length > 2)
            {
                item.key = sItem[0].Trim();
                item.value = text.Substring(text.IndexOf('=')+1, text.Length - text.IndexOf('=') -1);
            }
            return item;
        }

        /// <summary>
        /// Parse INI string to section list
        /// </summary>
        /// <param name="text">INI string</param>
        private void Parse(string text)
        {
            section.Clear();
            string nowComment = "";
            string nowSection = "";
            item_t nowKey;
            section_t iSection;

            // ----- SPLIT LINES -----
            string[] lines = text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Trim();             // trim lines
                // ----- CHECK IF COMMENT LINE -----
                if (isComment(ref lines[i]))
                {
                    if (nowComment != "") nowComment += "\n";
                    nowComment += lines[i];
                }
                else
                {
                    // ----- CHECK IF SECTION LINE -----
                    nowSection = isSection(lines[i]);
                    if (nowSection != "")
                    {
                        iSection.name = nowSection;
                        iSection.comment = nowComment;
                        iSection.item = new List<item_t>();   

                        nowComment = "";
                        section.Add(iSection);
                    }
                    else
                    {
                        // ----- CHECK IF KEY LINE -----
                        nowKey = isKey(lines[i]);
                        if (nowKey.key != "")
                        {
                            if (section.Count == 0)
                            {
                                iSection.name = "";
                                iSection.comment = "";
                                iSection.item = new List<item_t>();
                                section.Add(iSection);
                            }
                            nowKey.comment = nowComment;
                            nowComment = "";
                            section[section.Count - 1].item.Add(nowKey);
                        }
                    }
                }
            }
            endComment = nowComment;
        }

        /// <summary>
        /// Creating INI string from section list
        /// </summary>
        /// <param name="endl">Line Separator</param>
        /// <returns>INI string</returns>
        private string UnParse(string endl = "")
        {
            if (endl == "") endl = System.Environment.NewLine;
            string text = "";
            for (int i = 0; i < section.Count; i++)
            {
                if (section[i].comment.Length > 0)
                    text += createComment(section[i].comment, endl);
                if (section[i].name != "")
                    text += "[" + section[i].name + "]" + endl;

                for (int j = 0; j < section[i].item.Count; j++)
                {
                    if (section[i].item[j].comment.Length > 0)
                        text += createComment(section[i].item[j].comment, endl);
                    text += section[i].item[j].key + "=" + section[i].item[j].value + endl;
                }
            }
            text += createComment(endComment, endl);
            text = text.Remove(text.Length - endl.Length, endl.Length);
            return text;
        }

        /// <summary>
        /// Create comment function for UnParse function
        /// </summary>
        /// <param name="comment">Comment</param>
        /// <param name="endl">Line separator</param>
        /// <returns></returns>
        private string createComment(string comment, string endl = "")
        {
            if (endl == "") endl = System.Environment.NewLine;
            string text = "";
            string[] cmt = comment.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
            for (int c = 0; c < cmt.Length; c++)
            {
                if (cmt[c] != "") text += "#";
                text += cmt[c] + endl;
            }
            return text;
        }

        #endregion

        #region Public functions

        #region Load & Save
        /// <summary>
        /// Load INI settings from file. 
        /// </summary>
        /// <param name="filename">INI File name to load. If the filename is empty, then will be load default file specified in the constructor parameter</param>
        public void LoadFile(string filename = "")
        {
            if (filename == "") filename = path;
            if (filename != "")
            {
                path = filename;
                if (File.Exists(filename))
                {
                    try
                    {
                        // ----- LOAD INI FILE -----
                        StreamReader objReader = new StreamReader(filename);
                        string text = objReader.ReadToEnd();
                        objReader.Close();
                        objReader.Dispose();

                        // ----- PARSE INI FILE -----
                        Parse(text);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }


        /// <summary>
        /// Save INI settings to file
        /// </summary>
        /// <param name="filename">INI filename</param>
        /// <param name="endl">Line separator. If the line separator is empty, then will be used default Environment.NewLine separator</param>
        public void SaveFile(string filename = "", string endl = "")
        {
            if (filename == "") filename = path;
            if (filename != "")
            {
                try
                {
                    // ----- DEFAULT LINE SEPARATOR -----
                    if (endl == "") endl = System.Environment.NewLine;

                    // ----- CREATE INI STRING -----
                    string txt = UnParse(endl);

                    // ----- SAVE TO FILE -----
                    StreamWriter objWriter = new StreamWriter(filename); // append
                    objWriter.Write(txt);
                    objWriter.Close();
                    objWriter.Dispose();
                }
                catch (Exception)
                {

                }
            }
        }

        /// <summary>
        /// Load INI settings from string
        /// </summary>
        /// <param name="text">INI string</param>
        public void Load(string text)
        {
            Parse(text);
        }

        /// <summary>
        /// Create INI settings string
        /// </summary>
        /// <param name="endl">Line separator. If the line separator is empty, then will be used default Environment.NewLine separator</param>
        /// <returns>INI string</returns>
        public string Save(string endl = "")
        {
            if (endl == "") endl = System.Environment.NewLine;
            return UnParse(endl);
        }

        #endregion

        #region Change registers

        /// <summary>
        /// Set item boolean value
        /// </summary>
        /// <param name="Section">Section name</param>
        /// <param name="Key">Key name</param>
        /// <param name="Value">Value</param>
        /// <param name="Comment">Comment</param>
        public void Write(string Section, string Key, bool Value, string Comment = "")
        {
            string val;
            if (Value) val = "1";
            else val = "0";
            Write(Section, Key, val, Comment);
        }

        /// <summary>
        /// Set item byte value
        /// </summary>
        /// <param name="Section">Section name</param>
        /// <param name="Key">Key name</param>
        /// <param name="Value">Value</param>
        /// <param name="Comment">Comment</param>
        public void Write(string Section, string Key, byte Value, string Comment = "")
        {
            Write(Section, Key, Value.ToString(), Comment);
        }

        /// <summary>
        /// Set item short value
        /// </summary>
        /// <param name="Section">Section name</param>
        /// <param name="Key">Key name</param>
        /// <param name="Value">Value</param>
        /// <param name="Comment">Comment</param>
        public void Write(string Section, string Key, short Value, string Comment = "")
        {
            Write(Section, Key, Value.ToString(), Comment);
        }

        /// <summary>
        /// Set item unsigned short value
        /// </summary>
        /// <param name="Section">Section name</param>
        /// <param name="Key">Key name</param>
        /// <param name="Value">Value</param>
        /// <param name="Comment">Comment</param>
        public void Write(string Section, string Key, ushort Value, string Comment = "")
        {
            Write(Section, Key, Value.ToString(), Comment);
        }

        /// <summary>
        /// Set item integer value
        /// </summary>
        /// <param name="Section">Section name</param>
        /// <param name="Key">Key name</param>
        /// <param name="Value">Value</param>
        /// <param name="Comment">Comment</param>
        public void Write(string Section, string Key, int Value, string Comment = "")
        {
            Write(Section, Key, Value.ToString(), Comment);
        }

        /// <summary>
        /// Set item unsigned integer value
        /// </summary>
        /// <param name="Section">Section name</param>
        /// <param name="Key">Key name</param>
        /// <param name="Value">Value</param>
        /// <param name="Comment">Comment</param>
        public void Write(string Section, string Key, uint Value, string Comment = "")
        {
            Write(Section, Key, Value.ToString(), Comment);
        }

        /// <summary>
        /// Set item object value
        /// </summary>
        /// <param name="Section">Section name</param>
        /// <param name="Key">Key name</param>
        /// <param name="Value">Value</param>
        /// <param name="Comment">Comment</param>
        public void Write(string Section, string Key, object Value, string Comment = "")
        {
            string val;
            if (Value is bool)
            {
                if ((bool)Value)
                    val = "1";
                else
                    val = "0";
            }
            else val = Value.ToString();


            Write(Section, Key, val, Comment);
        }

        /// <summary>
        /// Set item string value
        /// </summary>
        /// <param name="Section">Section name</param>
        /// <param name="Key">Key name</param>
        /// <param name="Value">Value</param>
        /// <param name="Comment">Comment</param>
        public void Write(string Section, string Key, string Value, string Comment = "")
        {
            // ----- CHECK IF SECTION EXIST -----
            int res = -1;
            for (int i = 0; i < section.Count; i++)
            {
                if (section[i].name.ToLower() == Section.ToLower())
                {
                    res = i;
                    break;
                }
            }

            // ----- CREATE NEW SECTION IF NOT EXIST -----
            if (res == -1)
            {
                section_t sec;
                sec.name = Section;
                sec.item = new List<item_t>();
                sec.comment = "";
                section.Add(sec);
                res = section.Count - 1;
            }

            // ----- CHECK IF KEY EXIST -----
            int iKey = -1;
            for (int i = 0; i < section[res].item.Count; i++)
            {
                if (section[res].item[i].key.ToLower() == Key.ToLower())
                {
                    iKey = i;
                    break;
                }
            }

            item_t item;
            item.key = Key;
            item.value = Value;
            item.comment = Comment;

            // ----- CREATE NEW KEY IF NOT EXIST -----
            if (iKey == -1)
            {
                section[res].item.Add(item);
            }
            // ----- REPLACE KEY IF EXIST -----
            else
            {
                if (Comment == "") item.comment = section[res].item[iKey].comment;
                section[res].item.RemoveAt(iKey);
                section[res].item.Insert(iKey, item);
            }
        }

        /// <summary>
        /// Write Data at index to the INI File
        /// </summary>
        /// <param name="sectionIndex">Section index</param>
        /// <param name="keyIndex">Key index</param>
        /// <param name="Value">Value Name</param>
        /// <param name="Comment">Comment</param>
        public void WriteAt(int sectionIndex, int keyIndex, string Value, string Comment = "")
        {
            item_t item;
            item.key = section[sectionIndex].item[keyIndex].key;
            item.value = Value;
            item.comment = Comment;
            if (Comment == "") item.comment = section[sectionIndex].item[keyIndex].comment;
            section[sectionIndex].item.RemoveAt(keyIndex);
            section[sectionIndex].item.Insert(keyIndex, item);
        }

        /// <summary>
        /// Get item settings
        /// </summary>
        /// <param name="Section">Section</param>
        /// <param name="Key">Key</param>
        /// <param name="DefValue">Default value</param>
        /// <returns>Key value</returns>
        public string Read(string Section, string Key, string DefValue)
        {
            for (int i = 0; i < section.Count; i++)
            {
                if (section[i].name.ToLower() == Section.ToLower())
                {
                    for (int j = 0; j < section[i].item.Count; j++)
                    {
                        if (section[i].item[j].key.ToLower() == Key.ToLower())
                        {
                            return section[i].item[j].value;
                        }
                    }
                }
            }
            return DefValue;
        }

        /// <summary>
        /// Get item settings
        /// </summary>
        /// <param name="Section">Section</param>
        /// <param name="Key">Key</param>
        /// <param name="DefValue">Default value</param>
        /// <returns>Key value</returns>
        public int ReadInt(string Section, string Key, int DefValue)
        {
            try
            {
                for (int i = 0; i < section.Count; i++)
                {
                    if (section[i].name.ToLower() == Section.ToLower())
                    {
                        for (int j = 0; j < section[i].item.Count; j++)
                        {
                            if (section[i].item[j].key.ToLower() == Key.ToLower())
                            {
                                return Convert.ToInt32(section[i].item[j].value);
                            }
                        }
                    }
                }
                return DefValue;
            }
            catch { return DefValue; }
        }

        /// <summary>
        /// Get item settings
        /// </summary>
        /// <param name="Section">Section</param>
        /// <param name="Key">Key</param>
        /// <param name="DefValue">Default value</param>
        /// <returns>Key value</returns>
        public uint ReadUInt(string Section, string Key, uint DefValue)
        {
            try
            {
                for (int i = 0; i < section.Count; i++)
                {
                    if (section[i].name.ToLower() == Section.ToLower())
                    {
                        for (int j = 0; j < section[i].item.Count; j++)
                        {
                            if (section[i].item[j].key.ToLower() == Key.ToLower())
                            {
                                return Convert.ToUInt32(section[i].item[j].value);
                            }
                        }
                    }
                }
                return DefValue;
            }
            catch { return DefValue; }
        }

        /// <summary>
        /// Get item settings
        /// </summary>
        /// <param name="Section">Section</param>
        /// <param name="Key">Key</param>
        /// <param name="DefValue">Default value</param>
        /// <returns>Key value</returns>
        public bool ReadBool(string Section, string Key, bool DefValue)
        {
            try
            {
                for (int i = 0; i < section.Count; i++)
                {
                    if (section[i].name.ToLower() == Section.ToLower())
                    {
                        for (int j = 0; j < section[i].item.Count; j++)
                        {
                            if (section[i].item[j].key.ToLower() == Key.ToLower())
                            {
                                if (section[i].item[j].value.ToLower() == "true" || section[i].item[j].value.ToLower() == "1")
                                    return true;
                                else if (section[i].item[j].value.ToLower() == "false" || section[i].item[j].value.ToLower() == "0")
                                    return false;
                                else return DefValue;
                            }
                        }
                    }
                }
                return DefValue;
            }
            catch { return DefValue; }
        }
        #endregion

        #region Get sections

        /// <summary>
        /// Get section list
        /// </summary>
        /// <returns>Section list</returns>
        public List<section_t> GetSections()
        {
            return section;
        }

        /// <summary>
        /// Get key list in section
        /// </summary>
        /// <param name="Section">Section name</param>
        /// <returns>Key list</returns>
        public List<item_t> GetKeys(string Section)
        {
            // ----- CHECK IF SECTION EXIST -----
            for (int i = 0; i < section.Count; i++)
            {
                if (section[i].name.ToLower() == Section.ToLower())
                {
                    return section[i].item;
                }
            }
            List<item_t> item = new List<item_t>();
            return item;
        }

        #endregion

        #endregion

    }
}
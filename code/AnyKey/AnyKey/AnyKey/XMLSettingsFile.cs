using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Xml;

namespace AnyKey
{
    public class XMLSettingsFile
    {
        private XmlDocument xml_doc = new XmlDocument();
        private string temp_Filename = "";
        char[] charSeparators = new char[] { '|' };    //The character that separates all items in the string

        public XMLSettingsFile(string FileName)
        {
            temp_Filename = FileName;
            try
            {
                xml_doc.Load("Content\\" + temp_Filename);
            }
            catch
            {
                throw new Exception("Could not load XML file properly");
            }
        }
            
        public void Save()
        {
            xml_doc.Save(temp_Filename);
        }

        public void ClearSettings()
        {
            //remove child nodes of settings
            //todo:  This must be designed to clear the entire xml file
            //xml_doc.RemoveAll();
        }

        public string[] GetList(string section, string entry)
        {
            XmlNode Info = xml_doc.DocumentElement.SelectSingleNode(section + "/" + entry);
            return Info.InnerText.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
        }

        public string GetValue(string section, string entry)
        {
            XmlNode Info = xml_doc.DocumentElement.SelectSingleNode(section + "/" + entry);
            return Info.InnerText;
        }

        public void SetValue(string section, string entry, string[] information)
        {
            string string_of_info = "";
            foreach (string s_info in information)
            {
                if (s_info.Length > 0)
                {
                    string_of_info += s_info + "|";
                }
            }
            SetValue(section,entry,string_of_info);
        }

        public void SetValue(string section, string entry, string information)
        {
            try
            {
                //get section node, create if doesn't exist
                XmlNode SectionNode = xml_doc.DocumentElement.SelectSingleNode(section);

                if (SectionNode == null) //no node was found
                {
                    //create node
                    XmlElement newElement = xml_doc.DocumentElement.OwnerDocument.CreateElement(section);
                    SectionNode = xml_doc.DocumentElement.AppendChild(newElement);
                }


                //get the entry node, create if doesn't exist
                XmlNode entryNode = SectionNode.SelectSingleNode(entry);

                if (entryNode == null) //no node was found
                {
                    XmlElement newElement = SectionNode.OwnerDocument.CreateElement(entry);
                    entryNode = SectionNode.AppendChild(newElement);
                }


                //Assign the value to the entry node
                entryNode.InnerText = information;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
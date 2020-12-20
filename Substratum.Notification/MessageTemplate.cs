using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Collections.Specialized;
using System.IO;
using System.Collections;

namespace Substratum.Notification
{
    public class MessageTemplate
    {
        static XmlDocument BaseDocument;

        static MessageTemplate()
        {
            BaseDocument = new XmlDocument();
            BaseDocument.AppendChild(BaseDocument.CreateElement("Root"));
        }

        XmlDocument TemplateStyleSheet;
        XslCompiledTransform CompiledTransform;

        public MessageTemplate(string filename)
        {
            TemplateStyleSheet = new XmlDocument();
            TemplateStyleSheet.Load(filename);
            LoadTransform();
        }

        public MessageTemplate(XmlDocument styleSheet)
        {
            TemplateStyleSheet = styleSheet;
            LoadTransform();
        }

        public MessageTemplate()
        {
        }

        public void LoadTransform(string stylesheetData)
        {
            TemplateStyleSheet = new XmlDocument();
            TemplateStyleSheet.LoadXml(stylesheetData);
            LoadTransform();
        }

        private void LoadTransform()
        {
            //Create a new XslTransform object and load the template stylesheet
            CompiledTransform = new XslCompiledTransform();
            XmlNodeReader TemplateStyleSheetReader = new XmlNodeReader(TemplateStyleSheet.DocumentElement);
            CompiledTransform.Load(TemplateStyleSheetReader, null, null);
        }


        private XsltArgumentList GetParameters(Hashtable Data)
        {
            XsltArgumentList argList = new XsltArgumentList();
            XmlNamespaceManager nsm = new XmlNamespaceManager(TemplateStyleSheet.NameTable);
            nsm.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");

            foreach (XmlNode n in TemplateStyleSheet.SelectNodes("//xsl:param",nsm))
            {
                string name = n.Attributes["name"].Value;
                object value;


                // TODO: support reading configuration

                //if (name.StartsWith("Configuration."))
                //{
                //    string SettingName = name[14..];
                //    value = System.Configuration.ConfigurationManager.AppSettings[SettingName];
                //    if (value == null)
                //        throw new InvalidOperationException("Missing appSetting '" + SettingName + "' for template parameter");
                //}
                //else
                {
                    if (!Data.ContainsKey(name))
                        throw new InvalidOperationException("Missing data value for template parameter: " + name);
                    value = Data[name];
                }
                if (value!=null)
                    argList.AddParam(name, string.Empty, value);
            }
            return argList;
        }

        public string Transform(Hashtable Data)
        {
            // Fill the argument list with the parameters
            XsltArgumentList argList = GetParameters(Data);

            //Create a MemoryStream to receive tranformed data
            MemoryStream ms = new MemoryStream();
            XmlTextWriter xmlmswriter = new XmlTextWriter(ms, System.Text.Encoding.Default);

            //Transform the data 
            XmlNodeReader DocReader = new XmlNodeReader(BaseDocument.DocumentElement);
            CompiledTransform.Transform(DocReader, argList, xmlmswriter, null);

            //Copy results from memory stream into "Body" of message 
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader msreader = new StreamReader(ms);
            string text = msreader.ReadToEnd().Trim();

            return text;
        }
    }

}

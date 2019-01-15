using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace FPSService.TowTruck
{
    public class TowTruckXMLWriter
    {
        private MemoryStream stream;
        private XmlWriter writer;
        private ASCIIEncoding encoder;

        public TowTruckXMLWriter()
        {
            stream = new MemoryStream();
            encoder = new ASCIIEncoding();
            XmlWriterSettings xmlws = new XmlWriterSettings();
            xmlws.Indent = false;
            xmlws.IndentChars = String.Empty;
            xmlws.NewLineChars = String.Empty;
            xmlws.NewLineHandling = NewLineHandling.None;
            xmlws.NewLineOnAttributes = false;
            writer = XmlWriter.Create(stream, xmlws);
        }

        public void WriteStartElement(string Element)
        {
            writer.WriteStartElement(Element);
        }
        public void WriteElementString(string name, string value)
        {
            writer.WriteElementString(name, value);
        }

        public void WriteElementString()
        {
            writer.WriteEndElement();
        }

        public void WriteEndElement()
        {
            writer.WriteEndElement();
        }

        public string XmlString
        {
            get
            {
                writer.Flush();
                stream.Flush();
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                string s = sr.ReadToEnd();
                //Console.Write(XElement.Parse(s).ToString().Trim());
                return XElement.Parse(s).ToString().Trim();
            }
        }
    }
}
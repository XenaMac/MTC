using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
//using ICSharpCode.SharpZipLib.Core;
//using ICSharpCode.SharpZipLib.Zip;
using System.Text;

namespace FPSService
{
    public partial class MTCWazeXML : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            Response.ContentType = "application/zip";
            Response.AppendHeader("content-disposition", "attachment; filename=\"traffic-incidents.zip\"");
            Response.CacheControl = "Private";
            Response.Cache.SetExpires(DateTime.Now.AddMinutes(1));

            byte[] buffer = new byte[4096];
            string xmlString = ConvertObjectToXMLString(DataClasses.GlobalData.wazeFeed);
            MemoryStream stream = makeStream(xmlString);
            ZipOutputStream zipStream = new ZipOutputStream(Response.OutputStream);
            zipStream.SetLevel(3);
            ZipEntry entry = new ZipEntry("traffic-incidents.xml");
            entry.Size = stream.Length;
            entry.DateTime = DateTime.Now;
            zipStream.PutNextEntry(entry);

            StreamUtils.Copy(stream, zipStream, new byte[4096]);

            int count = stream.Read(buffer, 0, buffer.Length);
            while (count > 0) {
                zipStream.Write(buffer, 0, count);
                count = stream.Read(buffer, 0, buffer.Length);
                if (!Response.IsClientConnected) {
                    break;
                }
                Response.Flush();
            }
            stream.Close();

            zipStream.Close();

            Response.Flush();
            Response.End();
             * */

            ProcessRequest(HttpContext.Current);
        }

        private void ProcessRequest(HttpContext context) {
            string xmlString = ConvertObjectToXMLString(DataClasses.GlobalData.wazeFeed);
            XDocument doc = XDocument.Parse(xmlString);
            context.Response.ContentType = "text/xml";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.AppendHeader("content-disposition", "attachment; filename=\"traffic-incidents.xml\"");
            context.Response.Expires = -1;
            context.Response.Cache.SetAllowResponseInBrowserHistory(true);
            doc.Save(context.Response.Output);
        }

        private MemoryStream makeStream(string data) {
            byte[] byteArray = Encoding.ASCII.GetBytes(data);
            MemoryStream stream = new MemoryStream(byteArray);
            return stream;
        }

        private string ConvertObjectToXMLString(List<WAZE.WAZEClass> feed) {
            string xmlString = string.Empty;
            /*
            XmlSerializer ser = new XmlSerializer(waze.GetType());
            using (MemoryStream ms = new MemoryStream()) {
                ser.Serialize(ms, waze);
                ms.Position = 0;
                xmlString = new StreamReader(ms).ReadToEnd();
            }
             * */
            TimeZone zone = TimeZone.CurrentTimeZone;
            TimeSpan offset = zone.GetUtcOffset(DateTime.Now);
            int osAmount = offset.Hours;
            bool isNeg = false;
            if (osAmount < 0) {
                isNeg = true;
            }
            string osString = Math.Abs(osAmount).ToString();
            while (osString.Length < 2) {
                osString = "0" + osString;
            }
            if (isNeg)
            {
                osString = "-" + osString;
            }
            else {
                osString = "+" + osString;
            }
            //DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + osString
            osString = osString + ".00";
            xmlString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            xmlString += "<incidents timestamp=\"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz") + "\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://gstatic.com/road-incidents/incidents_feed.xsd\">";
            foreach (WAZE.WAZEClass w in feed) {
                xmlString += "<incident id=\"" + w.incidentID.ToString() + "\">";
                xmlString += "<creationtime>" + w.creationtime.ToString("yyyy-MM-ddTHH:mm:sszzz") + "</creationtime>";
                xmlString += "<updatetime>" + w.updatetime.ToString("yyyy-MM-ddTHH:mm:sszzz") + "</updatetime>";
                xmlString += "<type>" + w.type + "</type>";
                xmlString += "<subtype>" + w.subType + "</subtype>";
                if (!string.IsNullOrEmpty(w.description))
                {
                    xmlString += "<description>" + w.description + "</description>";
                }
                else {
                    xmlString += "<description>No Data</description>";
                }
                xmlString += "<location>";
                if (!string.IsNullOrEmpty(w.location.street)) {
                    xmlString += "<street>" + w.location.street + "</street>";
                }
                xmlString += "<polyline>" + w.location.polyline + "</polyline>";
                xmlString += "</location>";
                xmlString += "<starttime>" + w.starttime.ToString("yyyy-MM-ddTHH:mm:sszzz") + "</starttime>";
                xmlString += "<endtime>" + w.endtime.ToString("yyyy-MM-ddTHH:mm:sszzz") + "</endtime>";
                xmlString += "</incident>";
            }
            xmlString += "</incidents>";
            return xmlString;
        }
    }
}
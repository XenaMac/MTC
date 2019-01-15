using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace FPSService.WAZE
{
    public class PostWAZE
    {
        public void postWazeData(WAZEClass waze, string callSign, string DriverName) {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.Append("<incidents xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://www.gstatic.com/road-incidents/incidents_feed.xsd\">");
            sb.Append("<incident id=\"" + waze.incidentID + "\">");
            sb.Append("<creationtime>" + makeDate(waze.creationtime) + "</creationtime>");
            sb.Append("<updatetime>" + makeDate(waze.updatetime) + "</updatetime>");
            sb.Append("<type>" + waze.type + "</type>");
            sb.Append("<description>" + waze.description + "</description>");
            sb.Append("<location>");
            sb.Append("<street>" + waze.location.street + "</street>");
            sb.Append("<polyline>" + waze.location.polyline + "</polyline>");
            sb.Append("</location>");
            sb.Append("<starttime>" + makeDate(waze.creationtime) + "</starttime>");
            sb.Append("<endtime>" + makeDate(waze.updatetime) + "</endtime>");
            sb.Append("</incident>");
            sb.Append("</incidents>");
            string output = sb.ToString();
            SQL.SQLCode sql = new SQL.SQLCode();
            sql.logOutgoingWAZE(output, callSign, DriverName);
            if (DataClasses.GlobalData.wazeFeed.Count > 0)
            {
                for (int i = DataClasses.GlobalData.wazeFeed.Count - 1; i >= 0; i--)
                {
                    if (DataClasses.GlobalData.wazeFeed[i].incidentID == waze.incidentID)
                    {
                        DataClasses.GlobalData.wazeFeed.RemoveAt(i);
                    }
                }
            }
            DataClasses.GlobalData.wazeFeed.Add(waze);
        }

        public WAZEClass makeWaze(string incidentID, DateTime createTime, DateTime updateTime, string type, string subType, string description, string street, string polyline,
            DateTime startTime, DateTime endTime) {
                WAZEClass w = new WAZEClass();
                w.incidentID = incidentID;
                w.creationtime = createTime;
                w.updatetime = updateTime;
                w.description = description;
                w.type = (wType)Enum.Parse(typeof(wType), type);
                if (!string.IsNullOrEmpty(subType)) {
                    w.subType = (wSubType)Enum.Parse(typeof(wSubType), subType);
                }
                WAZE.Location l = new Location();
                l.street = street;
                l.polyline = polyline;
                w.location = l;
                w.starttime = startTime;
                w.endtime = endTime;
                return w;
        }

        private string makeDate(DateTime dt) {
            TimeZone zone = TimeZone.CurrentTimeZone;
            TimeSpan offset = zone.GetUtcOffset(dt);
            string osAmount = offset.Hours.ToString() + ".00";
            string dtOut = dt.ToString("yyyy-mm-ddThh:mm:ss-" + osAmount);
            return dtOut;
        }
    }
}
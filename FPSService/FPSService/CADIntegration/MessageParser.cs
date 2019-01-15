using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.CADIntegration
{
    public static class MessageParser
    {

        public static Guid cadMsgID;

        #region  " Message Process: Return a value "

        /// <summary>
        /// This mechanism returns a string value AND processes the message,
        /// Not recommended for production service use
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string parseIncoming(string msg)
        {
            try
            {
                string headerData = string.Empty;
                string bodyData = string.Empty;
                string[] splitter = msg.Split('\\');
                //splitter[0] = header data
                int result;
                if (int.TryParse(splitter[0].Substring(0, 1), out result))
                {
                    headerData = makeHeaderData(splitter[0]);
                    bodyData = makeBodyData(splitter[1]);
                }
                else
                {
                    bodyData = makeBodyData(splitter[0]);
                }
                return headerData + bodyData;
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
                return string.Empty;
            }
        }

        private static string makeHeaderData(string data)
        {
            try
            {
                string headerData = string.Empty;
                string[] splitHeader = data.Split('.');
                headerData = "MsgID: " + splitHeader[0] + Environment.NewLine;
                headerData += "Date: " + splitHeader[1].Substring(0, 2) + "/" + splitHeader[1].Substring(2, 2) + "/" +
                    splitHeader[1].Substring(4, 2) + Environment.NewLine;
                headerData += "Time: " + splitHeader[2].Substring(0, 2) + ":" + splitHeader[2].Substring(2, 2) + ":" +
                    splitHeader[2].Substring(4, 2) + Environment.NewLine;
                headerData += "Server: " + splitHeader[3] + Environment.NewLine;
                return headerData;
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
                return string.Empty;
            }
        }

        private static string makeBodyData(string data)
        {
            string bodyData = string.Empty;
            string[] splitBody = data.Split('.');
            bodyData += "Command: " + splitBody[0] + Environment.NewLine;
            if (splitBody[0] == "US")
            {
                bodyData += "Unit Number: " + splitBody[1] + Environment.NewLine;
            }
            if (splitBody[0] == "IS")
            {
                bodyData += "Incident Status: " + splitBody[1] + Environment.NewLine;
            }
            if (splitBody[0] == "RM")
            {
                //message rejected, don't parse the rest
                bodyData = "Inbound data was rejected by CAD";
                return bodyData;
            }
            if (splitBody[0] == "SM") //I don't think this is what the CAD is sending, it should be TO (see further down)
            {
                //message to a truck
                bodyData = "SEND MESSAGE:" + Environment.NewLine;
                bodyData += "Truck Number: " + splitBody[1].ToString() + Environment.NewLine;
                string[] splitter = splitBody[2].ToString().Split('/');
                bodyData += "Message: " + splitter[1].ToString();
                string _truckNumber = splitBody[1].ToString();
                //return bodyData;
                TowTruck.TowTruck t = DataClasses.GlobalData.currentTrucks.Find(delegate(TowTruck.TowTruck find) { return find.TruckNumber == _truckNumber; });
                if (t != null)
                {
                    TruckMessage tm = new TruckMessage();
                    tm.UserEmail = "CAD SYSTEM";
                    tm.SentTime = DateTime.Now;
                    tm.MessageID = Guid.NewGuid();
                    tm.TruckIP = t.Identifier;
                    tm.TruckNumber = _truckNumber;
                    tm.Beat = t.beatNumber;
                    tm.CallSign = t.Driver.callSign;
                    tm.Driver = t.Driver.LastName + ", " + t.Driver.FirstName;
                    tm.MessageText = splitter[1].ToString();
                    tm.MessageType = 0;
                    DataClasses.GlobalData.AddTruckMessage(tm);
                    return "SENT";
                }
            }

            for (int i = 2; i < splitBody.Length; i++)
            {

                if (splitBody[i].Substring(0, 2) == "S/")
                {
                    //status component
                    string[] splitter = splitBody[i].Split('/');
                    bodyData += "Status: " + splitter[1] + Environment.NewLine;
                }
                if (splitBody[i].Substring(0, 2) == "DT")
                {
                    //date component
                    string[] splitter = splitBody[i].Split('/');
                    bodyData += "Date: " + splitter[1].Substring(0, 2) + "/" + splitter[1].Substring(2, 2) + "/" +
                        splitter[1].Substring(4, 2) + Environment.NewLine;
                }
                if (splitBody[i].Substring(0, 2) == "TM")
                {
                    //time component
                    string[] splitter = splitBody[i].Split('/');
                    bodyData += "Time: " + splitter[1].Substring(0, 2) + ":" + splitter[1].Substring(2, 2) + ":" +
                        splitter[1].Substring(4, 2) + Environment.NewLine;
                }
                if (splitBody[i].Substring(0, 2) == "II") //gives us incident number
                {
                    string[] splitter = splitBody[i].Split('/');
                    bodyData += "Incident ID: " + splitter[1].ToString();
                }
            }
            return bodyData;
        }

        #endregion

        #region " Message Process: DO NOT return a value "

        /// <summary>
        /// This processes the message but does NOT return a string value
        /// This is what should be used.
        /// </summary>
        /// <param name="msg"></param>
        public static string processMessage(string msg, Guid? _cadMsgID)
        {
            if (_cadMsgID != null && _cadMsgID != Guid.Empty) {
                cadMsgID = (Guid)_cadMsgID;
            }
            string msgOUT = "AM";
            try
            {
                bool headerData = false;
                string[] splitter = msg.Split('\\');
                int result;
                if (int.TryParse(splitter[0].Substring(0, 1), out result))
                {
                    headerData = checkHeaderData(splitter[0]); //verify we're getting data from the right server
                    if (headerData == true)
                    {
                        processCADMessage(splitter[1]);
                    }
                    //make accept message

                }
                else //this should never happen in production because all CAD messages have a header, it's just a sanity checker
                {
                    processCADMessage(splitter[0]);
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("ERROR PROCESSING MESSAGE: " + ex.Message);
                string err = ex.ToString();
                msgOUT = "RM";
            }
            return msgOUT;
        }

        private static bool checkHeaderData(string data)
        {
            try
            {
                string headerData = string.Empty;
                string[] splitHeader = data.Split('.');
                headerData = "MsgID: " + splitHeader[0] + Environment.NewLine;
                headerData += "Date: " + splitHeader[1].Substring(0, 2) + "/" + splitHeader[1].Substring(2, 2) + "/" +
                    splitHeader[1].Substring(4, 2) + Environment.NewLine;
                headerData += "Time: " + splitHeader[2].Substring(0, 2) + ":" + splitHeader[2].Substring(2, 2) + ":" +
                    splitHeader[2].Substring(4, 2) + Environment.NewLine;
                headerData += "Server: " + splitHeader[3] + Environment.NewLine;
                string msgDate = splitHeader[1].Substring(0, 2) + "/" + splitHeader[1].Substring(2, 2) + "/" +
                    splitHeader[1].Substring(4, 2) + " " + splitHeader[2].Substring(0, 2) + ":" + splitHeader[2].Substring(2, 2) + ":" +
                    splitHeader[2].Substring(4, 2);
                DateTime dtMessage = Convert.ToDateTime(msgDate);
                TimeSpan ts = DateTime.Now - dtMessage;
                if (ts.TotalMinutes > 5)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
                return false;
            }
        }

        private static void processCADMessage(string data)
        {
            try
            {
                string bodyData = string.Empty;
                string shift = string.Empty;
                string[] splitBody = data.Split('.');
                if (splitBody[0] == "SM") //I don't think this is what the CAD actually sends for a message to a truck, it should be TO (see further down)
                {
                    sendMessageToTruck(data);
                }
                if (splitBody[0] == "US")
                {
                    //check to see if there's any incident information in the packet
                    string[] splitData = data.Split('.');

                    //int incIndex = Array.IndexOf(splitData, "II");
                    string incNumber = string.Empty;
                    if (data.Contains("II/"))
                    {
                        incNumber = Array.Find(splitData, i => i.StartsWith("II/")).ToString().Replace("II/", "");
                    }
                    if (!string.IsNullOrEmpty(incNumber)) 
                    {
                        string _callSign = splitData[1];

                        if (_callSign.Substring(0, 1) == "A" || _callSign.Substring(0, 1) == "*" || _callSign.Substring(0, 1) == "B")
                        {
                            switch (_callSign.Substring(0, 1))
                            {
                                case "A":
                                    shift = "AM";
                                    break;
                                case "B":
                                    shift = "PM";
                                    break;
                                case "*":
                                    shift = "MID";
                                    break;
                            }
                            _callSign = _callSign.Substring(1, _callSign.Length - 1);

                        }
                        /*
                        if (_callSign.Substring(_callSign.Length - 1, 1) == "A" || _callSign.Substring(_callSign.Length - 1, 1) == "B")
                        {
                            _callSign = _callSign.Substring(0, _callSign.Length - 1);
                        }
                        */
                        TowTruck.TowTruck truck;
                        if (!string.IsNullOrEmpty(shift))
                        {
                            truck = DataClasses.GlobalData.currentTrucks.Find(delegate(TowTruck.TowTruck find)
                            {
                                return find.Driver.callSign == _callSign && find.Driver.AssignedShift.ToUpper().Contains(shift);
                            });
                        }
                        else
                        {
                            truck = DataClasses.GlobalData.currentTrucks.Find(delegate(TowTruck.TowTruck find)
                            {
                                return find.Driver.callSign == _callSign;
                            });
                        }
                        if (truck != null)
                        {
                            //if a truck is currently on an incident or en route to an incident, reject the CAD message
                            if (truck.Status.VehicleStatus.ToUpper() == "ON INCIDENT" || truck.Status.VehicleStatus.ToUpper() == "EN ROUTE" ||
                                truck.Status.VehicleStatus.ToUpper() == "ENROUTE")
                            {
                                //TODO add reject message code
                            }
                            //Got a truck, check Incident Status
                            //incident is created when the truck enters the scene, check to see if we've got a current incidentid
                            MTCIncident i = null; //old
                            Incident iNew = null;
                            if (truck.currentIncidentID != new Guid("00000000-0000-0000-0000-000000000000"))
                            {
                                i = DataClasses.GlobalData.Incidents.Find(delegate(MTCIncident iFind) { return iFind.IncidentID == truck.currentIncidentID; });
                            }
                            if (truck.currentIncident != null) { 
                                //nothing we can do, the truck is currently on incident
                                iNew = truck.currentIncident;
                                if (!string.IsNullOrEmpty(incNumber) && string.IsNullOrEmpty(truck.currentIncident.CHPLogNumber)) {
                                    truck.currentIncident.CHPLogNumber = incNumber;
                                }
                            }

                            //new incident and assist code
                            if (truck.currentIncident == null && data.Contains("ENRT")) {
                                //This is a CAD Dispatched message. 1097 means a self-dispatch message. Self
                                //dispatches are handled elsewhere but the CAD still sends a CHP Incident Log Number that we have to process
                                iNew = new Incident();
                                // this is new stuff
                                iNew.incidentID = Guid.NewGuid();
                                iNew.fromTruck = false;
                                iNew.incidentDatePosted = DateTime.Now;
                                iNew.CHPLogNumber = incNumber;
                                iNew.logID = cadMsgID;
                                iNew.userPosted = "CHP CAD";

                                truck.addIncident(iNew);
                            }
                            else if (truck.currentIncident == null && !data.Contains("ENRT")) {
                                //this is usually a response message from CAD
                                /*
                                i = new MTCIncident();
                                List<MTCAssist> assistList = new List<MTCAssist>();
                                MTCPreAssistData pad = new MTCPreAssistData();
                                i.assistList = assistList;
                                i.preAssist = pad;
                                i.preAssist.CHPLogNumber = incNumber;
                                i.Acked = true;
                                i.sentToTruck = true;
                                iNew = new Incident();
                                iNew.incidentID = cadMsgID;

                                SQL.SQLCode mySQL = new SQL.SQLCode();
                                i.IncidentID = mySQL.PostMTCIncident("CHP CAD", i.TruckNumber, "0", 0, 0.0, 0.0);
                                DataClasses.GlobalData.addIncident(i);
                                truck.currentIncidentID = i.IncidentID;
                                truck.incidentID = incNumber;
                                 */
                                iNew = new Incident();
                                iNew.incidentID = Guid.NewGuid();
                                iNew.fromTruck = false;
                                iNew.incidentDatePosted = DateTime.Now;
                                iNew.CHPLogNumber = incNumber;
                                iNew.logID = cadMsgID;
                                iNew.userPosted = "CHP CAD";
                                iNew.logID = cadMsgID;
                                truck.addIncident(iNew);
                            }
                            
                            if (i == null && data.Contains("ENRT"))//enrt means it's a CAD dispatched message, 1097 means a self-dispatch message.  Self
                            //dispatches are handled elsewhere but the CAD still sends a CHP Incident Log Number that we have to process
                            {
                                //brand new incident
                                Guid g = Guid.NewGuid();
                                i = new MTCIncident(); //old
                                
                                /* this is old stuff */
                                List<MTCAssist> assistList = new List<MTCAssist>();
                                MTCPreAssistData pad = new MTCPreAssistData();
                                i.IncidentID = g;
                                i.assistList = assistList;
                                i.DatePosted = DateTime.Now;
                                i.fromTruck = 0;
                                i.preAssist = pad;
                                i.preAssist.CHPLogNumber = incNumber;
                                i.UserPosted = "CHP CAD";
                                i.Acked = true;
                                i.sentToTruck = true;
                                i.IPAddr = truck.Identifier;
                                i.TruckNumber = truck.TruckNumber;
                                i.incidentComplete = false;
                                SQL.SQLCode mySQL = new SQL.SQLCode();
                                i.IncidentID = mySQL.PostMTCIncident("CHP CAD", i.TruckNumber, "0", 0, 0.0, 0.0);
                                DataClasses.GlobalData.addIncident(i);
                                truck.incidentID = incNumber;
                                truck.currentIncidentID = i.IncidentID;

                            }
                            else if (i == null && !data.Contains("ENRT")) //this is usually a response message
                            {
                                i = new MTCIncident();
                                List<MTCAssist> assistList = new List<MTCAssist>();
                                MTCPreAssistData pad = new MTCPreAssistData();
                                i.assistList = assistList;
                                i.preAssist = pad;
                                i.preAssist.CHPLogNumber = incNumber;
                                i.Acked = true;
                                i.sentToTruck = true;
                                iNew = new Incident();
                                iNew.incidentID = cadMsgID;

                                SQL.SQLCode mySQL = new SQL.SQLCode();
                                i.IncidentID = mySQL.PostMTCIncident("CHP CAD", i.TruckNumber, "0", 0, 0.0, 0.0);
                                DataClasses.GlobalData.addIncident(i);
                                truck.currentIncidentID = i.IncidentID;
                                truck.incidentID = incNumber;
                            }
                            else
                            {
                                if (i.preAssist != null)
                                {
                                    i.preAssist.CHPLogNumber = incNumber;
                                }
                            }
                        }
                    }
                    setTruckStatus(data); //if .II/x we need to create an incident and hold on to the Incident ID number from the ./II data
                }
                if (splitBody[0] == "IS") //this should just set up the incident, not create it.
                {
                    
                    //should never have an IS message without an II first setting up the incident.  If an IS message comes through that relates to
                    //an incident that we don't have, bag it.
                    TowTruck.TowTruck t = DataClasses.GlobalData.currentTrucks.Find(delegate(TowTruck.TowTruck find) {
                        return find.currentIncident != null && find.currentIncident.CHPLogNumber == splitBody[1];
                    });
                    if (t != null) {
                        //this is the new way
                        setIncident(data);
                    }
                    MTCIncident check = DataClasses.GlobalData.Incidents.Find(delegate(MTCIncident finder) { return finder.preAssist.CHPLogNumber == splitBody[1]; });
                    if (check != null)
                    {
                        setIncident(data);
                    }
                    else
                    {
                        string[] splitter = data.Split('.');
                        string _beatData = Array.Find(splitter, b => b.StartsWith("B/")).ToString().Replace("B/", "");
                        string _callSign = string.Empty;
                        if (_beatData.Substring(0, 1) == "A" || _beatData.Substring(0, 1) == "*" || _beatData.Substring(0, 1) == "B")
                        {
                            switch (_beatData.Substring(0, 1))
                            {
                                case "A":
                                    shift = "AM";
                                    break;
                                case "*":
                                    shift = "MID";
                                    break;
                                case "B":
                                    shift = "PM";
                                    break;
                            }
                            _callSign = _beatData.Substring(1, _beatData.Length - 1);
                        }
                        else
                        {
                            _callSign = _beatData;
                        }

                        TowTruck.TowTruck truck;
                        if (!string.IsNullOrEmpty(shift))
                        {
                            truck = DataClasses.GlobalData.currentTrucks.Find(delegate(TowTruck.TowTruck find)
                            {
                                return find.Driver.callSign == _callSign && find.Driver.AssignedShift.ToUpper().Contains(shift);
                            });
                        }
                        else
                        {
                            truck = DataClasses.GlobalData.currentTrucks.Find(delegate(TowTruck.TowTruck find)
                            {
                                return find.Driver.callSign == _callSign;
                            });
                        }

                        if (truck != null)
                        {
                            MTCIncident mi = DataClasses.GlobalData.Incidents.Find(delegate(MTCIncident finder) { return 
                                finder.TruckNumber == truck.TruckNumber &&
                                finder.incidentComplete == false;
                            });
                            if (mi != null)
                            {
                                int incIndex = Array.IndexOf(splitter, "IS");
                                string IncidentID = splitter[incIndex + 1];
                                truck.incidentID = IncidentID;
                                truck.currentIncident.CHPLogNumber = IncidentID;
                                //test for CAD Assigned incident id
                                string cadMSG = "US." + truck.shiftType + truck.Driver.callSign + ".S/1097.II/" + truck.currentIncident.CHPLogNumber + makeCADDate(DateTime.Now) + makeCADTime(DateTime.Now);
                                //this was removed, it doesn't appear we need it. In fact, it might be breaking things.
                                Global.cSender.sendMessage(cadMSG);
                                if (mi.preAssist != null)
                                {
                                    mi.preAssist.CHPLogNumber = IncidentID;
                                }
                                else
                                {
                                    MTCPreAssistData pad = new MTCPreAssistData();
                                    pad.CHPLogNumber = IncidentID;
                                    mi.preAssist = pad;
                                }
                            }
                        }
                    }
                }
                if (splitBody[0] == "AM")
                {
                    //message accept from CAD
                    //Don't worry about it
                }
                if (splitBody[0] == "TO")
                {
                    //this is a message from CAD to the tablet
                    sendAMessage(data);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR PROCESSING INCOMING CAD MESSAGE: " + ex.Message + Environment.NewLine + data);
            }
        }

        #endregion

        #region "Datetime helpers"

        private static string makeCADDate(DateTime dt)
        {
            string cadDate = ".DT/";

            string month = dt.Month.ToString();
            string year = dt.Year.ToString();
            string day = dt.Day.ToString();
            while (day.Length < 2)
            {
                day = "0" + day;
            }
            while (month.Length < 2)
            {
                month = "0" + month;
            }

            year = year.Substring(2, 2);

            cadDate += month + day + year;

            return cadDate;
        }

        private static string makeCADTime(DateTime dt)
        {
            string cadTime = ".TM/";

            string hour = dt.Hour.ToString();
            string minute = dt.Minute.ToString();
            string second = dt.Second.ToString();

            while (hour.Length < 2)
            {
                hour = "0" + hour;
            }
            while (minute.Length < 2)
            {
                minute = "0" + minute;
            }
            while (second.Length < 2)
            {
                second = "0" + second;
            }

            cadTime += hour + minute + second;

            return cadTime;
        }

        #endregion

        #region " Integrate with FSP Service, send messages, set status, so on "

        private static void sendAMessage(string data)
        {
            //split the header from the message
            string shift = string.Empty;
            string[] splitter = data.Split('/');
            string header = splitter[0];
            string body = splitter[1];
            string[] splitHead = header.Split('.');
            string callSign = splitHead[1].ToString();
            if (callSign.Substring(0, 1) == "A" || callSign.Substring(0, 1) == "B" || callSign.Substring(0, 1) == "*")
            {
                switch (callSign.Substring(0, 1))
                {
                    case "A":
                        shift = "AM";
                        break;
                    case "*":
                        shift = "MID";
                        break;
                    case "B":
                        shift = "PM";
                        break;
                }
                callSign = callSign.Substring(1, callSign.Length - 1);
            }
            /*    
            string msg = splitter[splitter.Length - 1].Replace(".M/", "");
                string[] splitMsg = msg.Split(':');
                string subject = splitMsg[1].Replace("Message:", "");
                string message = splitMsg[2];
                 * */

            TowTruck.TowTruck t;
            if (!string.IsNullOrEmpty(shift))
            {
                t = DataClasses.GlobalData.currentTrucks.Find(delegate(TowTruck.TowTruck find)
                {
                    return find.Driver.callSign == callSign && find.Driver.AssignedShift.ToUpper().Contains(shift);
                });
            }
            else
            {
                t = DataClasses.GlobalData.currentTrucks.Find(delegate(TowTruck.TowTruck find)
                {
                    return find.Driver.callSign == callSign;
                });
            }

            if (t != null)
            {
                //found a truck, send it the message.
                TruckMessage tm = new TruckMessage();
                tm.Acked = false;
                tm.MessageID = Guid.NewGuid();
                tm.MessageText = body;
                tm.MessageType = 0;
                tm.TruckIP = t.Identifier;
                tm.Driver = t.Driver.LastName + ", " + t.Driver.FirstName;
                tm.CallSign = callSign;
                tm.TruckNumber = t.TruckNumber;
                tm.Beat = t.beatNumber;
                tm.UserEmail = "CHP CAD";
                DataClasses.GlobalData.AddTruckMessage(tm);
                }
        }

        private static void updateIncidentWithCADNum(string data)
        {
            string[] splitter = data.Split('.');
            string shift = string.Empty;
            int incIndex = Array.IndexOf(splitter, "IS"); //get the incident number
            string IncidentID = splitter[incIndex + 1];
                /************
                 * Find Truck
                 * **********/
            string _beatData = Array.Find(splitter, b => b.StartsWith("B/")).ToString().Replace("B/", "");
            string _callSign = string.Empty;
            string _incidentType = Array.Find(splitter, it => it.StartsWith("T/")).ToString().Replace("T/", "");

            if (_beatData.Substring(0, 1) == "A" || _beatData.Substring(0, 1) == "*" || _beatData.Substring(0, 1) == "B")
            {
                switch (_beatData.Substring(0, 1))
                {
                    case "A":
                        shift = "AM";
                        break;
                    case "*":
                        shift = "MID";
                        break;
                    case "B":
                        shift = "PM";
                        break;
                }
                _callSign = _beatData.Substring(1, _beatData.Length - 1);
            }
            else
            {
                _callSign = _beatData;
            }
            /*
            if (_callSign.Substring(_callSign.Length - 1, 1) == "A" || _callSign.Substring(_callSign.Length - 1, 1) == "B") //we don't track a/b stuff
            {
                _callSign = _callSign.Substring(0, _callSign.Length - 1);
            }
            */
            TowTruck.TowTruck truck;
            if (!string.IsNullOrEmpty(shift))
            {
                truck = DataClasses.GlobalData.currentTrucks.Find(delegate(TowTruck.TowTruck find)
                {
                    return find.Driver.callSign == _callSign && find.Driver.AssignedShift.ToUpper().Contains(shift);
                });
            }
            else
            {
                truck = DataClasses.GlobalData.currentTrucks.Find(delegate(TowTruck.TowTruck find)
                {
                    return find.Driver.callSign == _callSign;
                });
            }

            if (truck != null)
            {
                //find the open incident
                MTCIncident i = null;
                if (truck.currentIncidentID != new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    i = DataClasses.GlobalData.Incidents.Find(delegate(MTCIncident iFind) { return iFind.IncidentID == truck.currentIncidentID; });
                }
                if (i != null)
                {
                    //check for pre assist data
                    if (i.preAssist != null && !string.IsNullOrEmpty(i.preAssist.CHPLogNumber))
                    {
                        i.preAssist.CHPLogNumber = IncidentID;
                        i.preAssist.CHPIncidentType = _incidentType;
                    }
                }
                truck.incidentID = IncidentID;
            }
        }

        private static void setIncident(string data)
        {
            try
            {
                //IS.2943217.DT/022315.TM/095226.T/FSP.B/A618-052.L/I880 N I80 W Onr - Over I80 W.ST/A
                string[] splitter = data.Split('.');
                string shift = string.Empty;
                /*************
                 * DT
                 * **********/

                string tm = Array.Find(splitter, t => t.StartsWith("TM"));
                string dt = Array.Find(splitter, d => d.StartsWith("DT"));
                DateTime dtIncident = makeDateTime(dt, tm);
                int incIndex = Array.IndexOf(splitter, "IS");
                string IncidentID = splitter[incIndex + 1];

                /************
                 * Find Truck
                 * **********/
                //string _callSign = splitter[5].Replace("B/", "");
                string _beatData = Array.Find(splitter, b => b.StartsWith("B/")).ToString().Replace("B/", "");
                string _callSign = string.Empty;
                if (_beatData.Substring(0, 1) == "A" || _beatData.Substring(0, 1) == "*" || _beatData.Substring(0, 1) == "B")
                {
                    switch (_beatData.Substring(0, 1))
                    {
                        case "A":
                            shift = "AM";
                            break;
                        case "*":
                            shift = "MID";
                            break;
                        case "B":
                            shift = "PM";
                            break;
                    }
                    _callSign = _beatData.Substring(1, _beatData.Length - 1);
                }
                else
                {
                    _callSign = _beatData;
                }
                /*
                if (_callSign.Substring(_callSign.Length - 1, 1) == "A" || _callSign.Substring(_callSign.Length - 1, 1) == "B") //we don't track a/b stuff
                {
                    _callSign = _callSign.Substring(0, _callSign.Length - 1);
                }
                */
                //string[] beatSplit = _beatData.Split('-');
                //string _callSign = beatSplit[0].ToString();
                TowTruck.TowTruck truck;
                if (!string.IsNullOrEmpty(shift))
                {
                    truck = DataClasses.GlobalData.currentTrucks.Find(delegate(TowTruck.TowTruck find)
                    {
                        return find.Driver.callSign == _callSign && find.Driver.AssignedShift.ToUpper().Contains(shift);
                    });
                }
                else
                {
                    truck = DataClasses.GlobalData.currentTrucks.Find(delegate(TowTruck.TowTruck find)
                    {
                        return find.Driver.callSign == _callSign;
                    });
                }


                if (truck != null)
                {

                    //first see if we already have an incident
                    MTCIncident i = DataClasses.GlobalData.Incidents.Find(delegate(MTCIncident find) { return find.IncidentID == truck.currentIncidentID ; });

                    MTCPreAssistData pad = new MTCPreAssistData();
                    //string[] beatSplit = _callSign.Split('-');
                    string beat = truck.beatNumber;
                    //build the preassist data
                    pad.Beat = beat;
                    pad.CHPLogNumber = IncidentID.ToString();
                    //pad.LocationofInitialDispatch = Array.Find(splitter, l => l.StartsWith("L/")).ToString().Replace("L/", "");
                    pad.FSPLocation = Array.Find(splitter, l => l.StartsWith("L/")).ToString().Replace("L/", "");
                    string[] typeSplitter = Array.Find(splitter, t => t.StartsWith("T/")).Split('.');
                    string incidentType = string.Empty;
                    if (typeSplitter[0].Contains("1126"))
                    {
                        incidentType = "Disabled Vehicle-Occupied (11-26)";
                    }
                    else if (typeSplitter[0].Contains("1182"))
                    {
                        incidentType = "Accident-Property Damage (11-82)";
                    }
                    else if (typeSplitter[0].Contains("1125"))
                    {
                        incidentType = "Traffic Hazard (11-25)";
                    }
                    else if (typeSplitter[0].Contains("1124"))
                    {
                        incidentType = "Abandoned Vehicle (11-24)";
                    }
                    else if (typeSplitter[0].Contains("1183"))
                    {
                        incidentType = "Accident-No Details (11-83)";
                    }
                    else
                    {
                        incidentType = "OTHER";
                    }

                    if (truck.currentIncident != null) {
                        truck.currentIncident.chpIncidentType = incidentType;
                        
                    }

                    pad.CHPIncidentType = incidentType;
                    truck.incidentID = IncidentID;
                    //new way
                    if (truck.currentIncident == null) {
                        Incident iNew = new Incident();
                        iNew.incidentDatePosted = DateTime.Now;
                        iNew.incidentID = Guid.NewGuid();
                        iNew.userPosted = "CHP CAD";
                        if (data.Contains("S/1097")) {
                            //this is a response message to a self-assigned incident
                            iNew.sentToTruck = true;
                        }
                        if (data.Contains("S/ENRT")) {
                            //this is a CAD assigned message
                            iNew.sentToTruck = false;
                        }
                        iNew.chpIncidentType = incidentType;
                        iNew.FSPLocation = Array.Find(splitter, l => l.StartsWith("L/")).ToString().Replace("L/", "");
                        iNew.CHPLogNumber = IncidentID.ToString();
                        iNew.logID = cadMsgID;
                        iNew.requestSent = true;
                        truck.addIncident(iNew);
                    }
                    else {
                        Incident iNew = new Incident();
                        iNew.sentToTruck = false;
                        iNew.chpIncidentType = incidentType;
                        iNew.incidentID = truck.currentIncident.incidentID;
                        iNew.beat = truck.beatNumber;
                        iNew.logID = cadMsgID;
                        iNew.CHPLogNumber = IncidentID.ToString();
                        iNew.FSPLocation = Array.Find(splitter, l => l.StartsWith("L/")).ToString().Replace("L/", "");
                        iNew.requestSent = true;
                        truck.updateIncident(iNew);
                    }

                    if (i == null)
                    {
                        //brand new incident, build from scratch
                        /************
                        * Build
                        * *********/
                        MTCIncident mi = new MTCIncident();
                        mi.assistList = new List<MTCAssist>();
                        mi.DatePosted = dtIncident;
                        mi.fromTruck = 0;
                        mi.preAssist = pad;
                        mi.UserPosted = "CHP CAD";
                        if (data.Contains("S/1097")) //this is a response message to a self-assigned incident
                        {
                            mi.Acked = true;
                            mi.sentToTruck = true;
                        }
                        if (data.Contains("S/ENRT")) //this is a CAD assigned message
                        {
                            mi.Acked = false;
                            mi.sentToTruck = false;
                        }
                        
                        mi.IPAddr = truck.Identifier;
                        mi.incidentComplete = false;
                        if (!data.Contains("T/FSP"))
                        {
                            if (truck.currentIncidentID != new Guid("00000000-0000-0000-0000-000000000000"))
                            {
                                //we have an unclosed incident, discard this message

                            }
                            else
                            {
                                MTCIncident iHave = DataClasses.GlobalData.Incidents.Find(delegate(MTCIncident find) { return find.preAssist.CHPLogNumber == IncidentID; });
                                if (iHave == null)
                                {
                                    SQL.SQLCode mySQL = new SQL.SQLCode();
                                    mi.IncidentID = mySQL.PostMTCIncident("CHP CAD", truck.TruckNumber, "0", 0, 0.0, 0.0);
                                    DataClasses.GlobalData.addIncident(mi);
                                }
                            }
                        }
                    }
                    else
                    {
                        //already got one
                        i.preAssist = pad;
                        i.incidentComplete = false;
                        i.sentToTruck = false;
                        i.Acked = false;
                        i.preAssist.CHPIncidentType = incidentType;
                        i.preAssist.DispatchCode = incidentType;
                        truck.incidentID = IncidentID;
                        truck.currentIncidentID = i.IncidentID;
                    }
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("ERROR ADDING INCIDENT: " + ex.Message);
                string err = ex.ToString();
            }
        }

        private static void sendMessageToTruck(string data) //force a message pop-up on the client
        {
            string[] splitBody = data.Split('.');
            string[] splitter = splitBody[2].ToString().Split('/');
            string _truckNumber = splitBody[1].ToString();
            //return bodyData;
            TowTruck.TowTruck t = DataClasses.GlobalData.currentTrucks.Find(delegate(TowTruck.TowTruck find) { return find.TruckNumber == _truckNumber; });
            if (t != null)
            {
                TruckMessage tm = new TruckMessage();
                tm.UserEmail = "CAD SYSTEM";
                tm.SentTime = DateTime.Now;
                tm.MessageID = Guid.NewGuid();
                tm.TruckIP = t.Identifier;
                tm.TruckNumber = _truckNumber;
                tm.Beat = t.beatNumber;
                tm.CallSign = t.Driver.callSign;
                tm.Driver = t.Driver.LastName + ", " + t.Driver.FirstName;
                tm.MessageText = splitter[1].ToString();
                tm.MessageType = 0;
                DataClasses.GlobalData.AddTruckMessage(tm);
            }
        }

        private static void setTruckStatus(string data) //set the truck status from the CAD System
        {
            try
            {
                string[] splitter = data.Split('.');
                string _callSign = splitter[1].ToString();
                string shift = string.Empty;
                if (_callSign.Substring(0, 1) == "A" || _callSign.Substring(0, 1) == "*" || _callSign.Substring(0, 1) == "B")
                {
                    switch (_callSign.Substring(0, 1))
                    {
                        case "A":
                            shift = "AM";
                            break;
                        case "B":
                            shift = "PM";
                            break;
                        case "*":
                            shift = "MID";
                            break;
                    }
                    _callSign = _callSign.Substring(1, _callSign.Length - 1);
                }
                //string _callSign = splitter[1].ToString().Substring(1, splitter[1].ToString().Length - 1);
                if (_callSign.Substring(_callSign.Length - 1, 1) == "A" || _callSign.Substring(_callSign.Length - 1, 1) == "B")
                {
                    //_callSign = _callSign.Substring(0, _callSign.Length - 1); we need to keep the trailing A for some of the drivers.
                }
                string newStatus = splitter[2].ToString().Replace("S/", "");
                /*
                                string _callSign = string.Empty;
                if (_beatData.Substring(0, 1) == "A" || _beatData.Substring(0, 1) == "*" || _beatData.Substring(0, 1) == "B")
                {
                    _callSign = _beatData.Substring(1, _beatData.Length - 1);
                }
                else
                {
                    _callSign = _beatData;
                }
                 * */
                //see if there's an incident information message
                string findIncident = "0";
                if (data.Contains("II/"))
                {
                    findIncident = Array.Find(splitter, l => l.StartsWith("II/")).ToString().Replace("II/", "");
                }
                TowTruck.TowTruck t;
                if (!string.IsNullOrEmpty(shift))
                {
                    t = DataClasses.GlobalData.currentTrucks.Find(delegate(TowTruck.TowTruck find)
                    { return find.Driver.callSign == _callSign && find.Driver.AssignedShift.ToUpper().Contains(shift); });
                }
                else
                {
                    t = DataClasses.GlobalData.currentTrucks.Find(delegate(TowTruck.TowTruck find)
                    { return find.Driver.callSign == _callSign; });
                }
                    

                if (t != null)
                {
                    if (findIncident != "0")
                    {
                        t.incidentID = findIncident;
                        //this means there's an incoming incident request.  This should create the incident information
                        //and notify the driver there's an incident.  The next packet for this truck should contain
                        //an IS notification for the same incident id that will contain the location the truck is supposed to go to.
                    }
                    switch (newStatus)
                    {
                        case "106": //This is a busy status that we represent as a forced break.
                            t.setStatus("FORCED BREAK", true); //busy status. set light blue color
                            break;
                        case "107":
                            //Regular ON BREAK Status can be set from CAD
                            t.setStatus("ON BREAK", true);
                            break;
                        case "108":
                            //set on patrol
                            t.setStatus("ON PATROL", true);
                            break;
                        case "1097":
                            //set on incident
                            t.setStatus("ON INCIDENT", true);
                            break;
                        case "1098":
                            //set incident complete
                            t.setStatus("ON PATROL", true);
                            break;
                        case "1010":
                            //forced roll in
                            t.setStatus("ROLL IN", true);
                            break;
                        case "1022":
                            //cancel current incident assignment
                            if (t.currentIncidentID != new Guid("00000000-0000-0000-0000-000000000000"))
                            {
                                MTCIncident i = DataClasses.GlobalData.Incidents.Find(s => s.IncidentID == t.currentIncidentID);
                                if (i != null)
                                {
                                    i.incidentComplete = true;
                                    i.Canceled = true;
                                    i.Reason = "Cancelled by CAD";
                                    SQL.SQLCode sql = new SQL.SQLCode();
                                    sql.UpdateMTCIncident(i.IncidentID, true, "Cancelled by CAD");
                                    t.setStatus("ON PATROL", true);
                                }
                            }
                            break;
                        case "ENRT":
                            //set en route
                            t.setStatus("ENROUTE", true);
                            break;
                        case "BREAK":
                            t.setStatus("FORCED BREAK", true);
                            //set forced break, this will need to be changed when we find the actual status code
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
        }

        #endregion

        #region " Message Helpers "

        public static string makeDate(string DT)
        {
            try
            {
                string date = DT.Substring(0, 2) + "/" + DT.Substring(2, 2) + "/" +
                    DT.Substring(4, 2);
                return date;
            }
            catch
            {
                return "01/01/2001";
            }
        }

        public static string makeTime(string TM)
        {
            try
            {
                string time = TM.Substring(0, 2) + ":" + TM.Substring(2, 2) + ":" +
                    TM.Substring(4, 2);
                return time;
            }
            catch
            {
                return "00:00:00";
            }
        }

        private static DateTime makeDateTime(string DT, string TM)
        {
            string date = makeDate(DT.Replace("DT/",""));
            string time = makeTime(TM.Replace("TM/",""));
            DateTime dt = new DateTime();
            if (DateTime.TryParse(date + " " + time, out dt))
            {
                return dt;
            }
            else
            {
                dt = Convert.ToDateTime("01/01/2001 00:00:00");
                return dt;
            }
        }

        #endregion
    }
}
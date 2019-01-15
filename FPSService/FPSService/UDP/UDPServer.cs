using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Web.Script;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using Microsoft.SqlServer.Types;
using System.IO;
using System.Configuration;

namespace FPSService.UDP
{
    public class UDPServer
    {
        private Logging.EventLogger logger; //error logging
        private Thread listenThread; //each truck gets its own thread
        private byte[] data = new byte[4096]; //incoming packet data
        private List<string> OtherServers = new List<string>();
        private bool forward = false;
        private SendMessage udpSend = new SendMessage();
        public Socket udpListener;
        public UDPServer()
        {
            string others = ConfigurationManager.AppSettings["OtherServers"].ToString();
            string[] listOthers = others.Split('|');
            for (int i = 0; i < listOthers.Count(); i++)
            {
                OtherServers.Add(listOthers[i].ToString());
            }
            string fwd = ConfigurationManager.AppSettings["forward"].ToString();
            if (fwd.ToUpper() == "TRUE")
            {
                forward = true;
            }
            listenThread = new Thread(new ThreadStart(UDPListenThread));
            listenThread.Start();
        }

        public void disconnectUDP()
        {
            if (udpListener.IsBound)
            {
                udpListener.Disconnect(false);
            }
        }

        private void UDPListenThread()
        {
            logger = new Logging.EventLogger(); //for the initial dev phase we're going to log a lot, disable this before
            //rolling out to production
            udpListener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ourEndPoint = new IPEndPoint(IPAddress.Any, 9017);
            IPEndPoint end = new IPEndPoint(IPAddress.Any, 9017);
            EndPoint Identifier = (EndPoint)end;
            udpListener.Bind(ourEndPoint);

            while (true)
            {
                string message = null; ;
                try
                {
                    bool fwdThisMessage = true;
                    int length = udpListener.ReceiveFrom(data, ref Identifier);
                    message = System.Text.Encoding.UTF8.GetString(data, 0, length);
                    string _ipaddr = ((IPEndPoint)Identifier).Address.ToString();
                    if (message.Contains("<FWD>"))
                    {
                        //it's a forwarded message, remove the <FWD> tags and find the specified IPAddress
                        //then treat it like a normal packet
                        message = message.Replace("<FWD>", "");
                        int firstLT = message.IndexOf('<');
                        _ipaddr = message.Substring(0, firstLT);
                        message = message.Replace(_ipaddr + "</FWD>", "");
                        fwdThisMessage = false;
                        //done, let it move on
                    }
                    //JavaScriptSerializer js = new JavaScriptSerializer();
                    TowTruck.TowTruck thisTruck;
                    thisTruck = DataClasses.GlobalData.FindTowTruck(_ipaddr);
                    string type = "";
                    try
                    {
                        if (message.Contains("<GPS>"))
                        {
                            type = "GPS";
                            if (forward == true && fwdThisMessage == true)
                            {
                                string fwdMsg = "<FWD>" + _ipaddr + "</FWD>" + message;
                                foreach (string s in OtherServers)
                                {
                                    udpSend.ForwardMessage(fwdMsg, s);
                                }
                            }
                        }
                    }
                    catch
                    { }
                    try
                    {
                        if (message.Contains("<State>"))
                        {
                            type = "State";
                            if (forward == true && fwdThisMessage == true)
                            {
                                string fwdMsg = "<FWD>" + _ipaddr + "</FWD>" + message;
                                foreach (string s in OtherServers)
                                {
                                    udpSend.ForwardMessage(fwdMsg, s);
                                }
                            }
                        }
                    }
                    catch
                    { }
                    try
                    {
                        if (message.Contains("<IPHistory>"))
                        {
                            type = "IPHistory";
                            if (forward == true && fwdThisMessage == true)
                            {
                                string fwdMsg = "<FWD>" + _ipaddr + "</FWD>" + message;
                                foreach (string s in OtherServers)
                                {
                                    udpSend.ForwardMessage(fwdMsg, s);
                                }
                            }
                        }
                    }
                    catch 
                    { }
                    /*
                    string[] splitMsg = message.Split('|');
                    string type = splitMsg[0].ToString();
                    string msg = splitMsg[1].ToString();
                     * */
                    TowTruck.GPS thisGPS = null;
                    TowTruck.State thisState = null;
                    TowTruck.IPHistory history = null;
                    if (type == "GPS")
                    {
                        //thisGPS = js.Deserialize<TowTruck.GPS>(msg);
                        XmlSerializer ser = new XmlSerializer(typeof(TowTruck.GPS));
                        StringReader rdr = new StringReader(message);
                        thisGPS = (TowTruck.GPS)ser.Deserialize(rdr);
                        SqlGeographyBuilder builder = new SqlGeographyBuilder();
                        builder.SetSrid(4326);
                        builder.BeginGeography(OpenGisGeographyType.Point);
                        builder.BeginFigure(thisGPS.Lat, thisGPS.Lon);
                        builder.EndFigure();
                        builder.EndGeography();
                        thisGPS.Position = builder.ConstructedGeography;
                    }
                    else if (type == "State")
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(TowTruck.State));
                        //testing, this was causing an error on parse, but couldn't find a reason why
                        message = message.Replace(" xmlns=''", "");
                        StringReader rdr = new StringReader(message);
                        thisState = (TowTruck.State)ser.Deserialize(rdr);
                    }
                    else if (type == "IPHistory")
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(TowTruck.IPHistory));
                        StringReader rdr = new StringReader(message);
                        history = (TowTruck.IPHistory)ser.Deserialize(rdr);
                    }
                    if (thisTruck != null)
                    {
                        try
                        {
                            thisTruck.LastMessage.LastMessageReceived = DateTime.Now;
                            DataClasses.GlobalData.UpdateTowTruck(_ipaddr, thisTruck);
                            //DataClasses.GlobalData.AddTowTruck(thisTruck);
                            if (string.IsNullOrEmpty(thisTruck.Extended.TruckNumber))
                            {
                                SQL.SQLCode mySQL = new SQL.SQLCode();
                                TowTruck.TowTruckExtended thisExtended = mySQL.GetExtendedData(thisTruck.Identifier);
                                thisTruck.Extended = thisExtended;
                                thisTruck.TruckNumber = thisExtended.TruckNumber;
                            }
                            /*if (thisTruck.assignedBeat.Loaded == false)  //
                            {
                                SQL.SQLCode mySQL = new SQL.SQLCode();
                                TowTruck.AssignedBeat thisAssignedBeat = mySQL.GetAssignedBeat(thisTruck.Extended.FleetVehicleID);
                                if (thisAssignedBeat != null)
                                {
                                    thisTruck.assignedBeat = thisAssignedBeat;
                                }
                            }*/
                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {
                            thisTruck = new TowTruck.TowTruck(_ipaddr);
                            DataClasses.GlobalData.AddTowTruck(thisTruck);
                            SQL.SQLCode mySQL = new SQL.SQLCode();
                            TowTruck.TowTruckExtended thisExtended = mySQL.GetExtendedData(thisTruck.Identifier);
                            thisTruck.Extended = thisExtended;
                            thisTruck.TruckNumber = thisExtended.TruckNumber;
                            thisTruck.Status.StatusStarted = DateTime.Now;
                        }
                        catch { }
                    }
                    if (type == "GPS")
                    {
                        thisTruck.UpdateGPS(thisGPS);
                        thisTruck.TowTruckGPSUpdate();
                    }
                    if (type == "State")
                    {
                        thisTruck.UpdateState(thisState);
                        thisTruck.TowTruckChanged();
                    }
                    if (type == "IPHistory")
                    {
                        if (thisTruck.State != null && string.IsNullOrEmpty(thisTruck.State.IPList))
                        {
                            thisTruck.State.IPList += history.IP;
                        }
                        else if(thisTruck.State != null && !string.IsNullOrEmpty(thisTruck.State.IPList))
                        {
                            thisTruck.State.IPList += "," + history.IP;
                        }
                        
                    }
                    thisTruck.TTQueue.Enqueue(message);
                }
                catch (Exception ex)
                {
                    
                    logger.LogEvent(DateTime.Now.ToString() + " Error in UDP Listening Thread" + Environment.NewLine + ex.ToString() +
                    Environment.NewLine + "Original Message:" + Environment.NewLine + message, true);
                }
            }
        }

        private void queueMessage(string _message, string _ipaddr)
        {
            TowTruck.TowTruck towTruck = DataClasses.GlobalData.FindTowTruck(_ipaddr);
            if (towTruck == null)
            {
                try
                {
                    towTruck = new TowTruck.TowTruck(_ipaddr);
                    towTruck.TowTruckChangedEventHandler += this.TowTruckChanged;
                    towTruck.TowTruckGPSUpdateEventHandler += this.TowTruckGPSUpdated;
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error queueing message" + Environment.NewLine + ex.ToString(), true);
                }
            }
        }

        public void TowTruckChanged(TowTruck.TowTruck towTruck)
        {
 
        }

        private void UpdateTowTruck(TowTruck.TowTruck towTruck)
        {
            towTruck.LastMessage.LastMessageReceived = DateTime.Now;

        }

        public void TowTruckGPSUpdated(TowTruck.TowTruck towTruck)
        {
            
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace FPSService.TowTruck
{
    public class TowTruckMessage
    {
        protected bool sendAck = false;
        protected bool getAck = false;
        protected string messageNumber = "0";
        protected int id = SequenceIdentifier.Instance.SequenceNumber;
        protected int numRetries = 2;


        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// LMTMessage virtual functions
        public virtual void AddAttribute(string attribute, string value)
        {
            if (attribute == "Id")
            {
                messageNumber = value;
                id = Convert.ToInt32(messageNumber);
            }
            //else
                //LMTServerLog.log("Unhandled attribute: " + attribute);
        }

        public virtual void Execute(TowTruck towTruck)
        {
            //updated 10/11/2011, added hashtable to LMT class to track messages already acked. 
            //Check to see if a message has been acked before sending it out again.
            //if (!lmt.msgIDs.ContainsValue(id))
            //{
            ExecuteMessage(towTruck);
            //}
        }

        public virtual void ExecuteMessage(TowTruck towTruck)
        {
            //LMTServerLog.log("Execute message to unknown type of message");
        }

        public virtual String SendMessage()
        {
            return "";
        }

        public virtual string XmlMessage
        {
            get { return ""; }
        }

        public virtual void FailToSend(TowTruck towTruck)
        {
            Console.WriteLine("A message requiring an ACK failed.");
        }

        public virtual void AckReceived(TowTruck towTruck)
        {
        }

        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// LMTMessage Properties    
        public bool Ack
        {
            get { return sendAck; }
        }

        public bool GetAck
        {
            get
            {
                return getAck;
            }
        }

        public virtual string AckString
        {
            get
            {
                TowTruckXMLWriter writer = new TowTruckXMLWriter();
                writer.WriteStartElement("Ack");
                writer.WriteElementString("Id", id.ToString());
                writer.WriteEndElement();
                return writer.XmlString;
                //probably the best place to hash ids
            }
        }

        public int ID
        {
            get { return id; }
        }

        public int Retries
        {
            get { return numRetries; }
        }
    }
}
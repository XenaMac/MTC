using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.ServiceModel.Syndication;

namespace FPSService
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class RSSFeeder
    {
        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        public void DoWork()
        {
            // Add your operation implementation here
            return;
        }
        /*
        [OperationContract]
        [WebGet]
        public Rss20FeedFormatter GetTrucks()
        {
            
            SyndicationFeed feed = new SyndicationFeed("Current Truck Status", "Status of currently connected FSP Trucks", new Uri("http://latatrax.net:9007/RssFeeder.svc"));
            feed.Authors.Add(new SyndicationPerson("FSPService"));
            feed.Categories.Add(new SyndicationCategory("Tow Truck Tracking"));
            feed.Description = new TextSyndicationContent("This feed shows currently connected trucks and states for FSP");
            List<SyndicationItem> items = new List<SyndicationItem>();
            foreach (TowTruck.TowTruck thisTruck in DataClasses.GlobalData.currentTrucks)
            {
                SyndicationItem item = new SyndicationItem(
                    "Vehicle IP: " + thisTruck.Identifier.ToString(),
                    "Truck Number: " + thisTruck.TruckNumber.ToString() + "|" +
                    "Logon Status: " + thisTruck.Status.VehicleStatus + "|" +
                    "Driver Name: " + thisTruck.Driver.LastName + ", " + thisTruck.Driver.FirstName + "|" +
                    "Out of Bounds Alarms: " + thisTruck.Status.OutOfBoundsAlarm.ToString() + "|" +
                    "Speeding Alarm: " + thisTruck.Status.SpeedingAlarm.ToString(),
                    new Uri("http://latatrax.net:9007"),
                    thisTruck.Identifier.ToString(),
                    DateTime.Now
                    );
                items.Add(item);
            }
            feed.Items = items;
            return new Rss20FeedFormatter(feed);
            
        }
        */
        // Add more operations here and mark them with [OperationContract]
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using FPSService.TowTruck;

namespace FPSService
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FSPService
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

        public List<TowTruck.TowTruck> GetAllTrucks()
        {
            return DataClasses.GlobalData.currentTrucks;
        }

        public TowTruck.TowTruckExtended GetExtendedData(string TruckID)
        {
            TowTruck.TowTruckExtended thisTruck = new TowTruckExtended();
            var lstTrucks = from tt in DataClasses.GlobalData.currentTrucks
                            where tt.Identifier == TruckID
                            select tt.Extended;
            foreach(TowTruck.TowTruckExtended myTruck in lstTrucks)
            {
                thisTruck = myTruck;
            }
            return thisTruck;
        }

        // Add more operations here and mark them with [OperationContract]
    }
}

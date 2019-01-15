using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;

namespace FPSService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IEsriService" in both code and config file together.
    [ServiceContract]
    public interface IEsriService
    {
        [OperationContract]
        void DoWork();

        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        List<EsriTruck> GetEsriTrucks();

    }
}

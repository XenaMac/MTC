using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class DriverInteraction
    {
        public System.Guid InteractionID { get; set; }
        public System.Guid ContractorID { get; set; }
        public System.Guid DriverID { get; set; }
        public System.Guid InteractionTypeID { get; set; }
        public string InteractionArea { get; set; }
        public string InteractionDescription { get; set; }
        public bool InspectionPassFail { get; set; }
        public bool AccidentPreventable { get; set; }
        public bool FollowupRequired { get; set; }
        public string FollowupDescription { get; set; }
        public Nullable<System.DateTime> FollowupDate { get; set; }
        public Nullable<System.DateTime> FollowupCompletionDate { get; set; }
        public string FollowupComments { get; set; }
        public Nullable<System.DateTime> CloseDate { get; set; }
        public string BadgeID { get; set; }
        public Nullable<System.DateTime> InteractionDate { get; set; }
        public string VehicleNumber { get; set; }
        public string BeatNumber { get; set; }
        public virtual Contractor Contractor { get; set; }
        public virtual Driver Driver { get; set; }
        public virtual InteractionType InteractionType { get; set; }
    }
}

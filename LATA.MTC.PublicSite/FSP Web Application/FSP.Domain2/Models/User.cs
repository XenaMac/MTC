using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class User
    {
        public System.Guid UserID { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<bool> WantsNotification { get; set; }
        public Nullable<double> HourlyCost { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        public Nullable<System.DateTime> LastActivityDate { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public System.Guid RoleID { get; set; }
        public Nullable<System.Guid> ContractorID { get; set; }
        public virtual Role Role { get; set; }
    }
}

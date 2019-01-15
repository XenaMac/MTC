using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSP.Web.Areas.AdminArea.ViewModels
{
    public class UsersViewModel
    {
        public class UsersIndexViewModel
        {
            public List<UIUser> UIUsers { get; set; }
        }
    }

    public class UIUser
    {
        public Guid UserID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String RoleName { get; set; }
        public bool IsApproved { get; set; }
    }
}
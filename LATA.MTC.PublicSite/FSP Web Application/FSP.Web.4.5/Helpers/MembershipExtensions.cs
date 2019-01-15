using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace FSP.Web.Helpers
{
    public static class MembershipExtensions
    {

        public static Guid GetUserId()
        {
            Guid result = Guid.Empty;
            try
            {
                MembershipUser myUser = Membership.GetUser();
                Guid.TryParse(myUser.ProviderUserKey.ToString(), out result);
            }
            catch { }

            return result;
        }
    }
}
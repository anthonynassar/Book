using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace Backend.Helpers
{
    public static class Settings
    {
        public static string GetUserId(IPrincipal user)
        {
            ClaimsPrincipal claimsUser = (ClaimsPrincipal)user;

            string provider = claimsUser.FindFirst("http://schemas.microsoft.com/identity/claims/identityprovider").Value;
            string sid = claimsUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            // The above assumes WEBSITE_AUTH_HIDE_DEPRECATED_SID is true. Otherwise, use the stable_sid claim:
            // string sid = claimsUser.FindFirst("stable_sid").Value; 

            return $"{provider}|{sid}";
        }

        public static string GetUserEmail(IPrincipal user)
        {
            ClaimsPrincipal claimsUser = (ClaimsPrincipal)user;
            string email2 = "";
            string email3 = "";
            try
            {
                email3 = claimsUser.FindFirst("emails").Value;
            }
            catch (Exception)
            {
                email2 = "";
                email3 = "";
            }
            

            return $"{email2}";
        }
    }
}
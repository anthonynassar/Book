using Microsoft.Azure.Mobile.Server.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Net.Http;
using Backend.DataObjects;
using System.Threading.Tasks;
using System.Globalization;

namespace Backend.Helpers
{
    public static class Settings
    {
        public static string GetUserId(IPrincipal user)
        {
            ClaimsPrincipal claimsUser = (ClaimsPrincipal)user;

            string provider = claimsUser.FindFirst("http://schemas.microsoft.com/identity/claims/identityprovider").Value;
            string sid = claimsUser.FindFirst(ClaimTypes.NameIdentifier).Value.Replace(':','|');

            // The above assumes WEBSITE_AUTH_HIDE_DEPRECATED_SID is true. Otherwise, use the stable_sid claim:
            // string sid = claimsUser.FindFirst("stable_sid").Value; 

            return $"{provider}|{sid}";
        }

        public static async Task<User> FillUpUser(IPrincipal user, HttpRequestMessage request, User item)
        {
            ClaimsPrincipal claimsUser = (ClaimsPrincipal)user;
            //ProviderCredentials providerCredentials;

            string provider = claimsUser.FindFirst("http://schemas.microsoft.com/identity/claims/identityprovider").Value;
            if (provider.Equals("aad"))
            {
                var aadIdentity = await user.GetAppServiceIdentityAsync<AzureActiveDirectoryCredentials>(request);
                item.GivenName = aadIdentity.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname");
                item.Surname = aadIdentity.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname");
                item.Email = aadIdentity.FindFirstValue("emails");
                item.Country = aadIdentity.FindFirstValue("country");
                item.City = aadIdentity.FindFirstValue("city");
                item.Username = aadIdentity.FindFirstValue("name");
                item.Birthdate = DateTime.Today.AddYears(-50);
            }
            else
            {
                var facebookIdentity = await user.GetAppServiceIdentityAsync<FacebookCredentials>(request);
                item.GivenName = facebookIdentity.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname");
                item.Surname = facebookIdentity.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname");
                var email = item.Email = facebookIdentity.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
                item.Gender = facebookIdentity.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/gender");
                item.Username = email.Split('@')[0];
                var cultureInfo = item.CultureInfo = facebookIdentity.FindFirstValue("urn:facebook:locale");
                cultureInfo = cultureInfo.Replace('_', '-');
                CultureInfo info = new CultureInfo(cultureInfo);
                string format = "d";
                string dateString = facebookIdentity.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth");
                item.Birthdate = DateTime.ParseExact(dateString, format, info);
            }

            return item;
        }

        public static string FindFirstValue(this ProviderCredentials principal, string claimType)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            var claim = principal.UserClaims.FirstOrDefault(c => c.Type == claimType);
            return !(string.IsNullOrWhiteSpace(claim.Value)) ? claim.Value : null;
        }
    }
}
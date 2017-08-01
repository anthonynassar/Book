using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleApp.Models.ViewsRelated
{
    public class UserInfo
    {
        [JsonProperty("id_token")]
        public string IdToken { get; set; }
        [JsonProperty("provider_name")]
        public string ProviderName { get; set; }
        [JsonProperty("user_claims")]
        public List<UserClaim> UserClaims { get; set; }
        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }

    public class UserClaim
    {
        [JsonProperty("typ")]
        public string Type { get; set; }
        [JsonProperty("val")]
        public string Value { get; set; }
    }
}

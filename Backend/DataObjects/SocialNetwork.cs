using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.DataObjects
{
    public class SocialNetwork : EntityData
    {
        public SocialNetwork()
        {
            Subscription = new List<Subscription>();
        }

        public string Name { get; set; }
        public string Sid { get; set; }

        public virtual ICollection<Subscription> Subscription { get; set; }
    }
}
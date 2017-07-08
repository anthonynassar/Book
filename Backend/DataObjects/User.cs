using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.DataObjects
{
    public class User : EntityData
    {
        public User()
        {
            Object = new List<Object>();
            SharingSpace = new List<SharingSpace>();
            Subscription = new List<Subscription>();
        }

        //public long IdUser { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public long? Address { get; set; }
        public string Profession { get; set; }
        public string Privilege { get; set; }

        public virtual ICollection<Object> Object { get; set; }
        public virtual ICollection<SharingSpace> SharingSpace { get; set; }
        public virtual ICollection<Subscription> Subscription { get; set; }
    }

    
}
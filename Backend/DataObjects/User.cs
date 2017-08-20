using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Backend.DataObjects
{
    public class User : EntityData
    {
        public User()
        {
            //Object = new List<Object>();
            //SharingSpace = new List<SharingSpace>();
            //Subscription = new List<Subscription>();
            //Interest = new List<Interest>();
        }

        //public long IdUser { get; set; }
        //public string Name { get; set; }
        [Index]
        [StringLength(40)]
        public string Email { get; set; }
        public string Address { get; set; }
        public string Profession { get; set; }
        public string Privilege { get; set; }

        // to add
        public string Username { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public DateTime Birthdate { get; set; }
        public string Gender { get; set; }
        public string CultureInfo { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }


        //public virtual ICollection<Object> Object { get; set; }
        //public virtual ICollection<SharingSpace> SharingSpace { get; set; }
        //public virtual ICollection<Subscription> Subscription { get; set; }
        //public virtual ICollection<Interest> Interest { get; set; }
    }

    
}
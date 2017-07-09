using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.DataObjects
{
    public class Preference : EntityData
    {
        public Preference()
        {
            Interest = new List<Interest>();
        }
        public string Tag { get; set; }

        public virtual ICollection<Interest> Interest { get; set; }
    }

   
}
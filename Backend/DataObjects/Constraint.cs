using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.DataObjects
{
    public class Constraint : EntityData
    {
        public Constraint()
        {
            Event = new List<Event>();
        }
        public string Operator { get; set; }
        public string Value { get; set; }

        public virtual ICollection<Event> Event { get; set; }
    }
}
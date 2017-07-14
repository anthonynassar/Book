using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.DataObjects
{
    public class Dimension : EntityData
    {
        public Dimension()
        {
            Granularity = new List<Granularity>();
            DimDatatype = new List<DimDatatype>();
            Event = new List<Event>();
        }
        
        public string Label { get; set; }
        public bool? Interval { get; set; }

        public virtual ICollection<Granularity> Granularity { get; set; }
        public virtual ICollection<DimDatatype> DimDatatype { get; set; }
        public virtual ICollection<Event> Event { get; set; }
    }
}
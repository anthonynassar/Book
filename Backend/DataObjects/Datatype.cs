using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.DataObjects
{
    public class Datatype : EntityData
    {
        public Datatype()
        {
            DimDatatype = new List<DimDatatype>();
        }
        public string Type { get; set; }
        public string Domain { get; set; }

        public virtual ICollection<DimDatatype> DimDatatype { get; set; }
    }
}
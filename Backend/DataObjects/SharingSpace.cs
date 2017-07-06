using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.DataObjects
{
    public class SharingSpace : EntityData
    {
        public SharingSpace()
        {
            Object = new List<Object>();
        }

        //public long IdSs { get; set; }
        public long UserId { get; set; }
        public string Descriptor { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CreationLocation { get; set; }

        public virtual ICollection<Object> Object { get; set; }
    }
}
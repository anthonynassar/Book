using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Backend.DataObjects
{
    public class SharingSpace : EntityData
    {
        public SharingSpace()
        {
            Object = new List<Object>();
            Event = new List<Event>();
        }

        //public long IdSs { get; set; }
        [MaxLength(128)]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public string Descriptor { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.UtcNow;
        public string CreationLocation { get; set; }

        public virtual ICollection<Object> Object { get; set; }
        public virtual ICollection<Event> Event { get; set; }
    }
}
using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Backend.DataObjects
{
    public class Object : EntityData
    {
        public Object()
        {
            Attribute = new List<Attribute>();
        }
        public string Type { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.UtcNow;
        public string CreationLocation { get; set; }
        public string Uri { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        [MaxLength(128)]
        public string SharingSpaceId { get; set; }
        public virtual SharingSpace SharingSpace { get; set; }

        public virtual ICollection<Attribute> Attribute { get; set; }

    }
}
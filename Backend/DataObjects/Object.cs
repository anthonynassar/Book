using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.DataObjects
{
    public class Object : EntityData
    {
        //public long IdObject { get; set; }
        public long UserId { get; set; }
        public string Type { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CreationLocation { get; set; }

        //public long SharingSpaceId { get; set; }
        public virtual SharingSpace SharingSpace { get; set; }
    }
}
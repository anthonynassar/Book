using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Backend.DataObjects
{
    public class Event
    {

        [Key]
        [Column(Order = 1)]
        public string SharingSpaceId { get; set; }
        //public virtual SharingSpace SharingSpace { get; set; }

        [Key, ForeignKey("Dimension")]
        [Column(Order = 2)]
        public string DimensionId { get; set; }
        public virtual Dimension Dimension { get; set; }

        [Key, ForeignKey("Constraint")]
        [Column(Order = 3)]
        public string ConstraintId { get; set; }
        public virtual Constraint Constraint { get; set; }
    }
}
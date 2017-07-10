using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Backend.DataObjects
{
    public class Attribute
    {
        //[Key, ForeignKey("Object")] // detected automatically
        [Key]
        [Column(Order = 3)]
        public string ObjectId { get; set; }
        //public virtual Object Object { get; set; } -> waas creating a problem, better to remove it, detects automatically the relation

        [Key, ForeignKey("Datatype")]
        [Column(Order = 4)]
        public string DatatypeId { get; set; }
        public virtual Datatype Datatype { get; set; }

        public string Value { get; set; }
    }
}
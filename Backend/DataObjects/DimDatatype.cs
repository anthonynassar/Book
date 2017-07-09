using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Backend.DataObjects
{
    public class DimDatatype
    {
        [Key, ForeignKey("Dimension")]
        [Column(Order = 1)]
        public string DimensionId { get; set; }
        public virtual Dimension Dimension { get; set; }

        [Key, ForeignKey("Datatype")]
        [Column(Order = 2)]
        public string DatatypeId { get; set; }
        public virtual Datatype Datatype { get; set; }
    }
}
using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Backend.DataObjects
{
    public class Granularity : EntityData
    {
        [MaxLength(128)]
        public string DimensionId { get; set; }
        public virtual Dimension Dimension { get; set; }

        public string Value { get; set; }
    }
}
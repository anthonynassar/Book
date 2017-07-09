using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Backend.DataObjects
{
    public class Interest
    {
        [Key, ForeignKey("User")]
        [Column(Order = 1)]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        [Key, ForeignKey("Preference")]
        [Column(Order = 2)]
        public string PreferenceId { get; set; }
        public virtual Preference Preference { get; set; }
    }
}
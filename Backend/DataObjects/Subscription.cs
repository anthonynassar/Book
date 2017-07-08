using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Backend.DataObjects
{
    public class Subscription : EntityData
    {
        [ForeignKey("User")]
        [Column(Order = 1)]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("SocialNetwork")]
        [Column(Order = 2)]
        public string SnId { get; set; }
        public virtual SocialNetwork SocialNetwork { get; set; }

    }
}
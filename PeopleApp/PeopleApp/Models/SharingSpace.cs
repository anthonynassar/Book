using PeopleApp.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleApp.Models
{
    public class SharingSpace : TableData
    {
        public string UserId { get; set; }
        public string Descriptor { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.UtcNow;
        public string CreationLocation { get; set; }
        public bool Verified { get; set; }
    }
}

using PeopleApp.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleApp.Models
{
    public class Object : TableData
    {
        public string UserId { get; set; }
        public string Type { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.UtcNow;
        public string CreationLocation { get; set; }
        public string SharingSpaceId { get; set; }
        public string Uri { get; set; }
    }
}

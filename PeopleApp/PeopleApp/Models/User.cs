using PeopleApp.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleApp.Models
{
    public class User : TableData
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Profession { get; set; }
        public string Privilege { get; set; }
    }
}

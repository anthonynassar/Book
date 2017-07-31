using PeopleApp.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleApp.Models
{
    public class Datatype : TableData
    {
        public string Type { get; set; }
        public string Domain { get; set; }
    }
}

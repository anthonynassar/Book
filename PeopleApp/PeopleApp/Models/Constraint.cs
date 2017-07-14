using PeopleApp.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleApp.Models
{
    public class Constraint : TableData
    {
        public string Operator { get; set; }
        public string Value { get; set; }
    }
}

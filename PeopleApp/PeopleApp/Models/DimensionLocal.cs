using PeopleApp.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleApp.Models
{
    public class DimensionLocal : TableData
    {
        public DimensionLocal()
        {
            ConstraintList = new List<Constraint>();
        }

        public string Label { get; set; }
        public bool? Interval { get; set; }

        public List<Constraint> ConstraintList { get; set; }
    }
}

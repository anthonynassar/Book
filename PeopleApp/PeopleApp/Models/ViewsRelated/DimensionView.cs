using PeopleApp.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleApp.Models
{
    public class DimensionView : Dimension
    {
        public DimensionView()
        {
            //base.Id = dimension.Id;
            //base.Interval = dimension.Interval;
            //base.Label = dimension.Label;
            //base.CreatedAt = dimension.CreatedAt;
            //base.UpdatedAt = dimension.UpdatedAt;
            //base.Version = dimension.Version;

            ConstraintList = new List<Constraint>();
        }
        public List<Constraint> ConstraintList { get; set; }
    }
}

using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.DataObjects
{
    public class Constraint : EntityData
    {
        public string Operator { get; set; }
        public string Value { get; set; }
    }
}
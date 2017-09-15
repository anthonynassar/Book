using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleApp.Models
{
    public class StorageTokenViewModel
    {
        public string Name { get; set; }
        public Uri Uri { get; set; }
        public string SasToken { get; set; }
    }
}

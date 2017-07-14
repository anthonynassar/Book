using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleApp.Multiselect
{
    /// <summary>
    /// Example 'business model' class for items to appear 
    /// in the multi-select list. 
    /// By default it expects a Name property.
    /// </summary>
    public class CheckItem
    {
        public CheckItem()
        {
        }

        public string Name { get; set; }
    }
}

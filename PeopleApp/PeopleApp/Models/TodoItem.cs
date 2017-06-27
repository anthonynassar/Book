using PeopleApp.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleApp.Models
{
    public class TodoItem : TableData
    {
        public string Text { get; set; }
        public bool Complete { get; set; }
    }
}
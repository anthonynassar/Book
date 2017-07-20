using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamvvm;

namespace PeopleApp.Models.ViewsRelated
{
    class MasterPageItem : BaseModel
    {
        string title;
        public string Title
        {
            get { return title; }
            set { SetField(ref title, value); }
        }
        ICommand command;
        public ICommand Command
        {
            get { return command; }
            set { SetField(ref command, value); }
        }
        public string IconSource { get; set; }
        public System.Type TargetType { get; set; }
    }
}

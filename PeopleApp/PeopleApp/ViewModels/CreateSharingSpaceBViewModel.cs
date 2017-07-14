using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamvvm;

namespace PeopleApp.ViewModels
{
    public class CreateSharingSpaceBViewModel : BasePageModel
    {
        public CreateSharingSpaceBViewModel() 
        {
            ReloadTags();
            RemoveTagCommand = new BaseCommand<TagItem>((arg) => RemoveTag(arg));
        }

        public ObservableCollection<TagItem> Items
        {
            get { return GetField<ObservableCollection<TagItem>>(); }
            set { SetField(value); }
        }

        public void ReloadTags()
        {
            var tags = new ObservableCollection<TagItem>(){
                new TagItem() { Name = "#TagExample" },
                new TagItem() { Name = "#Xamarin" },
                new TagItem() { Name = "#AnthonyNassar" },
                new TagItem() { Name = "#Test" },
                new TagItem() { Name = "#XamarinForms" },
                new TagItem() { Name = "#TagEntryView" },
                new TagItem() { Name = "#TapMe!" },
                new TagItem() { Name = "#itsworking!" },
            };

            Items = tags;
        }

        public void RemoveTag(TagItem tagItem)
        {
            if (tagItem == null)
                return;

            Items.Remove(tagItem);
        }

        public TagItem ValidateAndReturn(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                return null;

            var tagString = tag.StartsWith("#") ? tag : "#" + tag;

            if (Items.Any(v => v.Name.Equals(tagString, StringComparison.OrdinalIgnoreCase)))
                return null;

            var newTag = new TagItem()
            {
                Name = tagString.ToLower()
            };

            //Items.Add(newTag);

            return newTag;
        }

        public IBaseCommand RemoveTagCommand
        {
            get { return GetField<IBaseCommand>(); }
            set { SetField(value); }
        }

        public class TagItem : BaseModel
        {
            string name;
            public string Name
            {
                get { return name; }
                set { SetField(ref name, value); }
            }
        }
    }
}



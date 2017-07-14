using PeopleApp.Multiselect;
using PeopleApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PeopleApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateSharingSpaceBPage : ContentPage
	{
        public CreateSharingSpaceBPage()
        {
            Resources = new ResourceDictionary();
            Resources.Add("TagValidatorFactory", new Func<string, object>(
                (arg) => (BindingContext as CreateSharingSpaceBViewModel)?.ValidateAndReturn(arg)));
            //BindingContext = new CreateSharingSpaceBViewModel();
            InitializeComponent();
        }

        private void SubmitButton_Clicked(object sender, EventArgs e)
        {

        }

        SelectMultipleBasePage<CheckItem> multiPage;

        private void HandleNavigation_Tapped(object sender, EventArgs e)
        {
            var items = new List<CheckItem>();
            items.Add(new CheckItem { Name = "Friends" });
            items.Add(new CheckItem { Name = "Colleagues" });
            items.Add(new CheckItem { Name = "Family" });

            if (multiPage == null)
                multiPage = new SelectMultipleBasePage<CheckItem>(items) { Title = "Check all that apply" };

            Navigation.PushAsync(multiPage);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (multiPage != null)
            {
                participantsCategories.Text = "";
                var answers = multiPage.GetSelection();
                if (answers.Count == 1)
                    participantsCategories.Text = answers.First<CheckItem>().Name;
                else
                {
                    foreach (var a in answers)
                    {
                        if (answers.Last<CheckItem>().Equals(a))
                            participantsCategories.Text += a.Name;
                        else
                            participantsCategories.Text += a.Name + ", ";

                    }
                }
            }
            else
            {
                participantsCategories.Text = "(none)";
            }
        }
    }
}
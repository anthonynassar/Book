using MvvmHelpers;
using PeopleApp.Helpers;
using PeopleApp.Models;
using PeopleApp.Multiselect;
using PeopleApp.Services;
using PeopleApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PeopleApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateSharingSpaceBPage : ContentPage
    {
        CreateSharingSpaceBViewModel vm;

        public CreateSharingSpaceBPage(SharingSpace sharingSpace, List<DimensionLocal> dimensions)
        {
            BindingContext = vm = new CreateSharingSpaceBViewModel(sharingSpace, dimensions, this);
            Resources = new ResourceDictionary();
            //Resources.Add("TagValidatorFactory", new Func<string, object>((arg) => (BindingContext as CreateSharingSpaceBViewModel)?.ValidateAndReturn(arg)));
            Resources.Add("TagValidatorFactory", new Func<string, object>(
                (arg) => vm?.ValidateAndReturn(arg)));
            InitializeComponent();
            //NavigationPage.SetHasNavigationBar(this, false);  // Hide nav bar
        }

        private async Task SubmitButton_ClickedAsync(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new EventOverviewPage(), Navigation.NavigationStack.FirstOrDefault());
            await Navigation.PopToRootAsync();
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
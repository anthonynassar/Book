using PeopleApp.Helpers;
using PeopleApp.Models;
using PeopleApp.Multiselect;
using PeopleApp.Services;
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
        ApiServices _apiServices = new ApiServices();

        public CreateSharingSpaceBPage()
        {
            Resources = new ResourceDictionary();
            Resources.Add("TagValidatorFactory", new Func<string, object>(
                (arg) => (BindingContext as CreateSharingSpaceBViewModel)?.ValidateAndReturn(arg)));
            //BindingContext = new CreateSharingSpaceBViewModel();
            InitializeComponent();
        }

        private async Task SubmitButton_ClickedAsync(object sender, EventArgs e)
        {
            //receive an object with everything list of dimension constraint

            // Post to SS table ( call SS controller)
            var userid = "08955621-0afc-4d7a-9830-7d4e02444c66";
            var sharingSpaceId = Utilities.NewGuid();
            SharingSpace sharingSpace = new SharingSpace
            {
                Id = sharingSpaceId,
                CreationDate = DateTime.Now,
                CreationLocation = "Anglet",
                Descriptor = "A manual sharing space",
                UserId = userid
            };
            var response = await _apiServices.PostSharingSpaceAsync(sharingSpace);
           

            // post dimensions
            Models.Constraint constraint1 = new Models.Constraint { Operator = "begin", Value = DateTime.Now.ToString() };
            Models.Constraint constraint2 = new Models.Constraint { Operator = "end", Value = DateTime.Now.AddDays(2).ToString() };
            Models.Constraint constraint3 = new Models.Constraint { Operator = "range", Value = "50"};
            var constraintList1 = new List<Models.Constraint> { constraint1, constraint2 };
            var constraintList2 = new List<Models.Constraint> { constraint3 };
            List<Dimension> dimensions = new List<Dimension>
            {
                new Dimension { Label = "Time", Interval = true, ConstraintList = constraintList1},
                new Dimension { Label = "Location", Interval = true, ConstraintList = constraintList2}
            };

            foreach (var dimension in dimensions)
            {
                dimension.Id = Utilities.NewGuid();
                await _apiServices.PostDimensionAsync(dimension);
                foreach (var constraint in dimension.ConstraintList)
                {
                    constraint.Id = Utilities.NewGuid();
                    await _apiServices.PostConstraintAsync(constraint);
                    await _apiServices.PostEventAsync(new Event { ConstraintId = constraint.Id , DimensionId = dimension.Id, });
                }
            }
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
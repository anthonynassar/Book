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
        public SharingSpace sharingSpace { get; set; }
        public List<DimensionLocal> dimensions { get; set; }

        public CreateSharingSpaceBPage()
        {
            
        }

        public CreateSharingSpaceBPage(SharingSpace sharingSpace, List<DimensionLocal> dimensions)
        {
            this.sharingSpace = sharingSpace;
            this.dimensions = dimensions;
            Resources = new ResourceDictionary();
            Resources.Add("TagValidatorFactory", new Func<string, object>(
                (arg) => (BindingContext as CreateSharingSpaceBViewModel)?.ValidateAndReturn(arg)));
            //BindingContext = new CreateSharingSpaceBViewModel();
            InitializeComponent();
        }

        private async Task SubmitButton_ClickedAsync(object sender, EventArgs e)
        {
            //receive an object with everything list of dimension constraint

            var response = await _apiServices.PostSharingSpaceAsync(this.sharingSpace, Settings.AccessToken);
           

            foreach (var dimension in this.dimensions)
            {
                var dimensionId = Utilities.NewGuid();
                await _apiServices.PostDimensionAsync(new Dimension { Id = dimensionId, Interval = dimension.Interval, Label = dimension.Label });
                foreach (var constraint in dimension.ConstraintList)
                {
                    constraint.Id = Utilities.NewGuid();
                    await _apiServices.PostConstraintAsync(constraint);
                    await _apiServices.PostEventAsync(new Event { ConstraintId = constraint.Id , DimensionId = dimensionId, SharingSpaceId = this.sharingSpace.Id});
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
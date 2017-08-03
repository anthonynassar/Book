using PeopleApp.Core;
using PeopleApp.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace PeopleApp.Views
{
    public class FullScreenImagePage : ContentPage
    {
        double currentScale = 1;
        double startScale = 1;
        double xOffset = 0;
        double yOffset = 0;

        double originalWidth;
        double originalHeight;

        double ScreenWidth;
        double ScreenHeight;

        PanGestureRecognizer panGesture;

        bool showEverything = false;
        StackLayout imageDescription;
        Button backButton;
        BoxView topBox;
        Image image;
        ContentView imageContainer;
        Label indexLabel;
        //Label xLabel, yLabel, transXLabel, transYLabel, widthLabel, heightLabel, scaleLabel, screenWidthLabel, screenHeightLabel;
        AbsoluteLayout absoluteLayout;

        protected override void OnAppearing()
        {
            ShowEverything = true;
            base.OnAppearing();
        }

        protected override bool OnBackButtonPressed()
        {
            //App.NavPage.BarTextColor = Color.Black; // turn the status bar back to black
            return base.OnBackButtonPressed();
        }

        public bool ShowEverything
        {
            set
            {
                showEverything = value;
                backButton.IsVisible = showEverything;
                imageDescription.IsVisible = showEverything;
                topBox.IsVisible = showEverything;
                indexLabel.IsVisible = showEverything;

                if (!showEverything)
                {
                    // hide the status bar by turning it black
                    MenuPage.NavPage.BarTextColor = Color.Black;
                    imageContainer.GestureRecognizers.Add(panGesture);
                }
                else
                {
                    // show the status bar by turning it white
                    MenuPage.NavPage.BarTextColor = Color.White;
                    imageContainer.GestureRecognizers.Remove(panGesture);
                }
            }
            get
            {
                return showEverything;
            }
        }

        public FullScreenImagePage(String ImageName, string DescriptionText, int index, int count)
        {
            NavigationPage.SetHasNavigationBar(this, false);

            image = new ZoomImage
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Aspect = Aspect.AspectFill,
                Source = ImageName
            };

            imageContainer = new ContentView
            {
                Content = image
            };

            //var tapGesture = new TapGestureRecognizer();
            //tapGesture.Tapped += OnTapped;
            //imageContainer.GestureRecognizers.Add(tapGesture);

            //var pinchGesture = new PinchGestureRecognizer();
            //pinchGesture.PinchUpdated += OnPinchUpdated;
            //imageContainer.GestureRecognizers.Add(pinchGesture);

            //panGesture = new PanGestureRecognizer();
            //panGesture.PanUpdated += OnPanUpdated;
            //imageContainer.GestureRecognizers.Add(panGesture);

            absoluteLayout = new AbsoluteLayout
            {
                BackgroundColor = MyAppStyle.blackColor,
            };

            var label = new Label
            {
                Text = DescriptionText,
                TextColor = MyAppStyle.whiteColor,
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
            };
            var separator = new BoxView() { HeightRequest = 1, BackgroundColor = MyAppStyle.whiteColor };

            imageDescription = new StackLayout
            {
                Padding = new Thickness(20),
                HorizontalOptions = LayoutOptions.Fill,
                Orientation = StackOrientation.Vertical,
                Children = { label, separator }
            };

            backButton = new Button { Text = "Back", WidthRequest = 80, HeightRequest = 40, BackgroundColor = MyAppStyle.whiteColor, TextColor = MyAppStyle.blackColor, FontAttributes = FontAttributes.Bold };
            backButton.Clicked += (object sender, EventArgs e) => { OnBackButtonPressed(); Navigation.PopAsync(); };

            indexLabel = new Label
            {
                Text = index.ToString() + " of " + count.ToString(),
                //Text = (index + 1).ToString() + " of " + count.ToString(),
                TextColor = MyAppStyle.whiteColor,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center
            };

            AbsoluteLayout.SetLayoutFlags(imageContainer, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(imageContainer, new Rectangle(0f, 0f, 1f, 1f));
            absoluteLayout.Children.Add(imageContainer);

            AbsoluteLayout.SetLayoutFlags(imageDescription, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);
            AbsoluteLayout.SetLayoutBounds(imageDescription, new Rectangle(0f, 1f, 1f, AbsoluteLayout.AutoSize));
            absoluteLayout.Children.Add(imageDescription);

            topBox = new BoxView { Color = MyAppStyle.blackColor, Opacity = 0.5 };
            AbsoluteLayout.SetLayoutFlags(topBox, AbsoluteLayoutFlags.WidthProportional);
            AbsoluteLayout.SetLayoutBounds(topBox, new Rectangle(0f, 0f, 1f, 50f));
            absoluteLayout.Children.Add(topBox);

            AbsoluteLayout.SetLayoutFlags(backButton, AbsoluteLayoutFlags.None);
            AbsoluteLayout.SetLayoutBounds(backButton, new Rectangle(0f, 10f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            absoluteLayout.Children.Add(backButton);

            AbsoluteLayout.SetLayoutFlags(indexLabel, AbsoluteLayoutFlags.XProportional);
            AbsoluteLayout.SetLayoutBounds(indexLabel, new Rectangle(.5f, 20f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            absoluteLayout.Children.Add(indexLabel);

            Content = absoluteLayout;
        }


        //protected override void OnSizeAllocated(double width, double height)
        //{
        //    base.OnSizeAllocated(width, height); //must be called

        //    if (width != -1 && (ScreenWidth != width || ScreenHeight != height))
        //    {
        //        imageContainer.Content.TranslateTo(0, 0, 0, Easing.Linear);
        //        imageContainer.Content.Scale = 1;
        //        BackToStory.WidthRequest = width;
        //        //reset imageContainer 
        //        imageContainer.Content = null;
        //        absoluteLayout.Children.Remove(imageContainer);

        //        imageContainer.Content = ImageMain;
        //        AbsoluteLayout.SetLayoutFlags(imageContainer, AbsoluteLayoutFlags.All);
        //        AbsoluteLayout.SetLayoutBounds(imageContainer, new Rectangle(0f, 0f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        //        absoluteLayout.Children.Add(imageContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
        //        //end reset imageContainer
        //        originalWidth = initialLoad ? ImageWidth / 320 : imageContainer.Content.Width / imageContainer.Content.Scale;

        //        var normalizedHeight = ImageHeight / (ImageWidth / 320);

        //        originalHeight = initialLoad ? normalizedHeight : (imageContainer.Content.Height / imageContainer.Content.Scale);

        //        ScreenWidth = width;
        //        ScreenHeight = height;

        //        xOffset = imageContainer.Content.TranslationX;
        //        yOffset = imageContainer.Content.TranslationY;

        //        currentScale = imageContainer.Content.Scale;

        //        if (initialLoad)
        //            initialLoad = false;
        //    }
        //}
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height); //must be called

            if (ScreenWidth != width || ScreenHeight != height)
            {

                absoluteLayout.ForceLayout();

                originalWidth = imageContainer.Content.Width / imageContainer.Content.Scale;
                originalHeight = imageContainer.Content.Height / imageContainer.Content.Scale;

                ScreenWidth = width;
                ScreenHeight = height;

                xOffset = imageContainer.Content.TranslationX;
                yOffset = imageContainer.Content.TranslationY;

                currentScale = imageContainer.Content.Scale;
            }
        }

        void OnTapped(object sender, EventArgs e)
        {
            try
            {
                ShowEverything = !ShowEverything;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                Debug.WriteLine("Error: " + ex);
                ShowEverything = !ShowEverything;
            }
        }

        void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            var s = (ContentView)sender;

            // do not allow pan if the image is in its intial size
            if (currentScale == 1)
                return;

            switch (e.StatusType)
            {
                case GestureStatus.Running:

                    double xTrans = xOffset + e.TotalX, yTrans = yOffset + e.TotalY;
                    // do not allow verical scorlling unless the image size is bigger than the screen
                    s.Content.TranslateTo(xTrans, yTrans, 0, Easing.Linear);

                    break;

                case GestureStatus.Completed:
                    // Store the translation applied during the pan
                    xOffset = s.Content.TranslationX;
                    yOffset = s.Content.TranslationY;

                    // center the image if the width of the image is smaller than the screen width
                    if (originalWidth * currentScale < ScreenWidth && ScreenWidth > ScreenHeight)
                        xOffset = (ScreenWidth - originalWidth * currentScale) / 2 - s.Content.X;
                    else
                        xOffset = Math.Max(Math.Min(0, xOffset), -Math.Abs(originalWidth * currentScale - ScreenWidth));

                    // center the image if the height of the image is smaller than the screen height
                    if (originalHeight * currentScale < ScreenHeight && ScreenHeight > ScreenWidth)
                        yOffset = (ScreenHeight - originalHeight * currentScale) / 2 - s.Content.Y;
                    else
                        yOffset = Math.Max(Math.Min((originalHeight - ScreenHeight) / 2, yOffset), -Math.Abs(originalHeight * currentScale - ScreenHeight - (originalHeight - ScreenHeight) / 2));

                    // bounce the image back to inside the bounds
                    s.Content.TranslateTo(xOffset, yOffset, 500, Easing.BounceOut);
                    break;
            }
        }

        void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {

        }

        //void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        //{
        //    var s = (ContentView)sender;

        //    if (e.Status == GestureStatus.Started)
        //    {
        //        // Store the current scale factor applied to the wrapped user interface element,
        //        // and zero the components for the center point of the translate transform.
        //        startScale = s.Content.Scale;
        //        s.Content.AnchorX = 0;
        //        s.Content.AnchorY = 0;
        //    }
        //    if (e.Status == GestureStatus.Running)
        //    {

        //        // Calculate the scale factor to be applied.
        //        currentScale += (e.Scale - 1) * startScale;
        //        currentScale = Math.Max(1, currentScale);
        //        currentScale = Math.Min(currentScale, 5);

        //        //scaleLabel.Text = "Scale: " + currentScale.ToString ();

        //        if (currentScale == 1)
        //            ShowEverything = true;
        //        else
        //            ShowEverything = false;

        //        // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
        //        // so get the X pixel coordinate.
        //        double renderedX = s.Content.X + xOffset;
        //        double deltaX = renderedX / ScreenWidth;
        //        double deltaWidth = ScreenWidth / (s.Content.Width * startScale);
        //        double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

        //        // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
        //        // so get the Y pixel coordinate.
        //        double renderedY = s.Content.Y + yOffset;
        //        double deltaY = renderedY / ScreenHeight;
        //        double deltaHeight = ScreenHeight / (s.Content.Height * startScale);
        //        double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

        //        // Calculate the transformed element pixel coordinates.
        //        double targetX = xOffset - (originX * s.Content.Width) * (currentScale - startScale);
        //        double targetY = yOffset - (originY * s.Content.Height) * (currentScale - startScale);

        //        // Apply translation based on the change in origin.
        //        var transX = targetX.Clamp(-s.Content.Width * (currentScale - 1), 0);
        //        var transY = targetY.Clamp(-s.Content.Height * (currentScale - 1), 0);
        //        s.Content.TranslateTo(transX, transY, 0, Easing.Linear);

        //        // Apply scale factor.
        //        s.Content.Scale = currentScale;
        //    }
        //    if (e.Status == GestureStatus.Completed)
        //    {
        //        // Store the translation applied during the pan
        //        xOffset = s.Content.TranslationX;
        //        yOffset = s.Content.TranslationY;

        //        // center the image if the width of the image is smaller than the screen width
        //        if (originalWidth * currentScale < ScreenWidth && ScreenWidth > ScreenHeight)
        //            xOffset = (ScreenWidth - originalWidth * currentScale) / 2 - s.Content.X;
        //        else
        //            xOffset = Math.Max(Math.Min(0, xOffset), -Math.Abs(originalWidth * currentScale - ScreenWidth));

        //        // center the image if the height of the image is smaller than the screen height
        //        if (originalHeight * currentScale < ScreenHeight && ScreenHeight > ScreenWidth)
        //            yOffset = (ScreenHeight - originalHeight * currentScale) / 2 - s.Content.Y;
        //        else
        //            yOffset = Math.Max(Math.Min((originalHeight - ScreenHeight) / 2, yOffset), -Math.Abs(originalHeight * currentScale - ScreenHeight - (originalHeight - ScreenHeight) / 2));

        //        // bounce the image back to inside the bounds
        //        s.Content.TranslateTo(xOffset, yOffset, 500, Easing.BounceOut);
        //    }
        //}
    }
}
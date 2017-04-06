using DementiaHelper.Resx;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DementiaHelper.Pages
{
    public partial class CreateAccountPage : ContentPage
    {
        ObservableCollection<string> roleCollection = new ObservableCollection<string>
        {
            {AppResources.DementiaRole}, {AppResources.CaregiverRole},{AppResources.RelativesRole}

        };

        private double width;
        private double height;
        public CreateAccountPage()
        {
            InitializeComponent();
            foreach (string role in roleCollection)
            {
                rolePicker.Items.Add(role);
            }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;
                if (width > height)
                {
                    outerStack.Orientation = StackOrientation.Horizontal;
                    secondInnerStack.VerticalOptions = LayoutOptions.StartAndExpand;
                    imageElderlyHands.HeightRequest = 120;
                }
                else
                {
                    outerStack.Orientation = StackOrientation.Vertical;
                    secondInnerStack.VerticalOptions = LayoutOptions.CenterAndExpand;
                    imageElderlyHands.HeightRequest = 200;
                }
            }
        }

        private void rolePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rolePicker.SelectedIndex == -1)
            {
                
            }
            else
            {
                string role = rolePicker.Items[rolePicker.SelectedIndex];
                if(AppResources.DementiaRole == role)
                {

                }else if(AppResources.CaregiverRole == role)
                {

                }else if(AppResources.RelativesRole == role)
                {

                }
                  
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DementiaHelper.PageModels;
using DementiaHelper.Resx;
using DementiaHelper.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DementiaHelper.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShoppingListPage : ContentPage
    {
        public ShoppingListPage()
        {
            InitializeComponent();
            if (Device.Idiom == TargetIdiom.Phone)
            {
                ShoppingListViewPhone.ItemSelected += (sender, e) =>
                {
                    ((ListView)sender).SelectedItem = null;
                };
            }
            else if(Device.Idiom==TargetIdiom.Tablet){
                ShoppingListViewTablet.ItemSelected += (sender, e) => {
                    ((ListView)sender).SelectedItem = null;
                };
            }
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                var result = await this.DisplayAlert(AppResources.Warning + "!", AppResources.AreYouSureThatYouWantToCloseThisApplication, AppResources.YesText, AppResources.NoText);
                if (result)
                {
                    INativeService nativeHelper = DependencyService.Get<INativeService>();
                    if (nativeHelper != null)
                    {
                        nativeHelper.CloseApp();
                    }
                }
            });
            return true;
        }
    }
}

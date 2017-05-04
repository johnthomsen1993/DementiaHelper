using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DementiaHelper.PageModels;
using Xamarin.Forms;

namespace DementiaHelper.Pages
{
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
    }
}

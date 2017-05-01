using DementiaHelper.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DementiaHelper.PageModels;
using DementiaHelper.Renders;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DementiaHelper.Model;

namespace DementiaHelper.Pages
{

    public partial class ChatPage
    {
        public ChatPage()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var last = MessageList.ItemsSource.Cast<object>().LastOrDefault();
            if (last != null)
            {
                MessageList.ScrollTo(last, ScrollToPosition.MakeVisible, true);
            }
            MessagingCenter.Subscribe<ChatPageModel>(this, "New Messages", (sender) => {
                var lastItem = MessageList.ItemsSource.Cast<object>().LastOrDefault();
                if (lastItem != null)
                {
                    MessageList.ScrollTo(lastItem, ScrollToPosition.MakeVisible, true);
                }
            });
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<ChatPageModel>(this, "New Messages");
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                var result = await this.DisplayAlert("Alert!", "Do you really want to exit?", "Yes", "No");
                if (result)
                {
                    INativeService nativeHelper = null;
                    nativeHelper = DependencyService.Get<INativeService>();
                    if (nativeHelper != null)
                    {
                        nativeHelper.CloseApp();
                    }
                } // or anything else
            });
            return true;
        }

    }
}

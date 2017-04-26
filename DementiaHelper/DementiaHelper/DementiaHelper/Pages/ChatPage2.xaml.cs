﻿using DementiaHelper.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DementiaHelper.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage2 : ContentPage
    {
        public ChatPage2()
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

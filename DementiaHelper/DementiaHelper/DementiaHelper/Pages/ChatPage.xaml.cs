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

namespace DementiaHelper.Pages
{

    public partial class ChatPage
    {
        private ChatPageModel chatPageModel;
        public ChatPage()
        {
            InitializeComponent();
            chatPageModel = new ChatPageModel();

            //chatPageModel.ScrollToLast += ScrollToLast; // Subscribe to event
        }

        

        //public void ScrollToLast(object obj, string str)
        //{
        //    ObservableCollection<ChatMassagePageModel> chatMassage = MessageList.ItemsSource as ObservableCollection<ChatMassagePageModel>;
        //    if (chatMassage != null)
        //    {
        //        var target = chatMassage.Count - 1;
        //        MessageList.ScrollTo(target, ScrollToPosition.Start, true);
        //    }
            
        //}



        //protected override bool OnBackButtonPressed()
        //{
        //    Device.BeginInvokeOnMainThread(async () => {
        //        var result = await this.DisplayAlert("Alert!", "Do you really want to exit?", "Yes", "No");
        //        if (result)
        //        {
        //            INativeService nativeHelper = null;
        //            nativeHelper = DependencyService.Get<INativeService>();
        //            if (nativeHelper != null)
        //            {
        //                nativeHelper.CloseApp();
        //            }
        //        } // or anything else
        //    });

        //    return true;
        //}
    }
}

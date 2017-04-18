using DementiaHelper.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DementiaHelper.Renders;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DementiaHelper.Pages
{

    public partial class ChatPage 
    {
        public ChatPage()
        {
            InitializeComponent();
            
            MessageList.ItemTemplate = new DataTemplate(CreateMessageCell);
        }

        

        private Cell CreateMessageCell()
        {
            var timestampLabel = new Label();
            timestampLabel.SetBinding(Label.TextProperty, new Binding("Timestamp", stringFormat: "[{0:HH:mm}]"));
            timestampLabel.TextColor = Color.Silver;
            timestampLabel.Font = Font.SystemFontOfSize(14);

            var authorLabel = new Label();
            authorLabel.SetBinding(Label.TextProperty, new Binding("ChatMessage.Name", stringFormat: "{0}: "));
            authorLabel.TextColor = Device.OnPlatform(Color.Blue, Color.Yellow, Color.Yellow);
            authorLabel.Font = Font.SystemFontOfSize(14);

            var messageLabel = new Label();
            messageLabel.SetBinding(Label.TextProperty, new Binding("ChatMessage.Message"));
            messageLabel.Font = Font.SystemFontOfSize(14);

            var stack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { authorLabel, messageLabel }
            };

            if (Device.Idiom == TargetIdiom.Tablet)
            {
                stack.Children.Insert(0, timestampLabel);
            }

            var view = new MessageViewCell
            {
                View = stack
            };
            return view;
        }

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

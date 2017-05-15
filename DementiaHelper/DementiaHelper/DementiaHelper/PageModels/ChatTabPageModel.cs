using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DementiaHelper.Extensions;
using DementiaHelper.Pages;
using DementiaHelper.Renders.CustomControlRenders;
using DementiaHelper.Resx;
using FreshMvvm;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
    class ChatTabPageModel : FreshMvvm.FreshBasePageModel
    {
        public ChatTabPageModel()
        {
            
        }

        void OnButtonClicked(object sender, EventArgs e)
        {
            CoreMethods.PopModalNavigationService();
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            var tabbedNavigation = new FreshTabbedNavigationContainer();
            tabbedNavigation.AddTab<ChatPageModel>("Chat1", null);
            tabbedNavigation.AddTab<ChatPageModel>("Chat2", null);
            CoreMethods.PushNewNavigationServiceModal(tabbedNavigation);

            CustomButtonRender button = new CustomButtonRender
            {
                Text = AppResources.General_Back,
                Style = App.Current.Resources["ButtonStyle"] as Style
            };

            button.Clicked += OnButtonClicked;
        }

        #region Back Command

        Command backCommand;

        /// <summary>
        /// Command to Send Message
        /// </summary>
        public Command BackCommand
        {
            get
            {
                return backCommand ??
                    (backCommand = new Command(ExecuteBackCommand));
            }
        }

        async void ExecuteBackCommand()
        {
            await CoreMethods.PopModalNavigationService();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DementiaHelper.Extensions;
using DementiaHelper.Model;
using DementiaHelper.Services;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace DementiaHelper.ViewModel
{
    class EditAccountInformationViewModel : BaseViewModel
    {
        HttpClient h = new HttpClient();
        public User User { get; set; }
        public ICommand SaveCommand { get; protected set; }
        public ICommand CancelCommand { get; protected set; }

        public EditAccountInformationViewModel(User user)
        {
            User = user;
            this.SaveCommand = new Command(async () => await Save());
            this.CancelCommand = new Command(async () => await Cancel());
        }
        async Task Save()
        {
            var values = new Dictionary<string, string>
            {
                {"Id", User.Id},
                {"FirstName", User.FirstName},
                {"LaseName", User.LastName},
                {"Email", User.Email},
                {"Description", User.Description}
            };
            var content = new FormUrlEncodedContent(values);
            var result = h.PostAsync(new Uri("http://dementiahelperwebapi20170302110209.azurewebsites.net/api/values/save"), content).Result;
            var response = result.Content.ReadAsStringAsync();

            await App.Current.MainPage.DisplayAlert(response.Result, "Test", "OK");

            await NavigationService.PopModalAsync();
        }
        async Task Cancel()
        {
            await NavigationService.PopModalAsync();
        }

        
    }
}

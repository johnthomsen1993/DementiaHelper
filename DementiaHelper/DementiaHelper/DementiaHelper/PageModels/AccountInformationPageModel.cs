using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DementiaHelper.Extensions;
using DementiaHelper.Model;
using DementiaHelper.Services;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
    public class AccountInformationPageModel : FreshMvvm.FreshBasePageModel
    {
        HttpClient h = new HttpClient();
        public UserInformation User { get; set; }
        public bool EditButton { get; set; }
        public ICommand GoToEditAccountInformationCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }


        public AddToShoppingList AddItem { get; set; }

        public AccountInformationPageModel()
        {
            AddItem = new AddToShoppingList();
            var values = new Dictionary<string, string>
            {
                {"Email", "test@gmail.com"}
            };
            var content = new FormUrlEncodedContent(values);
            var result = h.PostAsync(new Uri("http://dementiahelper.azurewebsites.net/api/values/getspecific"), content).Result;
            //var result = h.PostAsync(new Uri("http://localhost:29342//api/values/save"), content).Result;

            var response = result.Content.ReadAsStringAsync();

            User = new UserInformation() {FirstName = "Claus", Email = "test@email.com", Description = "This is a default in the viewmodel"};

            EditButton = User.Id == "test@email.com";
            GoToEditAccountInformationCommand = new Command(async () => await GoToEditAccountInformation());
            BackCommand = new Command(async () => await Back());

            if (Device.Idiom == TargetIdiom.Phone)
            {
                
            }
            

        }

        async Task GoToEditAccountInformation()
        {
            await CoreMethods.PushPageModel<EditAccountInformationPageModel>(User);
        }
        async Task Back()
        {
            await CoreMethods.PopPageModel();
        }

    }
}

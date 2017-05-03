
using DementiaHelper.Model;
using DementiaHelper.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PropertyChanged;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
    [ImplementPropertyChanged]
    public class ContactListPageModel : FreshMvvm.FreshBasePageModel
    {
        public const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/account/contactlist/";
        public ObservableCollection<Contact> ApplicationUserContactCollection { get; set; }
        public ObservableCollection<Contact> ContactCollection { get; set; }
        public Command<Contact> CallContactCommand { get; set; }
        ApplicationUser User = (ApplicationUser) App.Current.Properties["ApplicationUser"];
        public ContactListPageModel()
        {
            _SearchText = "";
            ApplicationUserContactCollection = new ObservableCollection<Contact>() { }; ; ;
            ContactCollection = new ObservableCollection<Contact>() { new Contact() { } };
            CallContactCommand = new Command<Contact>(CallNumber);

        }
       

        private async Task<ObservableCollection<Contact>> GetApplicationUserContactCollection(int? id)
        {
            if (id == null) { return new ObservableCollection<Contact>() { }; }
            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>() { { "CitizenId", id } });
                    var result = await client.GetStringAsync(new Uri(URI_BASE + encoded));
                    var decoded = JWTService.Decode(result);
                    return MapToContactsCollection(decoded);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            Device.BeginInvokeOnMainThread(async () =>
            {
                ApplicationUserContactCollection = await GetApplicationUserContactCollection(User.CitizenId);
                FilterContacts();
            });
        }

        private ObservableCollection<Contact> MapToContactsCollection(IDictionary<string, object> dict)
        {
            var tempCaregiversCitizenCollection = new ObservableCollection<Contact>();
            var list = dict.SingleOrDefault(x => x.Key.Equals("contactList")).Value as IEnumerable<object>;
            
            foreach (var obj in list)
            {
                var jsonContainer = obj as JContainer;
                tempCaregiversCitizenCollection.Add(item: new Contact()
                {
                    Phone = jsonContainer.SelectToken("ApplicationUser").SelectToken("Phone").ToObject<int>(),
                    Name = jsonContainer.SelectToken("ApplicationUser").SelectToken("FirstName").ToObject<string>() + " " + jsonContainer.SelectToken("ApplicationUser").SelectToken("LastName").ToObject<string>()
                });
            }

            var jContainer = dict["caregiverCenter"] as JContainer;
            tempCaregiversCitizenCollection.Add(new Contact()
            {
                Phone = jContainer.SelectToken("Phone").ToObject<int>(),
                Name = jContainer.SelectToken("Name").ToObject<string>()
            });
            return tempCaregiversCitizenCollection;
        }
        #region Filter

        private string _SearchText;
        public string SearchText
        {
            get
            {
                return _SearchText;
            }
            set
            {
                if (_SearchText != value)
                {
                    _SearchText = value;
                    RaisePropertyChanged("SearchText");
                    this.FilterContacts();

                }
            }
        }

        #endregion
        private void FilterContacts()
        {
            ContactCollection.Clear();
            foreach (var Citizen in ApplicationUserContactCollection)
            {
                if (_SearchText == "")
                {
                    if (ContactCollection == null)
                    {
                        ContactCollection = new ObservableCollection<Contact>(ApplicationUserContactCollection);
                    }
                    else
                    {
                        ContactCollection.Add(Citizen);
                    }

                }
                else if (Citizen.Name.IndexOf(_SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    ContactCollection.Add(Citizen);
                }
            }
        }


        public void CallNumber(Contact contact)
        {
            Device.OpenUri(new Uri("tel:"+contact.Phone));
        }
    }
}
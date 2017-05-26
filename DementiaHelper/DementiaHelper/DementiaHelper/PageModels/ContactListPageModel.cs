
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
        public ObservableCollection<Contact> ApplicationUserContactCollection { get; set; }
        public ObservableCollection<Contact> ContactCollection { get; set; }
        public Command<Contact> CallContactCommand { get; set; }
        public ContactListPageModel()
        {
            _SearchText = "";
            ApplicationUserContactCollection = new ObservableCollection<Contact>() { }; ; ;
            ContactCollection = new ObservableCollection<Contact>() { new Contact() { } };
            CallContactCommand = new Command<Contact>(CallNumber);
            
        }
        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            Device.BeginInvokeOnMainThread(async () =>
            {
                ApplicationUserContactCollection = await ModelAccessor.Instance.AccountController.GetApplicationUserContactCollection(((ApplicationUser)App.Current.Properties["ApplicationUser"]).CitizenId);
                FilterContacts();
            });
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
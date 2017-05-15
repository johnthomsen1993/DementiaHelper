using DementiaHelper.Model;
using DementiaHelper.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DementiaHelper.Resx;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
   public class ChoosePrimaryRelativePageModel : FreshMvvm.FreshBasePageModel
    {

        public const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/account/";
        public ObservableCollection<Relative> CitizenRelatveCollection { get; set; }
        public ObservableCollection<Relative> RelativeCollection { get; set; }
        public Relative ChoosenRelative{ get; set; }
        public bool RelativeChoosen { get; set; }
        public ObservableCollection<Relative> RelativeList { get; set; }
        public string NewPrimaryRelative { get; set; }
        public ICommand ChoosePrimaryRelativeCommand { get; protected set; }
        public ChoosePrimaryRelativePageModel()
        {
            RelativeChoosen = false;
            _SearchText = "";
            ChoosePrimaryRelativeCommand = new Command<Relative>(ChoosePrimaryRelative);
            //new Command(async () => await ChoosePrimaryRelative(ChoosenRelative));
            //new Command(async (obj) => await ChoosePrimaryRelative((Relative)obj)); 
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            ApplicationUser User = (ApplicationUser)App.Current.Properties["ApplicationUser"];
            base.ViewIsAppearing(sender, e);
            Device.BeginInvokeOnMainThread(async () =>
            {
                RelativeList = await GetRelativeList();
                RelativeChoosen = false;
                if (RelativeList != null)
                {
                    CitizenRelatveCollection = RelativeList;
                    foreach (var relative in CitizenRelatveCollection)
                    {
                        if (relative.PrimaryRelative)
                        {
                            ChoosenRelative = relative;
                            RelativeChoosen = true;
                            break;
                        }
                    }

                    RelativeCollection = new ObservableCollection<Relative>();
                    this.FilterRelatives();
                }
            });
        }

       private async Task<ObservableCollection<Relative>> GetRelativeList()
       {
            var payload = new Dictionary<string, object>()
            {
                { "CitizenId", ((ApplicationUser)App.Current.Properties["ApplicationUser"]).ApplicationUserId },
            };
           
           using (var client = new HttpClient())
           {
                var encoded = JWTService.Encode(payload);
                var result = await client.GetStringAsync(new Uri(URI_BASE + "contactlist/" + encoded));
                var decoded = JWTService.Decode(result);
                return decoded.ContainsKey("contactList") ? MapToRelativeList(decoded["contactList"] as IList): null;
            }
       }
        private static ObservableCollection<Relative> MapToRelativeList(IList RelativeList)
        {
            var tempRelativeList = new ObservableCollection<Relative>();
            foreach (var relative in RelativeList)
            {
                var jsonContainer = relative as JContainer;
                tempRelativeList.Add(new Relative()
                {
                    RelativeId = jsonContainer.SelectToken("RelativeId").ToObject<int>(),
                    FirstName = jsonContainer.SelectToken("ApplicationUser").SelectToken("FirstName").ToObject<string>(),
                    LastName = jsonContainer.SelectToken("ApplicationUser").SelectToken("LastName").ToObject<string>(),
                    PrimaryRelative = jsonContainer.SelectToken("PrimaryRelative").ToObject<bool>()
                });
            }
            return tempRelativeList;
        }

       private async void ChoosePrimaryRelative(Relative relative)
       {
           if (relative != null)
           {
               using (var client = new HttpClient())
               {
                   var encoded = JWTService.Encode(new Dictionary<string, object>()
                   {
                       {"CitizenId", ((ApplicationUser) App.Current.Properties["ApplicationUser"]).ApplicationUserId},
                       {"NewPrimaryRelative", relative.RelativeId}
                   });

                   var values = new Dictionary<string, string> {{"token", encoded}};
                   var content = new FormUrlEncodedContent(values);
                   var result = await client.PutAsync(new Uri(URI_BASE + "primaryrelative/"), content);
                   var decoded = JWTService.Decode(await result.Content.ReadAsStringAsync());

                   await CoreMethods.PopPageModel();
                   
                }
           }
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
                    this.FilterRelatives();

                }
            }
        }
        #endregion

        private void FilterRelatives()
        {
            RelativeCollection.Clear();
            foreach (var relative in CitizenRelatveCollection)
            {
                if (_SearchText == "")
                {
                    if (RelativeCollection == null)
                    {
                        RelativeCollection = new ObservableCollection<Relative>(CitizenRelatveCollection);
                        RelativeCollection.Add(relative);
                    }
                    else
                    {
                        RelativeCollection.Add(relative);
                    }

                }
                else if (relative.FirstName.IndexOf(_SearchText, StringComparison.OrdinalIgnoreCase) >= 0 || relative.LastName.IndexOf(_SearchText, StringComparison.OrdinalIgnoreCase) >= 0 || (relative.FirstName + " " + relative.LastName).IndexOf(_SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    RelativeCollection.Add(relative);
                }
            }
        }
    }
}

using DementiaHelper.Model;
using DementiaHelper.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
namespace DementiaHelper.PageModels
{
    
    public class ChooseCitizenPageModel : FreshMvvm.FreshBasePageModel
    {
        public const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/values/caregiver/";
        public ObservableCollection<Citizen> CaregiversCitizenCollection { get; set; }
        public ObservableCollection<Citizen> CitizenCollection { get; set; }
        public Command<Citizen> CitizenTappedCommand { get; set; }
        public ChooseCitizenPageModel()
        {
            _SearchText = "";
            var User = (ApplicationUser)App.Current.Properties["ApplicationUser"];
            Device.BeginInvokeOnMainThread(async () =>
            {
                CaregiversCitizenCollection = await GetCaregiverCitizenCollection(User.CitizenId);
            });
            CaregiversCitizenCollection = new ObservableCollection<Citizen>() {  }; ;
            CitizenCollection = new ObservableCollection<Citizen>();
            this.FilterCitizens();
            CitizenTappedCommand = new Command<Citizen>(ChooseCitizen);
            
        }

        private async Task<ObservableCollection<Citizen>> GetCaregiverCitizenCollection(int? id)
        {
            if (id == null) { return new ObservableCollection<Citizen>() { new Citizen() { FirstName = "John", LastName = "Thomsen", CitizenId = "Greetings" } }; }
            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>() { { "caregiverId", id } });
                    var result = await client.GetStringAsync(new Uri(URI_BASE + encoded));
                    var decoded = JWTService.Decode(result);
                    return decoded.ContainsKey("List") ? null : MapToCaregiversCitizenCollection(decoded);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }


        private  ObservableCollection<Citizen> MapToCaregiversCitizenCollection(IDictionary<string, object> dict)
        {
            var tempCaregiversCitizenCollection = new ObservableCollection<Citizen>();
            var list = dict.SingleOrDefault(x => x.Key.Equals("CaregiverCitizenCollection")).Value as IEnumerable<object>;
            //var list = dict.Where(x => x.Key.Contains("ShoppingList")).Select(x => x.Value).ToList().FirstOrDefault() as IEnumerable<object>;
            foreach (var obj in list)
            {
                var jsonContainer = obj as JContainer;
                tempCaregiversCitizenCollection.Add(new Citizen()
                {
                    CitizenId = jsonContainer.SelectToken("").ToString(),
                    FirstName = jsonContainer.SelectToken("").ToString(),
                    LastName = jsonContainer.SelectToken("").ToString()
                });
            }
            return tempCaregiversCitizenCollection;
        }
        #region Filter

        public override void Init(object initData)
        {
            base.Init(initData);
        }

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
                    this.FilterCitizens();
                    
                }
            }
        }

        #endregion
        private void FilterCitizens()
        {
            CitizenCollection.Clear();
           foreach (var Citizen in CaregiversCitizenCollection)
            {
                if (_SearchText==""){
                    if (CitizenCollection == null){
                        CitizenCollection = new ObservableCollection<Citizen>(CaregiversCitizenCollection);
                    }else {
                        CitizenCollection.Add(Citizen);
                    }
                    
                }else if (Citizen.FirstName.IndexOf(_SearchText, StringComparison.OrdinalIgnoreCase) >= 0 || Citizen.LastName.IndexOf(_SearchText, StringComparison.OrdinalIgnoreCase) >= 0 || (Citizen.FirstName+" "+ Citizen.LastName).IndexOf(_SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    CitizenCollection.Add(Citizen);
                }
            }
        }
       

        public void ChooseCitizen(Citizen citizen)
        {
           
        }
    }
}

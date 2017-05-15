using DementiaHelper.Model;
using DementiaHelper.Services;
using Newtonsoft.Json.Linq;
using PropertyChanged;
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
    [ImplementPropertyChanged]
    public class ChooseCitizenPageModel : FreshMvvm.FreshBasePageModel
    {
        public const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/values/caregiver/";
        public ObservableCollection<Citizen> CaregiversCitizenCollection { get; set; }
        public ObservableCollection<Citizen> CitizenCollection { get; set; }
        public Citizen ChoosenCitizen { get; set; }
        public bool CitizenChoosen { get; set; }
        public Command<Citizen> CitizenTappedCommand { get; set; }

        public ChooseCitizenPageModel()
        {
            CitizenChoosen = false;
            _SearchText = "";
            CitizenTappedCommand = new Command<Citizen>(ChooseCitizen);
            
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            CitizenChoosen = false;
            if (((ApplicationUser)App.Current.Properties["ApplicationUser"])?.CitizenList != null)
            {
                CaregiversCitizenCollection = ((ApplicationUser)App.Current.Properties["ApplicationUser"]).CitizenList;
                if (((ApplicationUser)App.Current.Properties["ApplicationUser"]).CitizenId != null)
                {
                    foreach (var Citizen in CaregiversCitizenCollection)
                    {
                        if (((ApplicationUser)App.Current.Properties["ApplicationUser"]).CitizenId == Citizen.CitizenId)
                        {
                            ChoosenCitizen = new Citizen() { FirstName = Citizen.FirstName, LastName = Citizen.LastName, CitizenId = Citizen.CitizenId };
                            CitizenChoosen = true;
                            break;
                        }
                    }
                }

                // CaregiversCitizenCollection = new ObservableCollection<Citizen>() {  }; ;
                CitizenCollection = new ObservableCollection<Citizen>();
                this.FilterCitizens();
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
                    
                }
                else if (Citizen.FirstName.IndexOf(_SearchText, StringComparison.OrdinalIgnoreCase) >= 0 || Citizen.LastName.IndexOf(_SearchText, StringComparison.OrdinalIgnoreCase) >= 0 || (Citizen.FirstName+" "+ Citizen.LastName).IndexOf(_SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    CitizenCollection.Add(Citizen);
                }
            }
        }

        public void ChooseCitizen(Citizen citizen)
        {
            ((ApplicationUser)App.Current.Properties["ApplicationUser"]).CitizenId = citizen.CitizenId;
       //     App.Current.Properties["ApplicationUser"] = User;
            CoreMethods.SwitchSelectedMaster<CalendarPageModel>();
        }
    }
}

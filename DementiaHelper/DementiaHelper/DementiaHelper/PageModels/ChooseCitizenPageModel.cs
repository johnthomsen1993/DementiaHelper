using DementiaHelper.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
namespace DementiaHelper.PageModels
{
    
    public class ChooseCitizenPageModel : FreshMvvm.FreshBasePageModel
    {
        public ObservableCollection<Citizen> CaregiversCitizenCollection { get; set; }
        public ObservableCollection<Citizen> CitizenCollection { get; set; }
        public Command<Citizen> CitizenTappedCommand { get; set; }
        public ChooseCitizenPageModel()
        {
            _SearchText = "";
            CaregiversCitizenCollection = new ObservableCollection<Citizen>() { new Citizen() { FirstName = "John",LastName="Thomsen", CitizenId = 100 } }; ;
            CitizenCollection = new ObservableCollection<Citizen>();
            this.FilterCitizens();
            CitizenTappedCommand = new Command<Citizen>(ChooseCitizen);
            
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
                    
                }else if (Citizen.FirstName.IndexOf(_SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    CitizenCollection.Add(Citizen);
                }
            }
        }
       

        public void ChooseCitizen(Citizen citizen)
        {
            var h = citizen;
        }
    }
}

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
        #region ViewModel Properties
        public ObservableCollection<Relative> CitizenRelatveCollection { get; set; }
        public ObservableCollection<Relative> RelativeCollection { get; set; }
        public Relative ChoosenRelative{ get; set; }
        public bool RelativeChoosen { get; set; }
        public ObservableCollection<Relative> RelativeList { get; set; }
        public string NewPrimaryRelative { get; set; }
        public ICommand ChoosePrimaryRelativeCommand { get; protected set; }
        #endregion
        public ChoosePrimaryRelativePageModel()
        {
            RelativeChoosen = false;
            _SearchText = "";
            ChoosePrimaryRelativeCommand = new Command<Relative>(ChoosePrimaryRelative);
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            Device.BeginInvokeOnMainThread(async () =>
            {
                RelativeList = await ModelAccessor.Instance.AccountController.GetRelativeList();
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




       private async void ChoosePrimaryRelative(Relative relative)
       {
           if (relative != null)
           {
               await ModelAccessor.Instance.AccountController.ChoosePrimaryRelative(relative);
               await CoreMethods.PopPageModel();
           }
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
                    this.FilterRelatives();
                }
            }
        }


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

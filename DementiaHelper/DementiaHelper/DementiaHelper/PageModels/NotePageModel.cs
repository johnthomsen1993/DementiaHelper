using DementiaHelper.Model;
using DementiaHelper.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using PropertyChanged;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
    [ImplementPropertyChanged]
    public class NotePageModel : FreshMvvm.FreshBasePageModel
    {
        public ObservableCollection<Note> NoteList { get; set; }
        public string Note { get; set; }
        public ICommand NewNoteCommand { get; protected set; }
        public NotePageModel()
        {
            NewNoteCommand = new Command(async () => await CreatedNewNote());
            NoteList = new ObservableCollection<Note>();

        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            Device.BeginInvokeOnMainThread(async () =>
            {
                NoteList = await ModelAccessor.Instance.NoteController.GetNoteCollection();
            });
        }

        public async Task CreatedNewNote()
        {
            var decoded = await ModelAccessor.Instance.NoteController.CreateNewNote(Note);
            if (decoded != null)
            {
                if ((bool)decoded["NoteCreated"])
                {
                    Note = "";
                    NoteList = await ModelAccessor.Instance.NoteController.GetNoteCollection();
                }
            }
        }
    }
}

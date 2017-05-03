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
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
    public class NotePageModel : FreshMvvm.FreshBasePageModel
    {
        ObservableCollection<Note> NoteList { get; set; }
        public const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/values/notelist/";
        public string Note { get; set; }
        public ICommand NewNoteCommand { get; protected set; }
        public NotePageModel()
        {
            NewNoteCommand = new Command(async () => await CreatedNewNote());
            NoteList = new ObservableCollection<Note>() { new Note() { Subject = "TEST1234241", CreatedTime = DateTime.Now } };
        }

        public async Task CreatedNewNote()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>() { { "Subject", Note }, { "CreatedTime", DateTime.Now } });
                    var values = new Dictionary<string, string> { { "content", encoded } };
                    var content = new FormUrlEncodedContent(values);
                    await client.PutAsync(new Uri(URI_BASE), content);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}

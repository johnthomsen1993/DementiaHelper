﻿using DementiaHelper.Model;
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
        public const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/values/note/";
        public string Note { get; set; }
        public ICommand NewNoteCommand { get; protected set; }
        ApplicationUser User = (ApplicationUser)App.Current.Properties["ApplicationUser"];
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
                NoteList = await GetNoteCollection(User.CitizenId);
            });
        }

        public async Task CreatedNewNote()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>() { { "CitizenId", ((ApplicationUser)App.Current.Properties["ApplicationUser"]).CitizenId }, { "Subject", Note }, { "CreatedTime", DateTime.Now.ToUniversalTime() } });
                    var values = new Dictionary<string, string> { { "token", encoded } };
                    var content = new FormUrlEncodedContent(values);
                    var result = await client.PutAsync(new Uri(URI_BASE), content);
                    var decoded = JWTService.Decode(await result.Content.ReadAsStringAsync());
                    if (decoded != null)
                    {
                        if (NoteCreated(decoded))
                        {
                            Note = "";
                            NoteList = await GetNoteCollection(User.CitizenId);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private bool NoteCreated(IDictionary<string, object> dict)
        {
            return (bool)dict["NoteCreated"];
        }

        public async Task<ObservableCollection<Note>> GetNoteCollection(int? citizenId)
        {
            if (citizenId == null) { return new ObservableCollection<Note>(); }

            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>() {{"CitizenId", citizenId}});
                    var result = await client.GetStringAsync(new Uri(URI_BASE) + encoded);
                    var decoded = JWTService.Decode(result);
                    return MapNoteCollection(decoded);
                }
                catch (Exception)
                {
                    return new ObservableCollection<Note>();
                }
            }
        }

        private ObservableCollection<Note> MapNoteCollection(IDictionary<string, object> dict)
        {
            var tempNoteCollection = new ObservableCollection<Note>();
            var list = dict.SingleOrDefault(x => x.Key.Equals("Notes")).Value as IEnumerable<object>; 

            foreach (var obj in list)
            {
                var jsonContainer = obj as JContainer;
                tempNoteCollection.Add(new Note()
                {
                    Subject = jsonContainer.SelectToken("Subject").ToString(),
                    CreatedTime = jsonContainer.SelectToken("CreatedTime").ToObject<DateTime>().ToLocalTime()
                });
            }
            return tempNoteCollection;
        }

    }
}

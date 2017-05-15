using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DementiaHelper.Services;
using Newtonsoft.Json.Linq;

namespace DementiaHelper.Model.Controllers
{
    public class NoteController : INoteController
    {



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
        public async Task<ObservableCollection<Note>> GetNoteCollection()
        {
            if (((ApplicationUser)App.Current.Properties["ApplicationUser"]).CitizenId == null) { return new ObservableCollection<Note>(); }

            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>() { { "CitizenId", ((ApplicationUser)App.Current.Properties["ApplicationUser"]).CitizenId } });
                    var result = await client.GetStringAsync(new Uri("http://dementiahelper.azurewebsites.net/api/values/note/") + encoded);
                    var decoded = JWTService.Decode(result);
                    return MapNoteCollection(decoded);
                }
                catch (Exception)
                {
                    return new ObservableCollection<Note>();
                }
            }
        }
        public async Task<IDictionary<string,object>> CreateNewNote(string note)
        {
            using (var client = new HttpClient())
            {
                    var encoded = JWTService.Encode(new Dictionary<string, object>() { { "CitizenId", ((ApplicationUser)App.Current.Properties["ApplicationUser"]).CitizenId }, { "Subject", note }, { "CreatedTime", DateTime.Now.ToUniversalTime() } });
                    var values = new Dictionary<string, string> { { "token", encoded } };
                    var content = new FormUrlEncodedContent(values);
                    var result = await client.PutAsync(new Uri("http://dementiahelper.azurewebsites.net/api/values/note/"), content);
                    return JWTService.Decode(await result.Content.ReadAsStringAsync());
            }
        }
    }
}
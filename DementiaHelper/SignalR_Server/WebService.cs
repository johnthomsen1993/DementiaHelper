using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace SignalR_Server
{
    public class WebService
    {
        public const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/chat/saveChatMessage/";

        public void SaveMessage(string message, string group, string sender)
        {
            var values = new Dictionary<string, object>
            {
                {"Message", message},
                {"Group", group},
                {"Sender", sender}
            };

            var payload = JWTService.Encode(values);

            using (HttpClient h = new HttpClient())
            {
                var content = new StringContent(payload);
                var result = h.PutAsync(new Uri(URI_BASE), content).Result;
                var response = result.Content.ReadAsStringAsync();
            }
        }
    }
}
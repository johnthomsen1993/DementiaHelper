using Newtonsoft.Json;

namespace DementiaHelper.Model
{
    [JsonObject]
    public class UserInformation
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastname")]
        public string LastName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

    }
}
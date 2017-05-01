using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DementiaHelper.Model
{
    [JsonObject]
    public class UserInformation 
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string Phone { get; set; }
    }

    public class AddToShoppingList
    {
        public string Product { get; set; }
        public int Amount { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace DementiaHelper.Model
{
    [JsonObject]
    public class ShoppingList
    {
        [JsonExtensionData]
        public ObservableCollection<ShoppingListItem> ShoppingListItems { get; set; }
    }
}

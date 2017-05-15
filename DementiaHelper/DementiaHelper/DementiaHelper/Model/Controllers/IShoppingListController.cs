using System.Collections.Generic;
using System.Threading.Tasks;

namespace DementiaHelper.Model.Controllers
{
    public interface IShoppingListController
    {
        Task AddShoppingItemToShoppingList(int citizenId, string item, int quantity);
        Task<IDictionary<string, object>> ChangeBoughtStateOfItem(ShoppingListItem item);
        Task<IDictionary<string, object>> RemoveShoppingItem(ShoppingListItem item);
        ShoppingList MapToShoppingListModel(IDictionary<string, object> dict);
        Task<ShoppingList> GetShoppingList(int? id);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DementiaHelper.Model.Controllers;

namespace DementiaHelper.Model
{
    public class ModelAccessor : IModelAccessor
    {
        public static readonly ModelAccessor Instance = new ModelAccessor();
        public IAccountController AccountController { get; set; }
        public IChatController ChatController { get; set; }
        public ICalendarController CalendarController { get; set; }
        public IShoppingListController ShoppingListController { get; set; }
        public INoteController NoteController { get; set; }

        public ModelAccessor()
        {
            AccountController = new AccountController();
            ChatController = new ChatController();
            CalendarController = new CalendarController();
            ShoppingListController = new ShoppingListController();
            NoteController = new NoteController();
        }
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DementiaHelper.Model.Controllers
{
    public interface INoteController
    {
        Task<IDictionary<string, object>> CreateNewNote(string note);
        Task<ObservableCollection<Note>> GetNoteCollection();
    }
}
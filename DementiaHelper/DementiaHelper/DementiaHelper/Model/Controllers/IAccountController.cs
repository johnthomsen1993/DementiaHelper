using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DementiaHelper.Model.Controllers
{
    public interface IAccountController
    {
        Task<IDictionary<string, object>> ConnectToCitizen(string connectionId);
        Task<IDictionary<string, object>> ConnectToNursingHome(string connectionId);
        bool MapToApplicationUser(IDictionary<string, object> dict);
        Task<bool> LoginAsync(string email, string password);
        bool Login(string email, string password);
        Task<UserInformation> GetProfile(string email);
        Task<IDictionary<string, object>> CreateAccount(string email, string password, int roleId, string firstName, string lastName);
        ObservableCollection<Contact> MapToContactsCollection(IDictionary<string, object> dict);
        Task<ObservableCollection<Contact>> GetApplicationUserContactCollection(int? id);
        Task SaveUpdateUserInformation(UserInformation updatedUser, string oldEmail);
        Task<ObservableCollection<Relative>> GetRelativeList();
        Task ChoosePrimaryRelative(Relative relative);

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DementiaHelper.PageModels;
using DementiaHelper.Resx;
using DementiaHelper.Services;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace DementiaHelper.Model.Controllers
{
    public class AccountController : IAccountController
    {
        public async Task<IDictionary<string,object>> ConnectToCitizen(string ConnectionId)
        {
            using (var client = new HttpClient())
            {
                var encoded = JWTService.Encode(new Dictionary<string, object>() { { "RelativeId", ((ApplicationUser)App.Current.Properties["ApplicationUser"]).ApplicationUserId }, { "ConnectionId", ConnectionId } });
                var values = new Dictionary<string, string> { { "token", encoded } };
                var content = new FormUrlEncodedContent(values);
                var result = await client.PutAsync(new Uri("http://dementiahelper.azurewebsites.net/api/account/connecttocitizen/"), content);
                var decoded = JWTService.Decode(await result.Content.ReadAsStringAsync());
                return decoded;
            }
        }

        public async Task<IDictionary<string, object>> ConnectToNursingHome(string connectionId)
        {
            using (var client = new HttpClient())
            {
                try
                {

                
                var encoded = JWTService.Encode(new Dictionary<string, object>()
                {
                    {"CitizenId", ((ApplicationUser) App.Current.Properties["ApplicationUser"]).ApplicationUserId},
                    {"ConnectionId", connectionId}
                });
                var values = new Dictionary<string, string> {{"token", encoded}};
                var content = new FormUrlEncodedContent(values);
                var result = await client.PutAsync(new Uri("http://dementiahelper.azurewebsites.net/api/account/connectcitizentocenter/"), content);
                    var ssad = result;
                return JWTService.Decode(await result.Content.ReadAsStringAsync());
                }
                catch(Exception e) {
                    return new Dictionary<string, object>();
                }
            }
        }

        public bool MapToApplicationUser(IDictionary<string, object> dict)
        {
            if (dict.ContainsKey("Password") || dict.ContainsKey("UserExists") || dict.ContainsKey("ErrorRole")) { return false; }
            var User = dict["User"] as JContainer;

            var ApplicationUser = new ApplicationUser()
            {
                ApplicationUserId = User.SelectToken("ApplicationUserId").ToObject<int>(),
                Email = User.SelectToken("Email")?.ToObject<string>(),
                FirstName = User.SelectToken("FirstName")?.ToObject<string>(),
                LastName = User.SelectToken("LastName")?.ToObject<string>(),
                RoleId = User.SelectToken("RoleId").ToObject<int>(),
                Description = User.SelectToken("Description")?.ToObject<string>(),
                Phone = User.SelectToken("Phone")?.ToObject<string>()
            };
            switch (ApplicationUser.RoleId)
            {
                case 1:
                    ApplicationUser.ConnectionId = dict["ConnectionId"].ToString();
                    ApplicationUser.CitizenId = ApplicationUser.ApplicationUserId;
                    break;
                case 2:
                    ApplicationUser.PrimaryRelative = dict.ContainsKey("PrimaryRelative") ? Convert.ToBoolean(dict["PrimaryRelative"]) : false;
                    ApplicationUser.CitizenId = dict["CitizenId"] != null ? Convert.ToInt32(dict["CitizenId"]) : (int?)null;
                    break;
                case 3:
                    if (!dict.ContainsKey("CitizenIds")) break;
                    var list = dict["CitizenIds"] as IList;
                    ApplicationUser.CitizenList = MapToCitizenList(list);
                    break;
            }

            App.Current.Properties["ApplicationUser"] = ApplicationUser;
            return true;
        }
        public async Task<bool> LoginAsync(string email, string password)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>
                    {
                        {"email",email},
                        {"password", password}
                    });
                    client.DefaultRequestHeaders.Add("token", encoded);
                    var result = await client.GetAsync(new Uri("http://dementiahelper.azurewebsites.net/api/account/login"));
                    var decoded = JWTService.Decode(await result.Content.ReadAsStringAsync());
                    if (MapToApplicationUser(decoded))
                    {
                        DependencyService.Get<ICredentialsService>().DeleteCredentials();
                        DependencyService.Get<ICredentialsService>().SaveCredentials(email, password);
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        public bool Login(string email, string password)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>
                    {
                        {"email",email},
                        {"password", password}
                    });
                    client.DefaultRequestHeaders.Add("token", encoded);
                    var result = client.GetAsync(new Uri("http://dementiahelper.azurewebsites.net/api/account/login")).Result;
                    var decoded = JWTService.Decode(result.Content.ReadAsStringAsync().Result);
                    if (MapToApplicationUser(decoded))
                    {
                        DependencyService.Get<ICredentialsService>().DeleteCredentials();
                        DependencyService.Get<ICredentialsService>().SaveCredentials(email, password);
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public async Task<IDictionary<string, object>> CreateAccount(string email, string password, int roleId, string firstName, string lastName)
        {
            using (var client = new HttpClient())
            {
                var encoded = JWTService.Encode(new Dictionary<string, object>
                {
                    {"email", email},
                    {"password", password},
                    {"role", roleId},
                    {"firstName", firstName},
                    {"lastName", lastName}
                });
                var values = new Dictionary<string, string> {{"token", encoded}};
                var content = new FormUrlEncodedContent(values);
                var result = await client.PutAsync(new Uri("http://dementiahelper.azurewebsites.net/api/account/createaccount"), content);
                return JWTService.Decode(await result.Content.ReadAsStringAsync());
            }
        }

        public ObservableCollection<Citizen> MapToCitizenList(IList list)
        {
            var tempCitizenList = new ObservableCollection<Citizen>();
            foreach (var obj in list)
            {
                var jsonContainer = obj as JContainer;
                tempCitizenList.Add(new Citizen() { CitizenId = jsonContainer.SelectToken("CitizenId").ToObject<int>(), FirstName = jsonContainer.SelectToken("ApplicationUser").SelectToken("FirstName").ToObject<string>(), LastName = jsonContainer.SelectToken("ApplicationUser").SelectToken("LastName").ToObject<string>() });

            }
            return tempCitizenList;
        }
        public async Task<UserInformation> GetProfile(string email)
        {
            var payload = new Dictionary<string, object>
            {
                {"email", email}
            };

            using (HttpClient h = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(payload);
                    var result = await h.GetStringAsync(new Uri("http://dementiahelper.azurewebsites.net/api/account/getuser/" + encoded));
                    var decoded = JWTService.Decode(result);

                    return new UserInformation()
                    {
                        FirstName = decoded["firstName"]?.ToString(),
                        LastName = decoded["lastName"]?.ToString(),
                        Email = decoded["email"]?.ToString(),
                        Description = decoded["description"]?.ToString(),
                        Phone = decoded["phone"]?.ToString()
                    };
                }
                catch (Exception)
                {
                    return new UserInformation() { FirstName = "FirstName", LastName = "LastName", Email = "Email", Description = "Description" }; //Test string

                }
            }
        }
        public async Task<ObservableCollection<Contact>> GetApplicationUserContactCollection(int? id)
        {
            if (id == null) { return new ObservableCollection<Contact>() { }; }
            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>() { { "CitizenId", id } });
                    var result = await client.GetStringAsync(new Uri("http://dementiahelper.azurewebsites.net/api/account/contactlist/" + encoded));
                    var decoded = JWTService.Decode(result);
                    if (decoded != null)
                    {
                        return MapToContactsCollection(decoded);
                    }
                    {
                        return new ObservableCollection<Contact>() { };
                    }

                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public ObservableCollection<Contact> MapToContactsCollection(IDictionary<string, object> dict)
        {
            var tempCaregiversCitizenCollection = new ObservableCollection<Contact>();
            var list = dict.SingleOrDefault(x => x.Key.Equals("contactList")).Value as IEnumerable<object>;

            foreach (var obj in list)
            {
                var jsonContainer = obj as JContainer;
                var number = jsonContainer.SelectToken("ApplicationUser").SelectToken("Phone").ToObject<string>();
                if (number != "" && number != null)
                {
                    tempCaregiversCitizenCollection.Add(item: new Contact()
                    {
                        Phone = jsonContainer.SelectToken("ApplicationUser").SelectToken("Phone").ToObject<string>(),
                        Name = jsonContainer.SelectToken("ApplicationUser").SelectToken("FirstName").ToObject<string>() + " " + jsonContainer.SelectToken("ApplicationUser").SelectToken("LastName").ToObject<string>()
                    });
                }
            }

            if (dict["caregiverCenter"] != null)
            {
                var jContainer = dict["caregiverCenter"] as JContainer;
                tempCaregiversCitizenCollection.Add(new Contact()
                {
                    Phone = jContainer.SelectToken("Phone").ToObject<string>(),
                    Name = jContainer.SelectToken("Name").ToObject<string>()
                });
            }
            return tempCaregiversCitizenCollection;
        }

        public async Task SaveUpdateUserInformation(UserInformation updatedUser, string oldEmail)
        {

                var payload = new Dictionary<string, object>
                {
                    {"firstName", updatedUser.FirstName},
                    {"lastName", updatedUser.LastName},
                    {"email", updatedUser.Email},
                    {"description", updatedUser.Description},
                    {"phone", updatedUser.Phone},
                    {"oldEmail", oldEmail}
                };

                var encoded = JWTService.Encode(payload);

                using (var h = new HttpClient())
                {
                    var values = new Dictionary<string, string> {{"token", encoded}};
                    var content = new FormUrlEncodedContent(values);
                    var result = h.PutAsync(new Uri("http://dementiahelper.azurewebsites.net/api/account/update/"), content).Result;
                    var decoded = JWTService.Decode(await result.Content.ReadAsStringAsync());
                    if (decoded != null)
                    {
                        if (Convert.ToBoolean(decoded["UserUpdated"]))
                        {
                            ((ApplicationUser)App.Current.Properties["ApplicationUser"]).FirstName = updatedUser.FirstName;
                            ((ApplicationUser)App.Current.Properties["ApplicationUser"]).LastName = updatedUser.LastName;
                            ((ApplicationUser)App.Current.Properties["ApplicationUser"]).Description = updatedUser.Description;
                            ((ApplicationUser)App.Current.Properties["ApplicationUser"]).Email = updatedUser.Email;
                            ((ApplicationUser)App.Current.Properties["ApplicationUser"]).Phone = updatedUser.Phone;
                        }
                    }
                } 
        }
       
    }
}

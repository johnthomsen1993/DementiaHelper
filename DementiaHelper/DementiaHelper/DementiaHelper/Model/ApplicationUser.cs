using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DementiaHelper.Model
{
    public class ApplicationUser
    {
        public int? ApplicationUserId { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? CitizenId { get; set; }
        public ObservableCollection<int?> ListOfCitizens {get;set;}
        public int? RoleId { get; set; }
        public int? GroupId { get; set; }
    }
}

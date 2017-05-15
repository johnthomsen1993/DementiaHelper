using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DementiaHelper.Model
{
    public class Relative
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RelativeId { get; set; }
        public bool PrimaryRelative { get; set; }
    }
}

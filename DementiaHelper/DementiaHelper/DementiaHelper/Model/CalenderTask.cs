using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DementiaHelper.Model
{
    [JsonObject]
    public class CalenderTask
    {
       public DateTime Time { get; set; }
       public string Description { get; set; }
    }
}
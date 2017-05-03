using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DementiaHelper.WebApi.model
{
    public class Note
    {
        [Key]
        public int NoteId { get; set; }

        public string Subject { get; set; }
        public DateTime CreatedTime { get; set; }

        [ForeignKey("Citizen")]
        public int CitizenId { get; set; }
        public Citizen Citizen { get; set; }
    }
}

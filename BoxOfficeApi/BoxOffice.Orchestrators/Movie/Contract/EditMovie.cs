using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Orchestrators.Ticket.Contract
{
    public class EditMovie
    {
        public string Title { get; set; }
        public string Director { get; set; }
        public string? Poster { get; set; }

        /*[Required]
        public string Name { get; set; }
        [Required]
        public int Duration { get; set; }*/
    }
}

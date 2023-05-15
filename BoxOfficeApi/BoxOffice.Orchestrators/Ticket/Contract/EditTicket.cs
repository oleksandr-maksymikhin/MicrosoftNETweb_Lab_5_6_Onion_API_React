using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Orchestrators.Ticket.Contract
{
    public class EditTicket
    {
        [Required]
        public int Row { get; set; }
        [Required]
        public int Seat { get; set; }
        [Required]
        public int Price { get; set; }

        /*[Required]
        public int Seat { get; set; }
        [Required]
        public int Price { get; set; }*/
    }
}

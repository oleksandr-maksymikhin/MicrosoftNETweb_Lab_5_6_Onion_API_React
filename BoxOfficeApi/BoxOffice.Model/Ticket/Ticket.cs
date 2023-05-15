using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Model.Ticket
{
    public class Ticket
    {
        public int Id { get; set; }
        public int Row { get; set; }
        public int Seat { get; set; }
        public int Price { get; set; }


        /*public int Id { get; set; }
        public int Seat { get; set; }
        public int Price { get; set; }*/
    }
}

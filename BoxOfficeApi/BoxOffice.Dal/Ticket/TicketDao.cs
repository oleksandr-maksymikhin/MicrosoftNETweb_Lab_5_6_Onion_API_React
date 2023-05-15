using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Dal.Ticket
{
    [Table("Tickets")]
    public class TicketDao
    {
        [Column("id"), Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column("row")]
        public int Row { get; set; }
        [Column("seat")]
        public int Seat { get; set; }
        [Column("price")]
        public int Price { get; set; }

        /*[Column("id"), Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column("seat")]
        public int Seat { get; set; }
        [Column("price")]
        public int Price { get; set; }*/
    }
}

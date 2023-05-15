using System;
using System.ComponentModel.DataAnnotations;

namespace BoxOffice.Orchestrators.Ticket.Contract
{
    public class CreateMovie
    {
        public string Title { get; set; }
        public string Director { get; set; }
        public string? Poster { get; set; }

        /*[Required]
        public string Name { get; set; }
        [Required, Range(typeof(int), "60", "240")]
        public int Duration { get; set; }*/
    }
}

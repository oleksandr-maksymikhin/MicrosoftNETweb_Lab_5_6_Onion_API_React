using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Dal.Movie
{
    public class MovieDaoStep
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public string? Poster { get; set; }


        /*[Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }*/
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Model.Movie
{
    public class Movie
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public string? Poster { get; set; }

        /*public Guid Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }*/
    }
}

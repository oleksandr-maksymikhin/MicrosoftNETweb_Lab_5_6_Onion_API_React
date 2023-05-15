using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Model.MovieStats
{
    public interface IMovieStatsOrchestrator
    {
        Task<List<string>> GetStatsAsync();
    }
}

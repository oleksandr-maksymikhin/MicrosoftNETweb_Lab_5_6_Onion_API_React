using BoxOffice.Model.MovieStats;
using BoxOffice.Platform.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Orchestrators.MovieStatsOrchestrator
{
    public class MovieStatsOrchestrator : IMovieStatsOrchestrator
    {
        private readonly ISubscriber _subscriber;

        public MovieStatsOrchestrator(ISubscriber subscriber)
        {
            _subscriber = subscriber;
        }
        public async Task<List<string>> GetStatsAsync()
        {
            return _subscriber.Data;
        }
    }
}

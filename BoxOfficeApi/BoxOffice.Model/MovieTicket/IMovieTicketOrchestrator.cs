using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Model.MovieTicket
{
    public interface IMovieTicketOrchestrator
    {
        Task<MovieTicket> CreateAsync(Guid movieId, int ticketId);
        Task <List<int>> GetTicketsAsync(Guid movieId);
    }
}

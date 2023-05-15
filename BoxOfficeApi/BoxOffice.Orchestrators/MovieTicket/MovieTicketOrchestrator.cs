using Azure.Storage.Blobs.Models;
using BoxOffice.Model.Movie;
using BoxOffice.Model.MovieTicket;
using BoxOffice.Model.Ticket;
using BoxOffice.Orchestrators.Movie;
using BoxOffice.Orchestrators.Ticket;
using BoxOffice.Platform.BlobStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Orchestrators.MovieTicket
{
    public class MovieTicketOrchestrator : IMovieTicketOrchestrator
    {
        private readonly ITicketOrchestrator _ticketOrchestrator;
        private readonly IMovieOrchestrator _movieOrchestrator;
        private readonly IBlobStorage _movieTicketStorage;

        public MovieTicketOrchestrator(
            ITicketOrchestrator ticketOrchestrator, 
            IMovieOrchestrator movieOrchestrator,
            IBlobStorage movieTicketStorage)
        {
            _ticketOrchestrator = ticketOrchestrator;
            _movieOrchestrator = movieOrchestrator;
            _movieTicketStorage = movieTicketStorage;
        }

        public async Task<Model.MovieTicket.MovieTicket> CreateAsync(Guid movieId, int ticketId)
        {
            //validation on Orchestrator level
            var ticket = await _ticketOrchestrator.GetByIdAsync(ticketId);
            var movie = await _movieOrchestrator.GetByIdAsync(movieId);

            var relationFileName = $"{movieId:N}_{ticketId}";
            var exists = await _movieTicketStorage.ContainsFileByNameAsync(relationFileName);
            if (!exists)
            {
                /*System.NullReferenceException: 'Object reference not set to an instance of an object.'
                BoxOffice.Platform.BlobStorage.IBlobStorage.PutContentAsync(...) returned null.
                */
                //Azure.Response<Azure.Storage.Blobs.Models.BlobContentInfo>
                await _movieTicketStorage.PutContentAsync(relationFileName);
            }

            return new Model.MovieTicket.MovieTicket()
            {
                MovieId = movieId,
                TicketId = ticketId
            };
        }

        public async Task<List<int>> GetTicketsAsync(Guid movieId)
        {
            var movie = await _movieOrchestrator.GetByIdAsync(movieId);
            var clients = await _movieTicketStorage.FindByMovieAsync(movieId);
            return clients;
        }
    }
}

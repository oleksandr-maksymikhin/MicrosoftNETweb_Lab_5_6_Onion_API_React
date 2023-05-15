using BoxOffice.Model.MovieTicket;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System;
using System.Threading.Tasks;

namespace BoxOffice.Api.Controllers
{
    [ApiController]
    //[Route("api/[controller]")]
    [Route("api/v1/movietickets")]
    public class MovieTicketController : ControllerBase
    {
        private readonly IMovieTicketOrchestrator _movieTicketOrchestrator;
        public MovieTicketController(IMovieTicketOrchestrator movieTicketOrchestrator)
        {
            _movieTicketOrchestrator = movieTicketOrchestrator;
        }

        [HttpPost("{movieId}/tickets/{ticketId}")]
        public async Task<IActionResult> PostAsync(Guid movieId, int ticketId)
        {
            var model = await _movieTicketOrchestrator.CreateAsync(movieId, ticketId);
            return Ok(model);
        }

        [HttpGet("{movieId}/tickets")]
        public async Task<IActionResult> GetTicketsByMovieId(Guid movieId)
        {
            var model = await _movieTicketOrchestrator.GetTicketsAsync(movieId);
            return Ok(model);
        }
    }
}

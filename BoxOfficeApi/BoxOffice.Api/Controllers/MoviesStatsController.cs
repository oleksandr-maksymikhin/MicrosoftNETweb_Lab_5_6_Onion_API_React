using AutoMapper;
using BoxOffice.Model.Movie;
using BoxOffice.Model.MovieStats;
using BoxOffice.Orchestrators.Ticket.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BoxOffice.Api.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    [Route("api/v1/stats")]
    public class MoviesStatsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<MoviesController> _logger;
        private IMovieStatsOrchestrator _movieStatsOrchestrator;

        public MoviesStatsController(
            IMapper mapper,
            ILogger<MoviesController> logger,
            IMovieStatsOrchestrator movieStatsOrchestrator)
        {
            _mapper = mapper;
            _logger = logger;
            _movieStatsOrchestrator = movieStatsOrchestrator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var dataResult = await _movieStatsOrchestrator.GetStatsAsync();

            return Ok(dataResult);
        }
    }
}
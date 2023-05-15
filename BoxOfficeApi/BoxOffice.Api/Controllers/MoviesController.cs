using AutoMapper;
using BoxOffice.Model.Movie;
using BoxOffice.Orchestrators.Ticket.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BoxOffice.Api.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    [Route("api/v1/movies")]
    public class MoviesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<MoviesController> _logger;
        private IMovieOrchestrator _movieOrchestrator;

        public MoviesController(
            IMapper mapper,
            ILogger<MoviesController> logger,
            IMovieOrchestrator movieOrchestrator)
        {
            _mapper = mapper;
            _logger = logger;
            _movieOrchestrator = movieOrchestrator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var movie = await _movieOrchestrator.GetAllAsync();

            return Ok(movie);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var movieById = await _movieOrchestrator.GetByIdAsync(id);

            return Ok(movieById);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(CreateMovie movie)
        {
            var model = _mapper.Map<CreateMovie, Movie>(movie);
            var result = await _movieOrchestrator.CreateAsync(model);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, EditMovie model)
        {
            Movie modelToUpdate = _mapper.Map<EditMovie, Movie>(model);
            modelToUpdate.Id = id;
            Movie entity = await _movieOrchestrator.UpdateAsync(modelToUpdate);

            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var deleteEntity = await _movieOrchestrator.DeleteAsync(id);
            
            return Ok(deleteEntity);
        }

    }
}
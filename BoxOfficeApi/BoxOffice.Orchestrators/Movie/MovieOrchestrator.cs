using BoxOffice.Model.Movie;
using BoxOffice.Platform.Exception;
using BoxOffice.Platform.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Orchestrators.Movie
{
    public class MovieOrchestrator : IMovieOrchestrator
    {
        private readonly IMovieRepository _movieRepository;

        public IPublisher _publisher;

        public MovieOrchestrator(
            IMovieRepository movieRepository,
            IPublisher publisher)
            {
                _movieRepository = movieRepository;
                _publisher = publisher;
            }

        public async Task<List<Model.Movie.Movie>> GetAllAsync()
        {
            return await _movieRepository.GetAllAsync();
        }

        public async Task<Model.Movie.Movie> GetByIdAsync(Guid id)
        {
            var movieById =  await _movieRepository.GetByIdAsync(id);
            if (movieById == null)
            {
                throw new ResourceNotFoundException($"Movie with id = {id} not found");
            }
            return movieById;
        }

        public async Task<Model.Movie.Movie> CreateAsync(Model.Movie.Movie model)
        {
            var entity = await _movieRepository.CreateAsync(model);
            await _publisher.PublishAsync(entity.Id);
            return entity;
            //return await _movieRepository.CreateAsync(model);
        }

        public async Task<Model.Movie.Movie> UpdateAsync(Model.Movie.Movie modelToUpdate)
        {
            Model.Movie.Movie? existingEntity = await _movieRepository.GetByIdAsync(modelToUpdate.Id);
            if (existingEntity == null)
            {
                throw new ResourceNotFoundException($"Ticket with id = {modelToUpdate.Id} not found");
            }
            //!!!!!!!!!!!!!!!!**********************fix the issue with Fieled by Field mapping with Automapper********************
            /*existingEntity.Name = modelToUpdate.Name;
            existingEntity.Duration = modelToUpdate.Duration;*/

            var updateResult = await _movieRepository.UpdateAsync(modelToUpdate);

            return updateResult;
        }

        public async Task<Model.Movie.Movie> DeleteAsync(Guid id)
        {
            var entity = await _movieRepository.GetByIdAsync(id);
            await _movieRepository.DeleteAsync(id);
            return entity;
        }
    }
}

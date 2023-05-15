using AutoMapper;
using BoxOffice.Model.Movie;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Dal.Movie
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IMapper _mapper;
        private readonly CosmosDbContext _dbCosmosContext;

        public MovieRepository(IMapper mapper, CosmosDbContext context)
        {
            _mapper = mapper;
            _dbCosmosContext = context;
        }

        public async Task<List<Model.Movie.Movie>> GetAllAsync()
        {
            var entities = await _dbCosmosContext.Movies.ToListAsync();
            var entitiesMovieModel = _mapper.Map<List<MovieDaoStep>, List<Model.Movie.Movie>>(entities);
            return entitiesMovieModel;
        }

        public async Task<Model.Movie.Movie> GetByIdAsync(Guid id)
        {
            var entity = await _dbCosmosContext.Movies
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
/*            if(entity == null)
            {
                return null;
            }*/
            return _mapper.Map<MovieDaoStep, Model.Movie.Movie>(entity);
        }

        public async Task<Model.Movie.Movie> CreateAsync(Model.Movie.Movie model)
        {
            var entity = _mapper.Map<Model.Movie.Movie, MovieDaoStep>(model);
            entity.Id = Guid.NewGuid();
            await _dbCosmosContext.Movies.AddAsync(entity);
            await _dbCosmosContext.SaveChangesAsync();

            return _mapper.Map<MovieDaoStep, Model.Movie.Movie>(entity);
        }

        public async Task<Model.Movie.Movie> UpdateAsync(Model.Movie.Movie entity)
        {
            var existingEntity = await _dbCosmosContext.Movies.FirstOrDefaultAsync(t => t.Id == entity.Id);

            existingEntity.Title = entity.Title;
            existingEntity.Director = entity.Director;
            existingEntity.Poster = entity.Poster;

            await _dbCosmosContext.SaveChangesAsync();
            return entity;

            //***************this code is better because of AutoMapper, but it fail in PUT integration test************
            /*var existingEntity = _mapper.Map<Model.Movie.Movie, MovieDaoStep>(entity);
            var entityEntry = _dbCosmosContext.Movies.Update(existingEntity);
            var updatedEntity = _mapper.Map<MovieDaoStep, Model.Movie.Movie>(entityEntry.Entity);
            return updatedEntity;*/
        }

        public async Task DeleteAsync(Guid id)
        {
            var existingEntity = await _dbCosmosContext.Movies.FirstOrDefaultAsync(t => t.Id == id);
            _dbCosmosContext.Movies.Remove(existingEntity);
            await _dbCosmosContext.SaveChangesAsync();
        }
    }
}

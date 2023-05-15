using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Model.Movie
{
    public interface IMovieRepository
    {
        Task<Movie> CreateAsync(Movie model);
        Task<List<Movie>> GetAllAsync();
        Task<Movie> GetByIdAsync(Guid id);
        Task<Movie> UpdateAsync(Movie existingEntity);
        Task DeleteAsync(Guid id);

    }
}

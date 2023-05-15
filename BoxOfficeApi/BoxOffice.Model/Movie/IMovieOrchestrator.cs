using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Model.Movie
{
    public interface IMovieOrchestrator
    {
        Task<List<Movie>> GetAllAsync();
        Task<Movie> GetByIdAsync(Guid id);
        Task<Movie> CreateAsync(Movie model);
        Task<Movie> UpdateAsync(Movie modelToUpdate);
        Task<Movie> DeleteAsync(Guid id);
    }
}

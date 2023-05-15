using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;

namespace BoxOffice.Platform.BlobStorage
{
    public interface IBlobStorage
    {
        //Task<Stream> GetContentAsync();
        Task PutContentAsync(string fileName);
        Task<bool> ContainsFileByNameAsync(string fileName);
        Task<List<int>> FindByMovieAsync(Guid movieId);
    }
}

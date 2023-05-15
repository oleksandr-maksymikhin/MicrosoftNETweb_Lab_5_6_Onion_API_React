using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace BoxOffice.Platform.BlobStorage
{
    public class BlobStorage : IBlobStorage
    {
        //protected virtual string BlobContainerName { get; set; } = "dataincloudphase3";
        protected virtual string BlobContainerName { get; set; } = "stepwebproject";
        private readonly BlobServiceClient _client;

        public BlobStorage(BlobConfiguration blobConfiguration)
        {
            _client = new BlobServiceClient(blobConfiguration.ConnectionString);
        }

        /*public async Task<Stream> GetContentAsync(string fileName)
        {
            return null;
        }*/

        public async Task PutContentAsync(string fileName)
        {
            await _client
                .GetBlobContainerClient(BlobContainerName)
                .GetBlobClient(fileName)
                .UploadAsync(new MemoryStream());
        }

        public async Task<bool> ContainsFileByNameAsync(string fileName)
        {
            return await _client
                .GetBlobContainerClient(BlobContainerName)
                .GetBlobClient(fileName)
                .ExistsAsync();
        }

        public async Task<List<int>> FindByMovieAsync (Guid movieId)
        {
            var results = _client.GetBlobContainerClient(BlobContainerName)
            .GetBlobs(prefix: movieId.ToString("N"))
            .AsPages(default, 1000)
            .SelectMany(dt => dt.Values).Select(bi => int.Parse(bi.Name.Split('_').Last())).ToList();

            return results;
        }
    }
}

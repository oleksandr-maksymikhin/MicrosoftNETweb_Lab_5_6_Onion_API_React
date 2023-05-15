using BoxOffice.Dal.Ticket;
using BoxOffice.Dal;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BoxOffice.Orchestrators.Ticket.Contract;
using BoxOffice.Dal.Movie;

namespace BoxOffice.IntegrationTests.Movie
{
    public class PutMovieTest: CosmosBaseTest
    {
        private readonly HttpClient _httpClient;
        //private HttpClient _httpClient;
        public PutMovieTest()
        {
            _httpClient = InitTestServer().GetClient();
        }

        [Fact]
        public async Task PutAsync_IfNoExceptionOccurred_UpdateEntityInDb()
        {
            //Arrange
            var inputModel_1 = new CreateMovie
            {
                Title = "Interstellar",
                Director = "Christopher Nolan",
                Poster = null
            };

            EntityEntry<MovieDaoStep> postResult = null;
            await using var context = Host.Services.GetService<CosmosDbContext>();
            postResult = await context.Movies.AddAsync(new MovieDaoStep
            {
                Title = inputModel_1.Title,
                Director = inputModel_1.Director,
                Poster = inputModel_1.Poster
            });
            await context.SaveChangesAsync();

            var modifiedModel = new EditMovie
            {
                Title = "Matrix",
                Director = "Wachowskis brothers",
                Poster = null
            };

            //Act
            var message = new HttpRequestMessage(
                HttpMethod.Put,
                $"api/v1/movies/{postResult.Entity.Id}")
            {
                Content = new StringContent(JsonConvert.SerializeObject(modifiedModel), Encoding.UTF8, "application/json")
            };
            var putResult = await _httpClient.SendAsync(message);

            //Assert
            putResult.StatusCode.Should().Be(HttpStatusCode.OK);
            putResult.Content.Should().NotBeNull();
            var putResultModel = JsonConvert.DeserializeObject<Model.Movie.Movie>(
                await putResult.Content.ReadAsStringAsync());
            putResultModel.Should().NotBeNull();
            putResultModel.Id.Should().Be(postResult.Entity.Id);
            putResultModel.Title.Should().Be(modifiedModel.Title);
            putResultModel.Director.Should().Be(modifiedModel.Director);
            putResultModel.Poster.Should().Be(modifiedModel.Poster);
        }
    }
}

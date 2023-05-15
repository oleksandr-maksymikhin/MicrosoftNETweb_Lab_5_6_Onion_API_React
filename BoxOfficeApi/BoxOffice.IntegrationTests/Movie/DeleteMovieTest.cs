using BoxOffice.Dal.Ticket;
using BoxOffice.Dal;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using BoxOffice.Orchestrators.Ticket.Contract;
using BoxOffice.Dal.Movie;
using FluentAssertions;

namespace BoxOffice.IntegrationTests.Movie
{
    public class DeleteMovieTest: CosmosBaseTest
    {
        private readonly HttpClient _httpClient;
        public DeleteMovieTest()
        {
            _httpClient = InitTestServer().GetClient();
        }

        [Fact]
        public async Task DeleteAsync_IfNoExceptionOccurred_DeleteEntityFromDb()
        {
            //Arrange
            var inputModel_1 = new CreateMovie
            {
                Title = "Interstellar",
                Director = "Christopher Nolan",
                Poster = null
            };
            var inputModel_2 = new CreateMovie
            {
                Title = "Matrix",
                Director = "Wachowskis brothers",
                Poster = null
            };

            await using var context = Host.Services.GetService<CosmosDbContext>();
            var postResult_1 = await context.Movies.AddAsync(new MovieDaoStep
            {
                Title = inputModel_1.Title,
                Director = inputModel_1.Director,
                Poster = inputModel_1.Poster
            });
            var postResult_2 = await context.Movies.AddAsync(new MovieDaoStep
            {
                Title = inputModel_2.Title,
                Director = inputModel_2.Director,
                Poster = inputModel_2.Poster
            });
            await context.SaveChangesAsync();
            
            //Act
            var message = new HttpRequestMessage(
                HttpMethod.Delete,
                $"api/v1/movies/{postResult_1.Entity.Id}");
            var deleteResult = await _httpClient.SendAsync(message);

            //Assert
            deleteResult.StatusCode.Should().Be(HttpStatusCode.OK);
            deleteResult.Content.Should().NotBeNull();
            var deleteResponseModel = JsonConvert.DeserializeObject<Model.Movie.Movie>(
                await deleteResult.Content.ReadAsStringAsync());
            deleteResponseModel.Should().NotBeNull();
            deleteResponseModel.Id.Should().Be(postResult_1.Entity.Id);
            deleteResponseModel.Title.Should().Be(inputModel_1.Title);
            deleteResponseModel.Director.Should().Be(inputModel_1.Director);
            deleteResponseModel.Poster.Should().Be(inputModel_1.Poster);
        }
    }
}

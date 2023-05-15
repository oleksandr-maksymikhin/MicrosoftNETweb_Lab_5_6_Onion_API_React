using BoxOffice.Dal;
using BoxOffice.Orchestrators.Ticket.Contract;
using FluentAssertions;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.IntegrationTests.Movie
{
    public class PostMovieTest: CosmosBaseTest
    {
        private readonly HttpClient _httpClient;
        public PostMovieTest()
        {
            _httpClient = InitTestServer().GetClient();
        }

        [Fact]
        public async Task PostAsync_IfNoExceptionOccurred_SavesNewEntityInDb()
        {
            // Arrange
            var inputModel_1 = new CreateMovie
            {
                Title = "Interstellar",
                Director = "Christopher Nolan",
                Poster = null
            };

            // Act
            var message = new HttpRequestMessage(
                HttpMethod.Post,
                $"api/v1/movies")
            {
                Content = new StringContent(JsonConvert.SerializeObject(inputModel_1), Encoding.UTF8, "application/json")
            };
            var postResult = await _httpClient.SendAsync(message);

            //Assert
            postResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            postResult.Content.Should().NotBeNull();
            var postResponseModel = JsonConvert.DeserializeObject<Model.Movie.Movie>(
                await postResult.Content.ReadAsStringAsync());
            postResponseModel.Should().NotBeNull();
            var savedEntity = CosmosDbContext.Movies
                .FirstOrDefault(t =>
                    t.Title == inputModel_1.Title
                    && t.Director == inputModel_1.Director
                    && t.Id == postResponseModel.Id);
            savedEntity.Should().NotBeNull();
        }
    }
}


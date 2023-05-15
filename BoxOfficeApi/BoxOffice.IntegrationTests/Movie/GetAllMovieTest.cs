using BoxOffice.Dal.Ticket;
using BoxOffice.Dal;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using BoxOffice.Orchestrators.Ticket.Contract;
using BoxOffice.Dal.Movie;
using FluentAssertions;
using System.Linq;

namespace BoxOffice.IntegrationTests.Movie
{
    public class GetAllMovieTest: CosmosBaseTest
    {
        private readonly HttpClient _httpClient;
        public GetAllMovieTest()
        {
            _httpClient = InitTestServer().GetClient();
        }

        [Fact]
        public async Task GetAllAsync_IfNoExceptionOccurred_GetAllEntityFromDb()
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
                HttpMethod.Get,
                $"api/v1/movies");
            var getAllResult = await _httpClient.SendAsync(message);

            //Assert
            getAllResult.StatusCode.Should().Be(HttpStatusCode.OK);
            getAllResult.Content.Should().NotBeNull();
            var getAllResponseModel = JsonConvert.DeserializeObject<List<Model.Movie.Movie>>(
                await getAllResult.Content.ReadAsStringAsync());
            getAllResponseModel.Should().NotBeNull();
            int countInTickets = context.Movies.Count();
            getAllResponseModel.Count.Should().Be(countInTickets);
        }
    }
}

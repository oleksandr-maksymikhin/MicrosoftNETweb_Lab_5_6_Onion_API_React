using BoxOffice.Dal.Movie;
using BoxOffice.Dal.Ticket;
using BoxOffice.Dal;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using FluentAssertions;

namespace BoxOffice.IntegrationTests.MovieTicket
{
    public class GetTicketsByMovieIdTest : BlobBaseTest
    {
        private readonly HttpClient _httpClient;
        public GetTicketsByMovieIdTest()
        {
            _httpClient = InitTestServer().GetClient();
        }

        [Fact]
        public async Task GetTicketsByMovieId_IfNoExceptionOccurred_GetAllEntityFromDb()
        {
            // Arrange
            Guid inputMovieId = Guid.Parse("93094f7d-3926-483f-9f44-3e4cf28929a2");
            int inputTicketId_1 = 1;
            int inputTicketId_2 = 2;
            //string relationFileName = $"{inputMovieId:N}_{inputTicketId_1}";

            Model.Ticket.Ticket tiketById_1 = new Model.Ticket.Ticket
            {
                Id = inputTicketId_1,
                Seat = 1,
                Price = 100
            };

            Model.Ticket.Ticket tiketById_2 = new Model.Ticket.Ticket
            {
                Id = inputTicketId_2,
                Seat = 2,
                Price = 200
            };

            List<int> ticketsListResult = new List<int>() { tiketById_1.Id, tiketById_2.Id };

            Model.Movie.Movie movieById = new Model.Movie.Movie
            {
                Id = inputMovieId,
                Title = "Matrix",
                Director = "Wachowskis brothers",
                Poster = null
            };

/*            Model.MovieTicket.MovieTicket modelMovieTicketResult = new Model.MovieTicket.MovieTicket
            {
                MovieId = inputMovieId,
                TicketId = inputTicketId_1
            };*/

            await using var contextMovie = Host.Services.GetService<CosmosDbContext>();
            var postMovieResult = await contextMovie.Movies.AddAsync(new MovieDaoStep
            {
                Id = inputMovieId,
                Title = movieById.Title,
                Director = movieById.Director,
                Poster = movieById.Poster,

            });
            await contextMovie.SaveChangesAsync();

            // Act
            var message = new HttpRequestMessage(
                HttpMethod.Get,
                $"api/v1/movietickets/{inputMovieId}/tickets");

            var getTicketsByMovieIdResult = await _httpClient.SendAsync(message);

            //Assert
            getTicketsByMovieIdResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            getTicketsByMovieIdResult.Content.Should().NotBeNull();
            var getTicketsByMovieIdResultResponseModel = JsonConvert.DeserializeObject<List<int>>(
                await getTicketsByMovieIdResult.Content.ReadAsStringAsync());
            getTicketsByMovieIdResultResponseModel.Should().NotBeNull();
            getTicketsByMovieIdResultResponseModel.Count.Should().Be(ticketsListResult.Count);
            getTicketsByMovieIdResultResponseModel.Should().Equal(ticketsListResult);
        }
    }
}

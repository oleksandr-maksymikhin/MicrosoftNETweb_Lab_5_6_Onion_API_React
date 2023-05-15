using BoxOffice.Orchestrators.Ticket.Contract;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BoxOffice.Model.Movie;
using BoxOffice.Model.Ticket;
using BoxOffice.Dal.Movie;
using BoxOffice.Dal;
using Microsoft.Extensions.DependencyInjection;
using BoxOffice.Dal.Ticket;
using FluentAssertions;

namespace BoxOffice.IntegrationTests.MovieTicket
{
    public class PostMovieTicketTest: BlobBaseTest
    {
        private readonly HttpClient _httpClient;
        public PostMovieTicketTest()
        {
            _httpClient = InitTestServer().GetClient();
        }

        [Fact]
        public async Task PostAsync_IfNoExceptionOccurred_SavesNewEntityInDb()
        {
            // Arrange
            Guid inputMovieId = Guid.Parse("93094f7d-3926-483f-9f44-3e4cf28929a2");
            int inputTicketId_1 = 1;
            //int inputTicketId_2 = 2;
            string relationFileName = $"{inputMovieId:N}_{inputTicketId_1}";

            Model.Ticket.Ticket tiketById_1 = new Model.Ticket.Ticket
            {
                Id = inputTicketId_1,
                Seat = 1,
                Price = 100
            };

            /*Model.Ticket.Ticket tiketById_2 = new Model.Ticket.Ticket
            {
                Id = inputTicketId_2,
                Seat = 2,
                Price = 200
            };*/

            //List<int> ticketsListResult = new List<int>() { tiketById_1.Id, tiketById_2.Id };

            Model.Movie.Movie movieById = new Model.Movie.Movie
            {
                Id = inputMovieId,
                Title = "Matrix",
                Director = "Wachowskis brothers",
                Poster = null
            };

            Model.MovieTicket.MovieTicket modelMovieTicketResult = new Model.MovieTicket.MovieTicket
            {
                MovieId = inputMovieId,
                TicketId = inputTicketId_1
            };

            await using var contextMovie = Host.Services.GetService<CosmosDbContext>();
            var postMovieResult = await contextMovie.Movies.AddAsync(new MovieDaoStep
            {
                Id = inputMovieId,
                Title = movieById.Title,
                Director = movieById.Director,
                Poster = movieById.Poster
            });
            await contextMovie.SaveChangesAsync();

            await using var contextTicket = Host.Services.GetService<AppDbContext>();
            var postTicketResult_1 = await contextTicket.Tickets.AddAsync(new TicketDao
            {
                Seat = tiketById_1.Seat,
                Price = tiketById_1.Price
            });
            await contextTicket.SaveChangesAsync();

            // Act
            var message = new HttpRequestMessage(
                HttpMethod.Post,
                $"api/v1/movietickets/{inputMovieId}/tickets/{inputTicketId_1}");

            var postResult = await _httpClient.SendAsync(message);

            //Assert
            postResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            postResult.Content.Should().NotBeNull();
            var postResponseModel = JsonConvert.DeserializeObject<Model.MovieTicket.MovieTicket>(
                await postResult.Content.ReadAsStringAsync());
            postResponseModel.Should().NotBeNull();
            postResponseModel.MovieId.Should().Be(modelMovieTicketResult.MovieId);
            postResponseModel.TicketId.Should().Be(modelMovieTicketResult.TicketId);
        }
    }
}

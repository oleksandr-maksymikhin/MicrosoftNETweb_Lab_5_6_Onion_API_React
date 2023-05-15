using BoxOffice.Dal.Ticket;
using BoxOffice.Dal;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using System.Linq;

namespace BoxOffice.IntegrationTests.Ticket
{
    public class GetByIdTicketTest : BaseTest
    {
        private readonly HttpClient _httpClient;
        public GetByIdTicketTest()
        {
            _httpClient = InitTestServer().GetClient();
        }

        [Fact]
        public async Task GetByIdAsync_IfNoExceptionOccurred_GetByIdEntityFromDb()
        {
            //Arrange
            var inputModel_1 = new Orchestrators.Ticket.Contract.CreateTicket
            {
                Row = 1,
                Seat = 4,
                Price = 400
            };
            var inputModel_2 = new Orchestrators.Ticket.Contract.CreateTicket
            {
                Row = 1,
                Seat = 5,
                Price = 500
            };

            await using var context = Host.Services.GetService<AppDbContext>();
            var postResult_1 = await context.Tickets.AddAsync(new TicketDao
            {
                Row = inputModel_1.Row,
                Seat = inputModel_1.Seat,
                Price = inputModel_1.Price
            });
            var postResult_2 = await context.Tickets.AddAsync(new TicketDao
            {
                Row = inputModel_2.Row,
                Seat = inputModel_2.Seat,
                Price = inputModel_2.Price
            });
            await context.SaveChangesAsync();
            //Act
            var message = new HttpRequestMessage(
                HttpMethod.Get,
                $"api/v1/tickets/{postResult_1.Entity.Id}");
            var getyIdResult = await _httpClient.SendAsync(message);

            //Assert
            getyIdResult.StatusCode.Should().Be(HttpStatusCode.OK);
            getyIdResult.Content.Should().NotBeNull();
            var getyIdResponseModel = JsonConvert.DeserializeObject<Model.Ticket.Ticket>(
                await getyIdResult.Content.ReadAsStringAsync());
            getyIdResponseModel.Should().NotBeNull();
            getyIdResponseModel.Id.Should().Be(postResult_1.Entity.Id);
            getyIdResponseModel.Row.Should().Be(inputModel_1.Row);
            getyIdResponseModel.Seat.Should().Be(inputModel_1.Seat);
            getyIdResponseModel.Price.Should().Be(inputModel_1.Price);

        }
    }
}

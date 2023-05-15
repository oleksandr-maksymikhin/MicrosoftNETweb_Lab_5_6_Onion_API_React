using BoxOffice.Dal.Ticket;
using BoxOffice.Dal;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;

namespace BoxOffice.IntegrationTests.Ticket
{
    public class DeleteTicketTest : BaseTest
    {
        private readonly HttpClient _httpClient;
        public DeleteTicketTest()
        {
            _httpClient = InitTestServer().GetClient();
        }

        [Fact]
        public async Task DeleteAsync_IfNoExceptionOccurred_DeleteEntityFromDb()
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
                HttpMethod.Delete,
                $"api/v1/tickets/{postResult_1.Entity.Id}");
            var deleteResult = await _httpClient.SendAsync(message);

            //Assert
            deleteResult.StatusCode.Should().Be(HttpStatusCode.OK);
            deleteResult.Content.Should().NotBeNull();
            var deleteResponseModel = JsonConvert.DeserializeObject<Model.Ticket.Ticket>(
                await deleteResult.Content.ReadAsStringAsync());
            deleteResponseModel.Should().NotBeNull();
            deleteResponseModel.Id.Should().Be(postResult_1.Entity.Id);
            deleteResponseModel.Row.Should().Be(inputModel_1.Row);
            deleteResponseModel.Seat.Should().Be(inputModel_1.Seat);
            deleteResponseModel.Price.Should().Be(inputModel_1.Price);
        }
    }
}

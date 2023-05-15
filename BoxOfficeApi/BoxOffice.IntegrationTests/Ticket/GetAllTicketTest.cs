using BoxOffice.Dal.Ticket;
using BoxOffice.Dal;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using FluentAssertions;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BoxOffice.IntegrationTests.Ticket
{
    public class GetAllTicketTest : BaseTest
    {
        private readonly HttpClient _httpClient;
        public GetAllTicketTest()
        {
            _httpClient = InitTestServer().GetClient();
        }

        [Fact]
        public async Task GetAllAsync_IfNoExceptionOccurred_GetAllEntityFromDb()
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
                $"api/v1/tickets");
            var getAllResult = await _httpClient.SendAsync(message);

            //Assert
            getAllResult.StatusCode.Should().Be(HttpStatusCode.OK);
            getAllResult.Content.Should().NotBeNull();
            var getAllResponseModel = JsonConvert.DeserializeObject<List<Model.Ticket.Ticket>>(
                await getAllResult.Content.ReadAsStringAsync());
            getAllResponseModel.Should().NotBeNull();
            int countInTickets = context.Tickets.Count();
            getAllResponseModel.Count.Should().Be(countInTickets);
        }

    }
}

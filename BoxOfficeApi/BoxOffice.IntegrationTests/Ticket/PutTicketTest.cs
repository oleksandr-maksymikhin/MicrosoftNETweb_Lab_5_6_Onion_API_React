using FluentAssertions;
using Newtonsoft.Json;
using BoxOffice.Dal;
using System.Net;
using System.Text;
using BoxOffice.Dal.Ticket;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BoxOffice.IntegrationTests.Ticket
{
    public class PutTicketTest : BaseTest
    {
        //private readonly HttpClient _httpClient;
        private HttpClient _httpClient;
        public PutTicketTest()
        {
            _httpClient = InitTestServer().GetClient();
        }

        [Fact]
        public async Task PutAsync_IfNoExceptionOccurred_UpdateEntityInDb()
        {
            //Arrange
            var inputModel = new Orchestrators.Ticket.Contract.CreateTicket
            {
                Row = 3,
                Seat = 3,
                Price = 300
            };

            EntityEntry<TicketDao> postResult = null;
            await using var context = Host.Services.GetService<AppDbContext>();
            postResult = await context.Tickets.AddAsync(new TicketDao
            {
                Row = inputModel.Row,
                Seat = inputModel.Seat,
                Price = inputModel.Price
            });
            await context.SaveChangesAsync();

            var modifiedModel = new Orchestrators.Ticket.Contract.EditTicket
            {
                Row = 4,
                Seat = 4,
                Price = 400
            };

            //Act
            var message = new HttpRequestMessage(
                HttpMethod.Put,
                $"api/v1/tickets/{postResult.Entity.Id}")
            {
                Content = new StringContent(JsonConvert.SerializeObject(modifiedModel), Encoding.UTF8, "application/json")
            };
            var putResult = await _httpClient.SendAsync(message);

            //Assert
            putResult.StatusCode.Should().Be(HttpStatusCode.OK);
            putResult.Content.Should().NotBeNull();
            var putResultModel = JsonConvert.DeserializeObject<Model.Ticket.Ticket>(
                await putResult.Content.ReadAsStringAsync());
            putResultModel.Should().NotBeNull();
            putResultModel.Id.Should().Be(postResult.Entity.Id);
            putResultModel.Row.Should().Be(modifiedModel.Row);
            putResultModel.Seat.Should().Be(modifiedModel.Seat);
            putResultModel.Price.Should().Be(modifiedModel.Price);
        }

    }
}

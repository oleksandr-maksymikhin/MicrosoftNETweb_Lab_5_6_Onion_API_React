using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.IntegrationTests.Ticket
{
    public class PostTicketTest : BaseTest
    {
        private readonly HttpClient _httpClient;
        public PostTicketTest()
        {
            _httpClient = InitTestServer().GetClient();
        }

        [Fact]
        public async Task PostAsync_IfNoExceptionOccurred_SavesNewEntityInDb()
        {
            // Arrange
            var inputModel = new Orchestrators.Ticket.Contract.CreateTicket
            {
                Row = 1,
                Seat = 2,
                Price = 200
            };

            // Act
            var message = new HttpRequestMessage(
                HttpMethod.Post,
                $"api/v1/tickets")
            {
                Content = new StringContent(JsonConvert.SerializeObject(inputModel), Encoding.UTF8, "application/json")
            };
            var postResult = await _httpClient.SendAsync(message);

            //Assert
            postResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            postResult.Content.Should().NotBeNull();
            var postResponseModel = JsonConvert.DeserializeObject<Model.Ticket.Ticket>(
                await postResult.Content.ReadAsStringAsync());
            postResponseModel.Should().NotBeNull();
            var savedEntity = AppDbContext.Tickets
                .FirstOrDefault(t =>
                    t.Row == inputModel.Row
                    && t.Seat == inputModel.Seat
                    && t.Price == inputModel.Price
                    && t.Id == postResponseModel.Id);
            savedEntity.Should().NotBeNull();
        }
    }
}

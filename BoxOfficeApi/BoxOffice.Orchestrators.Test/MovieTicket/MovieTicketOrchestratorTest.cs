using BoxOffice.Model.Movie;
using BoxOffice.Model.Ticket;
using BoxOffice.Orchestrators.Movie;
using BoxOffice.Orchestrators.MovieTicket;
using BoxOffice.Orchestrators.Ticket;
using BoxOffice.Platform.BlobStorage;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Orchestrators.Test.MovieTicket
{
    public class MovieTicketOrchestratorTest
    {
        /*static Guid inputMovieId = Guid.NewGuid();
        static int inputTicketId_1 = 1;
        static int inputTicketId_2 = 2;
        string relationFileName = $"{inputMovieId:N}_{inputTicketId_1}";

        static Model.Ticket.Ticket tiketById_1 = new Model.Ticket.Ticket
        {
            Id = inputTicketId_1,
            Seat = 1,
            Price = 100
        };

        static Model.Ticket.Ticket tiketById_2 = new Model.Ticket.Ticket
        {
            Id = inputTicketId_2,
            Seat = 2,
            Price = 200
        };

        static List<int> ticketsListResult = new List<int>() { tiketById_1.Id, tiketById_2.Id };

        static Model.Movie.Movie movieById = new Model.Movie.Movie
        {
            Id = inputMovieId,
            Name = "Matrix",
            Duration = 160
        };

        static Model.MovieTicket.MovieTicket modelResult = new Model.MovieTicket.MovieTicket
        {
            MovieId = inputMovieId,
            TicketId = inputTicketId_1
        };

        [Fact]
        public async Task CreateAsync_IfNoException_StoresEntityAndReturnsResult()
        {
            //Arrange
            var blobStorageMock = new Mock<IBlobStorage>();
            blobStorageMock
                .Setup(rm => rm.ContainsFileByNameAsync(relationFileName))
                .ReturnsAsync(false);
            blobStorageMock
                .Setup(rm => rm.PutContentAsync(relationFileName));

            var ticketOrchestratorMock = new Mock<ITicketOrchestrator>();
            ticketOrchestratorMock
                .Setup(rm => rm.GetByIdAsync(inputTicketId_1))
                .ReturnsAsync(tiketById_1);

            var movieOrchestratorMock = new Mock<IMovieOrchestrator>();
            movieOrchestratorMock
                .Setup(rm => rm.GetByIdAsync(inputMovieId))
                .ReturnsAsync(movieById);

            var movieTicketOrchestrator = new MovieTicketOrchestrator(
                    ticketOrchestratorMock.Object,
                    movieOrchestratorMock.Object,
                    blobStorageMock.Object);

            //Act
            var createResult = await movieTicketOrchestrator.CreateAsync(inputMovieId, inputTicketId_1);

            //Assert
            blobStorageMock.Verify(sar => sar.PutContentAsync(relationFileName), Times.Once);
            createResult.MovieId.Should().Be(modelResult.MovieId);
            createResult.TicketId.Should().Be(modelResult.TicketId);
        }

        [Fact]
        public async Task GetTicketsAsync_IfNoException_GetAllEntityAndReturnsResult()
        {
            //Arrange
            var blobStorageMock = new Mock<IBlobStorage>();
            blobStorageMock
                .Setup(rm => rm.FindByMovieAsync(inputMovieId))
                .ReturnsAsync(ticketsListResult);

            var ticketOrchestratorMock = new Mock<ITicketOrchestrator>();

            var movieOrchestratorMock = new Mock<IMovieOrchestrator>();
            movieOrchestratorMock
                .Setup(rm => rm.GetByIdAsync(inputMovieId))
                .ReturnsAsync(movieById);

            var movieTicketOrchestrator = new MovieTicketOrchestrator(
                    ticketOrchestratorMock.Object,
                    movieOrchestratorMock.Object,
                    blobStorageMock.Object);

            //Act
            List<int> GetTicketsAsyncResult = await movieTicketOrchestrator.GetTicketsAsync(inputMovieId);

            //Assert
            GetTicketsAsyncResult.Should().NotBeEmpty();
            GetTicketsAsyncResult.Should().Equal(ticketsListResult);
            GetTicketsAsyncResult.Count.Should().Be(ticketsListResult.Count);
        }*/
    }
}

using BoxOffice.Model.Ticket;
using BoxOffice.Orchestrators.Ticket;
using BoxOffice.Platform.Exception;
using FluentAssertions;
using Moq;
using System;

namespace BoxOffice.Orchestrators.Test.Ticket
{
    public class TicketOrchestratorTests
    {
        Model.Ticket.Ticket inputModel_1 = new Model.Ticket.Ticket
        {
            Row = 1,
            Seat = 1,
            Price = 100
        };
        Model.Ticket.Ticket inputModel_2 = new Model.Ticket.Ticket
        {
            Row = 2,
            Seat = 2,
            Price = 200
        };

        Model.Ticket.Ticket resultModel_1 = new Model.Ticket.Ticket
        {
            Id = 1,
            Row = 1,
            Seat = 1,
            Price = 100
        };
        Model.Ticket.Ticket resultModel_2 = new Model.Ticket.Ticket
        {
            Id = 2,
            Row = 2,
            Seat = 2,
            Price = 200
        };
        private List<Model.Ticket.Ticket> GetTestTickets()
        {
            List<Model.Ticket.Ticket> testTickts = new List<Model.Ticket.Ticket>
            {
                resultModel_1,
                resultModel_2
            };
            return testTickts;
        }
        
        [Fact]
        public async Task CreateAsync_IfNoException_StoresEntityAndReturnsResult()
        {
            //Arrange
            var repositoryMock = new Mock<ITicketRepository>();
            repositoryMock
                .Setup(rm => rm.CreateAsync(inputModel_1))
                .ReturnsAsync(resultModel_1);
            var orchestrator = new TicketOrchestrator(repositoryMock.Object);
            
            //Act
            var result = await orchestrator.CreateAsync(inputModel_1);

            //Assert
            result.Should().Be(resultModel_1);
            repositoryMock.Verify(sar => sar.CreateAsync(inputModel_1), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_IfNoException_GetAllEntityAndReturnsResult()
        {
            //Arrange
            var repositoryMock = new Mock<ITicketRepository>();
            repositoryMock
                .Setup(rm => rm.GetAllAsync())
                .ReturnsAsync(GetTestTickets());
            var orchestrator = new TicketOrchestrator(repositoryMock.Object);

            //Act
            List<Model.Ticket.Ticket> getAllResult = await orchestrator.GetAllAsync();

            //Assert
            getAllResult.Should().NotBeEmpty();
            getAllResult.Count.Should().Be(GetTestTickets().Count);
            getAllResult.Should().Equal(GetTestTickets());
        }

        [Fact]
        public async Task GetByIdAsync_IfNoException_GetByIdAsyncEntityAndReturnsResult()
        {
            //Arrange
            var repositoryMock = new Mock<ITicketRepository>();
            repositoryMock
                .Setup(rm => rm.GetByIdAsync(resultModel_1.Id))
                .ReturnsAsync(GetTestTickets().FirstOrDefault(t => t.Id == resultModel_1.Id));
            var orchestrator = new TicketOrchestrator(repositoryMock.Object);

            //Act
            Model.Ticket.Ticket getByIdResult = await orchestrator.GetByIdAsync(resultModel_1.Id);

            //Assert
            getByIdResult.Should().NotBeNull();
            getByIdResult.Should().Be(resultModel_1);
        }

        [Fact]
        public async Task GetByIdAsync_Exception_GetByIdAsyncEntityAndReturnsResult()
        {
            //Arrange
            ResourceNotFoundException exception = new ResourceNotFoundException($"Ticket with id = {resultModel_1.Id} not found");

            var repositoryMock = new Mock<ITicketRepository>();
            repositoryMock
                .Setup(rm => rm.GetByIdAsync(resultModel_1.Id))
                .ReturnsAsync(null as Model.Ticket.Ticket);

            var orchestrator = new TicketOrchestrator(repositoryMock.Object);

            //Act
            //Model.Ticket.Ticket getByIdResult = await orchestrator.GetByIdAsync(resultModel_1.Id);

            //Assert
            Exception ex = await Assert.ThrowsAsync(exception.GetType(), () => orchestrator.GetByIdAsync(resultModel_1.Id));
        }

        [Fact]
        public async Task DeleteAsync_IfNoException_DeleteAsyncEntityAndReturnsResult()
        {
            //Arrange

            var repositoryMock = new Mock<ITicketRepository>();
            repositoryMock
                .Setup(rm => rm.GetByIdAsync(resultModel_1.Id))
                .ReturnsAsync(resultModel_1);
            //repositoryMock
            //    .Setup(rm => rm.DeleteAsync(resultModel_1.Id));
            var orchestrator = new TicketOrchestrator(repositoryMock.Object);

            //Act
            Model.Ticket.Ticket deleteResult = await orchestrator.DeleteAsync(resultModel_1.Id);

            //Assert
            deleteResult.Should().NotBeNull();
            deleteResult.Should().Be(resultModel_1);
        }

        [Fact]
        public async Task UpdateAsync_IfNoException_UpdateAsyncEntityAndReturnsResult()
        {
            //Arrange
            Model.Ticket.Ticket resultModel_1_update = new Model.Ticket.Ticket
            {
                Id = 1,
                Row = 1,
                Seat = 1,
                Price = 300
            };

            var repositoryMock = new Mock<ITicketRepository>();

            //repositoryMock
            //    .Setup(rm => rm.UpdateAsync(updateModel))
            //    .ReturnsAsync(updateModel);
            repositoryMock
                .Setup(rm => rm.UpdateAsync(resultModel_1))
                .ReturnsAsync(resultModel_1_update);

            repositoryMock
                .Setup(rm => rm.GetByIdAsync(resultModel_1.Id))
                .ReturnsAsync(resultModel_1);

            var orchestrator = new TicketOrchestrator(repositoryMock.Object);

            //Act
            Model.Ticket.Ticket updateResult = await orchestrator.UpdateAsync(resultModel_1.Id, resultModel_1_update);

            //Assert
            updateResult.Should().NotBeNull();
            updateResult.Should().Be(resultModel_1_update);
        }



    }
}
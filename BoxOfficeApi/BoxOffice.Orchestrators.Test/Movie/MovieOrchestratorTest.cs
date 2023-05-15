using BoxOffice.Model.Movie;
using BoxOffice.Model.Ticket;
using BoxOffice.Orchestrators.Movie;
using BoxOffice.Orchestrators.Ticket;
using BoxOffice.Platform.Exception;
using BoxOffice.Platform.ServiceBus;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Orchestrators.Test.Movie
{
    public class MovieOrchestratorTest
    {

        Model.Movie.Movie inputModel_1 = new Model.Movie.Movie
        {
            Title = "Interstellar",
            Director = "Christopher Nolan",
            Poster = null
        };
        Model.Movie.Movie inputModel_2 = new Model.Movie.Movie
        {
            Title = "Matrix",
            Director = "Wachowskis brothers",
            Poster = null
        };

        Model.Movie.Movie resultModel_1 = new Model.Movie.Movie
        {
            Id = Guid.NewGuid(),
            Title = "Interstellar",
            Director = "Christopher Nolan",
            Poster = null
        };
        Model.Movie.Movie resultModel_2 = new Model.Movie.Movie
        {
            Id = Guid.NewGuid(),
            Title = "Matrix",
            Director = "Wachowskis brothers",
            Poster = null
        };

        private List<Model.Movie.Movie> GetTestTickets()
        {
            List<Model.Movie.Movie> testMovies = new List<Model.Movie.Movie>
            {
                resultModel_1,
                resultModel_2
            };
            return testMovies;
        }

        [Fact]
        public async Task CreateAsync_IfNoException_StoresEntityAndReturnsResult()
        {
            //Arrange
            var repositoryMock = new Mock<IMovieRepository>();
            repositoryMock
                .Setup(rm => rm.CreateAsync(inputModel_1))
                .ReturnsAsync(resultModel_1);

            var publisherMock = new Mock<IPublisher>();
            publisherMock
                .Setup(rm => rm.PublishAsync(resultModel_1.Id))
                .Returns(Task.FromResult(5));

            var orchestrator = new MovieOrchestrator(repositoryMock.Object, publisherMock.Object);

            //Act
            var createResult = await orchestrator.CreateAsync(inputModel_1);

            //Assert
            createResult.Should().Be(resultModel_1);
            repositoryMock.Verify(sar => sar.CreateAsync(inputModel_1), Times.Once);
            createResult.Title.Should().Be(resultModel_1.Title);
            createResult.Director.Should().Be(resultModel_1.Director);
            createResult.Poster.Should().Be(resultModel_1.Poster);
        }

        [Fact]
        public async Task GetAllAsync_IfNoException_GetAllEntityAndReturnsResult()
        {
            //Arrange
            var repositoryMock = new Mock<IMovieRepository>();
            repositoryMock
                .Setup(rm => rm.GetAllAsync())
                .ReturnsAsync(GetTestTickets());

            var publisherMock = new Mock<IPublisher>();
            publisherMock
                .Setup(rm => rm.PublishAsync(resultModel_1.Id))
                .Returns(Task.FromResult(5));

            var orchestrator = new MovieOrchestrator(repositoryMock.Object, publisherMock.Object);

            //Act
            List<Model.Movie.Movie> getAllResult = await orchestrator.GetAllAsync();

            //Assert
            getAllResult.Should().NotBeEmpty();
            getAllResult.Count.Should().Be(GetTestTickets().Count);
            getAllResult.Should().Equal(GetTestTickets());
        }

        [Fact]
        public async Task GetByIdAsync_IfNoException_GetByIdAsyncEntityAndReturnsResult()
        {
            //Arrange
            var repositoryMock = new Mock<IMovieRepository>();
            repositoryMock
                .Setup(rm => rm.GetByIdAsync(resultModel_1.Id))
                .ReturnsAsync(GetTestTickets().FirstOrDefault(t => t.Id == resultModel_1.Id));

            var publisherMock = new Mock<IPublisher>();
            publisherMock
                .Setup(rm => rm.PublishAsync(resultModel_1.Id))
                .Returns(Task.FromResult(5));

            var orchestrator = new MovieOrchestrator(repositoryMock.Object, publisherMock.Object);

            //Act
            Model.Movie.Movie getByIdResult = await orchestrator.GetByIdAsync(resultModel_1.Id);

            //Assert
            getByIdResult.Should().NotBeNull();
            getByIdResult.Should().Be(resultModel_1);
        }

        [Fact]
        public async Task GetByIdAsync_Exception_GetByIdAsyncEntityAndReturnsResult()
        {
            //Arrange
            ResourceNotFoundException exception = new ResourceNotFoundException($"Movie with id = {resultModel_1.Id} not found");

            var repositoryMock = new Mock<IMovieRepository>();
            repositoryMock
                .Setup(rm => rm.GetByIdAsync(resultModel_1.Id))
                .ReturnsAsync(null as Model.Movie.Movie);

            var publisherMock = new Mock<IPublisher>();
            publisherMock
                .Setup(rm => rm.PublishAsync(resultModel_1.Id))
                .Returns(Task.FromResult(5));

            var orchestrator = new MovieOrchestrator(repositoryMock.Object, publisherMock.Object);

            //Act
            //Model.Movie.Movie getByIdResult = await orchestrator.GetByIdAsync(resultModel_1.Id);

            //Assert
            Exception ex = await Assert.ThrowsAsync(exception.GetType(), () => orchestrator.GetByIdAsync(resultModel_1.Id));
        }

        [Fact]
        public async Task DeleteAsync_IfNoException_DeleteAsyncEntityAndReturnsResult()
        {
            //Arrange

            var repositoryMock = new Mock<IMovieRepository>();
            repositoryMock
                .Setup(rm => rm.GetByIdAsync(resultModel_1.Id))
                .ReturnsAsync(resultModel_1);
            //repositoryMock
            //    .Setup(rm => rm.DeleteAsync(resultModel_1.Id));

            var publisherMock = new Mock<IPublisher>();
            publisherMock
                .Setup(rm => rm.PublishAsync(resultModel_1.Id))
                .Returns(Task.FromResult(5));

            var orchestrator = new MovieOrchestrator(repositoryMock.Object, publisherMock.Object);

            //Act
            Model.Movie.Movie deleteResult = await orchestrator.DeleteAsync(resultModel_1.Id);

            //Assert
            deleteResult.Should().NotBeNull();
            deleteResult.Should().Be(resultModel_1);
        }

        [Fact]
        public async Task UpdateAsync_IfNoException_UpdateAsyncEntityAndReturnsResult()
        {
            //Arrange
            Model.Movie.Movie resultModel_1_update = new Model.Movie.Movie
            {
                Id = resultModel_1.Id,
                Title = "Interstellar-2",
                Director = "Christopher Nolan",
                Poster = null
            };

            var repositoryMock = new Mock<IMovieRepository>();
            repositoryMock
                .Setup(rm => rm.UpdateAsync(resultModel_1_update))
                .ReturnsAsync(resultModel_1_update);
            repositoryMock
                .Setup(rm => rm.GetByIdAsync(resultModel_1.Id))
                .ReturnsAsync(resultModel_1);

            var publisherMock = new Mock<IPublisher>();
            publisherMock
                .Setup(rm => rm.PublishAsync(resultModel_1.Id))
                .Returns(Task.FromResult(5));

            var orchestrator = new MovieOrchestrator(repositoryMock.Object, publisherMock.Object);

            //Act
            Model.Movie.Movie updateResult = await orchestrator.UpdateAsync(resultModel_1_update);

            //Assert
            updateResult.Should().NotBeNull();
            updateResult.Should().Be(resultModel_1_update);
        }

    }
}

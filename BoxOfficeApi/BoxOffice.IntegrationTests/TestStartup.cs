using Azure;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs.Models;
using BoxOffice.Api;
using BoxOffice.Dal;
using BoxOffice.Platform.BlobStorage;
using BoxOffice.Platform.ServiceBus;
using EntityFrameworkCore.Testing.Common.Helpers;
using EntityFrameworkCore.Testing.Moq.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoxOffice.IntegrationTests;

public class TestStartup : Startup
{
    static Guid inputMovieId = Guid.Parse("93094f7d-3926-483f-9f44-3e4cf28929a2");
    static int inputTicketId_1 = 1;
    static int inputTicketId_2 = 2;
    string relationFileName = $"{inputMovieId:N}_{inputTicketId_1}";
    List<string> listResult = new List<string>
            {
                "93094f7d-3926-483f-9f44-3e4cf28929a1",
                "93094f7d-3926-483f-9f44-3e4cf28929a2",
                "93094f7d-3926-483f-9f44-3e4cf28929a3",
            };

    static Model.Ticket.Ticket tiketById_1 = new Model.Ticket.Ticket
    {
        Id = inputTicketId_1,
        Row = 1,
        Seat = 1,
        Price = 100
    };

    static Model.Ticket.Ticket tiketById_2 = new Model.Ticket.Ticket
    {
        Id = inputTicketId_2,
        Row = 2,
        Seat = 2,
        Price = 200
    };

    static List<int> ticketsListResult = new List<int>() { tiketById_1.Id, tiketById_2.Id };

    static Model.Movie.Movie movieById = new Model.Movie.Movie
    {
        Id = inputMovieId,
        Title = "Interstellar",
        Director = "Christopher Nolan",
        Poster = null
    };

    static Model.MovieTicket.MovieTicket modelResult = new Model.MovieTicket.MovieTicket
    {
        MovieId = inputMovieId,
        TicketId = inputTicketId_1
    };


    public TestStartup(IConfiguration configuration)
        : base(configuration)
    {
    }

    protected override void SetEdgeCommunicationDependencies(IServiceCollection services)
    {
        //var mockResponse = new Mock<Azure.Response<Azure.Storage.Blobs.Models.BlobContentInfo>>();
        //var mockResponse = new Mock<Response<BlobContentInfo>>();
        //https://programtalk.com/csharp-examples/System.IO.Stream.Flush()/
        //BlobContentInfo mockedBlobInfo = BlobsModelFactory.BlobContentInfo(new ETag("ETagSuccess"), DateTime.Now, new byte[1], DateTime.Now.ToUniversalTime().ToString(), "encryptionKeySha256", "encryptionScope", 1);
        //Mock<Response<BlobContentInfo>> mockResponse = new Mock<Response<BlobContentInfo>>();

        var blobStorageMock = new Mock<IBlobStorage>();
        blobStorageMock
            .Setup(rm => rm.ContainsFileByNameAsync(relationFileName))
            .ReturnsAsync(false);
        //just return some Task to avoid NullReferenceException
        blobStorageMock
             .Setup(rm => rm.PutContentAsync(relationFileName))
             .Returns(Task.FromResult(5));
        blobStorageMock
            .Setup(rm => rm.FindByMovieAsync(inputMovieId))
            .ReturnsAsync(ticketsListResult);

        services.AddSingleton(blobStorageMock.Object);
    }

    protected override void ConfigureEdgeService(IServiceCollection services)
    {
        var serviceBusClientMock = new Mock<ServiceBusClient>();
        services.AddSingleton(serviceBusClientMock.Object);

        var publisherMock = new Mock<IPublisher>();
        publisherMock
                .Setup(rm => rm.PublishAsync(movieById.Id))
                .Returns(Task.FromResult(5));
        services.AddSingleton(publisherMock.Object);

        var subscriberMock = new Mock<ISubscriber>();
        subscriberMock
            .Setup(rm => rm.Data)
            .Returns(listResult);

        services.AddSingleton(subscriberMock.Object);
    }

    protected override void ConfigureDb(IServiceCollection services)
    {
        var context = ConfigureDb<AppDbContext>().MockedDbContext;
        services.AddSingleton<AppDbContext>(c => context);
        //services.AddScoped<AppDbContext>(c => context);
        //services.AddTransient<AppDbContext>(c => context);
        var cosmosDbContext = ConfigureDb<CosmosDbContext>().MockedDbContext;
        services.AddSingleton<CosmosDbContext>(c => cosmosDbContext);
        //services.AddScoped<CosmosDbContext>(c => cosmosDbContext);
        //services.AddTransient<CosmosDbContext>(c => cosmosDbContext);
    }

    private IMockedDbContextBuilder<T> ConfigureDb<T>()
        where T : DbContext
    {
        var options = new DbContextOptionsBuilder<T>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var dbContextToMock = (T)Activator.CreateInstance(typeof(T), options);
        return new MockedDbContextBuilder<T>()
            .UseDbContext(dbContextToMock)
            .UseConstructorWithParameters(options);
    }
}
using Azure.Messaging.ServiceBus;
using BoxOffice.Dal.Movie;
using BoxOffice.Dal.Ticket;
using BoxOffice.Model.Movie;
using BoxOffice.Model.MovieStats;
using BoxOffice.Model.MovieTicket;
using BoxOffice.Model.Ticket;
using BoxOffice.Orchestrators.Movie;
using BoxOffice.Orchestrators.MovieStatsOrchestrator;
using BoxOffice.Orchestrators.MovieTicket;
using BoxOffice.Orchestrators.Ticket;
using BoxOffice.Platform.BlobStorage;
using BoxOffice.Platform.Exception;
using BoxOffice.Platform.ServiceBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BoxOffice.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            //**************
            services.AddSwaggerGen();

            //Orchestrator
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<ITicketOrchestrator, TicketOrchestrator>();
            services.AddScoped<IMovieOrchestrator, MovieOrchestrator>();
            services.AddScoped<IMovieStatsOrchestrator, MovieStatsOrchestrator>();
            services.AddScoped<IMovieTicketOrchestrator, MovieTicketOrchestrator>();
            services.AddScoped<StatsContainer, StatsContainer>();

            //Dal
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            ConfigureDb(services);

            var config = new BlobConfiguration();
            _configuration.Bind("AzureBlobConnectionString", config);
            services.AddSingleton(config);

            //misc
            SetEdgeCommunicationDependencies(services);
            ConfigureEdgeService(services);

            //to override the error of "Access to XMLHttpRequest at 'http://localhost:5127/api/v1/movies' from origin 'http://localhost:3000' has been blocked by CORS policy: No 'Access-Control-Allow-Origin' header is present on the requested resource."
            services.AddCors(p => p.AddPolicy("corsapp", builder =>
            {
                builder.WithOrigins(
                    "http://192.168.1.100:4200", 
                    "http://localhost:4200", 
                    "http://localhost:3000", 
                    "http://localhost:3001", 
                    "http://localhost:3002")
                .AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            }));
        }

        protected virtual void ConfigureEdgeService(IServiceCollection services)
        {
            services.AddTransient(
                            typeof(ServiceBusClient),
                            builder => new ServiceBusClient(_configuration.GetConnectionString("ServiceBusConnectionString")));
            services.AddScoped<IPublisher, MovieStatsPublisher>();
            //services.AddSingleton<ISubscriber, MovieStatsSubscriber>();
            /*services.AddSingleton<ISubscriber> ( builder => 
                new MovieStatsSubscriber ( 
                    services
                        .BuildServiceProvider()
                        .GetRequiredService<ServiceBusClient>()
                    )
                );*/

            //method is not Async -> GetAwaiter().GetResult()
            /*services
                .BuildServiceProvider()
                .GetService<ISubscriber>()
                .SubscribeAsync()
                .GetAwaiter()
                .GetResult();*/

            var subscriber = new MovieStatsSubscriber(
                    (ServiceBusClient)services
                    .BuildServiceProvider()
                    .GetService(typeof(ServiceBusClient))
                );
                //.SubscribeAsync();

            //use non-asynchronous method in asynchronous
            subscriber.SubscribeAsync().GetAwaiter().GetResult();

            services.AddSingleton<ISubscriber>(subscriber);
        }

        protected virtual void SetEdgeCommunicationDependencies(IServiceCollection services)
        {
            services.AddScoped<IBlobStorage, BlobStorage>();
        }

        protected virtual void ConfigureDb(IServiceCollection services)
        {
            string SQLServConnectionString = _configuration.GetConnectionString("DatabaseConnectionString");
            services.AddSqlServer<Dal.AppDbContext>
                (_configuration.GetConnectionString("DatabaseConnectionString"));
            services.AddCosmos<Dal.CosmosDbContext>
                (_configuration.GetConnectionString("CosmosConnectionString")!, "ToDoList");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();
            //************
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors("corsapp");

            app.UseRouting();
            app.UseEndpoints(action => action.MapControllers());
        }

    }
}

using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Platform.ServiceBus
{
    public class MovieStatsSubscriber : ISubscriber
    {
        private readonly ServiceBusClient _client;
        private ServiceBusProcessor _subscriber;
        protected virtual string QueueName => "movie-stats";
        //private List<Guid> _guids = new List<Guid>();
        private List<string> _guids = new List<string>();
        public List<string> Data 
        {
            get { return _guids; } 
        }

        public MovieStatsSubscriber(ServiceBusClient client)
        {
            _client = client;
            
        }

        protected virtual async Task ProcessAsync(ProcessMessageEventArgs arg)
        {
            //_guids.Add(Guid.Parse(arg.Message.Body.ToString()));
            _guids.Add(arg.Message.Body.ToString());
            await arg.CompleteMessageAsync(arg.Message);
        }

        public async Task SubscribeAsync()
        {
            
            _subscriber = _client.CreateProcessor(QueueName);
            _subscriber.ProcessMessageAsync += ProcessAsync;
            
            //subscribe for errors
            _subscriber.ProcessErrorAsync += HandleErrorAsync;

            await _subscriber?.StartProcessingAsync();
        }

        private async Task HandleErrorAsync(ProcessErrorEventArgs arg)
        {
            Console.WriteLine($"Error: {arg.Exception.Message}");
        }

        /*public void Subscribe()
        {
            //ISubscriber is Singleton service (othervise create field and DI via ctor)
            _subscriber = _client.CreateProcessor(QueueName);
            _subscriber.ProcessMessageAsync += ProcessAsync;

            //subscriber.StartProcessingAsync();
        }*/
    }
}

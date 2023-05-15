using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Platform.ServiceBus
{
    public class MovieStatsPublisher : IPublisher
    {
        private readonly ServiceBusClient _client;
        protected virtual string QueueName => "movie-stats";
        private readonly ServiceBusSender _publisher;
        public MovieStatsPublisher(ServiceBusClient client)
        {
            _client = client;
            _publisher = _client.CreateSender(QueueName);
        }

        public async Task PublishAsync(Guid guid)
        {
            await _publisher.SendMessageAsync(new ServiceBusMessage(guid.ToString("N")));
        }
    }
}

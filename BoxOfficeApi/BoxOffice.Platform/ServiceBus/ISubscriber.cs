using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Platform.ServiceBus
{
    public interface ISubscriber
    {
        //void Subscribe();
        Task SubscribeAsync();
        public List<string> Data { get; }
    }
}

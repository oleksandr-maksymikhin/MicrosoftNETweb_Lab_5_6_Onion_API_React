using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Platform.ServiceBus
{
    public interface IPublisher
    {
        Task PublishAsync(Guid guid);
    }
}

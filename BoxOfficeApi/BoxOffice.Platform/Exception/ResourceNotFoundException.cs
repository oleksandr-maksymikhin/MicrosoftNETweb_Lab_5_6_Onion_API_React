using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Platform.Exception
{
    public class ResourceNotFoundException: System.Exception
    {
        public ResourceNotFoundException(string message): base(message) 
        { 
        }
    }
}

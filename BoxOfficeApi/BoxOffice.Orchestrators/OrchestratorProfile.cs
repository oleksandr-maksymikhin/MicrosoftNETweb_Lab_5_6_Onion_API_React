using AutoMapper;
using BoxOffice.Orchestrators.Ticket.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Orchestrators
{
    internal class OrchestratorProfile: Profile
    {
        public OrchestratorProfile()
        {
            CreateMap<CreateTicket, Model.Ticket.Ticket>().ReverseMap();
            CreateMap<EditTicket,  Model.Ticket.Ticket>().ReverseMap();
            CreateMap<CreateMovie, Model.Movie.Movie>().ReverseMap();
            CreateMap<EditMovie, Model.Movie.Movie>().ReverseMap();
        }
    }
}

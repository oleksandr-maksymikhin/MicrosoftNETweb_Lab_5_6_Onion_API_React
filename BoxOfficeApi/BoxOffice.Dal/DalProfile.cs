using AutoMapper;
using BoxOffice.Dal.Movie;
using BoxOffice.Dal.Ticket;


namespace BoxOffice.Dal
{
    internal class DalProfile: Profile
    {
        public DalProfile()
        {
            CreateMap<TicketDao, Model.Ticket.Ticket>().ReverseMap();
            CreateMap<MovieDaoStep, Model.Movie.Movie>().ReverseMap();
            //CreateMap<MovieDaoStep, Model.Movie.Movie>().ReverseMap();
        }
    }
}

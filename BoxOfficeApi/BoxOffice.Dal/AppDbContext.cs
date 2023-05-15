using BoxOffice.Dal.Ticket;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Dal
{
    public class AppDbContext: DbContext
    {
        public DbSet<TicketDao> Tickets { get; set; }

        public AppDbContext (DbContextOptions<AppDbContext> options): base(options)
        {
            Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}

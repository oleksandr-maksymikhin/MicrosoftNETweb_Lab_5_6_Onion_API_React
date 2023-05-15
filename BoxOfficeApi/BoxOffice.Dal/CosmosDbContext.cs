using Microsoft.EntityFrameworkCore;
using BoxOffice.Dal.Movie;

namespace BoxOffice.Dal
{
    public class CosmosDbContext : DbContext
    {
        public CosmosDbContext(DbContextOptions<CosmosDbContext> options) : base(options)
        {
        }

        public DbSet<MovieDaoStep> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("Items");
            //modelBuilder.HasDefaultContainer("StepWebProject");
        }
    }
}

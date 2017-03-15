using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OnDemandTools.Jobs.Models;

namespace OnDemandTools.Jobs.Helpers
{
    public partial class OnDemandReportingContext : DbContext
    {
        public static string ConnectionString { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        public virtual DbSet<Airing> Airings { get; set; }
        public virtual DbSet<AiringDestination> AiringDestinations { get; set; }
        public virtual DbSet<AiringTitle> AiringTitles { get; set; }
        public virtual DbSet<DestinationFlight> DestinationFlights { get; set; }
    }
}

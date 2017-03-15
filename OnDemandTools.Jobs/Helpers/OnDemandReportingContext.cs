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

        public virtual DbSet<Airing> Airing { get; set; }
        public virtual DbSet<AiringDestination> AiringDestination { get; set; }
        public virtual DbSet<AiringTitle> AiringTitle { get; set; }
        public virtual DbSet<DestinationFlight> DestinationFlight { get; set; }
    }
}

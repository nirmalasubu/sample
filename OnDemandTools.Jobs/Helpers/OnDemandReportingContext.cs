using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Jobs.Models;

namespace OnDemandTools.Jobs.Helpers
{

    public partial class OnDemandReportingContext : DbContext
    {



        string connectionString;
        public OnDemandReportingContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }



        public virtual DbSet<Airing> Airing { get; set; }
        public virtual DbSet<AiringDestination> AiringDestination { get; set; }
        public virtual DbSet<AiringTitle> AiringTitle { get; set; }
        public virtual DbSet<DestinationFlight> DestinationFlight { get; set; }
    }
}

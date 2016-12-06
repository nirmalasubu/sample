using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTPOCHarbor.Models
{

    public class JustCheck
    {
        public int Id { get; set; }


        public string Airing { get; set; }
        public string PostMessage { get; set; }        
    }


    public class ApplicationDbContext: DbContext
    {
        public DbSet<JustCheck> JustCheck { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=nonprod-sead.database.windows.net;database=ODT-QA;persist security info=True;user id=ondemanduser;password=Ndmndsr1!;MultipleActiveResultSets=True");
        }
    }
}

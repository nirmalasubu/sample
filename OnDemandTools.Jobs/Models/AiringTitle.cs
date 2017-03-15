using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Jobs.Models
{
    public class AiringTitle
    {
        public int Id { get; set; }
        public string AiringId { get; set; }
        public int TitleId { get; set; }

        public virtual Airing Airing { get; set; }
    }
}

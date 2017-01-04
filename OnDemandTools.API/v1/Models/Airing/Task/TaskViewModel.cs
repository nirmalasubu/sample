using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.API.v1.Models.Airing.Task
{
    public class TaskViewModel
    {
        public List<string> AiringIds { get; set; }
        public List<string> Tasks { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.API.v1.Models.Airing.Update
{
    public class PlayListRequest
    {
        public IList<PlayItem> PlayList { get; set; }

        public string ReleasedBy { get; set; }

        public PlayListRequest()
        {
            PlayList = new List<PlayItem>();
            ReleasedBy = string.Empty;
        }
    }
}

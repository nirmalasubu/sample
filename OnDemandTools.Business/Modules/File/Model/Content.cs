using System;
using System.Collections.Generic;

namespace OnDemandTools.Business.Modules.File.Model
{ /// <summary>
  /// Model elements designated for Encoding
  /// </summary>
    public class Content
    {
        public Content()
        {
            MediaCollection = new List<Media>();
        }

        public String Name { get; set; }
        public List<Media> MediaCollection { get; set; }
    }
}

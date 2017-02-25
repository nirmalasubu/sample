
using System;
using OnDemandTools.Common.Model;
using System.Collections.Generic;


namespace OnDemandTools.Business.Modules.Package.Model
{

    public class Package: IModel
    {
        public Package()
        {
            TitleIds = new List<int>();
            ContentIds = new List<string>();
        }
        public List<int> TitleIds { get; set; }

        public List<string> ContentIds { get; set; }

        public string DestinationCode { get; set; }

        public string Type { get; set; }
                
        public object PackageData { get; set; }

        public string Data { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedDateTime { get; set; }
    }
}
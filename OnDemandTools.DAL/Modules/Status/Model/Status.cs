using MongoDB.Bson;
using OnDemandTools.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.DAL.Modules.Status.Model
{
  
    public class Status : IModel
    {
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string User { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedDateTime { get; set; }
    }
}

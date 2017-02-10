
using System;

namespace OnDemandTools.Business.Modules.ModifiedTitles.Model
{
    public class UpdatedTitle
    {
        public string _id { get; set; }
        public int TitleId { get; set; }
        public DateTime ProcessedDatetimeUTC { get; set; }
    }
}

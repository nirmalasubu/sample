using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.DAL.Modules.Airings.Model
{
    public class Property
    {
        public Property()
        {
            Brands = new List<string>();
            TitleIds = new List<int>();
            SeriesIds = new List<int>();
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public List<string> Brands { get; set; }

        public List<int> TitleIds { get; set; }

        public List<int> SeriesIds { get; set; }

        #region Serialisation

        public bool ShouldSerializeTitleIds()
        {
            return TitleIds.Any();
        }

        public bool ShouldSerializeSeriesIdss()
        {
            return SeriesIds.Any();
        }
        #endregion
    }

}
using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.Business.Modules.Airing.Model.Alternate.Title
{
    public class Rating
    {
        public List<RatingDescriptor> RatingDescriptors { get; set; }
        public string RatingSystem { get; set; }

        public Rating()
        {
            RatingDescriptors = new List<RatingDescriptor>();
        }

        public string GetDigitalRating(string linearNetworkCode)
        {
            return GetDigitalRatingDescriptor(linearNetworkCode).Rating;
        }

        public RatingDescriptor GetDigitalRatingDescriptor(string linearNetworkCode)
        {
            return RatingDescriptors.Count > 0
                ? (RatingDescriptors.Any(rd => (rd.NetworkCode == linearNetworkCode)))
                    ? RatingDescriptors.First(rd => (rd.NetworkCode == linearNetworkCode))
                    : RatingDescriptors.Any(rd => string.IsNullOrEmpty(rd.NetworkCode))
                        ? RatingDescriptors.First(rd => string.IsNullOrEmpty(rd.NetworkCode))
                        : new RatingDescriptor()
                : new RatingDescriptor();
        }
    }
}

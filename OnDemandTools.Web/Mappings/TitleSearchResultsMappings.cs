using AutoMapper;
using OnDemandTools.Business.Modules.Airing.Model.Alternate.Title;
using OnDemandTools.Web.Models.TitleSearch;

namespace OnDemandTools.Web.Mappings
{
    public class TitleSearchResultsMappings : Profile
    {
        public TitleSearchResultsMappings()
        {
            CreateMap<Title, TitleShort>()
                .ForSourceMember(d => d.ExternalSources, opt => opt.Ignore())
                .ForSourceMember(d => d.Genres, opt => opt.Ignore())
                .ForSourceMember(d => d.OtherNames, opt => opt.Ignore())
                .ForSourceMember(d => d.Participants, opt => opt.Ignore())
                .ForSourceMember(d => d.Ratings, opt => opt.Ignore())
                .ForSourceMember(d => d.SeasonEpisodeNumber, opt => opt.Ignore())
                .ForSourceMember(d => d.SeasonNumber, opt => opt.Ignore())
                .ForSourceMember(d => d.SeriesItemNumber, opt => opt.Ignore())
                .ForSourceMember(d => d.SeriesTitleId, opt => opt.Ignore())
                .ForSourceMember(d => d.Storylines, opt => opt.Ignore())
                .ForSourceMember(d => d.EpisodeNumber, opt => opt.Ignore())
                .ForSourceMember(d => d.Keywords, opt => opt.Ignore());
        }
    }
}

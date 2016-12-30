using MongoDB.Bson.Serialization.Conventions;

namespace OnDemandTools.Utilities
{
    public static class MongoBoostrapper
    {

        public static void Setup()
        {
            var pack = new ConventionPack
            {
                new IgnoreIfDefaultConvention(true)
                           
            };

            ConventionRegistry.Register("airing", pack, type => type == typeof(DAL.Modules.Airings.Model.Airing));
            ConventionRegistry.Register("title", pack, type => type == typeof(DAL.Modules.Airings.Model.Title));
            ConventionRegistry.Register("destination", pack, type => type == typeof(DAL.Modules.Airings.Model.Destination));
            ConventionRegistry.Register("version", pack, type => type == typeof(DAL.Modules.Airings.Model.Version));
            ConventionRegistry.Register("playItem", pack, type => type == typeof(DAL.Modules.Airings.Model.PlayItem));
            ConventionRegistry.Register("package", pack, type => type == typeof(DAL.Modules.Airings.Model.Package));
            ConventionRegistry.Register("episode", pack, type => type == typeof(DAL.Modules.Airings.Model.Episode));
            ConventionRegistry.Register("element", pack, type => type == typeof(DAL.Modules.Airings.Model.Element));
            ConventionRegistry.Register("flight", pack, type => type == typeof(DAL.Modules.Airings.Model.Flight));
            ConventionRegistry.Register("series", pack, type => type == typeof(DAL.Modules.Airings.Model.Series));
            ConventionRegistry.Register("rating", pack, type => type == typeof(DAL.Modules.Airings.Model.TVRating));
            ConventionRegistry.Register("story", pack, type => type == typeof(DAL.Modules.Airings.Model.Story));
            ConventionRegistry.Register("series", pack, type => type == typeof(DAL.Modules.Airings.Model.Participant));
            ConventionRegistry.Register("genre", pack, type => type == typeof(DAL.Modules.Airings.Model.Genre));
            ConventionRegistry.Register("guideCategory", pack, type => type == typeof(DAL.Modules.Airings.Model.GuideCategory));
            ConventionRegistry.Register("package", pack, type => type == typeof(DAL.Modules.Airings.Model.Package));
            ConventionRegistry.Register("product", pack, type => type == typeof(DAL.Modules.Airings.Model.Product));
            ConventionRegistry.Register("productCode", pack, type => type == typeof(DAL.Modules.Airings.Model.ProductCode));
            ConventionRegistry.Register("programType", pack, type => type == typeof(DAL.Modules.Airings.Model.ProgramType));
            ConventionRegistry.Register("providerContentTier", pack, type => type == typeof(DAL.Modules.Airings.Model.ProviderContentTier));
            ConventionRegistry.Register("season", pack, type => type == typeof(DAL.Modules.Airings.Model.Season));
            ConventionRegistry.Register("turniverse", pack, type => type == typeof(DAL.Modules.Airings.Model.Turniverse));
            ConventionRegistry.Register("titleId", pack, type => type == typeof(DAL.Modules.Airings.Model.TitleId));
            ConventionRegistry.Register("closedCaptioning", pack, type => type == typeof(DAL.Modules.Airings.Model.ClosedCaptioning));

            ConventionRegistry.Register("file", pack, type => type == typeof(DAL.Modules.File.Model.File));
        }
    }
}

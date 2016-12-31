using System.Collections.Generic;
using System.Linq;

using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using System.Text.RegularExpressions;
using OnDemandTools.DAL.Modules.Pathing.Model;
using System;
using OnDemandTools.DAL.Database;
using MongoDB.Driver;

namespace OnDemandTools.DAL.Modules.Pathing.Queries
{
    public class PathTranslationQueries : IPathTranslationQueries
    {
        private readonly MongoCollection<Model.PathTranslation> _pathtranslationCollection;

        public PathTranslationQueries(IODTDatastore connection)
        {
            _pathtranslationCollection = connection.GetDatabase().GetCollection<PathTranslation>("PathTranslation");
        }

        /// <summary>
        /// Return translations that match the parameter values.
        /// </summary>
        /// <param name="sourceBaseUrl"></param>
        /// <param name="sourceBrand"></param>
        /// <returns></returns>
        List<Model.PathTranslation> IPathTranslationQueries.GetBySourceBaseUrlAndBrand(String sourceBaseUrl, String sourceBrand)
        {
            var qURL = Query.Matches("Source.BaseUrl", new BsonRegularExpression("^" + Regex.Escape(sourceBaseUrl.ToString()) + "$", "i"));
            var qBrand = Query.Matches("Source.Brand", new BsonRegularExpression("^" + Regex.Escape(sourceBrand.ToString()) + "$", "i"));
            var query = Query.And(qURL, qBrand);
            return (_pathtranslationCollection.Find(query).ToList<Model.PathTranslation>());
        }

        /// <summary>
        /// Return all defined translations
        /// </summary>
        /// <returns></returns>
        public List<Model.PathTranslation> GetAll()
        {
            return _pathtranslationCollection.AsQueryable().ToList();
        }


        /// <summary>
        /// Return translation that match either sourcebaseurl or
        /// brand and not both
        /// </summary>
        /// <param name="sourceBaseUrl">The source base URL.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public List<Model.PathTranslation> GetBySourceBaseUrl(string sourceBaseUrl)
        {
            var qURL = Query.Matches("Source.BaseUrl", new BsonRegularExpression("^" + Regex.Escape(sourceBaseUrl.ToString()) + "$", "i"));
            var query = Query.And(qURL, Query.NotExists("Source.Brand"));
            return (_pathtranslationCollection.Find(query).ToList<Model.PathTranslation>());
        }

     
    }
}

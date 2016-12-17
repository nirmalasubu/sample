using System.Linq;
using AutoMapper;
using System;
using System.Text.RegularExpressions;
using OnDemandTools.DAL.Modules.Airings.Model;
using OnDemandTools.Common.Model;

namespace OnDemandTools.DAL.Helpers
{
    //TODO moved this from web down to DAL. Refactor as needed
    public static class ModelExtensions
    {
        public static V ToViewModel<D, V>(this D dataModel)
        {
            return Mapper.Map<V>(dataModel);
        }

        public static D ToDataModel<V, D>(this V viewModel)
        {
            return Mapper.Map<D>(viewModel);
        }             

        private static string RemoveDomain(string userName)
        {
            return Regex.Replace(userName, ".*\\\\(.*)", "$1", RegexOptions.None);
        }

        public static void UpdateCreatedBy(this IModel model)
        {
            model.CreatedBy = GetCurrentUser();
            model.CreatedDateTime = DateTime.UtcNow;
        }

        public static void UpdateModifiedBy(this IModel model)
        {
            model.ModifiedBy = GetCurrentUser();
            model.ModifiedDateTime = DateTime.UtcNow;
        }

        public static string GetCurrentUser()
        {
            //TODO Add a way to retrieve users
            return "";
        }


        /// <summary>
        /// Compute the # of content segments for this airing.
        /// Every group of playlist items of type 'segment' between every item of 
        /// type 'trigger' constitutes a content segment.
        /// 
        /// If there are no item of type 'trigger' in the playlist then whole group
        /// of 'segment' items constitute a content segment
        /// </summary>
        /// <param name="ar">The ar.</param>
        /// <returns></returns>
        public static int ContentSegmentsCount(this Airing ar)
        {

            return ar.PlayList.Select((e, i) => new { Element = e, Index = i })
                          .Where(e => e.Element.ItemType == "Segment")
                          .GroupBy(e => ar.PlayList.ToList().IndexOf(ar.PlayList.Where(c => c.ItemType == "Trigger" && c.Position > e.Element.Position).FirstOrDefault(), e.Index))
                          .Count();
        }
    }
}

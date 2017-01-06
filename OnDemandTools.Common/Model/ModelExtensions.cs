using System.Linq;
using AutoMapper;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace OnDemandTools.Common.Model
{
    public static class ModelExtensions
    {
        public static B ToBusinessModel<D, B>(this D dataModel)
        {
            return Mapper.Map<B>(dataModel);
        }

        public static D ToDataModel<B, D>(this B businessModel)
        {
            return Mapper.Map<D>(businessModel);
        }

        public static V ToViewModel<B, V>(this B businessModel)
        {
            return Mapper.Map<V>(businessModel);
        }



        private static string RemoveDomain(string userName)
        {
            return Regex.Replace(userName, ".*\\\\(.*)", "$1", RegexOptions.None);
        }

        public static void UpdateCreatedBy(this IModel model)
        {
            model.CreatedBy = model.CreatedBy;
            model.CreatedDateTime = DateTime.UtcNow;
        }

        public static void UpdateModifiedBy(this IModel model)
        {
            model.ModifiedBy = model.ModifiedBy;
            model.ModifiedDateTime = DateTime.UtcNow;
        }



    }
}

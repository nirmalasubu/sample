using System.Collections.Generic;
using System.Linq;
using OnDemandTools.DAL.Modules.Airings.Model;
using OnDemandTools.Jobs.JobRegistry.Publisher.Validating.Validators;
using OnDemandTools.Jobs.JobRegistry.Publisher.Validating;
using OnDemandTools.Jobs.Adapters.Queries;
using OnDemandTools.Jobs.JobRegistry.Models;

namespace OnDemandTools.Jobs.JobRegistry.Publisher.Validators
{
    public class BimContentValidator : IAiringValidatorStep
    {
        private readonly IGetBimContentQuery _bimQuery;
        private readonly IGetOrionContentQuery _orionQuery;

        private readonly List<string> _validContentIds;
        private readonly List<string> _invalidContentIds;
        private readonly List<string> _invalidOrionIds;

        public BimContentValidator(IGetBimContentQuery bimQuery, IGetOrionContentQuery orionQuery)
        {
            _bimQuery = bimQuery;
            _orionQuery = orionQuery;

            _validContentIds = new List<string>();
            _invalidContentIds = new List<string>();
            _invalidOrionIds = new List<string>();
        }

        public ValidationResult Validate(Airing airing, string remoteQueueName = "")
        {
            var contentIds = airing.Versions
                .Select(v => new BimContent(v.ContentId, "Version", v.Source))
                .Where(v => !string.IsNullOrEmpty(v.Id))
                .ToList();

            var promoContentIds = airing.PlayList
                .Where(l => l.ItemType != "Promo")
                .Where(l => !string.IsNullOrEmpty(l.Id))
                .Select(l => new BimContent(l.Id, l.ItemType, Sources.MAPS.ToString()));

            var adContentIds = airing.PlayList
                .Where(l => l.ItemType != "Commercial")
                .Where(l => !string.IsNullOrEmpty(l.Id))
                .Select(l => new BimContent(l.Id, l.ItemType, Sources.MTS.ToString()));

            var otherShortformContentIds = airing.PlayList
                .Where(l => l.ItemType != "Segment" && l.ItemType != "Promo" && l.ItemType != "Commercial")
                .Where(l => !string.IsNullOrEmpty(l.Id))
                .Select(l => new BimContent(l.Id, l.ItemType, Sources.NotSpecified.ToString()));

            contentIds.AddRange(promoContentIds);
            contentIds.AddRange(adContentIds);
            contentIds.AddRange(otherShortformContentIds);

            if (!contentIds.Any())  
             {
               return new ValidationResult(true);
             }

            List<string> invalidIds = new List<string>();
            List<string> validIds = new List<string>();
            List<string> invalidOrionIds = new List<string>();
       
            foreach (var contentId in contentIds)
            {
                if (_validContentIds.Contains(contentId.Id))
                {
                    validIds.Add(contentId.Id);
                    continue;
                }
                if (_invalidContentIds.Contains(contentId.Id))
                {
                    invalidIds.Add(contentId.Id);
                    continue;
                }
                if (_invalidOrionIds.Contains(contentId.Id))
                {
                    invalidOrionIds.Add(contentId.Id);
                    continue;
                }

                var bimContent = _bimQuery.Get(contentId.Id);
                if (string.IsNullOrEmpty(bimContent.ContentId))
                {
                    _invalidContentIds.Add(contentId.Id);
                    invalidIds.Add(contentId.Id);
                }
                else
                {
                    if (contentId.Type == "Version" && contentId.Source == Sources.Orion.ToString())
                    {
                        var orionContent = _orionQuery.Get(contentId.Id);
                        if (string.IsNullOrEmpty(orionContent.ContentId))
                        {
                            return new ValidationResult(false, 4, string.Format("Content not found in Orion. {0}: {1}", contentId.Type, contentId.Id));
                        }

                        if (!AreMaterialIdsInBim(orionContent, bimContent))
                        {
                            _invalidOrionIds.Add(contentId.Id);
                            invalidOrionIds.Add(contentId.Id);
                            continue;
                        }
                    }
                    validIds.Add(contentId.Id);
                    _validContentIds.Add(contentId.Id);
                }
            }
            if (invalidIds.Any())
            {
                List<string> formattedlist = FormateList(contentIds, invalidIds);
                return new ValidationResult(false, 18, string.Format("Queue validation error. Content Not found in BIM.  {0} ", string.Join(",", formattedlist.ToArray())));
            }

            if (invalidOrionIds.Any())
            {
                List<string> formattedlist = FormateList(contentIds, invalidOrionIds);
                return new ValidationResult(false, 19, string.Format("Queue validation error. (material id mismatch).  {0}", string.Join(",", formattedlist.ToArray())));
            }

            return new ValidationResult(true, 17, string.Format("Content found in BIM")); ;
        }

        private List<string> FormateList(List<BimContent> contentIds, List<string> invalidIds)
        {
            List<string> formattedlist = new List<string>();
            foreach (string id in invalidIds)
            {
                string formattedstring = string.Format("{0}:{1}", contentIds.Where(i => i.Id == id).Select(i => i.Type).FirstOrDefault().ToString(), id);
                formattedlist.Add(formattedstring);
            }
            return formattedlist;
        }

        private bool AreMaterialIdsInBim(Content orionVersion, Content bimVersion)
        {
            return orionVersion.MaterialIds.Count <= bimVersion.MaterialIds.Count &&
                orionVersion.MaterialIds.Count > 0 &&
                bimVersion.MaterialIds.Count > 0 &&
                AreOrionMaterialIdsInBim(orionVersion.MaterialIds, bimVersion.MaterialIds);
        }

        private bool AreOrionMaterialIdsInBim(IEnumerable<string> orionMaterialIds, ICollection<string> bimMaterialIds)
        {
            return orionMaterialIds.All(bimMaterialIds.Contains);
        }
    }

    enum Sources
    {
        NotSpecified,
        Orion,
        MAPS,
        MTS
    }
}
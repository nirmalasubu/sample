using FluentValidation;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Business.Modules.Airing.Model;
using OnDemandTools.Common.Extensions;
using System;
using System.Linq;

namespace OnDemandTools.API.v1.Models.Handler
{
    /// <summary>
    /// Validation logic for encoding file contents.
    /// </summary>
    public class EncodingFileContentValidator : AbstractValidator<EncodingFileContentViewModel>
    {
        #region Private Properties
        private readonly IAiringService _airingSvc;
        #endregion

        #region Constructor
        public EncodingFileContentValidator(IAiringService airingSvc)
        {
            _airingSvc = airingSvc;

            // Verify if the main field: media-id is provided
            RuleFor(request => request.MediaId).
                    NotEmpty().WithMessage("media-id required");

            // Verify that airing id is provided
            // Verify that the given AiringId exist                   
            Func<String, bool> airingIdExistRule = new Func<String, bool>((airingId) =>
            {
                if (String.IsNullOrEmpty(airingId))
                    return false;

                return (_airingSvc.GetBy(airingId) == null) ? false : true;
            });
            RuleFor(c => c.AiringId)
                .Must(airingIdExistRule)
                .WithMessage("AiringId {0} doesn't exist", c => c.AiringId);


            //Verify that required data points are provided
            RuleFor(request => request.RootId).NotEmpty().WithMessage("root-id required");

            // Only apply the remaining validation if mediaid is provided
            When(x => !String.IsNullOrWhiteSpace(x.MediaId), () =>
            {
                // Verify that the given MediaId exist                   
                Func<String, bool> mediaIdExistRule = new Func<String, bool>((mediaId) =>
                {
                    return (_airingSvc.GetByMediaId(mediaId).IsNullOrEmpty()) ? false : true;
                });
                RuleFor(c => c.MediaId)
                   .Must(mediaIdExistRule)
                   .WithMessage("MediaId {0} doesn't exist", c => c.MediaId);

                // Verify that outputs property is provided
                RuleFor(c => c.MediaCollection)
                    .NotEmpty()
                    .WithMessage("Output property required and cannot be empty");

                // Apply content segment validation if output is provided
                When(x => !x.MediaCollection.IsNullOrEmpty(), () =>
                {
                    // Apply content segment validation rule
                    SetContentSegmentsRule();
                });

            });

        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Content segment(s) specified in the payload should match 
        /// with that of the Airing 
        /// </summary>
        void SetContentSegmentsRule()
        {
            // Create validate and message creation logic
            int contentSegmentsCount = default(int);
            Func<EncodingFileContentViewModel, MediaViewModel, bool> rule = new Func<EncodingFileContentViewModel, MediaViewModel, bool>((request, media) =>
            {
                // Apply rule only if the content is NOT preview, else return TRUE as default
                if (!media.Output.Contains("preview"))
                {
                    // If the airingid doesn't exist, there is no need to validate. So assume that payload segments
                    // and airing segments are same
                    var query = (String.IsNullOrEmpty(request.AiringId)) ? null : _airingSvc.GetBy(request.AiringId);
                    contentSegmentsCount = (query == null) ? media.ContentSegments.Count() : ContentSegmentsCount(query);
                    return ((media.ContentSegments.Count() != contentSegmentsCount) ? false : true);
                }
                else
                {
                    return true;
                }
            });

            Func<EncodingFileContentViewModel, object> contentCount = new Func<EncodingFileContentViewModel, object>((request) =>
            {
                return contentSegmentsCount;
            });

            // Apply rule
            RuleForEach(request => request.MediaCollection)
                .Must(rule)
                .WithMessage("Content segments provided in payload and that of airing doesn't match");

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
        int ContentSegmentsCount(Airing ar)
        {
            //var k = ar.PlayList.Select((e, i) => new { Element = e, Index = i })
            //              .Where(e => e.Element.ItemType == "Segment")
            //              .GroupBy(e => ar.PlayList.ToList().IndexOf(ar.PlayList.Where(c => c.ItemType == "Trigger" && c.Position > e.Element.Position).FirstOrDefault(), e.Index)).ToList();

            //foreach (var group in k)
            //{
            //    var groupKey = group.Key;
            //    foreach (var groupedItem in group)
            //        Console.WriteLine(groupedItem.Index + " : " + groupedItem.Element.Id);
            //}


            return ar.PlayList.Select((e, i) => new { Element = e, Index = i })
                          .Where(e => e.Element.ItemType == "Segment")
                          .GroupBy(e => ar.PlayList.ToList().IndexOf(ar.PlayList.Where(c => c.ItemType == "Trigger" && c.Position > e.Element.Position).FirstOrDefault(), e.Index))
                          .Count();
        }
        #endregion
    }
}

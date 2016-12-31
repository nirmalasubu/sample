using FluentValidation;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;


namespace OnDemandTools.API.v1.Models.File
{
    /// <summary>
    /// Validation logic to verify file contents
    /// </summary>
    public class FileValidator : AbstractValidator<FileViewModel>
    {
        #region Private Properties
        private readonly IAiringService airingSvc;
        #endregion

        #region Constructor
        public FileValidator(IAiringService airingSvc)
        {
            this.airingSvc = airingSvc;

            // Apply video specific file validation
            RuleSet("Video", () =>
            {

                /// <summary>
                /// This validation checks if content segment(s) specified 
                /// in the payload (# of entries in 'contents' array) should match that of the airing. 
                /// That is, # of elements in 'contents' array should match with the # of elements in ‘version’ array for the airing
                /// </summary>             
                int contentSegmentsCount = default(int);
                Func<FileViewModel, List<FileContentViewModel>, bool> contentSegmentsMatchRule = new Func<FileViewModel, List<FileContentViewModel>, bool>((request, filec) =>
                {
                    // If the mediaid is provided then do content segment validation using that. If the provided mediaid doesn't exist
                    // then set content segment count to 0
                    if (!request.MediaId.IsNullOrEmpty())
                    {
                        var query = this.airingSvc.GetByMediaId(request.MediaId);
                        contentSegmentsCount = (query.IsNullOrEmpty()) ? 0 : query.FirstOrDefault().Versions.Count();
                        return ((filec.Count() != contentSegmentsCount) ? false : true);
                    }
                    else if (!request.AiringId.IsNullOrEmpty())
                    {
                        try
                        {
                            var airing = this.airingSvc.GetBy(request.AiringId);
                            contentSegmentsCount = (airing == null) ? 0 : airing.Versions.Count();
                            return ((filec.Count() != contentSegmentsCount) ? false : true);
                        }
                        catch (Exception)
                        {
                            //if it throws and exception during Get, throw a validation error
                            return false;
                        }
                    }
                    else if (request.TitleId.HasValue)
                    {
                        //var airing = airingSvc.GetNonExpiredBy(request.TitleId.Value, DateTime.MinValue);
                        //contentSegmentsCount = (airing == null) ? 0 : airing.FirstOrDefault().Versions.Count();
                        //return ((filec.Count() != contentSegmentsCount) ? false : true);
                        return true;
                    }
                    // return false if none of the above criteria matches
                    return false;

                });

                Func<FileViewModel, List<FileContentViewModel>, object> contentCount = new Func<FileViewModel, List<FileContentViewModel>, object>((request, filec) =>
                {
                    return contentSegmentsCount;
                });

                // Verify that required data points are provided 
                RuleFor(c => c)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .Must(c => !(string.IsNullOrEmpty(c.AiringId) && string.IsNullOrEmpty(c.MediaId) && !c.TitleId.HasValue))
                    .WithMessage("AiringId, MediaId or TitleId is required")
                    .Must(c => OnlyOneTrue(new bool[]
                    {
                        !string.IsNullOrEmpty(c.AiringId),
                        !string.IsNullOrEmpty(c.MediaId),
                        c.TitleId.HasValue
                    }))
                    .WithMessage("Either provide AiringId, MediaId or TitleId but not more than one")
                    .DependentRules(dr =>
                    {
                        // Apply the following validation if AiringId is not empty and MediaId is empty
                        dr.When(x => (!String.IsNullOrWhiteSpace(x.AiringId) && String.IsNullOrWhiteSpace(x.MediaId)), () =>
                        {
                            // Verify that the given AiringId exist                   
                            Func<String, bool> airingIdExistRule = new Func<String, bool>((airingId) =>
                            {
                                return (this.airingSvc.IsAiringExists(airingId));
                            });

                            dr.RuleFor(c => c.AiringId)
                               .Cascade(CascadeMode.StopOnFirstFailure)
                               .Must(airingIdExistRule)
                               .WithMessage("AiringId {0} doesn't exist/expired.", c => c.MediaId)
                               .DependentRules(o =>
                               {
                                   o.RuleFor(ox => ox.Contents)
                                        .Cascade(CascadeMode.StopOnFirstFailure)
                                        .NotEmpty()
                                        .WithMessage("Contents property required and cannot be empty when posting video payload by airing-id")
                                        .Must(contentSegmentsMatchRule)
                                        .WithMessage("Content segments provided in payload doesn't match with that of media/airing. Payload contains {0} content segments (# of items in 'contents' array), while media/airing contains {1} content segments",
                                          (request, mediac) => mediac.Count(),
                                          contentCount)
                                        .DependentRules(ox =>
                                        {
                                            ox.RuleForEach(xo => xo.Contents)
                                            .SetValidator(new FileContentValidator());
                                        });
                               });
                        });
                        // Apply the following validation if MediaId is provided
                        dr.When(x => !String.IsNullOrWhiteSpace(x.MediaId), () =>
                        {
                            // Verify that the given MediaId exist                   
                            Func<String, bool> mediaIdExistRule = new Func<String, bool>((mediaId) =>
                            {
                                return (airingSvc.GetByMediaId(mediaId).IsNullOrEmpty()) ? false : true;
                            });

                            dr.RuleFor(c => c.MediaId)
                               .Cascade(CascadeMode.StopOnFirstFailure)
                               .Must(mediaIdExistRule)
                               .WithMessage("MediaId {0} doesn't exist", c => c.MediaId)
                               .DependentRules(oxx =>
                               {
                                   oxx.RuleFor(ox => ox.Contents)
                                        .Cascade(CascadeMode.StopOnFirstFailure)
                                        .NotEmpty()
                                        .WithMessage("Contents property required and cannot be empty when posting video payload by media-id")
                                        .Must(contentSegmentsMatchRule)
                                        .WithMessage("Content segments provided in payload doesn't match with that of media/airing. Payload contains {0} content segments (# of items in 'contents' array), while media/airing contains {1} content segments",
                                          (request, mediac) => mediac.Count(),
                                          contentCount)
                                        .DependentRules(o =>
                                        {
                                            o.RuleForEach(xo => xo.Contents)
                                            .SetValidator(new FileContentValidator());
                                        });
                               });

                        });

                        // Apply the following validation if TitleId is provided
                        dr.When(x => x.TitleId.HasValue, () =>
                        {
                            // Verify that the given MediaId exist                   
                            Func<int, bool> titleIdExistRule = new Func<int, bool>((titleId) =>
                            {
                                //return (airingSvc.GetNonExpiredBy(titleId, DateTime.MinValue).IsNullOrEmpty()) ? false : true;
                                return true;
                            });

                            dr.RuleFor(c => c.TitleId.Value)
                               .Cascade(CascadeMode.StopOnFirstFailure)
                               .Must(titleIdExistRule)
                               .WithMessage("TitleId {0} doesn't exist", c => c.TitleId.Value)
                               .DependentRules(oxx =>
                               {
                                   oxx.RuleFor(ox => ox.Contents)
                                        .Cascade(CascadeMode.StopOnFirstFailure)
                                        .NotEmpty()
                                        .WithMessage("Contents property required and cannot be empty when posting video payload by title-id")
                                        .Must(contentSegmentsMatchRule)
                                        .WithMessage("Content segments provided in payload doesn't match with that of media/airing. Payload contains {0} content segments (# of items in 'contents' array), while media/airing contains {1} content segments",
                                          (request, mediac) => mediac.Count(),
                                          contentCount)
                                        .DependentRules(o =>
                                        {
                                            o.RuleForEach(xo => xo.Contents)
                                            .SetValidator(new FileContentValidator());
                                        });
                               });
                        });
                    });
            });


            // Apply non-video specific file validation
            RuleSet("NonVideo", () =>
            {
                // Verify that TitleId is provided
                RuleFor(c => c)
                       .Cascade(CascadeMode.StopOnFirstFailure)
                       .Must(c => c.TitleId.HasValue)
                       .WithMessage("Title Id Required")
                       .Must(c => c.TitleId > 0)
                       .WithMessage("Title Id must be greater than zero");

                RuleFor(c => c)
                    .Must(c => String.IsNullOrEmpty(c.AiringId) && String.IsNullOrEmpty(c.MediaId))
                    .WithMessage("Registering non video assets by Airing Id or Media Id not currently supported");


                // Verify that other required fileds are provided
                RuleFor(c => c)
                       .Must(c => !String.IsNullOrEmpty(c.Path))
                       .WithMessage("Path required");
                RuleFor(c => c)
                       .Must(c => !String.IsNullOrEmpty(c.Name))
                       .WithMessage("Name required");
                RuleFor(c => c)
                       .Must(c => !String.IsNullOrEmpty(c.Type))
                       .WithMessage("Type required");
                RuleFor(c => c)
                       .Must(c => !String.IsNullOrEmpty(c.Domain))
                       .WithMessage("Domain required");
                RuleFor(c => c)
                       .Must(c => !String.IsNullOrEmpty(c.AspectRatio))
                       .WithMessage("Aspect ratio required");
                RuleFor(c => c)
                       .Must(c => !String.IsNullOrEmpty(c.Category))
                       .WithMessage("Category required");
                RuleFor(c => c)
                       .Must(c => !String.IsNullOrEmpty(c.Match))
                       .WithMessage("Match required");


            });
        }
        #endregion

        #region Public Properties

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        private bool OnlyOneTrue(bool[] array)
        {
            return array.Count(t => t) == 1;
        }

        #endregion
    }

    class FileContentValidator : AbstractValidator<FileContentViewModel>
    {
        public FileContentValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Content name required");
            RuleFor(x => x.Media)
                .NotEmpty()
                .WithMessage("Content media required")
                .DependentRules(o =>
                {
                    o.RuleForEach(x => x.Media).SetValidator(new FileMediaValidator());
                });
        }
    }

    class ContentSegmentValidator : AbstractValidator<FileContentSegmentViewModel>
    {
        public ContentSegmentValidator()
        {
            RuleFor(x => x.SegmentIdx)
                .NotNull()
                .WithMessage("Content segment idx required")
                .GreaterThanOrEqualTo(0)
                .WithMessage("Valid content segment idx required");

            RuleFor(x => x.Start)
                .NotNull()
                .WithMessage("Content segment start property required")
                .GreaterThanOrEqualTo(0)
                .WithMessage("Valid content segment start required");

            RuleFor(x => x.Duration)
                .NotNull()
                .WithMessage("Content segment duration required")
                .GreaterThanOrEqualTo(0)
                .WithMessage("Valid content segment duration required");
        }
    }



    class FilePlayListValidator : AbstractValidator<FilePlayListViewModel>
    {
        // Verify that the properties within the playlist array has at least one of the following drm flags with values
        //  drmUnprotected
        //  drmWideVine
        //  drmFairPlay 
        Func<Dictionary<string, object>, bool> verifyDRM = new Func<Dictionary<string, object>, bool>((prop) =>
        {
            return (prop.Keys.Contains("drmUnprotected") || prop.Keys.Contains("drmWideVine")
                || prop.Keys.Contains("drmFairPlay"));
        });

        // Verify that each entry in the properties collection has nonempty value
        Func<Dictionary<string, object>, bool> verifyPropEntry = new Func<Dictionary<string, object>, bool>((prop) =>
        {
            foreach (KeyValuePair<string, object> entry in prop)
            {
                if (entry.Value != null)
                {
                    if (entry.Value.GetType() == typeof(String))
                    {
                        if (entry.Value == String.Empty)
                            return false;
                    }
                }
            }

            return true;
        });


        // Verify URL
        Func<List<Dictionary<string, FileUrlViewModel>>, bool> verifyURL = new Func<List<Dictionary<string, FileUrlViewModel>>, bool>((urls) =>
        {
            // Verify if akamaiURL exist
            if (urls.Where(c => c.Keys.Contains("akamaiUrl")).IsNullOrEmpty())
                return false;

            // Verify each url structure
            foreach (Dictionary<string, FileUrlViewModel> url in urls)
            {
                foreach (KeyValuePair<string, FileUrlViewModel> entry in url)
                {
                    if (entry.Value == null)
                        return false;

                    FileUrlViewModel urlEntry = entry.Value;
                    if (!(!urlEntry.Host.IsNullOrEmpty() && !urlEntry.Path.IsNullOrEmpty() && !urlEntry.FileName.IsNullOrEmpty()))
                        return false;
                }
            }

            return true;

        });


        public FilePlayListValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Playlist name required");
            RuleFor(x => x.Type).NotEmpty().WithMessage("Playlist type required");
            RuleFor(x => x.Properties)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithMessage("Playlist properties is required")
                .Must(verifyPropEntry)
                .WithMessage("Entries in playlist properties cannot be empty")
                .Must(verifyDRM)
                .WithMessage("At least one of the DRM (drmUnprotected, drmWideVine, drmFairPlay) property required");

            RuleFor(x => x.Urls)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithMessage("Playlist url required")
                .Must(urls => !urls.IsNullOrEmpty())
                .WithMessage("Playlist url should not be empty")
                .Must(verifyURL)
                .WithMessage("Playlist urls not valid. Should contain akamaiUrl. Each url entry should have host, file name and path");

        }
    }

    class FileMediaValidator : AbstractValidator<FileMediaViewModel>
    {
        public FileMediaValidator()
        {
            RuleFor(x => x.Type).NotEmpty().WithMessage("Media type required");
            RuleFor(x => x.TotalDuration)
                .NotNull()
                .WithMessage("Media total duration required")
                .GreaterThanOrEqualTo(0)
                .WithMessage("Valid media total duration required");


            RuleFor(x => x.ContentSegments)
                .NotEmpty()
                .WithMessage("Media content segments required")
                .DependentRules(o =>
                {
                    o.RuleForEach(x => x.ContentSegments).SetValidator(new ContentSegmentValidator());
                });

            RuleFor(x => x.Playlists)
               .NotEmpty()
               .WithMessage("Media playlist required")
               .DependentRules(o =>
               {
                   o.RuleForEach(x => x.Playlists).SetValidator(new FilePlayListValidator());
               });

        }
    }

}

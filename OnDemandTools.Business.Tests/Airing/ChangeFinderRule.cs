using OnDemandTools.Business.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using OnDemandTools.DAL.Modules.Airings;
using OnDemandTools.DAL.Modules.Queue.Queries;
using OnDemandTools.DAL.Modules.File.Queries;
using OnDemandTools.DAL.Modules.Destination.Queries;
using OnDemandTools.DAL.Modules.Package.Queries;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Business.Modules.Airing.Model;
using OnDemandTools.DAL.Modules.Airings.Queries;
using Newtonsoft.Json.Linq;

namespace OnDemandTools.Business.Tests.Airing
{
    [TestCaseOrderer("OnDemandTools.Business.Tests.Helpers.CustomTestCaseOrderer", "OnDemandTools.Business.Tests")]
    [Collection("Business Collection")]
    public class ChangeFinderRules
    {
        private readonly Mock<IGetAiringQuery> _airingQueryHelper = new Mock<IGetAiringQuery>();
        private readonly Mock<IAiringSaveCommand> _airingSaveCommandHelper = new Mock<IAiringSaveCommand>();
        private readonly Mock<IAiringDeleteCommand> _airingDeleteCommandHelper = new Mock<IAiringDeleteCommand>();
        private readonly Mock<IAiringMessagePusher> _airingMessagePusherCommandHelper = new Mock<IAiringMessagePusher>();
        private readonly Mock<IQueueQuery> _queueQueryHelper = new Mock<IQueueQuery>();
        private readonly Mock<ITaskUpdater> _taskUpdaterCommand = new Mock<ITaskUpdater>();

        private readonly Mock<IFileQuery> _fileQueryHelper = new Mock<IFileQuery>();
        private readonly Mock<IDestinationQuery> _destinationQueryHelper = new Mock<IDestinationQuery>();
        private readonly Mock<IPackageQuery> _packageQueryHelper = new Mock<IPackageQuery>();
        private readonly Mock<IChangeHistoricalAiringQuery> _changeHistoricalAiringQueryHelper = new Mock<IChangeHistoricalAiringQuery>();
        private readonly Mock<IChangeDeletedAiringQuery> _changeDeletedAiringQueryHelper = new Mock<IChangeDeletedAiringQuery>();
        private AppSettings appSettings = new AppSettings();
        private readonly AiringService _airingService;


       public ChangeFinderRules()
        {
            _airingService = new AiringService(
            _airingQueryHelper.Object,
            appSettings,
           _airingSaveCommandHelper.Object,
           _airingDeleteCommandHelper.Object, _airingMessagePusherCommandHelper.Object,
           _queueQueryHelper.Object,
            _taskUpdaterCommand.Object,
            _fileQueryHelper.Object,
           _destinationQueryHelper.Object,
           _packageQueryHelper.Object,
           _changeHistoricalAiringQueryHelper.Object,
          _changeDeletedAiringQueryHelper.Object);
        } 
      

        [Fact, Order(1)]
        public void OneToOneProperties_WhenDifferent_ShouldShowAsAChange()
        {

            // Arrange
            var mockService = new Mock<IChangeHistoricalAiringQuery>();

            JObject jObject = JObject.Parse(Resources.Resources.CartoonAiringWith3Flights);
            var dd = jObject.ToObject<DAL.Modules.Airings.Model.Airing>();
           

          
            List <DAL.Modules.Airings.Model.Airing> lstairings = new List<DAL.Modules.Airings.Model.Airing>();
            lstairings.Add(dd);
            var airing = jObject.ToObject<Modules.Airing.Model.Alternate.Long.Airing>();

            _changeHistoricalAiringQueryHelper.Setup(cr =>cr.Find(It.IsAny<List<string>>())).Returns(lstairings);
            _airingService.AppendChanges(ref airing);

        }
        

        //[Fact, Order(1)]
        //public void MultipleProperties_WhenDifferent_ShouldShowAsASeparateChanges()
        //{
        //    var currentAsset = new Airing
        //    {
        //        Flights = new List<Flight>
        //                                             {
        //                                                 new Flight { Start = DateTime.Today.AddDays(-60), End = DateTime.Today.AddDays(-55) },
        //                                                 new Flight { Start = DateTime.Today.AddDays(-50), End = DateTime.Today.AddDays(-45) }
        //                                             },
        //        Title = new Title
        //        {
        //            Keywords = "one two three",
        //            ReleaseYear = 1
        //        }
        //    };

        //    var originalAsset = new Airing
        //    {
        //        Flights = new List<Flight>
        //                                              {
        //                                                  new Flight
        //                                                      {
        //                                                          Start = DateTime.Today.AddDays(-60),
        //                                                          End = DateTime.Today.AddDays(-55)
        //                                                      },
        //                                                  new Flight
        //                                                      {
        //                                                          Start = DateTime.Today.AddDays(-50),
        //                                                          End = DateTime.Today.AddDays(-45)
        //                                                      }
        //                                              },
        //        Title = new Title
        //        {
        //            Keywords = "four five six",
        //            ReleaseYear = 2
        //        }
        //    };

        //    var results = _finder.Find(currentAsset, originalAsset);

        //    Assert.That(results.Count(), Is.EqualTo(2));

        //    Assert.That(results.Last().TheChange, Is.EqualTo(@"Title's Keywords"));
        //    Assert.That(results.First().TheChange, Is.EqualTo(@"Title's ReleaseYear"));
        //}

        //[Fact, Order(1)]
        //public void Collections_WhenHavingDifferentCounts_ShouldShowAsChange()
        //{
        //    var currentAsset = new Airing
        //    {
        //        Flights = new List<Flight>
        //                                             {
        //                                                 new Flight
        //                                                     {
        //                                                         Start = DateTime.Today.AddDays(-60),
        //                                                         End = DateTime.Today.AddDays(-55),
        //                                                         Destinations = new List<Destination>
        //                                                                            {
        //                                                                                new Destination {Name = "one"},
        //                                                                                new Destination {Name = "two"},
        //                                                                                new Destination {Name = "three"}
        //                                                                            }
        //                                                     },
        //                                                 new Flight
        //                                                     {
        //                                                         Start = DateTime.Today.AddDays(-50),
        //                                                         End = DateTime.Today.AddDays(-45),
        //                                                         Destinations = new List<Destination>
        //                                                                            {
        //                                                                                new Destination {Name = "one"},
        //                                                                                new Destination {Name = "two"},
        //                                                                                new Destination {Name = "three"}
        //                                                                            }
        //                                                     }
        //                                             }
        //    };

        //    var originalAsset = new Airing
        //    {
        //        Flights = new List<Flight>
        //                                              {
        //                                                  new Flight
        //                                                      {
        //                                                          Start = DateTime.Today.AddDays(-60),
        //                                                          End = DateTime.Today.AddDays(-55),
        //                                                          Destinations = new List<Destination>
        //                                                                             {
        //                                                                                 new Destination {Name = "one"},
        //                                                                                 new Destination {Name = "two"},
        //                                                                                 new Destination
        //                                                                                     {Name = "three"},
        //                                                                                 new Destination {Name = "four"}
        //                                                                             }
        //                                                      },
        //                                                  new Flight
        //                                                      {
        //                                                          Start = DateTime.Today.AddDays(-50),
        //                                                          End = DateTime.Today.AddDays(-45),
        //                                                          Destinations = new List<Destination>
        //                                                                             {
        //                                                                                 new Destination {Name = "one"},
        //                                                                                 new Destination {Name = "two"},
        //                                                                                 new Destination
        //                                                                                     {Name = "three"},
        //                                                                                 new Destination {Name = "four"}
        //                                                                             }
        //                                                      }
        //                                              }

        //    };

        //    var results = _finder.Find(currentAsset, originalAsset);

        //    Assert.That(results.Count(), Is.EqualTo(2));

        //    Assert.That(results.First().TheChange, Is.EqualTo(@"Flights's Destinations"));
        //}

        //[Fact, Order(1)]
        //public void Collections_WhenHavingSameCountsButPropertiesDontMatch_ShouldShowAsSeparateChanges()
        //{
        //    var currentAsset = new Airing
        //    {
        //        Flights = new List<Flight>
        //                                             {
        //                                                 new Flight
        //                                                     {
        //                                                         Start = DateTime.Today.AddDays(-60),
        //                                                         End = DateTime.Today.AddDays(-55),
        //                                                         Destinations = new List<Destination>
        //                                                                            {
        //                                                                                new Destination {Name = "one"},
        //                                                                                new Destination {Name = "two"},
        //                                                                                new Destination {Name = "three"}
        //                                                                            }
        //                                                     },
        //                                                 new Flight
        //                                                     {
        //                                                         Start = DateTime.Today.AddDays(-50),
        //                                                         End = DateTime.Today.AddDays(-45),
        //                                                         Destinations = new List<Destination>
        //                                                                            {
        //                                                                                new Destination {Name = "one"},
        //                                                                                new Destination {Name = "two"},
        //                                                                                new Destination {Name = "three"}
        //                                                                            }
        //                                                     }
        //                                             },

        //    };

        //    var originalAsset = new Airing
        //    {
        //        Flights = new List<Flight>
        //                                              {
        //                                                  new Flight
        //                                                      {
        //                                                          Start = DateTime.Today.AddDays(-60),
        //                                                          End = DateTime.Today.AddDays(-55),
        //                                                          Destinations = new List<Destination>
        //                                                                             {
        //                                                                                 new Destination {Name = "four"},
        //                                                                                 new Destination {Name = "five"},
        //                                                                                 new Destination {Name = "six"}
        //                                                                             }
        //                                                      },
        //                                                  new Flight
        //                                                      {
        //                                                          Start = DateTime.Today.AddDays(-50),
        //                                                          End = DateTime.Today.AddDays(-45),
        //                                                          Destinations = new List<Destination>
        //                                                                             {
        //                                                                                 new Destination {Name = "four"},
        //                                                                                 new Destination {Name = "five"},
        //                                                                                 new Destination {Name = "six"}
        //                                                                             }
        //                                                      }
        //                                              },

        //    };

        //    var results = _finder.Find(currentAsset, originalAsset);

        //    Assert.That(results.Count(), Is.EqualTo(6));

        //    Assert.That(results.First().TheChange, Is.EqualTo(@"Destinations's Name"));
        //    Assert.That(results.Skip(1).Take(1).First().TheChange, Is.EqualTo(@"Destinations's Name"));
        //    Assert.That(results.Skip(2).Take(1).First().TheChange, Is.EqualTo(@"Destinations's Name"));
        //    Assert.That(results.Skip(3).Take(1).First().TheChange, Is.EqualTo(@"Destinations's Name"));
        //    Assert.That(results.Skip(4).Take(1).First().TheChange, Is.EqualTo(@"Destinations's Name"));
        //    Assert.That(results.Skip(5).Take(1).First().TheChange, Is.EqualTo(@"Destinations's Name"));
        //}

    }
}

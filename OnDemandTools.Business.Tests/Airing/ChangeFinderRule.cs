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
        public void GetAiringwithOptionChange_InitialReleaseTest()
        {

            // Arrange
            var mockService = new Mock<IChangeHistoricalAiringQuery>();
            JObject jDalObject = JObject.Parse(Resources.AiringBusinessResource.ResourceManager.GetString("TBSEHistoryInitial"));
            JObject jBusinessObject = JObject.Parse(Resources.AiringBusinessResource.ResourceManager.GetString("TBSEAiring"));
            var HistoryAirings = jDalObject.ToObject<DAL.Modules.Airings.Model.Airing>();
            
            List<DAL.Modules.Airings.Model.Airing> lstHistoryairings = new List<DAL.Modules.Airings.Model.Airing>();
            lstHistoryairings.Add(HistoryAirings);
            var airing = jBusinessObject.ToObject<Modules.Airing.Model.Alternate.Long.Airing>();

            _changeHistoricalAiringQueryHelper.Setup(cr => cr.Find(It.IsAny<List<string>>())).Returns(lstHistoryairings);

            //Act
            _airingService.AppendChanges(ref airing);

            //Assert
            Assert.True(airing.Options.Changes[0].TheChange == "New Release", string.Format("The value returned for change should be 'New Release' but it is returned as {0}", airing.Options.Changes[0].TheChange));
        }

        [Fact, Order(2)]
        public void GetAiringwithOptionChange_ChangeTest()
        {

            // Arrange
            var mockService = new Mock<IChangeHistoricalAiringQuery>();
            JObject jDalHisoryInitialObject = JObject.Parse(Resources.AiringBusinessResource.ResourceManager.GetString("TBSEHistoryInitial"));
            JObject jDalHistorySecondObject = JObject.Parse(Resources.AiringBusinessResource.ResourceManager.GetString("TBSEHistoryWithFightDateModified"));
            JObject jBusinessObject = JObject.Parse(Resources.AiringBusinessResource.ResourceManager.GetString("TBSEAiring"));
            var HistoryAirings = jDalHisoryInitialObject.ToObject<DAL.Modules.Airings.Model.Airing>();

            List<DAL.Modules.Airings.Model.Airing> lstHistoryairings = new List<DAL.Modules.Airings.Model.Airing>();
            lstHistoryairings.Add(jDalHisoryInitialObject.ToObject<DAL.Modules.Airings.Model.Airing>());
            lstHistoryairings.Add(jDalHistorySecondObject.ToObject<DAL.Modules.Airings.Model.Airing>());
            var airing = jBusinessObject.ToObject<Modules.Airing.Model.Alternate.Long.Airing>();

            _changeHistoricalAiringQueryHelper.Setup(cr => cr.Find(It.IsAny<List<string>>())).Returns(lstHistoryairings);

            //Act
            _airingService.AppendChanges(ref airing);

            //Assert
            Assert.True(airing.Options.Changes[0].TheChange == "Flights's Start", string.Format("The value returned for change should be 'Flights's Start' but it is returned as {0}", airing.Options.Changes[0].TheChange));
            Assert.True(airing.Options.Changes[1].TheChange == "Flights's End", string.Format("The value returned for change should be 'Flights's End' but it is returned as {0}", airing.Options.Changes[1].TheChange));
        }

       
    }
}

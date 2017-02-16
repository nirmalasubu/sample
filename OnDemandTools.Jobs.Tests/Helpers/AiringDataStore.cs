using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Jobs.Tests.Helpers
{
    public class AiringDataStore
    {
        #region Constructor

        static AiringDataStore()
        {
            ProcessedAirings = new List<AiringDataStore>();
        }

        public AiringDataStore(string airing)
        {
            Airing = airing;

            ExpectedQueues = new List<string>();
            UnExpectedQueues = new List<string>();
            IgnoredQueues = new List<string>();

            Messages = new List<string>();
        }
        #endregion

        #region Static Methods
        public static void AddAiring(string airing, bool assetShouldExistsInCurrentCollection, string testName, string expectedQueue = "", string unexpectedQueue = "", bool isDeleted = false)
        {
            var model = new AiringDataStore(airing)
            {
                AssetShouldExistsInCurrentCollection = assetShouldExistsInCurrentCollection,
                TestName = testName,
                IsDeleted = isDeleted,
                Priority = null
            };
            if (expectedQueue != "")
                model.ExpectedQueues.Add(expectedQueue);
            if (unexpectedQueue != "")
                model.UnExpectedQueues.Add(unexpectedQueue);

            model.AddMessage(string.Format("Airing successfully posted."));

            ProcessedAirings.Add(model);
        }

        public static void AddAiring(string airing, string testName, string expectedQueue, byte priority)
        {
            var model = new AiringDataStore(airing)
            {
                AssetShouldExistsInCurrentCollection = true,
                TestName = testName,
                ExpectedQueues = new List<string> { expectedQueue },
                UnExpectedQueues = new List<string>(),
                IsDeleted = false,
                Priority = priority
            };
            if (expectedQueue != "")
                model.ExpectedQueues.Add(expectedQueue);


            model.AddMessage(string.Format("Airing successfully posted."));

            ProcessedAirings.Add(model);
        }

        public static void AddAiring(string airing, string testName, string expectedQueue, string ignoredQueue = "")
        {
            var model = new AiringDataStore(airing)
            {
                AssetShouldExistsInCurrentCollection = true,
                TestName = testName,
                ExpectedQueues = new List<string>(),
                UnExpectedQueues = new List<string>(),
                IsDeleted = false,
                IgnoredQueues = new List<string>()
            };
            if (expectedQueue != "")
                model.ExpectedQueues.Add(expectedQueue);
            if (ignoredQueue != "")
                model.IgnoredQueues.Add(ignoredQueue);

            model.AddMessage(string.Format("Airing successfully posted."));

            ProcessedAirings.Add(model);
        }
        #endregion

        #region public methods
        public void AddExpectedQueue(string expectedQueue)
        {
            if (!string.IsNullOrEmpty(expectedQueue) && !ExpectedQueues.Contains(expectedQueue))
            {
                ExpectedQueues.Add(expectedQueue);
            }
        }

        public void AddMessage(string message, bool isError = false)
        {
            Messages.Add(isError ? string.Format("<span>{0}</span>", message) : message);
        }

        public void AddUnExpectedQueue(string unExpectedQueue)
        {
            if (!string.IsNullOrEmpty(unExpectedQueue) && !UnExpectedQueues.Contains(unExpectedQueue))
            {
                UnExpectedQueues.Add(unExpectedQueue);
            }
        }

        #endregion
        
        #region Public Properties

        public static List<AiringDataStore> ProcessedAirings { get; private set; }

        public string Airing { get; private set; }

        public bool AssetShouldExistsInCurrentCollection { get; private set; }

        public List<string> ExpectedQueues { get; private set; }

        public List<string> UnExpectedQueues { get; private set; }

        public string TestName { get; private set; }

        public bool IsDeleted { get; private set; }

        public List<string> Messages { get; private set; }

        public bool IsAiringDeported { get; set; }

        public byte? Priority { get; private set; }

        public List<string> IgnoredQueues { get; private set; }
        #endregion

    }
}

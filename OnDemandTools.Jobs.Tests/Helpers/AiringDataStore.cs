using System.Collections.Generic;

namespace OnDemandTools.Jobs.Tests.Helpers
{
    public class AiringDataStore
    {
        #region Constructor


        public AiringDataStore(string airing)
        {
            AiringId = airing;

            ExpectedQueues = new List<string>();
            UnExpectedQueues = new List<string>();
            IgnoredQueues = new List<string>();

            Messages = new List<string>();
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

        public string AiringId { get; set; }

        public bool AssetShouldExistsInCurrentCollection { get; set; }

        public List<string> ExpectedQueues { get; set; }

        public List<string> UnExpectedQueues { get; set; }

        public string TestName { get; set; }

        public bool IsDeleted { get; set; }

        public List<string> Messages { get; set; }

        public bool IsAiringDeported { get; set; }

        public byte? Priority { get; set; }

        public List<string> IgnoredQueues { get; set; }
        #endregion

    }
}

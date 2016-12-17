using System;
using System.Text;
using System.Text.RegularExpressions;
using DLModel = OnDemandTools.DAL.Modules.AiringId.Model;
using OnDemandTools.DAL.Modules.AiringId.Commands;
using OnDemandTools.DAL.Modules.AiringId;
using OnDemandTools.Business.Modules.AiringId.Model;

namespace OnDemandTools.Business.Modules.AiringId
{
    public class AiringIdCreator : IAiringIdCreator
    {
        IAiringIdSaveCommand airingPerist;

        #region "Private Methods"

        private CurrentAiringId BuildAringId(string prefix, int fiveDigitNumber)
        {
            var builder = new StringBuilder(prefix);

            builder
                .Append("10")
                .Append(DateTime.Now.Date.ToString("MMddyy"))
                .Append("000")
                .Append(fiveDigitNumber.ToString("00000"));

            return new CurrentAiringId
            {
                AiringId = builder.ToString(),
                Prefix = prefix,
                SequenceNumber = fiveDigitNumber
            };
        }

        private int Increment(int previousFiveDigitNumber)
        {
            return previousFiveDigitNumber == 99999
                ? 1
                : ++previousFiveDigitNumber;
        }
        #endregion

        #region "Public Methods"
        public AiringIdCreator(IAiringIdSaveCommand airingIdHelper)
        {
            airingPerist = airingIdHelper;
        }

        public virtual CurrentAiringId Create(string prefix)
        {
            if (!Regex.IsMatch(prefix, "^[A-Z]{4}?"))
                throw new ArgumentException("must be four capital letters only", "prefix");

            return BuildAringId(prefix, 1);
        }

        public virtual CurrentAiringId Create(string prefix, int previousFiveDigitNumber)
        {
            if (!Regex.IsMatch(prefix, "^[A-Z]{4}?"))
                throw new ArgumentException("must be four capital letters only", "prefix");

            if (previousFiveDigitNumber > 99999 || previousFiveDigitNumber < 1)
                throw new ArgumentOutOfRangeException("previousFiveDigitNumber", "must be between 1 and 99,999");

            var nextFiveDigitNumber = Increment(previousFiveDigitNumber);

            return BuildAringId(prefix, nextFiveDigitNumber);
        }

        public CurrentAiringId Save(CurrentAiringId currentAiringId)
        {
            throw new Exception("");
        }
        #endregion

    }
}
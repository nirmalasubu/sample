using System;
using System.Text;
using System.Text.RegularExpressions;
using DLModel = OnDemandTools.DAL.Modules.AiringId.Model;
using OnDemandTools.DAL.Modules.AiringId;
using OnDemandTools.Business.Modules.AiringId.Model;
using OnDemandTools.Common.Model;
using OnDemandTools.Common.Configuration;

namespace OnDemandTools.Business.Modules.AiringId
{
    public class AiringIdCreator : IAiringIdCreator
    {
        IAiringIdSaveCommand airingPerist;
        IApplicationContext appContenxt;


        private CurrentAiringId BuildAringId(string prefix, int fiveDigitNumber)
        {
            var builder = new StringBuilder(prefix);

            builder
                .Append("10")
                .Append(DateTime.Now.Date.ToString("MMddyy"))
                .Append("000")
                .Append(fiveDigitNumber.ToString("00000"));


            Console.WriteLine(appContenxt.GetUser().UserName);
            return new CurrentAiringId
            {
                CreatedBy = appContenxt.GetUser().UserName,
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


        public AiringIdCreator(IAiringIdSaveCommand airingIdHelper, IApplicationContext context)
        {
            airingPerist = airingIdHelper;
            appContenxt = context;
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

        public CurrentAiringId Save(CurrentAiringId currentAiringId, UserIdentity user)
        {
            currentAiringId.CreatedBy = user.Name;
            currentAiringId.ModifiedBy = user.Name;

            return
            (airingPerist
                .Save(currentAiringId.ToDataModel<CurrentAiringId, DLModel.CurrentAiringId>())
                .ToBusinessModel<DLModel.CurrentAiringId, CurrentAiringId>());

        }
    

    }
}
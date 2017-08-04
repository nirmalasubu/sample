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
        IGetAiringIdsQuery getAiringIdQuery;


        private CurrentAiringId BuildAiringId(string prefix, int fiveDigitNumber)
        {
            var builder = new StringBuilder(prefix);

            builder
                .Append("10")
                .Append(DateTime.Now.Date.ToString("MMddyy"))
                .Append("000")
                .Append(fiveDigitNumber.ToString("00000"));


            return new CurrentAiringId
            {
                CreatedBy = appContenxt.GetUser()!=null?appContenxt.GetUser().UserName: appContenxt.GetUserName(),
                AiringId = builder.ToString(),
                Prefix = prefix,
                SequenceNumber = fiveDigitNumber,
                BillingNumber = new BillingNumber
                {
                    Lower = 1,
                    Current = 1,
                    Upper = 99999
                }
            };
        }


        public AiringIdCreator(IAiringIdSaveCommand airingIdHelper, IGetAiringIdsQuery airingIdQuery, IApplicationContext context)
        {
            airingPerist = airingIdHelper;
            appContenxt = context;
            getAiringIdQuery = airingIdQuery;
        }

        public virtual CurrentAiringId Create(string prefix)
        {
            if (!Regex.IsMatch(prefix, "^[A-Z]{4}?"))
                throw new ArgumentException("must be four capital letters only", "prefix");

            if (getAiringIdQuery.Get(prefix) != null)
            {
                throw new ArgumentException("already exists", "prefix");
            }

            return BuildAiringId(prefix, 1);
        }

        public virtual CurrentAiringId Create(string prefix, int nextFiveDigitNumber)
        {
            if (!Regex.IsMatch(prefix, "^[A-Z]{4}?"))
                throw new ArgumentException("must be four capital letters only", "prefix");

            if (nextFiveDigitNumber > 99999 || nextFiveDigitNumber < 1)
                throw new ArgumentOutOfRangeException("previousFiveDigitNumber", "must be between 1 and 99,999");

            return BuildAiringId(prefix, nextFiveDigitNumber);
        }

        public CurrentAiringId Save(CurrentAiringId currentAiringId)
        {
            currentAiringId.ModifiedBy = appContenxt.GetUser() != null ? appContenxt.GetUser().UserName : appContenxt.GetUserName();

            return
            (airingPerist
                .Save(currentAiringId.ToDataModel<CurrentAiringId, DLModel.CurrentAiringId>())
                .ToBusinessModel<DLModel.CurrentAiringId, CurrentAiringId>());

        }


    }
}
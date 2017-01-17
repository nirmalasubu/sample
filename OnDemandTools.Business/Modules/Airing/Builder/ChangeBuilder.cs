using OnDemandTools.Business.Modules.Airing.Model.Alternate.Change;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLModel = OnDemandTools.Business.Modules.Airing.Model;
namespace OnDemandTools.Business.Modules.Airing.Builder
{
    public class ChangeBuilder
    {
        public DeletionChange BuildDeletion(BLModel.Alternate.Long.Airing airing)
        {
            return new DeletionChange
            {
                End = airing.Flights.Max(y => y.End),
                Start = airing.Flights.Min(z => z.Start),
                Series = airing.Title.Series.Name,
                Name = airing.Name,
                Airing = airing
            };
        }

        public NewReleaseChange BuildNewChange(BLModel.Alternate.Long.Airing airing)
        {
            return new NewReleaseChange
            {
                End = airing.Flights.Max(y => y.End),
                Start = airing.Flights.Min(z => z.Start),
                Series = airing.Title.Series.Name,
                Name = airing.Name,
                Airing = airing
            };
        }

        public void SetCommonValues(FieldChange change, BLModel.Alternate.Long.Airing currentAiring, BLModel.Alternate.Long.Airing previousAiring)
        {
            change.End = currentAiring.Flights.Max(y => y.End);
            change.Start = currentAiring.Flights.Min(z => z.Start);
            change.Series = currentAiring.Title.Series.Name;
            change.Name = currentAiring.Name;

            change.Details.Current.By = currentAiring.ReleasedBy;
            change.Details.Current.On = currentAiring.ReleasedOn;

            change.Details.Previous.By = previousAiring.ReleasedBy;
            change.Details.Previous.On = previousAiring.ReleasedOn;

            change.Airing = currentAiring;
        }

        public void SetCommonValues(FieldChange change, BLModel.Alternate.Long.Airing currentAsset, BLModel.Alternate.Long.Airing previousAsset, BLModel.Alternate.Long.Airing originalAsset)
        {
            change.Details.Original.By = originalAsset.ReleasedBy;
            change.Details.Original.On = originalAsset.ReleasedOn;

            SetCommonValues(change, currentAsset, originalAsset);
        }
    }
}

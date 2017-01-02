﻿using System.Collections.Generic;
using BLAiringLongModel = OnDemandTools.Business.Modules.Airing.Model.Alternate;

namespace OnDemandTools.Business.Modules.Airing.Model.Alternate.Destination
{
    public class PropertyFormatter : Formatter
    {
        public PropertyFormatter(BLAiringLongModel.Long.Airing airing) : base(airing)
        {
        }

        public PropertyFormatter()
        {
        }

        public void Format(IEnumerable<BLAiringLongModel.Destination.Destination> viewModels)
        {
            foreach (var viewModel in viewModels)
            {
                Format(viewModel);
            }
        }

        public void Format(BLAiringLongModel.Destination.Destination viewModel)
        {
            foreach (var property in viewModel.Properties)
            {
                property.Value = Format(property.Value);
            }
        }
    }
}

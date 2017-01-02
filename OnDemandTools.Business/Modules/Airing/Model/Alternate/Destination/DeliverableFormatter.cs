using System.Collections.Generic;
using BLAiringLongModel = OnDemandTools.Business.Modules.Airing.Model.Alternate;

namespace OnDemandTools.Business.Modules.Airing.Model.Alternate.Destination
{
    public class DeliverableFormatter : Formatter
    {
        public DeliverableFormatter(BLAiringLongModel.Long.Airing airing) : base(airing)
        {

        }

        public DeliverableFormatter()
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
            foreach (var deliverable in viewModel.Deliverables)
            {
                deliverable.Value = Format(deliverable.Value);
            }
        }
    }
}

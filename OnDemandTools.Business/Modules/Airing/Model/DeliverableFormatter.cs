using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.Airing.Model
{
    public class DeliverableFormatter : Formatter
    {
        public DeliverableFormatter(Airing airing) : base(airing)
        {

        }

        public DeliverableFormatter()
        {

        }

        public void Format(IEnumerable<Destination> viewModels)
        {
            foreach (var viewModel in viewModels)
            {
                Format(viewModel);
            }
        }

        public void Format(Destination viewModel)
        {
            foreach (var deliverable in viewModel.Deliverables)
            {
                deliverable.Value = Format(deliverable.Value);
            }
        }
    }
}

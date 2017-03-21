using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.Airing.Model
{
    public class PropertyFormatter : Formatter
    {
        public PropertyFormatter(Airing airing) : base(airing)
        {
        }

        public PropertyFormatter()
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
            foreach (var property in viewModel.Properties)
            {
                property.Value = Format(property.Value);
            }
        }
    }
}

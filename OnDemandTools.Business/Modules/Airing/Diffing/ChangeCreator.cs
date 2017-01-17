using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnDemandTools.Business.Modules.Airing.Model.Alternate.Change;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.Airing.Diffing
{
    public class ChangeCreator
    {
        public TupleBuilder Builder { get; set; }

        public ChangeCreator(TupleBuilder builder)
        {
            Builder = builder;
        }

        public FieldChange CreateFieldChange()
        {
            return CreateFieldChange(Builder.CurrentProperty, Builder.PreviousProperty, Builder.OriginalProperty);
        }

        public FieldChange CreateFieldChangeFromArray()
        {
            return CreateFieldChange(Builder.CurrentArray, Builder.PreviousArray, Builder.OriginalArray);
        }

        public FieldChange CreateFieldChangeFromArrayItem()
        {
            return CreateFieldChange(Builder.CurrentArrayItem, Builder.PreviousArrayItem, Builder.OriginalArrayItem);
        }

        private FieldChange CreateFieldChange(JToken current, JToken previous, JToken original)
        {
            var result = new FieldChange
            {
                TheChange = Builder.Parent + @"'s " + Builder.CurrentProperty.Name,
                Details = new FieldDetail
                {
                    Current =
                        new ChangeValue { Value = current == null ? String.Empty : current.ToString(Formatting.None) },
                    Original =
                        new ChangeValue { Value = original == null ? String.Empty : original.ToString(Formatting.None) }
                }
            };

            if (Builder.HasPrevious)
            {
                result.Details.Previous.Value = previous == null ? String.Empty : previous.ToString(Formatting.None);
            }

            return result;
        }
    }
}

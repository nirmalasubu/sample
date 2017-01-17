using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.Airing.Diffing
{
    public class TupleBuilder
    {
        protected internal JObject Current { get; set; }
        protected JObject Previous { get; set; }
        protected JObject Original { get; set; }
        protected internal string Parent { get; set; }

        protected internal JProperty CurrentProperty { get; set; }
        protected internal JProperty OriginalProperty { get; set; }
        protected internal JProperty PreviousProperty { get; set; }

        protected JObject CurrentParam { get; set; }
        protected JObject OriginalParam { get; set; }
        protected JObject PreviousParam { get; set; }

        protected internal bool HasPrevious
        {
            get { return this.Previous != null; }
        }

        protected internal JArray OriginalArray { get; set; }
        protected internal JArray PreviousArray { get; set; }
        protected internal JArray CurrentArray { get; set; }

        protected internal JToken CurrentArrayItem { get; set; }
        protected internal JToken PreviousArrayItem { get; set; }
        protected internal JToken OriginalArrayItem { get; set; }

        public TupleBuilder(JObject current, JObject original, string parent = "Asset")
        {
            Current = current;
            Original = original;
            Parent = parent;
        }

        public TupleBuilder(JObject current, JObject previous, JObject original, string parent = "Asset")
        {
            Current = current;
            Previous = previous;
            Original = original;
            Parent = parent;
        }

        private JProperty CreateBlankProperty(JProperty source)
        {
            if (source.Value is JObject)
            {
                return new JProperty(source.Name, new JObject());
            }

            if (source.Value is JArray)
            {
                return new JProperty(source.Name, new JArray());
            }

            if (source.Value is JValue)
            {
                return new JProperty(source.Name, "");
            }

            throw new Exception(String.Format("Unknown type detected in CreateBlankProperty - '{0}'",
                source.Value.GetType()));
        }

        public void BuildProperties(JProperty currentProperty)
        {
            CurrentProperty = currentProperty;
            OriginalProperty = Original.Properties().Any(x => x.Name == currentProperty.Name) ? Original.Property(currentProperty.Name) : CreateBlankProperty(currentProperty);

            if (HasPrevious)
                PreviousProperty = Previous.Properties().Any(x => x.Name == currentProperty.Name) ? Previous.Property(currentProperty.Name) : CreateBlankProperty(currentProperty);
        }

        public void BuildArrays(JProperty currentProperty)
        {
            BuildProperties(currentProperty);

            CurrentArray = CurrentProperty.Value as JArray ?? new JArray();
            OriginalArray = OriginalProperty.Value as JArray ?? new JArray();

            if (HasPrevious)
                PreviousArray = PreviousProperty.Value as JArray ?? new JArray();
        }

        private JToken RetrieveArrayItem(JArray array, int index, JToken theCurrentArrayItem)
        {
            if (theCurrentArrayItem is JObject)
            {
                return array != null && array.Count > index ? array[index] : new JObject();
            }

            if (theCurrentArrayItem is JArray)
            {
                return array != null && array.Count > index ? array[index] : new JArray();
            }

            if (theCurrentArrayItem is JValue)
            {
                return array != null && array.Count > index ? array[index] : new JValue("");
            }

            throw new Exception(String.Format("Unknown type detected in RetrieveArrayItem - '{0}'",
                theCurrentArrayItem.GetType()));
        }

        public void BuildArrayItems(int i)
        {
            CurrentArrayItem = CurrentArray[i];
            OriginalArrayItem = RetrieveArrayItem(OriginalArray, i, CurrentArrayItem);

            if (HasPrevious)
                PreviousArrayItem = RetrieveArrayItem(PreviousArray, i, CurrentArrayItem);
        }

        public void AssignParameters()
        {
            AssignParameters(CurrentProperty.Value, HasPrevious ? PreviousProperty.Value : null, OriginalProperty.Value);
        }

        public void AssignParametersFromArrayItem()
        {
            AssignParameters(CurrentArrayItem, PreviousArrayItem, OriginalArrayItem);
        }

        private void AssignParameters(JToken current, JToken previous, JToken original)
        {
            CurrentParam = current as JObject ?? new JObject();
            OriginalParam = original as JObject ?? new JObject();

            if (HasPrevious)
                PreviousParam = previous as JObject ?? new JObject();
        }

        public TupleBuilder CreateChildTuple()
        {
            return HasPrevious ? new TupleBuilder(CurrentParam, PreviousParam, OriginalParam, CurrentProperty.Name)
                       : new TupleBuilder(CurrentParam, OriginalParam, CurrentProperty.Name);
        }

        public JToken GetFirstProperty()
        {
            return CurrentProperty;
        }

        public JToken GetSecondProperty()
        {
            return HasPrevious ? PreviousProperty : OriginalProperty;
        }

        public JArray GetFirstArray()
        {
            return CurrentArray;
        }

        public JArray GetSecondArray()
        {
            return HasPrevious ? PreviousArray : OriginalArray;
        }

        public JToken GetFirstArrayItem()
        {
            return CurrentArrayItem;
        }

        public JToken GetSecondArrayItem()
        {
            return HasPrevious ? PreviousArrayItem : OriginalArrayItem;
        }
    }
}

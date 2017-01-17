using Newtonsoft.Json.Linq;
using OnDemandTools.Business.Modules.Airing.Model.Alternate.Change;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.Airing.Diffing
{
    public class JsonDiffer
    {
        public IList<FieldChange> FindDifferences(JObject current, JObject original, string parent = "Asset")
        {
            var tuple = new TupleBuilder(current, original, parent);

            return FindDifferences(tuple).ToList();
        }

        public IList<FieldChange> FindDifferences(JObject current, JObject previous, JObject original, string parent = "Asset")
        {
            var tuple = new TupleBuilder(current, previous, original, parent);

            return FindDifferences(tuple).ToList();
        }

        public IEnumerable<FieldChange> FindDifferences(TupleBuilder tuple)
        {
            foreach (var fieldChange in FindDifferencesOfProperties(tuple)) yield return fieldChange;

            foreach (var fieldChange1 in FindDifferencesOfCollections(tuple)) yield return fieldChange1;
        }

        private IEnumerable<FieldChange> FindDifferencesOfProperties(TupleBuilder tuple)
        {
            foreach (var currentProperty in tuple.Current.Properties().Where(x => x.Value.Type != JTokenType.Array))
            {
                tuple.BuildProperties(currentProperty);

                var comparer = new TokenComparer();

                if (!comparer.AreEqual(tuple.GetFirstProperty(), tuple.GetSecondProperty()))
                {
                    if (currentProperty.Value is JObject)
                    {
                        tuple.AssignParameters();

                        foreach (var result in FindDifferences(tuple.CreateChildTuple()))
                        {
                            yield return result;
                        }
                    }
                    else
                    {
                        yield return new ChangeCreator(tuple).CreateFieldChange();
                    }
                }
            }
        }

        private IEnumerable<FieldChange> FindDifferencesOfCollections(TupleBuilder tuple)
        {
            foreach (var currentProperty in tuple.Current.Properties().Where(x => x.Value.Type == JTokenType.Array))
            {
                tuple.BuildArrays(currentProperty);

                var comparer = new TokenComparer();

                if (!comparer.ArraysHaveSameNumberOfItems(tuple.GetFirstArray(), tuple.GetSecondArray()))
                {
                    yield return new ChangeCreator(tuple).CreateFieldChangeFromArray();
                }
                else
                {
                    for (var i = 0; i < tuple.CurrentArray.Count; i++)
                    {
                        tuple.BuildArrayItems(i);

                        if (tuple.CurrentArrayItem is JObject)
                        {
                            tuple.AssignParametersFromArrayItem();

                            foreach (var result in FindDifferences(tuple.CreateChildTuple()))
                            {
                                yield return result;
                            }
                        }
                        else
                        {
                            if (!comparer.AreEqual(tuple.GetFirstArrayItem(), tuple.GetSecondArrayItem()))
                            {
                                yield return new ChangeCreator(tuple).CreateFieldChangeFromArrayItem();
                            }
                        }
                    }
                }
            }
        }
    }
}

using Newtonsoft.Json.Linq;

namespace OnDemandTools.Business.Modules.Airing.Diffing
{
    public class TokenComparer
    {
        public bool AreEqual(JToken first, JToken second)
        {
            return JToken.DeepEquals(first, second);
        }

        public bool ArrayContains(JArray array, JToken token)
        {
            foreach (var child in array.Children())
            {
                if (this.AreEqual(child, token))
                {
                    return true;
                }
            }

            return false;
        }

        public bool ArraysAreEquivalent(JArray first, JArray second)
        {
            if (!this.ArraysHaveSameNumberOfItems(first, second))
            {
                return false;
            }

            foreach (var child in first.Children())
            {
                if (!this.ArrayContains(second, child))
                {
                    return false;
                }
            }

            foreach (var child in second.Children())
            {
                if (!this.ArrayContains(first, child))
                {
                    return false;
                }
            }

            return true;
        }

        public bool ArraysHaveSameNumberOfItems(JArray first, JArray second)
        {
            return first.Count == second.Count;
        }
    }
}

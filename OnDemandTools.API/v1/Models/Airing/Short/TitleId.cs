using System;

namespace OnDemandTools.API.v1.Models.Airing.Short
{
    public class TitleId
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public string Authority { get; set; }

        public override bool Equals(object obj)
        {
            if (null == obj)
            {
                return false;
            }

            var otherTitleId = obj as TitleId;
            if (null == otherTitleId)
            {
                return false;
            }

            return (this.Type == otherTitleId.Type)
                    && (this.Value == otherTitleId.Value)
                    && (this.Authority == otherTitleId.Authority);
        }

        public override int GetHashCode()
        {
            return String.Format("{0}{1}{2}", this.Type, this.Value, this.Authority).GetHashCode();
        }
    }
}
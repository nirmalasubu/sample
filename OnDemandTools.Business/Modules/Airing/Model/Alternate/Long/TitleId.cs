using System;

namespace OnDemandTools.Business.Modules.Airing.Model.Alternate.Long
{
    public class TitleId
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public string Authority { get; set; }

        public bool Primary { get; set; }

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
using System;

namespace OnDemandTools.Business.Modules.Airing.Model
{
    public class TitleId
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public string Authority { get; set; }

        public bool Primary { get; set; }

        public override bool Equals(object obj)
        {
            var otherTitleId = obj as TitleId;

            if (otherTitleId == null)
            {
                return false;
            }

            return (Type == otherTitleId.Type)
                    && (Value == otherTitleId.Value)
                    && (Authority == otherTitleId.Authority);
        }

        public override int GetHashCode()
        {
            return String.Format("{0}{1}{2}", Type, Value, Authority).GetHashCode();
        }
    }
}
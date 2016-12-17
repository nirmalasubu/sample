using System;

namespace OnDemandTools.Common.Model
{
    public interface IModel
    {
        string CreatedBy { get; set; }
        DateTime CreatedDateTime { get; set; }

        string ModifiedBy { get; set; }
        DateTime ModifiedDateTime { get; set; }
    }
}
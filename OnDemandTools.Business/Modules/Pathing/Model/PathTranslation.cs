using OnDemandTools.Common.Model;
using System;
using System.Collections.Generic;

namespace OnDemandTools.Business.Modules.Pathing.Model
{
    /// <summary>
    /// PathTranslation business model
    /// </summary>
    public class PathTranslation : IModel
    {
        public String Id { get; set; }
        public PathInfo Source { get; set; }
        public PathInfo Target { get; set; }
        public String ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }        
        public string CreatedBy { get; set; }        
        public DateTime CreatedDateTime { get; set; }

    }

    public class PathInfo
    {
        public String BaseUrl { get; set; }       
        public String Brand { get; set; }      
        public String ProtectionType { get; set; }
    }

    public class PathTranslationNotFoundException : Exception
    {
        public PathTranslationNotFoundException(string message)
            : base(message)
        {
        }
    }


    /// <summary>
    ///  Compare two PathTranslation based on their Id
    /// </summary>
    public class PathTranslationEqualityComparer : IEqualityComparer<PathTranslation>
    {
        public bool Equals(PathTranslation t1, PathTranslation t2)
        {
            if (t1.Id == t2.Id)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(PathTranslation t)
        {
            return t.Id.GetHashCode();
        }
    }
}

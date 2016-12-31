using OnDemandTools.DAL.Modules.Pathing.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTools.DAL.Modules.Pathing
{
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

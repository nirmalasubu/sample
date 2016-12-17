using Nancy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.API
{
    public class CustomRootPathProvider : IRootPathProvider
    {
	       public string GetRootPath()
	       {
                return Directory.GetCurrentDirectory();
	       }
   }
}

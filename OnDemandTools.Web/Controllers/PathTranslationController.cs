using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnDemandTools.Common.Configuration;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Common.Model;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using BLModel = OnDemandTools.Business.Modules.Pathing.Model;
using VMModel = OnDemandTools.Web.Models.PathTranslation;
using OnDemandTools.Business.Modules.Pathing;



// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnDemandTools.Web.Controllers
{
    [Route("api/[controller]")]
    public class PathTranslationController : Controller
    {
        AppSettings appsettings;
        IPathingService pathTranslationSvc;

        public PathTranslationController(AppSettings appsettings, IPathingService pathTranslationSvc)
        {
            this.appsettings = appsettings;  
            this.pathTranslationSvc = pathTranslationSvc;         
        }


        /// <summary>
        /// Retreive full list of path translations
        /// </summary>        
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IEnumerable<VMModel.PathTranslationViewModel> GetAll()
        {
            return pathTranslationSvc.GetAll()
                    .ToViewModel<List<BLModel.PathTranslation>,List<VMModel.PathTranslationViewModel>>();
                    
        }


        /// <summary>
        /// Generate a shell for new path translation model
        /// </summary>        
        /// <returns></returns>
        [Authorize]
        [HttpGet("newPathTranslation")]
        public VMModel.PathTranslationViewModel GetEmptyModel()
        {
            return new VMModel.PathTranslationViewModel();
        }


        /// <summary>
        /// Add new path translation or Update an existing path translation.
        /// </summary>        
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public VMModel.PathTranslationViewModel UpdateExistingPathTranslation([FromBody]
            VMModel.PathTranslationViewModel data)
        {
            return
            pathTranslationSvc
                .Save(data.ToBusinessModel<VMModel.PathTranslationViewModel, BLModel.PathTranslation>())
                .ToViewModel<BLModel.PathTranslation, VMModel.PathTranslationViewModel>();
        }

      
        /// <summary>
        /// Delete existing path translation. Return 404 if resource not found.
        /// </summary>        
        /// <returns></returns>
        // [Authorize]
        [HttpDelete("{id}")]
        public void DeletePathTranslation(string id)
        {
            this.pathTranslationSvc.Delete(id);
        }
    }
}

using AutoMapper;
using OnDemandTools.Business.Modules.Job.Model;
using OnDemandTools.DAL.Modules.Job.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Common.EntityMapping.EntityMapping.Rules
{
    public class TitleJobProfile : Profile
    {
        public TitleJobProfile()
        {
            CreateMap<TitleJobModel, JobDataModel>();                 

            CreateMap<JobDataModel, TitleJobModel>();
            
        }     
    }
}

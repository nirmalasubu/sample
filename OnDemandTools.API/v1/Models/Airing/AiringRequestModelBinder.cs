using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nancy;
using VMAiringRequestModel = OnDemandTools.API.v1.Models.Airing.Update;
using System.IO;
using Newtonsoft.Json;

namespace OnDemandTools.API.v1.Models.Airing
{
    public class AiringRequestModelBinder : IModelBinder
    {
        public object Bind(NancyContext context, Type modelType, object instance, BindingConfig configuration, params string[] blackList)
        {
            var airingRequestModel = (instance as VMAiringRequestModel.AiringRequest) ?? new VMAiringRequestModel.AiringRequest();

            using (var sr = new StreamReader(context.Request.Body))
            {
                var json = sr.ReadToEnd();
                airingRequestModel = JsonConvert.DeserializeObject<VMAiringRequestModel.AiringRequest>(json);
            }

            return airingRequestModel;
        }

        public bool CanBind(Type modelType)
        {
            return modelType == typeof(VMAiringRequestModel.AiringRequest);
        }
    }
}

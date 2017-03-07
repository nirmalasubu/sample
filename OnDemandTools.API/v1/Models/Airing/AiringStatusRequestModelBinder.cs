using Nancy.ModelBinding;
using System;
using Nancy;
using VMAiringRequestModel = OnDemandTools.API.v1.Models.Airing.Update;
using System.IO;
using Newtonsoft.Json;

namespace OnDemandTools.API.v1.Models.Airing
{
    public class AiringStatusRequestModelBinder : IModelBinder
    {
        public object Bind(NancyContext context, Type modelType, object instance, BindingConfig configuration, params string[] blackList)
        {
            var airingRequestModel = (instance as VMAiringRequestModel.AiringStatusRequest) ?? new VMAiringRequestModel.AiringStatusRequest();

            using (var sr = new StreamReader(context.Request.Body))
            {
                var json = sr.ReadToEnd();
                airingRequestModel = JsonConvert.DeserializeObject<VMAiringRequestModel.AiringStatusRequest>(json);
            }

            return airingRequestModel;
        }

        public bool CanBind(Type modelType)
        {
            return modelType == typeof(VMAiringRequestModel.AiringStatusRequest);
        }
    }
}

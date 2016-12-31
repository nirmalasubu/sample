using System;
using Nancy.ModelBinding;
using Nancy;
using System.IO;
using Newtonsoft.Json;


namespace OnDemandTools.API.v1.Models.Handler
{

    /// <summary>
    /// Custom EncodingFileContentBinder binder. This is implemented to overcome
    /// the limitation of NancyFX default serialization not honoring
    /// Dictionary<T,V> objects by default 
    /// </summary>
    /// <seealso cref="Nancy.ModelBinding.IModelBinder" />
    public class EncodingFileContentBinder : IModelBinder
    {
        public object Bind(NancyContext context, Type modelType, object instance, BindingConfig configuration, params string[] blackList)
        {
            var encodingFileContentViewModel = (instance as EncodingFileContentViewModel) ?? new EncodingFileContentViewModel();

            using (var sr = new StreamReader(context.Request.Body))
            {
                var json = sr.ReadToEnd();
                encodingFileContentViewModel = JsonConvert.DeserializeObject<EncodingFileContentViewModel>(json);
            }

            return encodingFileContentViewModel;
        }

        public bool CanBind(Type modelType)
        {
            return modelType == typeof(EncodingFileContentViewModel);
        }
        
    }
}

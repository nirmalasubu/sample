using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using Nancy;
using System.IO;
using Newtonsoft.Json;

namespace OnDemandTools.API.v1.Models.File
{
    /// <summary>
    /// Custom FileViewModel binder. This is implemented to overcome
    /// the limitation of NancyFX default serialization not honoring
    /// Dictionary<T,V> objects by default 
    /// </summary>
    /// <seealso cref="Nancy.ModelBinding.IModelBinder" />
    public class FileViewModelBinder : IModelBinder
    {
        public object Bind(NancyContext context, Type modelType, object instance, BindingConfig configuration, params string[] blackList)
        {
            var fileViewModel = (instance as List<FileViewModel>) ?? new List<FileViewModel>();

            using (var sr = new StreamReader(context.Request.Body))
            {
                var json = sr.ReadToEnd();
                fileViewModel = JsonConvert.DeserializeObject<List<FileViewModel>>(json);               
            }

            return fileViewModel;
        }

        public bool CanBind(Type modelType)
        {
            return modelType == typeof(List<FileViewModel>);
        }
    }
}

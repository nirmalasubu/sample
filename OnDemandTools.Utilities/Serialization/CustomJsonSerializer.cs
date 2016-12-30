using System;
using System.Collections.Generic;
using System.IO;
using Nancy.Responses.Negotiation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace OnDemandTools.Utilities.Serialization
{
    public sealed class CustomJsonSerializer : JsonSerializer, Nancy.ISerializer
    {
        public CustomJsonSerializer()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver();
            Formatting = Formatting.None;
            NullValueHandling = NullValueHandling.Ignore;
            DefaultValueHandling = DefaultValueHandling.Include;

            Converters.Add(new ObjectIdConverter());
            Converters.Add(new IsoDateTimeConverter());
        }

        public bool CanSerialize(MediaRange mediaRange)
        {
            return mediaRange.ToString().Equals("application/json", StringComparison.OrdinalIgnoreCase);            
        }

        public void Serialize<TModel>(MediaRange mediaRange, TModel model, Stream outputStream)
        {
            using (var streamWriter = new StreamWriter(outputStream))
            using (var jsonWriter = new JsonTextWriter(streamWriter))
            {
                Serialize(jsonWriter, model);
            }
        }

        public IEnumerable<string> Extensions { get { yield return "json"; } }

        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }
        public string ContentType { get; set; }

    }
}
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace OnDemandTools.API.v1.Models.Package
{
    public class PackageDataConverter : JsonConverter
    {
        JsonSerializerSettings ExtractAndOverrideSettings(JsonSerializer serializer)
        {
            var settings = serializer.ExtractSettings();
            settings.ContractResolver = new DefaultContractResolver();
            settings.CheckAdditionalContent = false;
            if (settings.PreserveReferencesHandling != PreserveReferencesHandling.None)
            {
                // Log an error throw an exception?
                //Debug.WriteLine(string.Format("PreserveReferencesHandling.{0} not supported", serializer.PreserveReferencesHandling));
            }
            return settings;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JsonSerializer.CreateDefault(ExtractAndOverrideSettings(serializer)).Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return JsonSerializer.CreateDefault(ExtractAndOverrideSettings(serializer)).Deserialize(reader, objectType);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(object).IsAssignableFrom(objectType);
        }
    }

    internal static class JsonSerializerExtensions
    {
        public static JsonSerializerSettings ExtractSettings(this JsonSerializer serializer)
        {
            // There is no built-in API to extract the settings from a JsonSerializer back into JsonSerializerSettings,
            // so we have to fake it here.
            if (serializer == null)
                throw new ArgumentNullException("serializer");
            var settings = new JsonSerializerSettings
            {
                Binder = serializer.Binder,
                CheckAdditionalContent = serializer.CheckAdditionalContent,
                ConstructorHandling = serializer.ConstructorHandling,
                ContractResolver = serializer.ContractResolver,
                Converters = serializer.Converters,
                Context = serializer.Context,
                Culture = serializer.Culture,
                DateFormatHandling = serializer.DateFormatHandling,
                DateFormatString = serializer.DateFormatString,
                DateParseHandling = serializer.DateParseHandling,
                DateTimeZoneHandling = serializer.DateTimeZoneHandling,
                DefaultValueHandling = serializer.DefaultValueHandling,
                EqualityComparer = serializer.EqualityComparer,
                // No Get access to the error event, so it cannot be copied.
                // Error = += serializer.Error
                FloatFormatHandling = serializer.FloatFormatHandling,
                FloatParseHandling = serializer.FloatParseHandling,
                Formatting = serializer.Formatting,
                MaxDepth = serializer.MaxDepth,
                MetadataPropertyHandling = serializer.MetadataPropertyHandling,
                MissingMemberHandling = serializer.MissingMemberHandling,
                NullValueHandling = serializer.NullValueHandling,
                ObjectCreationHandling = serializer.ObjectCreationHandling,
                ReferenceLoopHandling = serializer.ReferenceLoopHandling,
                // Copying the reference resolver doesn't work in the default case, since the
                // actual BidirectionalDictionary<string, object> mappings are held in the 
                // JsonSerializerInternalBase.
                // See https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json/Serialization/DefaultReferenceResolver.cs
                ReferenceResolverProvider = () => serializer.ReferenceResolver,
                PreserveReferencesHandling = serializer.PreserveReferencesHandling,
                StringEscapeHandling = serializer.StringEscapeHandling,
                TraceWriter = serializer.TraceWriter,
                TypeNameAssemblyFormat = serializer.TypeNameAssemblyFormat,
                TypeNameHandling = serializer.TypeNameHandling,
            };
            return settings;
        }
    }
}

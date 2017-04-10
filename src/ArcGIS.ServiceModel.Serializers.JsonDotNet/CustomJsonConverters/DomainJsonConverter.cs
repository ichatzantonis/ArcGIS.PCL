using System;
using System.Reflection;
using ArcGIS.ServiceModel.Operation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ArcGIS.ServiceModel.Serializers.JsonDotNet.CustomJsonConverters
{
    public class DomainJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            JObject obj = JObject.Load(reader);
            string discriminator = (string)obj["type"];

            Domain domain;
            switch (discriminator)
            {
                case "codedValue":
                    domain = new CodedValueDomain();
                    break;
                case "range":
                    domain = new RangeDomain();
                    break;
                case "inherited":
                    domain = new InheritedDomain();
                    break;
                default:
                    throw new NotImplementedException();
            }

            serializer.Populate(obj.CreateReader(), domain);

            return domain;
        }

        public override bool CanConvert(Type objectType)
        {
            var res = typeof(Domain).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
            return res;
        }
    }
}

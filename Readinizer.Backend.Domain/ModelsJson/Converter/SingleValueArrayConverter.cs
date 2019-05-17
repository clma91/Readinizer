using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.ModelsJson.Converter
{
    public class SingleValueArrayConverter<T> : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite { get { return false; } }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object returnValue = new Object();
            if (reader.TokenType == JsonToken.StartObject)
            {
                T instance = (T)serializer.Deserialize(reader, typeof(T));
                returnValue = new List<T>() { instance };
            }
            else if (reader.TokenType == JsonToken.StartArray)
            {
                returnValue = serializer.Deserialize(reader, objectType);
            }
            return returnValue;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}

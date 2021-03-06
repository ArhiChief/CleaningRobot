using System;
using System.Linq;
using CleaningRobot.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CleaningRobot.Common
{
    public class FacingDirectionConverter : JsonConverter
    {
        private readonly Type _convertableType = typeof(FacingDirection);

        public override bool CanConvert(Type objectType) => _convertableType == objectType;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                FacingDirection result = (FacingDirection)Enum.Parse(_convertableType, reader.Value.ToString(), true);

                return result;
            }

            throw new JsonSerializationException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var jToken = JToken.FromObject(value.ToString());
            jToken.WriteTo(writer);
        }
    }
}
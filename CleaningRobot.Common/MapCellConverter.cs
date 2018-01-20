using System;
using System.Linq;
using CleaningRobot.CleaningRobot.Models;
using Newtonsoft.Json;

namespace CleaningRobot.Common
{
    public class MapCellConverter : JsonConverter
    {
        private readonly Type _convertableType =  typeof(MapCell);

        public override bool CanConvert(Type objectType) => _convertableType == objectType;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                MapCell result = (MapCell) Enum.Parse(_convertableType, reader.Value.ToString(), true);

                return result;
            }

            throw new JsonSerializationException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
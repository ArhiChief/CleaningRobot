using System;
using System.Linq;
using CleaningRobot.CleaningRobot.Models;
using Newtonsoft.Json;

namespace CleaningRobot.Common
{
    public class FacingDirectionConverter : JsonConverter
    {
        private readonly Type[] _types = new Type[] { typeof(FacingDirection) };

        public override bool CanConvert(Type objectType) => _types.Any(t => t == objectType);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
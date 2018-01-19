using System;
using System.Linq;
using CleaningRobot.CleaningRobot.Models;
using Newtonsoft.Json;

namespace CleaningRobot.Common
{
    /// <summary>
    /// Convert json representation of robot commands to internal Command
    /// </summary>
    public class CommandConverter : JsonConverter
    {
        private readonly Type[] _types = new Type[] { typeof(Command) };

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
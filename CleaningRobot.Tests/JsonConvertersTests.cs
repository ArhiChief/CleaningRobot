using System.Linq;
using CleaningRobot.Common;
using CleaningRobot.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace CleaningRobot.Tests
{
    [TestClass]
    public class JsonConvertersTests
    {
        [TestMethod]
        public void TestCommandConverter()
        {
            var result = new[] { Command.A, Command.B, Command.C, Command.TL, Command.TR };
            var stringResult = $"[{string.Join(",", result.Select(cmd => $"\"{cmd.ToString()}\""))}]"; // ["A","B","C","TL","TR"]
            var serializerSettings = new JsonSerializerSettings { Converters = new[] { new CommandConverter() } };

            var deserializedResult = JsonConvert.DeserializeObject<Command[]>(stringResult, serializerSettings);
            var serializedResult = JsonConvert.SerializeObject(deserializedResult, serializerSettings);

            CollectionAssert.AreEqual(result, deserializedResult);
            Assert.AreEqual(serializedResult, stringResult);
        }

        [TestMethod]
        public void TestFacingDirectionConverter()
        {
            var result = new[] { FacingDirection.E, FacingDirection.N, FacingDirection.S, FacingDirection.W };
            var stringResult = $"[{string.Join(",", result.Select(cmd => $"\"{cmd.ToString()}\""))}]"; // ["E","N","S","W"]
            var serializerSettings = new JsonSerializerSettings { Converters = new[] { new FacingDirectionConverter() } };

            var deserializedResult = JsonConvert.DeserializeObject<FacingDirection[]>(stringResult, serializerSettings);
            var serializedResult = JsonConvert.SerializeObject(deserializedResult, serializerSettings);

            CollectionAssert.AreEqual(result, deserializedResult);
            Assert.AreEqual(serializedResult, stringResult);
        }

        [TestMethod]
        public void TestMapCellConverter()
        {
            var result = new[] { MapCell.C, MapCell.Null, MapCell.S };
            var stringResult = $"[{string.Join(",", result.Select(cmd => $"\"{cmd.ToString()}\""))}]"; // ["C","null","S"]
            var serializerSettings = new JsonSerializerSettings { Converters = new[] { new MapCellConverter() } };

            var deserializedResult = JsonConvert.DeserializeObject<MapCell[]>(stringResult, serializerSettings);
            var serializedResult = JsonConvert.SerializeObject(deserializedResult, serializerSettings);

            CollectionAssert.AreEqual(result, deserializedResult);
            Assert.AreEqual(serializedResult, stringResult);
        }
    }
}

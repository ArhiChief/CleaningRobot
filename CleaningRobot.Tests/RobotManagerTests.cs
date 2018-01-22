using System.Linq;
using System.Threading.Tasks;
using CleaningRobot.Models;
using CleaningRobot.WebAPI.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CleaningRobot.Tests
{
    [TestClass]
    public class RobotManagerTests
    {
        static readonly RobotInput robotInput = new RobotInput
        {
            Map = new MapCell[][]
               {
                    new[] { MapCell.S, MapCell.S, MapCell.S, MapCell.S },
                    new[] { MapCell.S, MapCell.S, MapCell.C, MapCell.S },
                    new[] { MapCell.S, MapCell.S, MapCell.S, MapCell.S },
                    new[] { MapCell.S, MapCell.Null, MapCell.S, MapCell.S },
               },
            Start = new RobotLocation { X = 3, Y = 0, Facing = FacingDirection.N },
            Commands = new[] { Command.TL, Command.A, Command.C, Command.A, Command.C, Command.TR, Command.A, Command.C },
            Battery = 80
        };

        static readonly RobotOutput awaitedOutput = new RobotOutput
        {
            Visited = new[]
                {
                    new Location { X = 1, Y = 0 },
                    new Location { X = 2, Y = 0 },
                    new Location { X = 3, Y = 0 }
                },
            Cleaned = new[]
                {
                    new Location { X = 1, Y = 0 },
                    new Location { X = 2, Y = 0 }
                },
            Final = new RobotLocation { X = 2, Y = 0, Facing = FacingDirection.E },
            Battery = 54
        };

        [TestMethod]
        public async Task TestCreateAndCreateDuplicateAndDelete()
        {
            IRobotManager robotManager = new RobotManager();

            // create new robot instance
            var result = await robotManager.CreateAsync(robotInput, "Johny");
            Assert.IsNull(result.Error);
            Assert.IsTrue(result.Body);

            // try create duplicate
            result = await robotManager.CreateAsync(robotInput, "Johny");
            Assert.IsFalse(result.Body);
            Assert.IsNotNull(result.Error);
            Assert.AreEqual(result.Error.Code, 302);

            // try get robot
            var result2 = await robotManager.GetAllAsync();
            Assert.IsNotNull(result2.Body);
            Assert.IsNull(result2.Error);
            CollectionAssert.Contains(result2.Body, "Johny");

            // try delete robot
            var result3 = await robotManager.DeleteAsync("Johny");
            Assert.IsTrue(result3.Body);
            Assert.IsNull(result3.Error);
            // check what robot doesn't exists
            result2 = await robotManager.GetAllAsync();
            Assert.IsNotNull(result2.Body);
            Assert.IsNull(result2.Error);
            CollectionAssert.DoesNotContain(result2.Body, "Johny");
        }

        [TestMethod]
        public async Task TestExecuteAndGetFinalResult()
        {
            IRobotManager robotManager = new RobotManager();

            await robotManager.CreateAsync(robotInput, "Johny");

            await robotManager.ExecuteAsync(robotInput.Commands, "Johny");

            var result = await robotManager.GetFinalResultAsync("Johny");

            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.Body);

            var output = result.Body;

            Assert.AreEqual(awaitedOutput.Battery, output.Battery);
            Assert.AreEqual(awaitedOutput.Final.X, output.Final.X);
            Assert.AreEqual(awaitedOutput.Final.Y, output.Final.Y);
            Assert.AreEqual(awaitedOutput.Final.Facing, output.Final.Facing);

            Assert.AreEqual(awaitedOutput.Cleaned.Length, output.Cleaned.Length);
            foreach (var cleaned in awaitedOutput.Cleaned)
            {
                // use Linq.Any because of order in awaitedResult.Cleaned and result.Cleaned can't be same
                Assert.IsTrue(output.Cleaned.Any(clnd => cleaned.Equals(cleaned)));
            }

            Assert.AreEqual(awaitedOutput.Cleaned.Length, output.Cleaned.Length);
            foreach (var visited in awaitedOutput.Visited)
            {
                // same as before
                Assert.IsTrue(output.Visited.Any(vstd => vstd.Equals(visited)));
            }
        }
    }
}
using System.Linq;
using CleaningRobot.CleaningRobot;
using CleaningRobot.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace CleaningRobot.Tests
{
    [TestClass]
    public class CleaningRobotTests
    {
        [TestMethod]
        public void TestCase1()
        {
            var input = new RobotInput
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

            var awaitedOutput = new RobotOutput
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

            MakeTest(input, awaitedOutput);
        }

        [TestMethod]
        public void TestCase2()
        {
            var input = new RobotInput
            {
                Map = new MapCell[][]
                {
                    new[] { MapCell.S, MapCell.S, MapCell.S, MapCell.S },
                    new[] { MapCell.S, MapCell.S, MapCell.C, MapCell.S },
                    new[] { MapCell.S, MapCell.S, MapCell.S, MapCell.S },
                    new[] { MapCell.S, MapCell.Null, MapCell.S, MapCell.S },
                },
                Start = new RobotLocation { X = 3, Y = 1, Facing = FacingDirection.S },
                Commands = new[] { Command.TR, Command.A, Command.C, Command.A, Command.C, Command.TR, Command.A, Command.C },
                Battery = 1094
            };

            var awaitedOutput = new RobotOutput
            {
                Visited = new[]
                {
                    new Location { X = 2, Y = 2 },
                    new Location { X = 3, Y = 0 },
                    new Location { X = 3, Y = 1 },
                    new Location { X = 3, Y = 2 }
                },
                Cleaned = new[]
                {
                    new Location { X = 2, Y = 2 },
                    new Location { X = 3, Y = 0 },
                    new Location { X = 3, Y = 2 },
                },
                Final = new RobotLocation { X = 3, Y = 2, Facing = FacingDirection.E },
                Battery = 1040
            };

            MakeTest(input, awaitedOutput);
        }


        private void MakeTest(RobotInput input, RobotOutput awaitedOutput)
        {
            IRobot robot = new Robot(input);
            robot.ExecuteCommands(input.Commands);

            var result = robot.GetFinalResult();

            Assert.AreEqual(awaitedOutput.Battery, result.Battery);
            Assert.AreEqual(awaitedOutput.Final.X, result.Final.X);
            Assert.AreEqual(awaitedOutput.Final.Y, result.Final.Y);
            Assert.AreEqual(awaitedOutput.Final.Facing, result.Final.Facing);

            Assert.AreEqual(awaitedOutput.Cleaned.Length, result.Cleaned.Length);
            foreach (var cleaned in awaitedOutput.Cleaned)
            {
                // use Linq.Any because of order in awaitedResult.Cleaned and result.Cleaned can't be same
                Assert.IsTrue(result.Cleaned.Any(clnd => cleaned.Equals(cleaned)));
            }

            Assert.AreEqual(awaitedOutput.Cleaned.Length, result.Cleaned.Length);
            foreach (var visited in awaitedOutput.Visited)
            {
                // same as before
                Assert.IsTrue(result.Visited.Any(vstd => vstd.Equals(visited)));
            }
        }
    }
}
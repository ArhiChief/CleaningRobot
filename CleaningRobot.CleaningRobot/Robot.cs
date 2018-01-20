using System.Collections.Generic;
using System.Linq;
using CleaningRobot.Models;

namespace CleaningRobot.CleaningRobot
{
    /// <summary>
    /// Implenetation of cleaning robot
    /// </summary>
    public class Robot : IRobot
    {
        /// <summary>
        /// Creates instance of cleaning robot from input without command execution
        /// </summary>
        /// <param name="input">Initialization information</param>
        public Robot(RobotInput input)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException(nameof(input));
            }

            map = input.Map ?? throw new System.ArgumentNullException(nameof(input.Map));

            // check for map correctnes
            if (input.Map.Any())
            {
                throw new System.ArgumentException("Empty map", nameof(input.Map));
            }

            if (input.Map.Any(row => !row.Any()) || input.Map.GroupBy(row => row.Count).Count() > 1)
            {
                throw new System.ArgumentException("Map must be rectangle", nameof(input.Map));
            }

            maxY = input.Map.Count - 1;
            maxX = input.Map[0].Count - 1;

            currentLocation = input.Start ?? throw new System.ArgumentNullException(nameof(input.Start));

            // check for correctnes of robot position on the map
            if (input.Start.X < 0 || input.Start.X > maxX || input.Start.Y < 0 || input.Start.Y > maxY)
            {
                throw new System.ArgumentException("Robot is out of map", nameof(input.Start));
            }

            if (input.Battery <= 0)
            {
                throw new System.ArgumentException("Invalid battery charge", nameof(input.Map));
            }

            battery = input.Battery;
        }

        private List<List<MapCell>> map;
        private int battery;
        private List<Location> visited = new List<Location>();
        private List<Location> cleaned = new List<Location>();
        private RobotLocation currentLocation;

        /// <summary>
        /// Maximal X coordinate on the map
        /// </summary>
        private int maxX;

        /// <summary>
        /// Maximal Y coordinate on the map
        /// </summary>
        private int maxY;

        public RobotStatus Status => throw new System.NotImplementedException();

        public void ExecuteCommand(Command command)
        {
            throw new System.NotImplementedException();
        }

        public void ExecuteCommands(params Command[] commands)
        {
            throw new System.NotImplementedException();
        }

        public RobotOutput GetFinalResult()
        {
            return new RobotOutput
            {
                Battery = battery,
                Cleaned = cleaned.Distinct().ToList(), // no need to duplicate values
                Final = currentLocation,
                Visited = visited.Distinct().ToList()
            };
        }

        /// <summary>
        /// Update robot battery level depended on command
        /// </summary>
        /// <param name="command">Command to robot</param>
        /// <returns>True if battery level updated, false if there is not enought battery charge</returns>
        bool UpdateBaterryLevel(Command command)
        {
            int batteryConsumption = 0;

            switch (command)
            {
                case Command.TL:
                case Command.TR:
                    batteryConsumption = 1;
                    break;
                case Command.A:
                    batteryConsumption = 2;
                    break;
                case Command.B:
                    batteryConsumption = 3;
                    break;
                case Command.C:
                    batteryConsumption = 5;
                    break;
                default:
                    throw new System.ArgumentException("Invalid value", nameof(command));
            }

            if (battery >= batteryConsumption)
            {
                battery -= batteryConsumption;
                return true;
            }

            return false; // no batery left for command execution
        }

        /// <summary>
        /// Check if robot can advance next cell on the map
        /// </summary>
        /// <returns>True if robot can advance, false if robot will meet wall or column</returns>
        bool CanAdvance(Command command)
        {
            // make a copy of current location to see if robot can advance next move
            var location = new RobotLocation(currentLocation);

            UpdateLocation(command, location);

            return location.X > 0 && location.X < maxX && location.Y > 0 && location.Y < maxY;
        }

        void UpdateLocation(Command command, RobotLocation location)
        {
            int shiftX = 0;
            int shiftY = 0;

            switch (command)
            {
                case Command.TL:
                    currentLocation.Facing = currentLocation.Facing.TurnLeft();
                    return;
                case Command.TR:
                    currentLocation.Facing = currentLocation.Facing.TurnRight();
                    return;
                case Command.A:
                    switch (currentLocation.Facing)
                    {
                        case FacingDirection.N:
                            shiftY = -1;
                            break;
                        case FacingDirection.E:
                            shiftX = 1;
                            break;
                        case FacingDirection.S:
                            shiftY = 1;
                            break;
                        case FacingDirection.W:
                            shiftX = -1;
                            break;
                    }
                    break;
                case Command.B:
                    switch (currentLocation.Facing)
                    {
                        case FacingDirection.N:
                            shiftY = 1;
                            break;
                        case FacingDirection.E:
                            shiftX = -1;
                            break;
                        case FacingDirection.S:
                            shiftY = -1;
                            break;
                        case FacingDirection.W:
                            shiftX = 1;
                            break;
                    }
                    break;
            }

            currentLocation.X += shiftX;
            currentLocation.Y += shiftY;
        }
    }
}
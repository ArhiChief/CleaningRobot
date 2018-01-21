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
            if (!input.Map.Any())
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

        private static readonly Command[][] _backOffStartegies = new Command[][]
        {
            new [] { Command.TR, Command.A },
            new [] { Command.TL, Command.B, Command.TR, Command.A },
            new [] { Command.TL, Command.TL, Command.A },
            new [] { Command.TR, Command.B, Command.TR, Command.A },
            new [] { Command.TL, Command.TL, Command.A }
        };

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

        public void ExecuteCommands(params Command[] commands)
        {
            foreach (var command in commands)
            {
                if (!ExecuteCommand(command))
                {
                    break;
                }
            }
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

        bool ExecuteCommand(Command command)
        {
            if (CanAdvance(command))
            {
                if (UpdateBaterryLevel(command))
                {
                    var location = new Location(currentLocation);

                    // save visited and cleaned cells if needed

                    visited.Add(location);
                    if (command == Command.C)
                    {
                        cleaned.Add(location);
                    }

                    UpdateLocation(command, currentLocation);

                    return true;
                }
                else
                {
                    return false; // no battery left to execute command
                }
            }
            else
            {
                // probably robot will hit obstacle
                if (ExecuteBackOffStrategy())
                {
                    // Now try to execute command
                    return ExecuteCommand(command);
                }
            }

            return false;
        }

        bool ExecuteBackOffStrategy()
        {
            bool strategyExecuted = true;

            for (int i = 0; i < _backOffStartegies.Length; i++)
            {
                for (int j = 0; j < _backOffStartegies[i].Length; j++)
                {
                    Command command = _backOffStartegies[i][j];

                    if (CanAdvance(command))
                    {
                        if (UpdateBaterryLevel(command))
                        {
                            UpdateLocation(command, currentLocation);
                        }
                        else
                        {
                            return false; // no battery left
                        }
                    }
                    else
                    {
                        // Robot hits obstacle while execute back off strategy. Lets try another one.
                        strategyExecuted = false;
                        break;
                    }
                }

                if (strategyExecuted)
                {
                    return true;
                }
            }

            return false; // no strategies left
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

            return location.X >= 0 && location.X <= maxX && location.Y >= 0 && location.Y <= maxY;
        }

        /// <summary>
        /// Update location depend on provided command
        /// </summary>
        /// <param name="command">Command</param>
        /// <param name="location">Location to update</param>
        void UpdateLocation(Command command, RobotLocation location)
        {
            int shiftX = 0;
            int shiftY = 0;

            switch (command)
            {
                case Command.TL:
                    location.Facing = currentLocation.Facing.TurnLeft();
                    return;
                case Command.TR:
                    location.Facing = currentLocation.Facing.TurnRight();
                    return;
                case Command.A:
                    switch (location.Facing) // determine coordinate shift depend on robot facing direction
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
                    switch (location.Facing)
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
                default:    // skip C command
                    return;
            }

            location.X += shiftX;
            location.Y += shiftY;
        }
    }
}
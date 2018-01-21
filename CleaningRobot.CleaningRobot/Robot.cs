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

            // Check for map correctnes
            if (!input.Map.Any())
            {
                throw new System.ArgumentException("Empty map", nameof(input.Map));
            }

            if (input.Map.Any(row => !row.Any()) || input.Map.GroupBy(row => row.Length).Count() > 1)
            {
                throw new System.ArgumentException("Map must be rectangle", nameof(input.Map));
            }

            maxY = input.Map.Length - 1;
            maxX = input.Map[0].Length - 1;

            currentLocation = input.Start ?? throw new System.ArgumentNullException(nameof(input.Start));

            // Check for correctnes of robot position on the map
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

        // Debug only. Used to see how robot process each command
        public List<RobotCommandExecutionStatus> Log { get; private set; } = new List<RobotCommandExecutionStatus>();

        private static readonly Command[][] _backOffStartegies = new Command[][]
        {
            new [] { Command.TR, Command.A },                           // strategy 1
            new [] { Command.TL, Command.B, Command.TR, Command.A },    // strategy 2
            new [] { Command.TL, Command.TL, Command.A },               // strategy 3
            new [] { Command.TR, Command.B, Command.TR, Command.A },    // strategy 4
            new [] { Command.TL, Command.TL, Command.A }                // strategy 5
        };

        private MapCell[][] map;

        private int battery;

        private HashSet<Location> visited = new HashSet<Location>(); // HashSet is used to prevent duplicates

        private HashSet<Location> cleaned = new HashSet<Location>();

        private RobotLocation currentLocation;

        /// <summary>
        /// Counter of already executed commands
        /// </summary>
        private int commandNumber = 0;

        /// <summary>
        /// Maximal X coordinate on the map
        /// </summary>
        private int maxX;

        /// <summary>
        /// Maximal Y coordinate on the map
        /// </summary>
        private int maxY;

        public void ExecuteCommands(params Command[] commands)
        {
            Log.Add(new RobotCommandExecutionStatus(currentLocation, battery));
            foreach (var command in commands)
            {
                if (!ExecuteOrder(command))
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
                Cleaned = cleaned.ToArray(),
                Final = currentLocation,
                Visited = visited.ToArray()
            };
        }

        /// <summary>
        /// Execute provided command
        /// </summary>
        /// <param name="command">Command to execute</param>
        /// <returns>True if robot executed provided command and can execute next. False otherwise</returns>
        bool ExecuteOrder(Command command)
        {
            if (UpdateBaterryLevel(command))
            {
                if (NotHitsObstacle(command))
                {
                    SaveVisitedAndCleanedCells(currentLocation, command);

                    UpdateLocation(command, currentLocation);

                    Log.Add(new RobotCommandExecutionStatus(currentLocation, battery, commandNumber,
                        command, RobotCommandExecutionStatus.ExecutionCommandType.C,
                        RobotCommandExecutionStatus.CommandExecutionResult.OK));

                    return true;
                }
                else
                {
                    // If the command (which can only be advance) results on the robot entering 
                    // an obstacle the robot will ignore the order and instead follow the following 
                    // algorithm (back off strategy).
                    if (command == Command.A)
                    {
                        Log.Add(new RobotCommandExecutionStatus(currentLocation, battery, commandNumber,
                            command, RobotCommandExecutionStatus.ExecutionCommandType.C,
                            RobotCommandExecutionStatus.CommandExecutionResult.B1));
                        return ExecuteBackOffStrategy();
                    }

                    Log.Add(new RobotCommandExecutionStatus(currentLocation, battery, commandNumber,
                        command, RobotCommandExecutionStatus.ExecutionCommandType.C,
                        RobotCommandExecutionStatus.CommandExecutionResult.FailHitsObstacle));
                    return false;
                }
            }
            else
            {
                // If the robot doesn’t have enough battery left to execute the order, 
                // the robot will ignore the order and instead follow the following 
                // algorithm (back off strategy)
                ExecuteBackOffStrategy();
                Log.Add(new RobotCommandExecutionStatus(currentLocation, battery, commandNumber,
                        command, RobotCommandExecutionStatus.ExecutionCommandType.C,
                        RobotCommandExecutionStatus.CommandExecutionResult.FailBattery));
                return false;
            }
        }

        bool ExecuteBackOffStrategy()
        {
            Command command = Command.A;
            int i = 0;

            for (; i < _backOffStartegies.Length; i++)
            {
                bool strategyExecuted = true;
                for (int j = 0; j < _backOffStartegies[i].Length; j++)
                {
                    command = _backOffStartegies[i][j];

                    if (UpdateBaterryLevel(command))
                    {
                        if (NotHitsObstacle(command))
                        {
                            SaveVisitedAndCleanedCells(currentLocation, command);
                            UpdateLocation(command, currentLocation);

                            Log.Add(new RobotCommandExecutionStatus(currentLocation, battery, commandNumber,
                                command, (RobotCommandExecutionStatus.ExecutionCommandType)i,
                                RobotCommandExecutionStatus.CommandExecutionResult.OK));
                        }
                        else
                        {
                            // Robot hits obstacle while execute back off strategy. Lets try another one.
                            strategyExecuted = false;
                            Log.Add(new RobotCommandExecutionStatus(currentLocation, battery, commandNumber,
                                command, (RobotCommandExecutionStatus.ExecutionCommandType)i,
                                (RobotCommandExecutionStatus.CommandExecutionResult)i + 1));

                            break;
                        }
                    }
                    else
                    {
                        Log.Add(new RobotCommandExecutionStatus(currentLocation, battery, commandNumber,
                                command, (RobotCommandExecutionStatus.ExecutionCommandType)i,
                                RobotCommandExecutionStatus.CommandExecutionResult.FailBattery));
                        return false;
                    }
                }

                if (strategyExecuted)
                {
                    return true;
                }
            }

            Log.Add(new RobotCommandExecutionStatus(currentLocation, battery, commandNumber,
                                command, (RobotCommandExecutionStatus.ExecutionCommandType)i,
                                RobotCommandExecutionStatus.CommandExecutionResult.FailBackOff));
            return false; // Robot hits obstacle again on last startegy
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
                commandNumber++;
                battery -= batteryConsumption;
                return true;
            }

            return false; // No batery left for command execution
        }

        /// <summary>
        /// Check if robot can execute next command
        /// </summary>
        /// <returns>True if robot can move, false if robot will hit obstacle</returns>
        bool NotHitsObstacle(Command command)
        {
            if (command != Command.A && command != Command.B)
            {
                // no need to process something else instead of move forward or backward
                return true;
            }

            // Make a copy of current location to see what happen after command execution
            var location = new RobotLocation(currentLocation);

            UpdateLocation(command, location);

            if (location.X >= 0 && location.X <= maxX &&
                location.Y >= 0 && location.Y <= maxY)
            {
                var mapCell = map[location.Y][location.X];
                return mapCell != MapCell.C && mapCell != MapCell.Null;
            }
            return false;
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

        void SaveVisitedAndCleanedCells(Location location, Command command)
        {
            var loc = new Location(location);
            switch (command)
            {
                case Command.A:
                case Command.B:
                    visited.Add(loc);
                    break;
                case Command.C:
                    cleaned.Add(loc);
                    break;
            }
        }
    }
}
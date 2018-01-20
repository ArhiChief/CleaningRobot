using System.Collections.Generic;
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
            map = input.Map;
            battery = input.Battery;
            currentLocation = input.Start;
        }

        private List<List<MapCell>> map;
        private int battery;
        private List<Location> visited = new List<Location>();
        private List<Location> cleaned = new List<Location>();
        private RobotLocation currentLocation;

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
                Cleaned = cleaned,
                Final = currentLocation,
                Visited = visited
            };
        }

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

            return false; // no batery left
        }
    }
}
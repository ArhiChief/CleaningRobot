using System.Collections.Generic;

namespace CleaningRobot.CleaningRobot.Models
{
    /// <summary>
    /// Input file structure
    /// </summary>
    public class RobotInput
    {
        /// <summary>
        /// Map
        /// </summary>
        /// <returns>Map</returns>
        public List<List<MapCell>> Map { get; set; }
        /// <summary>
        /// Cleaning robot start position
        /// </summary>
        /// <returns>Map</returns>
        public RobotLocation Start { get; set; }
        /// <summary>
        /// List of commands what robot will execute
        /// </summary>
        /// <returns>List of commands</returns>
        public List<Command> Commands { get; set; }
        /// <summary>
        /// Volume of the robot battery
        /// </summary>
        /// <returns>volume of the robot battery</returns>
        public int Battery { get; set; }
    }
}
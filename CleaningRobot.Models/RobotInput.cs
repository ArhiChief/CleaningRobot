using System.Collections.Generic;
using Newtonsoft.Json;

namespace CleaningRobot.Models
{
    /// <summary>
    /// Input file structure
    /// </summary>
    public class RobotInput
    {
        /// <summary>
        /// Map
        /// </summary>
        [JsonProperty("map")]
        public MapCell[][] Map { get; set; }
        /// <summary>
        /// Cleaning robot start position
        /// </summary>
        [JsonProperty("start")]
        public RobotLocation Start { get; set; }
        /// <summary>
        /// List of commands what robot will execute
        /// </summary>
        [JsonProperty("commands")]
        public Command[] Commands { get; set; }
        /// <summary>
        /// Volume of the robot battery
        /// </summary>
        [JsonProperty("battery")]
        public int Battery { get; set; }
    }
}
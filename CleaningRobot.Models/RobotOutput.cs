using System.Collections.Generic;
using Newtonsoft.Json;

namespace CleaningRobot.CleaningRobot.Models
{
    /// <summary>
    /// Output file structure
    /// </summary>
    public class RobotOutput
    {
        /// <summary>
        /// List of visited map cells
        /// </summary>
        [JsonProperty("visited")]
        public List<Location> Visited { get; set; }
        /// <summary>
        /// List of cleaned map cells
        /// </summary>
        [JsonProperty("cleaned")]
        public List<Location> Cleaned { get; set; }
        /// <summary>
        /// Position on the map after all command executed or battery drained
        /// </summary>
        [JsonProperty("final")]
        public RobotLocation Final { get; set; }
        /// <summary>
        /// Remaining battery balance
        /// </summary>
        [JsonProperty("battery")]
        public int Battery { get; set; }
    }
}
using Newtonsoft.Json;

namespace CleaningRobot.CleaningRobot.Models
{
    /// <summary>
    /// Determines location on map
    /// </summary>
    public class Location
    {
        /// <summary>
        /// X coordinate
        /// </summary>
        [JsonProperty("X")]
        public int X { get; set; }
        /// <summary>
        /// Y coordinate
        /// </summary>
        [JsonProperty("Y")]
        public int Y { get; set; }
    }
}
using Newtonsoft.Json;

namespace CleaningRobot.Models
{
    /// <summary>
    /// Determines location on map
    /// </summary>
    public class Location
    {
        public Location() { }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="src">Source to copy</param>
        public Location(Location src)
        {
            X = src.X;
            Y = src.Y;
        }

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
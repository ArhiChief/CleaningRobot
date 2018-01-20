using Newtonsoft.Json;

namespace CleaningRobot.CleaningRobot.Models
{
    /// <summary>
    /// Determines cleaning robot position on the map
    /// </summary>
    public class RobotLocation : Location
    {
        /// <summary>
        /// Current direction of robot
        /// </summary>
        [JsonProperty("facing")]
        public FacingDirection Facing { get; set; }
    }
}
using Newtonsoft.Json;

namespace CleaningRobot.Models
{
    /// <summary>
    /// Determines cleaning robot position on the map
    /// </summary>
    public class RobotLocation : Location
    {
        public RobotLocation() : base() { }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public RobotLocation(RobotLocation src) : base(src)
        {
            Facing = src.Facing;
        }

        /// <summary>
        /// Current direction of robot
        /// </summary>
        [JsonProperty("facing")]
        public FacingDirection Facing { get; set; }
    }
}
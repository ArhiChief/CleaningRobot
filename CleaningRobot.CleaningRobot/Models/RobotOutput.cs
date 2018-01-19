using System.Collections.Generic;

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
        public List<Location> Visited { get; set; }
        /// <summary>
        /// List of cleaned map cells
        /// </summary>
        public List<Location> Cleaned { get; set; }
        /// <summary>
        /// Position on the map after all command executed or battery drained
        /// </summary>
        public RobotLocation Final { get; set; }
        /// <summary>
        /// Remaining battery balance
        /// </summary>
        public int Battery { get; set; }
    }
}
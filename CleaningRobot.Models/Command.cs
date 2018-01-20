namespace CleaningRobot.Models
{
    /// <summary>
    /// Possible enumeration of robot instructions
    /// </summary>
    public enum Command
    {
        /// <summary>
        /// Turn Left (TL). Instructs the robot to turn 90 degrees to the left.
        /// </summary>
        TL,
        /// <summary>
        /// Turn Right (TR). Instructs the robot to turn 90 degrees to the right.
        /// </summary>
        TR,
        /// <summary>
        /// Advance (A). Instructs the robot to advance one cell forward into the next cell.
        /// </summary>
        A,
        /// <summary>
        /// Back (B). Instructs the robot to move back one cell without changing direction.
        /// </summary>
        B,
        /// <summary>
        /// Clean (C). Instructs the robot to clean the current cell.
        /// </summary>
        C
    }
}
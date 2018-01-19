using CleaningRobot.CleaningRobot.Models;

namespace CleaningRobot.CleaningRobot
{
    /// <summary>
    /// Main cleaning robot interface
    /// </summary>
    public interface IRobot
    {
        /// <summary>
        /// Execute command
        /// </summary>
        /// <param name="command">Command to execute</param>
        void ExecuteCommand(Command command);
        /// <summary>
        /// Execute list of commands
        /// </summary>
        /// <param name="commands">List of commands to execute</param>
        void ExecuteCommands(params Command[] commands);
        /// <summary>
        /// Get current robot status. Used for debug,
        /// </summary>
        /// <returns>Current status of robot</returns>
        RobotStatus Status { get; }
        /// <summary>
        /// Returns robot final result
        /// </summary>
        /// <returns>Robot final result</returns>
        RobotOutput FinalResult();
    }
}
using System.Collections.Generic;
using CleaningRobot.Models;

namespace CleaningRobot.CleaningRobot
{
    /// <summary>
    /// Main cleaning robot interface
    /// </summary>
    public interface IRobot
    {
        /// <summary>
        /// Execute list of commands
        /// </summary>
        /// <param name="commands">List of commands to execute</param>
        void ExecuteCommands(params Command[] commands);
        /// <summary>
        /// Get log of robot command execution process. Used for debug,
        /// </summary>
        /// <returns>Robot command execution log</returns>
        List<RobotCommandExecutionStatus> Log { get; }
        /// <summary>
        /// Returns robot final result
        /// </summary>
        /// <returns>Robot final result</returns>
        RobotOutput GetFinalResult();
    }
}
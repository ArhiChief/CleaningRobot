using CleaningRobot.CleaningRobot.Models;

namespace CleaningRobot.CleaningRobot
{
    /// <summary>
    /// Implenetation of cleaning robot
    /// </summary>
    public class Robot : IRobot
    {
        /// <summary>
        /// Creates instance of cleaning robot from input without command execution
        /// </summary>
        /// <param name="input">Initialization information</param>
        public Robot(RobotInput input)
        {
        }

        public RobotStatus Status => throw new System.NotImplementedException();

        public void ExecuteCommand(Command command)
        {
            throw new System.NotImplementedException();
        }

        public void ExecuteCommands(params Command[] commands)
        {
            throw new System.NotImplementedException();
        }

        public RobotOutput FinalResult()
        {
            throw new System.NotImplementedException();
        }
    }
}
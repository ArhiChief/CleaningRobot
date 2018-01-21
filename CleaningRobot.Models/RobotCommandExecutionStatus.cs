namespace CleaningRobot.Models
{
    public class RobotCommandExecutionStatus
    {
        public RobotCommandExecutionStatus() { }

        public RobotCommandExecutionStatus(RobotLocation location, int battery)
        {
            CommandNumber = 0;
            Location = new RobotLocation(location);
            ExecutionResult = CommandExecutionResult.OK;
            Battery = battery;
        }

        public RobotCommandExecutionStatus(RobotLocation location, int battery,
            int commandNumber, Command command, ExecutionCommandType commandType, 
            CommandExecutionResult result) : this(location, battery)
        {
            CommandNumber = commandNumber;
            Command = command;
            CommandType = commandType;
            ExecutionResult = result;
        }

        public ExecutionCommandType CommandType { get; set; }

        public Command Command { get; set; }

        public CommandExecutionResult ExecutionResult { get; set; }

        public int CommandNumber { get; set; }

        public RobotLocation Location { get; set; }

        public int Battery { get; set; }

        public override string ToString() => (CommandNumber == 0)
            ? $"{CommandNumber}.\t\t\tX:{Location.X},\tY:{Location.Y},\tF:{Location.Facing},\tB:{Battery};"
            : $"{CommandNumber}.\t\t{CommandType}:{Command},\tX:{Location.X},\tY:{Location.Y},\tF:{Location.Facing},\tB:{Battery}\t-> {ExecutionResult};";

        public enum ExecutionCommandType
        {
            /// <summary>
            /// Commands executed throught robot pipeline
            /// </summary>
            C = 6,
            /// <summary>
            /// Commands executed in Back Off startegy 1
            /// </summary>
            B1 = 0,
            /// <summary>
            /// Commands executed in Back Off startegy 2
            /// </summary>
            B2 = 1,
            /// <summary>
            /// Commands executed in Back Off startegy 3
            /// </summary>
            B3 = 2,
            /// <summary>
            /// Commands executed in Back Off startegy 4
            /// </summary>
            B4 = 3,
            /// <summary>
            /// Commands executed in Back Off startegy 5
            /// </summary>
            B5 = 4,
        }

        public enum CommandExecutionResult
        {
            /// <summary>
            /// Command executed succesfully
            /// </summary>
            OK = 5,
            /// <summary>
            /// Command execution prevent robot to execute back off strategy 1
            /// </summary>
            B1 = 0,
            /// <summary>
            /// Command execution prevent robot to execute back off strategy 2
            /// </summary>
            B2 = 1,
            /// <summary>
            /// Command execution prevent robot to execute back off strategy 3
            /// </summary>
            B3 = 2,
            /// <summary>
            /// Command execution prevent robot to execute back off strategy 4
            /// </summary>
            B4 = 3,
            /// <summary>
            /// Command execution prevent robot to execute back off strategy 5
            /// </summary>
            B5 = 4,
            /// <summary>
            /// Execution of command failed due to low battery
            /// </summary>
            FailBattery = 6,
            /// <summary>
            /// Execution of command failed due to hitting of obstacle
            /// </summary>
            FailHitsObstacle = 7,
            /// <summary>
            /// Execution of command failed due impossible of execution back off strategy
            /// </summary>
            FailBackOff = 8
        }
    }
}
using System;
using System.IO;
using CleaningRobot.CleaningRobot;
using CleaningRobot.CleaningRobot.Models;
using CleaningRobot.Common;
using Newtonsoft.Json;

namespace CleaningRobot.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            if (CheckProgramArguments(args))
            {
                var converters = new JsonConverter[] {new CommandConverter(), new FacingDirectionConverter(), new MapCellConverter()};

                var input = JsonConvert.DeserializeObject<RobotInput>(File.ReadAllText(args[0]), converters);

                IRobot robot = new Robot(input);

                robot.ExecuteCommands(input.Commands.ToArray());

                var output = robot.FinalResult();

                File.WriteAllText(args[1], JsonConvert.SerializeObject(output, converters));
            }
        }

        static bool CheckProgramArguments(string[] args)
        {
            string errorMsg = null;
            bool isOk = false;
            switch (args.Length)
            {
                case 0:
                    errorMsg = "No input file provided.";
                    break;
                case 1:
                    errorMsg = "No output file provided.";
                    break;
                case 2:
                    if (File.Exists(args[0]))
                    {
                        isOk = true;
                    }
                    else
                    {
                        errorMsg = $"No such file: {args[0]}.";
                    }
                    break;
                default:
                    errorMsg = "Invalid number of arguments provided.";
                    break;
            }

            if (!isOk)
            {
                System.Console.WriteLine(errorMsg);
                PrintUsage();
            }

            return isOk;
        }

        static void PrintUsage() => System.Console.WriteLine("Usage: cleaning_robot source.json result.json");
    }
}

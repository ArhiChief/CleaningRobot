using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CleaningRobot.CleaningRobot;
using CleaningRobot.Models;
using CleaningRobot.WebAPI.Models;

namespace CleaningRobot.WebAPI.Infrastructure
{
    public class RobotManager : IRobotManager
    {
        // because of we use it in WebAPI we must handle race conditions using thread safe version of this structure
        private readonly ConcurrentDictionary<string, IRobot> _robotRepository = new ConcurrentDictionary<string, IRobot>(); // our fake robot repository

        public Task<ApiResult<bool>> CreateAsync(RobotInput robotInput, string name)
        {
            // It's good to use asynchronous functions. But we have nothing asynchronous 
            // here and nothing to await so we will immitate it using TaskCompletitionSource
            var tsc = new TaskCompletionSource<ApiResult<bool>>();
            ApiResult<bool> result;

            try 
            {
                var robot = new Robot(robotInput);
                if (_robotRepository.TryAdd(name, robot)) 
                {
                    result = new ApiResult<bool>(true);
                }
                else
                {
                    result = new ApiResult<bool>(new ApiError(){ Code = (int)HttpStatusCode.Found, Message = "Robot exists" });    
                }
                
            }
            catch (Exception e)
            {
                result = new ApiResult<bool>(new ApiError(){ Code = (int)HttpStatusCode.BadRequest, Message = e.Message });
            }

            tsc.SetResult(result);
            return tsc.Task;
        }


        public Task<ApiResult<bool>> DeleteAsync(string name) 
        {
            var tsc = new TaskCompletionSource<ApiResult<bool>>();
            ApiResult<bool> result;
            IRobot robot;

            _robotRepository.TryRemove(name, out robot);

            if (robot == null) 
            {
                result = new ApiResult<bool>(new ApiError(){ Code = (int)HttpStatusCode.NotFound, Message = "Not Found" });
            } 
            else 
            {
                result = new ApiResult<bool>(true);
            }

            tsc.SetResult(result);

            return tsc.Task;
        }

        public Task<ApiResult<RobotOutput>> GetFinalResultAsync(string name)
        {
            var tsc = new TaskCompletionSource<ApiResult<RobotOutput>>();
            ApiResult<RobotOutput> result;
            IRobot robot;

            if (_robotRepository.TryGetValue(name, out robot)) 
            {
                result = new ApiResult<RobotOutput>(robot.GetFinalResult());
            } 
            else 
            {
                result = new ApiResult<RobotOutput>(new ApiError { Code = (int)HttpStatusCode.NotFound, Message = "Not Found" });
            }

            tsc.SetResult(result);

            return tsc.Task;
        }

        public Task<ApiResult<List<RobotCommandExecutionStatus>>> GetLogAsync(string name) 
        {
            var tsc = new TaskCompletionSource<ApiResult<List<RobotCommandExecutionStatus>>>();
            ApiResult<List<RobotCommandExecutionStatus>> result;
            IRobot robot;

            if (_robotRepository.TryGetValue(name, out robot)) 
            {
                result = new ApiResult<List<RobotCommandExecutionStatus>>(robot.Log);
            } 
            else 
            {
                result = new ApiResult<List<RobotCommandExecutionStatus>>(new ApiError { Code = (int)HttpStatusCode.NotFound, Message = "Not Found" });
            }

            tsc.SetResult(result);

            return tsc.Task;
        }

        public Task<ApiResult<bool>> ExecuteAsync(Command[] commands, string name) 
        {
            var tsc = new TaskCompletionSource<ApiResult<bool>>();
            ApiResult<bool> result;
            IRobot robot;

            if (_robotRepository.TryGetValue(name, out robot)) 
            {
                robot.ExecuteCommands(commands);
                result = new ApiResult<bool>(true);
            } 
            else 
            {
                result = new ApiResult<bool>(new ApiError { Code = (int)HttpStatusCode.NotFound, Message = "Not Found" });
            }

            tsc.SetResult(result);

            return tsc.Task;
        }   

        public Task<ApiResult<string[]>> GetAllAsync() 
        {
            var tsc = new TaskCompletionSource<ApiResult<string[]>>();
            ApiResult<string[]> result;

            result = new ApiResult<string[]>(_robotRepository.Keys.ToArray());

            tsc.SetResult(result);

            return tsc.Task;
        }   
    }
}
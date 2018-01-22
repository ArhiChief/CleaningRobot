using System.Collections.Generic;
using System.Threading.Tasks;
using CleaningRobot.Models;
using CleaningRobot.WebAPI.Models;

namespace CleaningRobot.WebAPI.Infrastructure
{
    public interface IRobotManager
    {
        Task<ApiResult<bool>> CreateAsync(RobotInput robotInput, string name);

        Task<ApiResult<bool>> DeleteAsync(string name);

        Task<ApiResult<RobotOutput>> GetFinalResultAsync(string name);

        Task<ApiResult<List<RobotCommandExecutionStatus>>> GetLogAsync(string name);

        Task<ApiResult<bool>> ExecuteAsync(Command[] commands, string name);

        Task<ApiResult<string[]>> GetAllAsync();
    }
}
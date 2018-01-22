using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CleaningRobot.WebAPI.Infrastructure;
using CleaningRobot.Models;
using System.Net;

namespace CleaningRobot.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class RobotController : Controller
    {

        private readonly IRobotManager _robotManager;

        public RobotController(IRobotManager robotManager)
        {
            _robotManager = robotManager ?? throw new ArgumentNullException(nameof(robotManager));
        }

        // create new robot
        [HttpPut("{name}")]
        public async Task<IActionResult> Create([FromBody]RobotInput robotInput, [FromRoute]string name) 
        {
            var result = await _robotManager.CreateAsync(robotInput, name);

            if (result.Error != null) 
            {
                return BadRequest(result.Error);
            }

            return Created(Url.Action("Create", new {name = name}), robotInput);
        }

        // delete robot
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name) 
        {
            var result = await _robotManager.DeleteAsync(name);

            if (result.Error != null) 
            {
                return NotFound(result.Error);
            }

            return Ok(); 
        }

        // return robot final result
        [HttpGet("{name}")]
        public async Task<IActionResult> FinalResult(string name) 
        {
            var result = await _robotManager.GetFinalResultAsync(name);

            if (result.Error != null) 
            {
                return NotFound(result.Error);
            }

            return Ok(result.Body);
        }

        // execute robot commands
        [HttpPost("{name}/execute")]
        public async Task<IActionResult> Execute([FromBody] Command[] commands, string name) 
        {
            var result = await _robotManager.ExecuteAsync(commands, name);

            if (result.Error != null) 
            {
                return NotFound(result.Error);
            }

            return Ok();
        }

        // get log
        [HttpGet("{name}/log")]
        public async Task<IActionResult> Log(string name) 
        {
            var result = await _robotManager.GetLogAsync(name);

            if (result.Error != null) 
            {
                return NotFound(result.Error);
            }

            return Ok(result.Body);
        }

        // return list of avaliable robots
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            return Ok(await _robotManager.GetAllAsync());
        }
    }
}

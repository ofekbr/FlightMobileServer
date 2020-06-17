using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightMobileServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightMobileServer.Controllers
{
    [Route("api/command")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly SimulatorManager _simulatorManager;

        public CommandController(SimulatorManager sm)
        {
            _simulatorManager = sm;
        }

        // POST: api/Command
        //TODO make async
        [HttpPost]
        public async Task<ActionResult> Post(Command command)
        {
            try
            {
                var result = await _simulatorManager._outConnection.Execute(command);
                return result;
            }
            catch (Exception) {
                return BadRequest();
            }
        }

    }
}

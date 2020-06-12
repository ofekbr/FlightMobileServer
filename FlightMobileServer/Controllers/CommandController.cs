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
        [HttpPost]
        public void Post(CommandController command)
        {
            
            
        }

    }
}

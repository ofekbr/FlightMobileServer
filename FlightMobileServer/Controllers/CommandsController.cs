using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlightMobileServer.Models;

namespace FlightMobileServer.Controllers
{
    [Route("api/command")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly CommandContext _context;

        public CommandsController(CommandContext context)
        {
            _context = context;
        }

        // GET: api/Commands
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Command>>> GetCommandItems()
        {
            return await _context.CommandItems.ToListAsync();
        }

        // GET: api/Commands/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Command>> GetCommand(long id)
        {
            var command = await _context.CommandItems.FindAsync(id);

            if (command == null)
            {
                return NotFound();
            }

            return command;
        }

        // POST: api/Commands
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Command>> PostCommand(Command command)
        {
            _context.CommandItems.Add(command);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCommand", new { id = command.Id }, command);
        }

        // DELETE: api/Commands/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Command>> DeleteCommand(long id)
        {
            var command = await _context.CommandItems.FindAsync(id);
            if (command == null)
            {
                return NotFound();
            }

            _context.CommandItems.Remove(command);
            await _context.SaveChangesAsync();

            return command;
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Cnc.Insfrastructure.Data;
using Cnc.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cnc.Api.Controllers
{
    [Route("api/payloads")]
    [ApiController]
    public class PayloadsControler : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public PayloadsControler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerator<Payload>>> GetPayloads()
        {
            return Ok(await _dbContext.Payloads.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Payload>> GetPayload(int id)
        {
            var payload = await _dbContext.Payloads.FindAsync(id);

            if (payload == null)
            {
                return NotFound();
            }

            return Ok(payload);
        }

        [HttpPost]
        public async Task<ActionResult<Payload>> PostPayload(Payload payload)
        {
            _dbContext.Payloads.Add(payload);
            await _dbContext.SaveChangesAsync();
            return new CreatedAtActionResult("GetPayload", "PayloadsController", new { id = payload.Id }, payload);
        }
    }
}
using BankSystemBota.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankSystemBota.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepositController : Controller
    {
        private readonly ApplicationContext _context;
        public DepositController( ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepositInfo>>> Get()
        {
            if(_context.Deposit == null)
            {
                return NotFound();
            }
            return await _context.Deposit.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<DepositInfo>> GetDeposit(Guid? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var depositInfo = await _context.Deposit.FirstOrDefaultAsync(m => m.Id == Id);

            if (depositInfo == null)
            {
                return NotFound();
            }

            return depositInfo;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<DepositInfo>>> Post(DepositInfo info)
        {
            _context.Deposit.Add(info);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDeposit), new { id = info.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid? Id, DepositInfo info)
        {
            if(Id != info.Id)
            {
                return BadRequest();
            }
            _context.Entry(info).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!DepositExists(Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid? Id)
        {
            if (_context.Deposit == null)
            {
                return NotFound();
            }
            var depositInfo = await _context.Deposit.FirstOrDefaultAsync(m => m.Id == Id);

            if (depositInfo == null)
            {
                return NotFound();
            }

            _context.Deposit.Remove(depositInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("Id")]
        public async Task<ActionResult> Patch(Guid Id, DepositInfo info)
        {
            var deposit = await _context.Deposit.SingleAsync(x => x.Id == Id);

            deposit.Balance = info.Balance;
            deposit.Income = info.Income;
            deposit.Outcome = info.Outcome;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DepositExists(Guid? Id)
        {
            return (_context.Deposit?.Any(e => e.Id == Id)).GetValueOrDefault();
        }
    }
}

using AccountSystem.Models;
using AccountSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts()
        {
            var accounts = await _context.Accounts
                .Select(a => new AccountDto
                {
                    Id = a.Id,
                    Name = a.Name
                }).ToListAsync();

            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> GetAccount(Guid id)
        {
            var account = await _context.Accounts
                .Where(a => a.Id == id)
                .Select(a => new AccountDto
                {
                    Id = a.Id,
                    Name = a.Name
                }).FirstOrDefaultAsync();

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        [HttpPost]
        public async Task<ActionResult<AccountDto>> CreateAccount([FromBody] AccountDto request)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                return BadRequest("Account name is required.");
            }

            var existingAccount = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Name == request.Name);

            if (existingAccount != null)
            {
                return BadRequest("An account with this name already exists.");
            }

            var account = new Account
            {
                Name = request.Name
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            var accountDto = new AccountDto
            {
                Id = account.Id,
                Name = account.Name
            };

            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, accountDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

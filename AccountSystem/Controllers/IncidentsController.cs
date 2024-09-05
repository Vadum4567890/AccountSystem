using AccountSystem.Models;
using AccountSystem.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public IncidentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateIncident([FromBody] IncidentRequest request)
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Name == request.AccountName);

            if (account == null)
            {
                return NotFound("Account not found.");
            }

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.Email == request.ContactEmail);

            if (contact == null)
            {
                contact = new Contact
                {
                    FirstName = request.ContactFirstName,
                    LastName = request.ContactLastName,
                    Email = request.ContactEmail,
                    AccountId = account.Id 
                };
                _context.Contacts.Add(contact);
            }
            else
            {
                contact.FirstName = request.ContactFirstName;
                contact.LastName = request.ContactLastName;

                if (contact.AccountId != account.Id)
                {
                    contact.AccountId = account.Id; 
                    _context.Contacts.Update(contact); 
                }
            }

            var incident = new Incident
            {
                Name = Guid.NewGuid().ToString(), 
                Description = request.IncidentDescription,
                AccountId = account.Id 
            };
            _context.Incidents.Add(incident);

            await _context.SaveChangesAsync();

            return Ok(new { incidentId = incident.Id });
        }

    }
}

using BookStoreAPI.Data;
using BookStoreAPI.Entities.ClientEntities;
using BookStoreAPI.Models.Dto.ClientDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Controllers.ClientControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            var clients = await _context.Clients.ToListAsync();
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound($"Client with ID {id} not found.");
            }

            return Ok(client);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, [FromBody] PostClientDto clientDto)
        {
            try
            {
                var existingClient = await _context.Clients.FindAsync(id);

                if (existingClient == null)
                {
                    return NotFound($"Client with ID {id} not found.");
                }

                existingClient.FirstName = clientDto.FirstName;
                existingClient.LastName = clientDto.LastName;
                existingClient.Email = clientDto.Email;
                existingClient.Username = clientDto.Username;
                existingClient.Password = clientDto.Password;

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
                {
                    return NotFound($"Client with ID {id} not found.");
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Client>> PostClient([FromBody] PostClientDto clientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var client = new Client
                {
                    FirstName = clientDto.FirstName,
                    LastName = clientDto.LastName,
                    Email = clientDto.Email,
                    Username = clientDto.Username,
                    Password = clientDto.Password
                };

                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetClient", new { id = client.Id }, client);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound($"Client with ID {id} not found.");
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}

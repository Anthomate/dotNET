using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreAPI.Entities;
using BookStoreAPI.Models;

namespace BookStoreAPI.Controllers
{
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PublishersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("publisher")]
        public async Task<ActionResult<IEnumerable<Publisher>>> GetPublishers()
        {
            var publisher = await _context.Publishers.ToListAsync(); ;
            return Ok(publisher);
        }

        [HttpGet("publisher/{id}")]
        public async Task<ActionResult<Publisher>> GetPublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);

            if (publisher == null)
            {
                return NotFound($"Editeur avec l'ID {id} non trouvé.");
            }

            return publisher;
        }

        [HttpPut("publishers/{id}")]
        public async Task<IActionResult> PutPublisher(int id, [FromBody] PublisherDto publisherDto)
        {
            try
            {
                var existingPublisher = await _context.Publishers.FindAsync(id);

                if (existingPublisher == null)
                {
                    return NotFound($"Editeur avec l'ID {id} non trouvé.");
                }

                existingPublisher.Name = publisherDto.Name;

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublisherExists(id))
                {
                    return NotFound($"Editeur avec l'ID {id} non trouvé.");
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s'est produite : {ex.Message}");
            }
        }

        [HttpPost("publishers")]
        public async Task<ActionResult<Publisher>> PostPublisher([FromBody] PublisherDto publisherDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var publisher = new Publisher
                {
                    Name = publisherDto.Name
                };

                _context.Publishers.Add(publisher);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPublisher", new { id = publisher.Id }, publisher);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s'est produite : {ex.Message}");
            }
        }

        [HttpDelete("publisher/{id}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null)
            {
                return NotFound($"Editeur avec l'ID {id} non trouvé.");
            }

            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PublisherExists(int id)
        {
            return _context.Publishers.Any(e => e.Id == id);
        }
    }
}

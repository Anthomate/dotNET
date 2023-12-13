using BookStoreAPI.Data;
using BookStoreAPI.Entities.BookEntities;
using BookStoreAPI.Models.Dto.AuthorDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Controllers.BookControllers
{
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("author")]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            var author = await _context.Authors.ToListAsync(); ;
            return Ok(author);
        }

        [HttpGet("author/{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound($"Auteur avec l'ID {id} non trouvé.");
            }

            return Ok(author);
        }

        [HttpPut("author/{id}")]
        public async Task<IActionResult> PutAuthor(int id, [FromBody] PostAuthorDto authorDto)
        {
            try
            {
                var existingAuthor = await _context.Authors.FindAsync(id);

                if (existingAuthor == null)
                {
                    return NotFound($"Auteur avec l'ID {id} non trouvé.");
                }

                existingAuthor.Name = authorDto.Name;

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
                {
                    return NotFound($"Auteur avec l'ID {id} non trouvé.");
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

        [HttpPost("author")]
        public async Task<ActionResult<Author>> PostAuthor([FromBody] PostAuthorDto authorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var author = new Author
                {
                    Name = authorDto.Name
                };

                _context.Authors.Add(author);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetAuthors", new { id = author.Id }, author);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s'est produite : {ex.Message}");
            }
        }

        [HttpDelete("author/{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound($"Auteur avec l'ID {id} non trouvé.");
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}
using BookStoreAPI.Data;
using BookStoreAPI.Entities.BookEntities;
using BookStoreAPI.Models.Dto.GenreDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Controllers.BookControllers
{
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GenresController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("genres")]
        public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
        {
            var genres = await _context.Genres.ToListAsync(); ;
            return Ok(genres);
        }

        [HttpGet("genres/{id}")]
        public async Task<ActionResult<Genre>> GetGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);

            if (genre == null)
            {
                return NotFound($"Genre avec l'ID {id} non trouvé.");
            }

            return Ok(genre);
        }

        [HttpPut("genres/{id}")]
        public async Task<IActionResult> PutGenre(int id, [FromBody] PostGenreDto genreDto)
        {
            try
            {
                var existingGenre = await _context.Genres.FindAsync(id);

                if (existingGenre == null)
                {
                    return NotFound($"Genre avec l'ID {id} non trouvé.");
                }

                existingGenre.Title = genreDto.Title;

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(id))
                {
                    return NotFound($"Genre avec l'ID {id} non trouvé.");
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

        [HttpPost("genres")]
        public async Task<ActionResult<Genre>> PostGenre([FromBody] PostGenreDto genreDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var genre = new Genre
                {
                    Title = genreDto.Title
                };

                _context.Genres.Add(genre);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetGenre", new { id = genre.Id }, genre);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s'est produite : {ex.Message}");
            }
        }

        [HttpDelete("genres/{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound($"Genre avec l'ID {id} non trouvé.");
            }

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GenreExists(int id)
        {
            return _context.Genres.Any(e => e.Id == id);
        }
    }
}
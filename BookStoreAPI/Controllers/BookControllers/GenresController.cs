using AutoMapper;
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
        private readonly IMapper _mapper;

        public GenresController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("genre")]
        public async Task<ActionResult<IEnumerable<GenreGetRequestDto>>> GetGenres()
        {
            var genres = await _context.Genres.ToListAsync(); ;

            var genresDto = new List<GenreGetRequestDto>();

            foreach (var genre in genres)
            {
                genresDto.Add(_mapper.Map<GenreGetRequestDto>(genre));
            }

            return Ok(genresDto);
        }

        [HttpGet("genre/{id}")]
        public async Task<ActionResult<GenreGetRequestDto>> GetGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);

            if (genre == null)
            {
                return NotFound($"Genre avec l'ID {id} non trouvé.");
            }

            var genreDto = _mapper.Map<GenreGetRequestDto>(genre);

            return Ok(genreDto);
        }

        [HttpPut("genre/{id}")]
        public async Task<IActionResult> PutGenre(int id, [FromBody] GenreEditRequestDto genreDto)
        {
            try
            {
                var existingGenre = await _context.Genres.FindAsync(id);

                if (existingGenre == null)
                {
                    return NotFound($"Genre avec l'ID {id} non trouvé.");
                }

                _mapper.Map(genreDto, existingGenre);

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

        [HttpPost("genre")]
        public async Task<ActionResult<GenreCreateRequestDto>> PostGenre([FromBody] GenreCreateRequestDto genreDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var genre = _mapper.Map<Genre>(genreDto);

                _context.Genres.Add(genre);
                await _context.SaveChangesAsync();

                var genreRequestDto = _mapper.Map<GenreCreateRequestDto>(genre);

                return CreatedAtAction("GetGenre", new { id = genre.Id }, genreRequestDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s'est produite : {ex.Message}");
            }
        }

        [HttpDelete("genre/{id}")]
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
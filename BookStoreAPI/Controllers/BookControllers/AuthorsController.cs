using AutoMapper;
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
        private readonly IMapper _mapper;

        public AuthorsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("author")]
        public async Task<ActionResult<IEnumerable<AuthorGetRequestDto>>> GetAuthors()
        {
            var authors = await _context.Authors.ToListAsync(); ;

            var authorsDto = new List<AuthorGetRequestDto>();

            foreach (var author in authors)
            {
                authorsDto.Add(_mapper.Map<AuthorGetRequestDto>(author));
            }

            return Ok(authorsDto);
        }

        [HttpGet("author/{id}")]
        public async Task<ActionResult<AuthorGetRequestDto>> GetAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound($"Auteur avec l'ID {id} non trouvé.");
            }

            var authorDto = _mapper.Map<AuthorGetRequestDto>(author);

            return Ok(authorDto);
        }

        [HttpPut("author/{id}")]
        public async Task<IActionResult> PutAuthor(int id, [FromBody] AuthorEditRequestDto authorDto)
        {
            try
            {
                var existingAuthor = await _context.Authors.FindAsync(id);

                if (existingAuthor == null)
                {
                    return NotFound($"Auteur avec l'ID {id} non trouvé.");
                }

                _mapper.Map(authorDto, existingAuthor);

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
        public async Task<ActionResult<AuthorCreateRequestDto>> PostAuthor([FromBody] AuthorCreateRequestDto authorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var author = _mapper.Map<Author>(authorDto);

                _context.Authors.Add(author);
                await _context.SaveChangesAsync();

                var authorRequestDto = _mapper.Map<AuthorCreateRequestDto>(author);

                return CreatedAtAction("GetAuthors", new { id = author.Id }, authorRequestDto);
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
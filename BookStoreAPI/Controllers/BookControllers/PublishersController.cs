using AutoMapper;
using BookStoreAPI.Data;
using BookStoreAPI.Entities.BookEntities;
using BookStoreAPI.Models.Dto.PublisherDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Controllers.BookControllers
{
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PublishersController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("publisher")]
        public async Task<ActionResult<IEnumerable<PublisherGetRequestDto>>> GetPublishers()
        {
            var publishers = await _context.Publishers.ToListAsync();

            var publishersDto = new List<PublisherGetRequestDto>();

            foreach (var publisher in publishers)
            {
                publishersDto.Add(_mapper.Map<PublisherGetRequestDto>(publisher));
            }
            return Ok(publishersDto);
        }

        [HttpGet("publisher/{id}")]
        public async Task<ActionResult<PublisherGetRequestDto>> GetPublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);

            if (publisher == null)
            {
                return NotFound($"Editeur avec l'ID {id} non trouvé.");
            }

            var publisherDto = _mapper.Map<PublisherGetRequestDto>(publisher);

            return Ok(publisherDto);
        }

        [HttpPut("publisher/{id}")]
        public async Task<IActionResult> PutPublisher(int id, [FromBody] PublisherEditRequestDto publisherDto)
        {
            try
            {
                var existingPublisher = await _context.Publishers.FindAsync(id);

                if (existingPublisher == null)
                {
                    return NotFound($"Editeur avec l'ID {id} non trouvé.");
                }

                _mapper.Map(publisherDto, existingPublisher);

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

        [HttpPost("publisher")]
        public async Task<ActionResult<PublisherCreateRequestDto>> PostPublisher([FromBody] PublisherCreateRequestDto publisherDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var publisher = _mapper.Map<Publisher>(publisherDto);

                _context.Publishers.Add(publisher);
                await _context.SaveChangesAsync();

                var publisherRequestDto = _mapper.Map<PublisherCreateRequestDto>(publisher);

                return CreatedAtAction("GetPublisher", new { id = publisher.Id }, publisherRequestDto);
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
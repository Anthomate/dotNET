using AutoMapper;
using BookStoreAPI.Data;
using BookStoreAPI.Entities.BookEntities;
using BookStoreAPI.Models.Dto.BookDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Controllers.BookControllers
{
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BookController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("book")]
        public async Task<ActionResult<List<BookGetRequestDto>>> GetBooks()
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.BookGenre)
                .ToListAsync();

            var booksDto = books.Select(_mapper.Map<BookGetRequestDto>).ToList();

            return Ok(booksDto);
        }

        [Authorize]
        [HttpGet("book/{id}")]
        public async Task<ActionResult<BookGetRequestDto>> GetBookByIdAsync(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.BookGenre)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound($"Livre avec l'ID {id} non trouvé.");
            }

            var bookDto = _mapper.Map<BookGetRequestDto>(book);

            return Ok(bookDto);
        }

        [Authorize]
        [HttpPut("book/{id}")]
        public async Task<IActionResult> PutBook(int id, [FromBody] BookCreateRequestDto bookDto)
        {
            try
            {
                var existingBook = await _context.Books.FindAsync(id);

                if (existingBook == null)
                {
                    return NotFound($"Livre avec l'ID {id} non trouvé.");
                }

                _mapper.Map(bookDto, existingBook);

                if (bookDto.AuthorId != 0)
                {
                    var author = await _context.Authors.FindAsync(bookDto.AuthorId);
                    if (author != null)
                    {
                        existingBook.Author = author;
                    }
                }

                if (bookDto.PublisherId != 0)
                {
                    var publisher = await _context.Publishers.FindAsync(bookDto.PublisherId);
                    if (publisher != null)
                    {
                        existingBook.Publisher = publisher;
                    }
                }

                if (bookDto.GenreId != 0)
                {
                    var genre = await _context.Genres.FindAsync(bookDto.GenreId);
                    if (genre != null)
                    {
                        existingBook.BookGenre = genre;
                    }
                }

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound($"Livre avec l'ID {id} non trouvé.");
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

        [Authorize]
        [HttpPost("book")]
        public async Task<ActionResult<BookCreateRequestDto>> PostBook([FromBody] BookCreateRequestDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingAuthor = await _context.Authors.FindAsync(bookDto.AuthorId);
                var existingGenre = await _context.Genres.FindAsync(bookDto.GenreId);
                var existingPublisher = await _context.Publishers.FindAsync(bookDto.PublisherId);

                if (existingAuthor == null)
                {
                    return NotFound($"Auteur avec l'ID {bookDto.AuthorId} non trouvé.");
                }
                if (existingGenre == null)
                {
                    return NotFound($"Genre avec l'ID {bookDto.GenreId} non trouvé.");
                }
                if (existingPublisher == null)
                {
                    return NotFound($"Editeur avec l'ID {bookDto.PublisherId} non trouvé.");
                }

                var book = _mapper.Map<Book>(bookDto);

                book.Author = existingAuthor ;
                book.BookGenre = existingGenre;
                book.Publisher = existingPublisher;


                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                var bookRequestDto = _mapper.Map<BookCreateRequestDto>(book);

                return CreatedAtAction("GetBooks", new { id = book.Id }, bookRequestDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s'est produite : {ex.Message}");
            }
        }

        [Authorize]
        [HttpDelete("book/{id}")]
        public async Task<ActionResult<Book>> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound($"Livre avec l'ID {id} non trouvé.");
            }

            try
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s'est produite : {ex.Message}");
            }
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
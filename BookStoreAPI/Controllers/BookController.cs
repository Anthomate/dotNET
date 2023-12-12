using BookStoreAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Controllers
{
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }

        //private static List<Book> _books = new List<Book>
        //{
        //    new Book { Id = 1, Title = "Le Seigneur des Anneaux", Author = "J.R.R. Tolkien", Publisher = "Éditions du Seigneur", PublicationDate = DateTime.Parse("1954-07-29T00:00:00"), Price = 29.99, ISBN = "978-2-1234-5678-9", BookGenre = Genre.Fantasy },
        //    new Book { Id = 2, Title = "Harry Potter à l'école des sorciers", Author = "J.K. Rowling", Publisher = "Éditions Magiques", PublicationDate = DateTime.Parse("1997-06-26T00:00:00"), Price = 24.99, ISBN = "978-1-2345-6789-0", BookGenre = Genre.Fantasy },
        //    new Book { Id = 3, Title = "1984", Author = "George Orwell", Publisher = "Big Brother Publications", PublicationDate = DateTime.Parse("1949-06-08T00:00:00"), Price = 19.99, ISBN = "978-3-4567-8901-2", BookGenre = Genre.ScienceFiction },
        //    new Book { Id = 4, Title = "Le Petit Prince", Author = "Antoine de Saint-Exupéry", Publisher = "Éditions du Petit Prince", PublicationDate = DateTime.Parse("1943-04-06T00:00:00"), Price = 14.99, ISBN = "978-4-5678-9012-3", BookGenre = Genre.Fiction },
        //    new Book { Id = 5, Title = "To Kill a Mockingbird", Author = "Harper Lee", Publisher = "Harper & Brothers", PublicationDate = DateTime.Parse("1960-07-11T00:00:00"), Price = 22.99, ISBN = "978-5-6789-0123-4", BookGenre = Genre.Fiction }
        //}; niiko

        [HttpGet("books")]
        public async Task<ActionResult<List<Book>>> GetBooks()
        {
            var books = await _context.Books
                .Include(book => book.Author)
                .Include(book => book.Publisher)
                .Include(book => book.BookGenre)
                .ToListAsync();

            return Ok(books);
        }

        [HttpGet("books/{id}")]
        public async Task<ActionResult<Book>> GetBookByIdAsync(int id)
        {
            var book = await _context.Books
                .Include(book => book.Author)
                .Include(book => book.Publisher)
                .Include(book => book.BookGenre)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost("books")]
        public async Task<ActionResult<Book>> PostBook([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetBooks), new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s'est produite : {ex.Message}");
            }
        }
    }
}
using BookStoreAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    [ApiController]
    public class BookController : ControllerBase
    {
        [HttpGet("books")]
        public ActionResult<List<Book>> GetBooks()
        {
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Le Seigneur des Anneaux", Author = "J.R.R. Tolkien" },
                new Book { Id = 2, Title = "Harry Potter à l'école des sorciers", Author = "J.K. Rowling" },
                new Book { Id = 3, Title = "1984", Author = "George Orwell" },
                new Book { Id = 4, Title = "Le Petit Prince", Author = "Antoine de Saint-Exupéry" },
                new Book { Id = 5, Title = "To Kill a Mockingbird", Author = "Harper Lee" }
            };

            return Ok(books);
        }

        [HttpPost("books")]
        public ActionResult<Book> PostBook([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                return CreatedAtAction(nameof(GetBooks), new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s'est produite : {ex.Message}");
            }
        }
    }
}

﻿using AutoMapper;
using BookStoreAPI.Data;
using BookStoreAPI.Entities.BookEntities;
using BookStoreAPI.Models.Dto.BookDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Controllers.BookControllers
{
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BookController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("book")]
        public async Task<ActionResult<List<BookGetRequestDto>>> GetBooks()
        {
            var books = await _context.Books.ToListAsync();

            var booksDto = new List<BookGetRequestDto>();

            foreach (var book in books)
            {
                booksDto.Add(_mapper.Map<BookGetRequestDto>(book));
            }

            return Ok(books);
        }

        [HttpGet("book/{id}")]
        public async Task<ActionResult<Book>> GetBookByIdAsync(int id)
        {
            var book = await _context.Books
                .Include(book => book.Author)
                .Include(book => book.Publisher)
                .Include(book => book.BookGenre)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound($"Livre avec l'ID {id} non trouvé.");
            }

            return Ok(book);
        }

        [HttpPut("book/{id}")]
        public async Task<IActionResult> PutBook(int id, [FromBody] BookCreateRequestDto bookDto)
        {
            try
            {
                var existingBook = await _context.Books
                    .Include(b => b.Author)
                    .Include(b => b.Publisher)
                    .Include(b => b.BookGenre)
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (existingBook == null)
                {
                    return NotFound($"Livre avec l'ID {id} non trouvé.");
                }

                existingBook.Title = bookDto.Title;
                existingBook.PublicationDate = bookDto.PublicationDate;
                existingBook.Price = bookDto.Price;
                existingBook.ISBN = bookDto.ISBN;

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

        [HttpPost("book")]
        public async Task<ActionResult<Book>> PostBook([FromBody] BookCreateRequestDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var author = await _context.Authors.FindAsync(bookDto.AuthorId);
                var publisher = await _context.Publishers.FindAsync(bookDto.PublisherId);
                var genre = await _context.Genres.FindAsync(bookDto.GenreId);

                if (author == null || publisher == null || genre == null)
                {
                    return NotFound("Une ou plusieurs entités référencées n'ont pas été trouvées");
                }

                var book = new Book
                {
                    Title = bookDto.Title,
                    Author = author,
                    Publisher = publisher,
                    PublicationDate = bookDto.PublicationDate,
                    Price = bookDto.Price,
                    ISBN = bookDto.ISBN,
                    BookGenre = genre
                };

                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetBooks", new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s'est produite : {ex.Message}");
            }
        }

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
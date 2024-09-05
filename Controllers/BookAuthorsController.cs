using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APICodeFirst.Models;
using System.Net;

namespace APICodeFirst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookAuthorsController : ControllerBase
    {
        private readonly BookDbContext _context;

        public BookAuthorsController(BookDbContext context)
        {
            _context = context;
        }

        // GET: api/BookAuthors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookAuthor>>> GetAuthorBooks()
        {
            return await _context.AuthorBooks
                .Include(ba => ba.book)
                .Include(ba => ba.author).ToListAsync();
        }

        // GET: api/BookAuthors/5
        [HttpGet("{bookId}/{authorId}")]
        public async Task<ActionResult<BookAuthor>> GetBookAuthor(int bookId, int authorId)
        {
            var bookAuthor = await _context.AuthorBooks
                .Include(ba => ba.book)
            .Include(ba => ba.author)
            .FirstOrDefaultAsync(ba => ba.BookId == bookId && ba.AuthorId == authorId);

            if (bookAuthor == null)
            {
                return NotFound();
            }

            return bookAuthor;
        }

        // PUT: api/BookAuthors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookAuthor(int id, BookAuthor bookAuthor)
        {
            if (id != bookAuthor.BookId)
            {
                return BadRequest();
            }

            _context.Entry(bookAuthor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookAuthorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BookAuthors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookAuthor>> PostBookAuthor(BookAuthor bookAuthor)
        {
            _context.AuthorBooks.Add(bookAuthor);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BookAuthorExists(bookAuthor.BookId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(GetBookAuthor), new { bookId = bookAuthor.BookId, authorId = bookAuthor.AuthorId }, bookAuthor);
        }

        // DELETE: api/BookAuthors/5
        [HttpDelete("{bookId}/{authorId}")]
        public async Task<IActionResult> DeleteBookAuthor(int bookId,int authorId)
        {
            var bookAuthor = await _context.AuthorBooks.FirstOrDefaultAsync(ba => ba.BookId == bookId && ba.AuthorId == authorId);
            if (bookAuthor == null)
            {
                return NotFound();
            }

            _context.AuthorBooks.Remove(bookAuthor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookAuthorExists(int id)
        {
            return _context.AuthorBooks.Any(e => e.BookId == id);
        }

        // GET: api/Books/ByAuthor/{authorId}
        [HttpGet("ByAuthor/{authorId}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByAuthor(int authorId)
        {
            var books = await _context.Books
                .Where(b => b.BookAuthors!.Any(ba => ba.AuthorId == authorId))
                .Include(b => b.BookAuthors!)
                    .ThenInclude(ba => ba.author)
                .ToListAsync();

            if (books == null || books.Count == 0)
            {
                return NotFound();
            }

            return books;
        }

        // GET: api/Authors/ByBook/{bookId}
        [HttpGet("ByBook/{bookId}")]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthorsByBook(int bookId)
        {
            var authors = await _context.Authors
                .Where(a => a.BookAuthors!.Any(ba => ba.BookId == bookId))
                .Include(a => a.BookAuthors!)
                    .ThenInclude(ba => ba.book)
                .ToListAsync();

            if (authors == null || authors.Count == 0)
            {
                return NotFound();
            }

            return authors;
        }

        // GET: api/BookAuthors/BooksWithAuthors
        [HttpGet("BooksWithAuthors")]
        public async Task<ActionResult<IEnumerable<object>>> GetBooksWithAuthors()
        {
            var result = await (from b in _context.Books
                                join ba in _context.AuthorBooks on b.BookId equals ba.BookId
                                join a in _context.Authors on ba.AuthorId equals a.AuthorId
                                select new
                                {
                                    b.BookId,
                                    b.Title,
                                    b.Price,
                                    AuthorName = a.AuthorName
                                }).ToListAsync();

            return Ok(result);
        }
        // GET: api/BookAuthors/AuthorsWithBooks
        [HttpGet("AuthorsWithBooks")]
        public async Task<ActionResult<IEnumerable<object>>> GetAuthorsWithBooks()
        {
            var result = await (from a in _context.Authors
                                join ba in _context.AuthorBooks on a.AuthorId equals ba.AuthorId
                                join b in _context.Books on ba.BookId equals b.BookId
                                select new
                                {
                                    a.AuthorId,
                                    a.AuthorName,
                                    BookTitle = b.Title,
                                    b.Price
                                }).ToListAsync();

            return Ok(result);
        }
    }
}

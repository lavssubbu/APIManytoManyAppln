using APICodeFirst.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICodeFirst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly BookDbContext _context;

        public AuthorsController(BookDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuthor()
        {
            return (await _context.Authors
                .Include(a => a.BookAuthors!)
                .ThenInclude(b => b.book)
                .ToListAsync()) is List<Author> authors && authors.Any()
           ? Ok(authors)
           : NotFound();

            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorbyid(int id)
        {
            var authors = await _context.Authors
                          .Include(a => a.BookAuthors!)
                          .ThenInclude(b=>b.book)
                          .FirstOrDefaultAsync(x=>x.AuthorId == id);

            return Ok(authors);
        }
    }
}

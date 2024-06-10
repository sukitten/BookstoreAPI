using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using MyApp.Models;
using MyApp.Data;

namespace MyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly MyDbContext _context;

        public AuthorsController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Author>> GetAuthors()
        {
            return _context.Authors.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Author> GetAuthor(int id)
        {
            var author = _context.Authors.Find(id);

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        [HttpPost]
        public ActionResult<Author> PostAuthor(Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAuthor), new { id = author.AuthorId }, author);
        }

        [HttpPut("{id}")]
        public IActionResult PutAuthor(int id, Author author)
        {
            if (id != author.AuthorId)
            {
                return BadRequest();
            }

            _context.Entry(author).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (System.Exception)
            {
                if (!_context.Authors.Any(e => e.AuthorId == id))
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

        [HttpDelete("{id}")]
        public IActionResult DeleteAuthor(int id)
        {
            var author = _context.Authors.Find(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
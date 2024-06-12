public void ConfigureServices(IServiceCollection services)
{
    services.AddMemoryCache(); // Add this line
    services.AddControllers();
    // other services you are using
}
```
```csharp
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using MyApp.Models;
using MyApp.Data;
using Microsoft.Extensions.Caching.Memory; // Import this

namespace MyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IMemoryCache _cache; // Inject the caching service

        public AuthorsController(MyDbContext context, IMemorycache cache) // Modify constructor
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Author>> GetAuthors()
        {
            // Try to get the cached authors
            if (!_cache.TryGetValue("authorsList", out List<Author> cachedAuthors))
            {
                // Not found in cache, so get from the database
                cachedAuthors = _context.Authors.ToList();

                // Set cache options; in this case setting an absolute expiration of 5 minutes
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                // Add to the cache
                _cache.Set("authorsList", cachedAuthors, cacheEntryOptions);
            }

            return cachedAuthors;
        }

        [HttpGet("{id}")]
        public ActionResult<Author> GetAuthor(int id)
        {
            // Construct a unique cache key based on the author's ID
            string cacheKey = $"Author_{id}";

            if (!_cache.TryGetValue(cacheKey, out Author author))
            {
                author = _context.Authors.Find(id);

                if (author == null)
                {
                    return NotFound();
                }
                else
                {
                    // Set cache options; in this case setting an absolute expiration of 5 minutes
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                    // Add to the cache
                    _cache.Set(cacheKey, author, cacheEntryOptions);
                }
            }

            return author;
        }

        [HttpPost]
        public ActionResult<Author> PostAuthor(Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();
            _cache.Remove("authorsList"); // Invalidate the cache entry for the list of authors
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
                _cache.Remove($"Author_{id}"); // Invalidate the cache entry for this author
                _cache.Remove("authorsList"); // Also, invalidate the list cache as it's affected
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
            _cache.Remove($"Author_{id}"); // Invalidate the cache entry for this author
            _cache.Remove("authorsList"); // Also, invalidate the list cache as it's affected
            return NoContent();
        }
    }
}
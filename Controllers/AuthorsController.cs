using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
import System.Linq;
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

        public AuthorsController(MyDbContext context, IMemoryCache cache) // Correct typo in constructor
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Author>> GetAuthors()
        {
            try
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
            catch (Exception ex)
            {
                // Log the exception as needed
                
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Author> GetAuthor(int id)
        {
            try
            {
                string cacheKey = $"Author_{id}";

                if (!_cache.TryGetValue(cacheKey, out Author author))
                {
                    author = _context.Authors.Find(id);

                    if (author == null)
                    {
                        return NotFound("Author not found.");
                    }
                    else
                    {
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                        _cache.Set(cacheKey, author, cacheEntryOptions);
                    }
                }

                return author;
            }
            catch (Exception ex)
            {
                // Log the error as needed
                
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpPost]
        public ActionResult<Author> PostAuthor(Author author)
        {
            try
            {
                _context.Authors.Add(author);
                _context.SaveChanges();
                _cache.Remove("authorsList"); // Invalidate the cache entry for the list of authors
                return CreatedAtAction(nameof(GetAuthor), new { id = author.AuthorId }, author);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                
                return StatusCode(500, "A problem occurred while saving the author.");
            }
        }

        [HttpPut("{id}")]
        public IActionResult PutAuthor(int id, Author author)
        {
            if (id != author.AuthorId)
            {
                return BadRequest("Author ID mismatch");
            }

            try
            {
                _context.Entry(author).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                _cache.Remove($"Author_{id}"); // Invalidate the cache entry for this author
                _cache.Remove("authorsList"); // Also, invalidate the list cache as it's affected
                return NoContent();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
            {
                if (!_context.Authors.Any(e => e.AuthorId == id))
                {
                    return NotFound("Author not found.");
                }
                else
                {
                    return StatusCode(409, "A concurrency error happened.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                
                return StatusCode(500, "A problem occurred while updating the author.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAuthor(int id)
        {
            try
            {
                var author = _context.Authors.Find(id);
                if (author == null)
                {
                    return NotFound("Author not found.");
                }

                _context.Authors.Remove(author);
                _context.SaveChanges();
                _cache.Remove($"Author_{id}"); // Invalidate the cache entry for this author
                _cache.Remove("authorsList"); // Also, invalidate the list cache as it's affected
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                
                return StatusCode(500, "A problem occurred while deleting the author.");
            }
        }
    }
}
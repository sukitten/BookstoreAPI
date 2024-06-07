using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Models;
using YourNamespace.Data;

public class BooksController : Controller
{
    private readonly YourDbContext _dbContext;

    public BooksController(YourDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> ListBooks(string searchKeyword, string sortOption)
    {
        ViewData["PublishedDateSortParam"] = string.IsNullOrEmpty(sortOption) ? "date_desc" : "";
        ViewData["CurrentSearch"] = searchKeyword;

        var booksQuery = from book in _dbContext.Books
                     select book;

        if (!string.IsNullOrEmpty(searchKeyword))
        {
            booksQuery = booksQuery.Where(book => book.Title.Contains(searchKeyword)
                                   || book.Author.Contains(searchKeyword));
        }

        switch (sortOption)
        {
            case "date_desc":
                booksQuery = booksQuery.OrderByDescending(book => book.PublishedDate);
                break;
            default:
                booksQuery = booksQuery.OrderBy(book => book.PublishedDate);
                break;
        }

        return View(await booksQuery.AsNoTracking().ToListAsync());
    }

    public async Task<IActionResult> ViewDetails(int? bookId)
    {
        if (bookId == null)
        {
            return NotFound();
        }

        var bookDetails = await _dbContext.Books
            .FirstOrDefaultAsync(book => book.Id == bookId);
        if (bookDetails == null)
        {
            return NotFound();
        }

        return View(bookDetails);
    }

    public IActionResult CreateBook()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBook([Bind("Id,Title,Author,PublishedDate")] Book book)
    {
        if (ModelState.IsValid)
        {
            _dbContext.Add(book);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(ListBooks));
        }
        return View(book);
    }

    public async Task<IActionResult> EditBook(int? bookId)
    {
        if (bookId == null)
        {
            return NotFound();
        }

        var bookToEdit = await _dbContext.Books.FindAsync(bookId);
        if (bookToEdit == null)
        {
            return NotFound();
        }
        return View(bookToEdit);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditBook(int bookId, [Bind("Id,Title,Author,PublishedDate")] Book book)
    {
        if (bookId != book.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _dbContext.Update(book);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IsBookExists(book.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(ListBooks));
        }
        return View(book);
    }

    public async Task<IActionResult> DeleteBook(int? bookId)
    {
        if (bookId == null)
        {
            return NotFound();
        }

        var bookToDelete = await _dbContext.Books
            .FirstOrDefaultAsync(book => book.Id == bookId);
        if (bookToDelete == null)
        {
            return NotFound();
        }

        return View(bookToDelete);
    }

    [HttpPost, ActionName("DeleteBook")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteBookConfirmed(int bookId)
    {
        var bookToDelete = await _dbContext.Books.FindAsync(bookId);
        _dbContext.Books.Remove(bookToDelete);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(ListBooks));
    }

    private bool IsBookExists(int bookId)
    {
        return _dbContext.Books.Any(book => book.Id == bookId);
    }
}
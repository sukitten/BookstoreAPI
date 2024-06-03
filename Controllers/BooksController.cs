using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using YourNamespace.Models;
using YourNamespace.Data;

public class BooksController : Controller
{
    private readonly YourDbContext _context;

    public BooksController(YourDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var books = _context.Books.ToList();
        return View(books);
    }

    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = _context.Books
            .FirstOrDefault(m => m.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Id,Title,Author,PublishedDate")] Book book)
    {
        if (ModelState.IsValid)
        {
            _context.Add(book);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(book);
    }

    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = _context.Books.Find(id);
        if (book == null)
        {
            return NotFound();
        }
        return View(book);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("Id,Title,Author,PublishedDate")] Book book)
    {
        if (id != book.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Update(book);
            _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }
        return View(book);
    }

    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = _context.Books
            .FirstOrDefault(m => m.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
    var book = _context.Books.Find(id);
    _context.Books.Remove(book);
    _context.SaveChanges();
    return RedirectToAction(nameof(Index));
    }
}
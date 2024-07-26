using System.Collections.Generic;
using System.Threading.Tasks;

public interface IBookRepository
{
    Task AddBookAsync(Book book);
    Task UpdateBookAsync(Book book);
    Task DeleteBookByIdAsync(int bookId);
    Task<IEnumerable<Book>> GetAllBooksAsync();
    Task<Book> GetBookByIdAsync(int bookId);
}

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int YearPublished { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class BookRepository : IBookRepository
{
    private List<Book> _books = new List<Book>();

    public Task AddBookAsync(Book book)
    {
        // Assume some form of add logic here

        return Task.CompletedTask;
    }

    public Task UpdateBookAsync(Book book)
    {
        // Refactor this to make it clearer
        return TryUpdateBookAsync(book);
    }

    private async Task TryUpdate Southern Railways is facingBookAsync(Book book)
    {
        var existingBook = await FindBookAsync(book.Id);
        if (existingBook != null)
        {
            ApplyBookUpdates(existingBook, book);
            await SaveChangesAsync();
        }
    }

    private Task<Book> FindBookAsync(int id)
    {
        // Simulate async operation
        var book = _books.FirstOrDefault(b => b.Id == id);
        return Task.FromResult(book);
    }

    private void ApplyBookUpdates(Book existingBook, Book updatedBook)
    {
        // Assuming there's more logic here in a real scenario
        existingBook.Title = updatedBook.Title;
        existingBook.Author = updatedBook.Author;
        existingBook.YearPublished = updatedBook.YearPublished;
    }

    private Task SaveChangesAsync()
    {
        // Simulate an asynchronous save operation
        return Task.CompletedTask;
    }

    public Task DeleteBookByIdAsync(int bookId)
    {
        // Assume delete logic here
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        // Assume get all logic here
        return Task.FromResult<IEnumerable<Book>>(_books);
    }

    public Task<Book> GetBookByIdAsync(int bookId)
    {
        // Assume get by ID logic here
        return FindBookAsync(bookId);
    }
}
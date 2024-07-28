using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IBookRepository
{
    Task AddBookAsync(Book book);
    Task UpdateBookAsync(Book book);
    Task DeleteBookByIdAsync(int bookId);
    Task<IEnumerable<Book>> GetAllBooksAsync();
    Task<Book> GetBookByIdAsync(int bookId);
    Task<IEnumerable<Book>> SearchBooksByTitleAsync(string title);
    Task<IEnumerable<Book>> SearchBooksByAuthorAsync(string author);
}

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int YearPublished { get; set; }
}

public class BookRepository : IBookRepository
{
    private List<Book> _books = new List<Book>();

    public Task AddBookAsync(Book book)
    {
        return Task.CompletedTask;
    }

    public Task UpdateBookAsync(Book book)
    {
        return TryUpdateBookAsync(book);
    }

    private async Task TryUpdateBookAsync(Book book)
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
        var book = _books.FirstOrDefault(b => b.Id == id);
        return Task.FromResult(book);
    }

    private void ApplyBookUpdates(Book existingBook, Book updatedBook)
    {
        existingBook.Title = updatedBook.Title;
        existingBook.Author = updatedBook.Author;
        existingBook.YearPublished = updatedBook.YearPublished;
    }

    private Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }

    public Task DeleteBookByIdAsync(int bookId)
    {
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        return Task.FromResult<IEnumerable<Book>>(_books);
    }

    public Task<Book> GetBookByIdAsync(int bookId)
    {
        return FindBookAsync(bookId);
    }

    public Task<IEnumerable<Book>> SearchBooksByTitleAsync(string title)
    {
        var filteredBooks = _books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult<IEnumerable<Book>>(filteredBooks);
    }

    public Task<IEnumerable<Book>> SearchBooksByAuthorAsync(string author)
    {
        var filteredBooks = _books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult<IEnumerable<Book>>(filteredBooks);
    }
}
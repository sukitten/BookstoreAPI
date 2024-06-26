using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class BookService
{
    private readonly AppModelsDBContext _dbContext; // Assuming you have a DbContext for EF Core.
    private readonly ILogger<BookService> _logger;

    public BookService(AppModelsDBContext dbContext, ILogger<BookService> _logger)
    {
        _dbContext = dbContext;
        this._logger = _logger;
    }
    
    public async Task<bool> AddBookAsync(Book newBook)
    {
        try
        {
            await _dbContext.Books.AddAsync(newBook);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add a new book: {BookTitle}", newBook.Title);
            return false;
        }
    }

    public async Task<bool> AddBooksAsync(IEnumerable<Book> newBooks)
    {
        try
        {
            await _dbContext.Books.AddRangeAsync(newBooks);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            if(newBooks != null && await newBooks.AnyAsync())
            {
                _logger.LogError(ex, "Failed to add new books, example title: {BookTitle}", newBooks.First().Title);
            }
            else
            {
                _logger.LogError(ex, "Failed to add new books, no books provided");
            }
            return false;
        }
    }
}
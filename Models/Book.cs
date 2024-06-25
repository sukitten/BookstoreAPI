using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class BookService
{
    private readonly AppDbContext _dbContext; // Assuming you have a DbContext for EF Core.
    private readonly ILogger<BookService> _logger;

    // Constructor's logger parameter type fixed to match BookService
    public BookService(AppDbContext dbContext, ILogger<BookService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    // Improved method name for clarity
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
            // Enhanced the log message for clarity
            _logger.LogError(ex, "Failed to add a new book: {BookTitle}", newBook.Title);

            return false; // Or consider a specific exception handling strategy.
        }
    }
}
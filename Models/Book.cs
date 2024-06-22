public class BookService
{
    private readonly AppDbContext _dbContext; // Assuming you have a DbContext for EF Core.
    private readonly ILogger<BookService> _logger;

    public BookService(AppDbContext dbContext, ILogger<BookCategory> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<bool> AddBookAsync(Book book)
    {
        try
        {
            await _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            // Log the exception detail
            _logger.LogError(ex, "An error occurred while adding a new book: {Title}", book.Title);
            // Optionally, refine the exception handling to differentiate between types of exceptions
            // and rethrow or handle accordingly.

            return false; // Or throw a custom exception to be handled further up the call stack.
        }
    }
}
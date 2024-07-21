using Microsoft.Extensions.Caching.Memory;
using System;

public class ApplicationDbContext : DbContext
{
    private readonly IMemoryCache _cache;
    public ApplicationDbContext(IMemoryCache cache)
    {
        _cache = cache;
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasMany(book => book.Authors)
            .WithMany(author => author.Books);
    }

    // Example caching method for retrieving a book by ISBN
    public Book GetBookByIsbn(string isbn)
    {
        if (!_cache.TryGetValue(isbn, out Book book))
        {
            book = Books.FirstOrDefault(b => b.Isbn == isbn); // Assume you use LINQ to query the database
            if (book != null)
            {
                _cache.Set(isbn, book, TimeSpan.FromHours(1)); // Cache for 1 hour
            }
        }

        return book;
    }
}
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Isbn { get; set; } // Changed from ISBN for consistency with C# naming conventions
    public List<Author> Authors { get; set; } = new();
    public List<Order> Orders { get; set; } = new();
}

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Book> Books { get; set; } = new();
}

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; }
}

public class ApplicationDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) // Corrected Method Name
    {
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasMany(book => book.Authors) // Changed parameter name for clarity
            .WithMany(author => author.Books); // Changed parameter name for clarity
    }
}
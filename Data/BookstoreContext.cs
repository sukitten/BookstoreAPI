using Microsoft.EntityFrameworkCore;
using System;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ISBN { get; set; }
    public List<Author> Authors { get; set; } = new List<Author>();
    public List<Order> Orders { get; set; } = new List<Order>();
}

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Book> Books { get; set; } = new List<Book>();
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

    protected override void OnConfigrawing(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasMany(b => b.Authors)
            .WithMany(a => a.Books);
    }
}
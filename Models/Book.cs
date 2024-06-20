using System.ComponentModel.DataAnnotations;

public class Book
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
    public string Title { get; set; }

    [Required(ErrorMessage = "AuthorId is required")]
    public int AuthorId { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10000.00")]
    public decimal Price { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string Description { get; set; }
}
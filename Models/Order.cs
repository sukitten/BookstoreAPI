using System;
using System.ComponentModel.DataAnnotations;

namespace YourNamespace.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int BookID { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
        
        [Required]
        public DateTime OrderDate { get; set; }
    }
}
using System.ComponentModel.DataAnnotations.Schema;
namespace _4IdiotsInc.Model
{
    [Table("Products")]
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Unit { get; set; } = "Php";
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public int? CategoryId { get; set; }
        public int Stock { get; set; }
    }
}

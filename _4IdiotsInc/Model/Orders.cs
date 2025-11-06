using System.ComponentModel.DataAnnotations.Schema;

namespace _4IdiotsInc.Model
{
    [Table("Orders")]
    public class Orders
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public int StatusId { get; set; }

        public int? ShippingAddressId { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual OrderStatus? Status { get; set; }
        public virtual UserAccount? User { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
    }

    [Table("OrderItems")]
    public class OrderItems
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // Navigation properties
        public virtual Orders? Order { get; set; }
        public virtual Products? Product { get; set; }
    }

    [Table("OrderStatus")]
    public class OrderStatus
    {
        public int Id { get; set; }
        public string StatusName { get; set; } = string.Empty;

        // Navigation property
        public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
    }
}
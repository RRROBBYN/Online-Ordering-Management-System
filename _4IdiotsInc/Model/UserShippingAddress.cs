using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _4IdiotsInc.Model
{
    [Table("UserShippingAddress")]
    public class UserShippingAddress
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string PostalCode { get; set; } = string.Empty;

        public bool IsDefault { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation property
        public virtual UserAccount? User { get; set; }
    }
}
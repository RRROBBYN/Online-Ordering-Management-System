using System.ComponentModel.DataAnnotations.Schema;

namespace _4IdiotsInc.Model
{
    [Table("UserAccount")]
    public class UserAccount
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 10);
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsLocked { get; set; }
    }
}

using Core.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Users
{
    public class UserRefreshToken : Entity<int>
    {
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public bool IsUsed { get; set; }
        public DateTime ExpirationDate { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}

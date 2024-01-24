using System.ComponentModel.DataAnnotations;

namespace CyberTestingPlatform.Auth.API.Models
{
    public class Account
    {
        [Key]
        public Guid UserId { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Roles { get; set; }
    }
}

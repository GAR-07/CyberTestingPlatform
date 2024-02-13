using System.ComponentModel.DataAnnotations;

namespace CyberTestingPlatform.DataAccess.Entites
{
    public class AccountEntity
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

using System.ComponentModel.DataAnnotations;

namespace CyberTestingPlatform.Auth.API.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Birthday { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
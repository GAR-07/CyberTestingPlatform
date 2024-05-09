namespace CyberTestingPlatform.User.API.Models
{
    public record class RegisterRequest(
        string Birthday,
        string UserName,
        string Email,
        string Role,
        string Password);
}
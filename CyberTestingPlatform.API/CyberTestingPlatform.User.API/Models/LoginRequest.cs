namespace CyberTestingPlatform.User.API.Models
{
    public record class LoginRequest(
        string Email,
        string Password);
}
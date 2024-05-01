namespace CyberTestingPlatform.Auth.API.Models
{
    public record class LoginRequest(
        string Email,
        string Password);
}
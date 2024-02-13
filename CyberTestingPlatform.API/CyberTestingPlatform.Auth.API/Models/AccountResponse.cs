namespace CyberTestingPlatform.Auth.API.Models
{
    public record AccountResponse(
        Guid UserId,
        DateTime Birthday,
        string Email,
        string UserName,
        string Roles
    );
}

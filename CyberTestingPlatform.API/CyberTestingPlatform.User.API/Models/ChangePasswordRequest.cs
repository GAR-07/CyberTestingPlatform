namespace CyberTestingPlatform.User.API.Models
{
    public record class ChangePasswordRequest(
        string OldPassword,
        string NewPassword);
}
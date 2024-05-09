using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.Application.Services
{
    public interface IAuthService
    {
        DateTime ConvertBirtdayDate(string birthday);
        string GenerateJwt(Account account);
        Task<Account> GetAccountByEmailAsync(string email);
        string GetPasswordHash(string password);
        bool ValidateAccount(Account account, string password);
        Task<bool> ValidateRegistration(string email, string role);
    }
}
using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.Application.Services
{
    public interface IAccountService
    {
        DateTime ConvertBirtdayDate(string birthday);
        Task<Guid> CreateAccountAsync(Account account);
        Task<Guid> DeleteAccountAsync(Guid userId);
        string GenerateJwt(Account account);
        Task<Account> GetAccountAsync(Guid userId);
        Task<Account> GetAccountByEmailAsync(string email);
        Task<Account> GetAccountDataAsync(Guid userId);
        Task<List<Account>> GetAllAccountsAsync();
        string GetPasswordHash(string password);
        Task<List<Account>> GetSelectAccountsAsync(int sampleSize, int page);
        Task<Guid> UpdateAccountAsync(Account account);
        bool ValidateAccount(Account account, string password);
        Task<bool> ValidateRegistration(string email, string role);
    }
}
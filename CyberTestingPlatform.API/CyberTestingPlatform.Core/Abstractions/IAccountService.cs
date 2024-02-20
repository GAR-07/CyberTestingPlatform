using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.Application.Services
{
    public interface IAccountService
    {
        Task<Guid> CreateAccount(Account account);
        Task<Guid> DeleteAccount(Guid userId);
        string GenerateJwt(Account account);
        Task<Account?> GetAccountByEmail(string email);
        Task<List<Account>> GetAllAccounts();
        Task<List<Account>?> GetSelectAccounts(int sampleSize, int page);
        bool IsAccountBanned(string roles);
        Task<bool> IsEmailAlreadyExists(string email);
        bool IsPasswordValid(string password, string passwordHash);
        Task<Guid> UpdateAccount(Guid userId, DateTime birthday, string email, string userName, string passwordHash, string roles);
        string? ValidateAccount(Account? account, string password);
        Task<string?> ValidateRegistration(string email, string role);
    }
}
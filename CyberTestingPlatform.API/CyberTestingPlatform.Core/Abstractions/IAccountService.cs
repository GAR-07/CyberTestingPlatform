using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.Application.Services
{
    public interface IAccountService
    {
        Task<Guid> CreateAccountAsync(Account account);
        Task<Guid> DeleteAccountAsync(Guid userId);
        Task<Account> GetAccountAsync(Guid userId);
        Task<Account> GetAccountByEmailAsync(string email);
        Task<List<Account>> GetAccountsAsync(string? searchText, int page, int pageSize);
        Task<Guid> UpdateAccountAsync(Account account);
        Task<Account> UpdateRolesAsync(Guid userId, string roles);
        Task<Guid> BlockAccountAsync(Guid userId);
    }
}
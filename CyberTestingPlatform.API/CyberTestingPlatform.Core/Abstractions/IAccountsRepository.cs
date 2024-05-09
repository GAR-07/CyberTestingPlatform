using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.DataAccess.Repositories
{
    public interface IAccountsRepository
    {
        Task<Guid?> CreateAsync(Account account);
        Task<Guid?> DeleteAsync(Guid userId);
        Task<Account?> GetAsync(Guid userId);
        Task<Account?> GetByEmailAsync(string email);
        Task<List<Account>?> GetSelectionAsync(string? searchText, int page, int pageSize);
        Task<Guid?> UpdateAsync(Account account);
    }
}
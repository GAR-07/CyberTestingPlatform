using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.DataAccess.Repositories
{
    public interface IAccountsRepository
    {
        Task<Guid?> CreateAsync(Account account);
        Task<Guid?> DeleteAsync(Guid userId);
        Task<Account?> GetAsync(Guid userId);
        Task<List<Account>?> GetAllAsync();
        Task<Account?> GetByEmailAsync(string email);
        Task<List<Account>?> GetSelectionAsync(int sampleSize, int page);
        Task<Guid?> UpdateAsync(Account account);
    }
}
using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.DataAccess.Repositories
{
    public interface IAccountsRepository
    {
        Task<Guid> Create(Account account);
        Task<Guid> Delete(Guid userId);
        Task<Account?> Get(Guid userId);
        Task<List<Account>> GetAll();
        Task<Account?> GetByEmail(string email);
        Task<List<Account>> GetSelection(int sampleSize, int page);
        Task<Guid> Update(Guid userId, DateTime birthday, string email, string userName, string passwordHash, string roles);
    }
}
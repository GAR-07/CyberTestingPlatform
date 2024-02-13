using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.Core.Abstractions
{
    public interface IAccountsRepository
    {
        Task<Guid> Create(Account account);
        Task<Guid> Delete(Guid userId);
        Task<List<Account>> GetAll();
        Task<Account?> GetByEmail(string email);
        Task<List<Account>> GetSelection(int sampleSize, int page);
        Task<Guid> Update(Guid userId, DateTime birthday, string email, string userName, string passwordHash, string roles);
    }
}
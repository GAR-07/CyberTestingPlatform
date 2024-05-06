using CyberTestingPlatform.Core.Models;
using CyberTestingPlatform.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CyberTestingPlatform.DataAccess.Repositories
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AccountsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Account>?> GetAllAsync()
        {
            var accountEntities = await _dbContext.Accounts
                .AsNoTracking()
                .ToListAsync();

            if (accountEntities == null)
            {
                return null;
            }

            var accounts = accountEntities
                .Select(x => new Account(x.UserId, x.Birthday, x.Email, x.UserName, x.PasswordHash, x.Roles))
                .ToList();

            return accounts;
        }

        public async Task<List<Account>?> GetSelectionAsync(int sampleSize, int page)
        {
            var totalCount = await _dbContext.Accounts.AsNoTracking().CountAsync();
            var startIndex = Math.Max(0, totalCount - sampleSize * page);
            var countToTake = Math.Min(sampleSize, totalCount - startIndex);

            var accountEntities = await _dbContext.Accounts
                .Skip(startIndex)
                .Take(countToTake)
                .AsNoTracking()
                .ToListAsync();

            if (accountEntities == null)
            {
                return null;
            }

            var accounts = accountEntities
               .Select(x => new Account(x.UserId, x.Birthday, x.Email, x.UserName, "", x.Roles))
               .ToList();

            return accounts;
        }

        public async Task<Account?> GetByEmailAsync(string email)
        {
            var accountEntity = await _dbContext.Accounts.AsNoTracking().FirstOrDefaultAsync(p => p.Email == email);

            if (accountEntity == null)
            {
                return null;
            }

            return new Account(
                accountEntity.UserId,
                accountEntity.Birthday,
                accountEntity.Email,
                accountEntity.UserName,
                accountEntity.PasswordHash,
                accountEntity.Roles);
        }

        public async Task<Account?> GetAsync(Guid userId)
        {
            var accountEntity = await _dbContext.Accounts.FirstOrDefaultAsync(p => p.UserId == userId);

            if (accountEntity == null)
            {
                return null;
            }

            return new Account(
                accountEntity.UserId,
                accountEntity.Birthday,
                accountEntity.Email,
                accountEntity.UserName,
                accountEntity.PasswordHash,
                accountEntity.Roles);
        }

        public async Task<Guid?> CreateAsync(Account account)
        {
            var accountEntity = new AccountEntity
            {
                UserId = account.UserId,
                Birthday = account.Birthday,
                Email = account.Email,
                UserName = account.UserName,
                PasswordHash = account.PasswordHash,
                Roles = account.Roles
            };

            await _dbContext.Accounts.AddAsync(accountEntity);
            await _dbContext.SaveChangesAsync();

            return accountEntity.UserId;
        }

        public async Task<Guid?> UpdateAsync(Account account)
        {
            var accountEntity = await _dbContext.Accounts
                .Where(p => p.UserId == account.UserId)
                .FirstOrDefaultAsync();

            if (accountEntity == null)
            {
                return null;
            }

            accountEntity.Birthday = account.Birthday;
            accountEntity.Email = account.Email;
            accountEntity.UserName = account.UserName;
            accountEntity.PasswordHash = account.PasswordHash;
            accountEntity.Roles = account.Roles;

            await _dbContext.SaveChangesAsync();

            return accountEntity.UserId;
        }

        public async Task<Guid?> DeleteAsync(Guid userId)
        {
            var accountEntity = await _dbContext.Accounts
                .Where(p => p.UserId == userId)
                .FirstOrDefaultAsync();

            if (accountEntity == null)
            {
                return null;
            }

            _dbContext.Remove(accountEntity);
            await _dbContext.SaveChangesAsync();

            return accountEntity.UserId;
        }
    }
}

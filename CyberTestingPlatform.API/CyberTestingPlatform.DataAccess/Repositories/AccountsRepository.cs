using CyberTestingPlatform.Core.Abstractions;
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

        public async Task<List<Account>> GetAll()
        {
            var accountEntities = await _dbContext.Accounts
                .AsNoTracking()
                .ToListAsync();

            var accounts = accountEntities
                .Select(x => Account.Create(x.UserId, x.Birthday, x.Email, x.UserName, x.PasswordHash, x.Roles).account)
                .ToList();

            return accounts;
        }

        public async Task<List<Account>> GetSelection(int sampleSize, int page)
        {
            var totalCount = await _dbContext.Accounts.CountAsync();
            var startIndex = Math.Max(0, totalCount - sampleSize * page);
            var countToTake = Math.Min(sampleSize, totalCount - startIndex);
            
            var accountEntities = await _dbContext.Accounts
                .Skip(startIndex)
                .Take(countToTake)
                .AsNoTracking()
                .ToListAsync();

            var accounts = accountEntities
               .Select(x => Account.Create(x.UserId, x.Birthday, x.Email, x.UserName, "", x.Roles).account)
               .ToList();

            return accounts;
        }

        public async Task<Account?> GetByEmail(string email)
        {
            var accountEntity = await _dbContext.Accounts.FirstOrDefaultAsync(p => p.Email == email);
            return accountEntity != null ? Account.Create(
                accountEntity.UserId,
                accountEntity.Birthday,
                accountEntity.Email,
                accountEntity.UserName,
                accountEntity.PasswordHash,
                accountEntity.Roles).account
                : null;
        }

        public async Task<Guid> Create(Account account)
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

        public async Task<Guid> Update(Guid userId, DateTime birthday, string email, string userName, string passwordHash, string roles)
        {
            var accountEntity = await _dbContext.Accounts
                .Where(p => p.UserId == userId)
                .FirstOrDefaultAsync();

            if (accountEntity != null)
            {
                accountEntity.Birthday = birthday;
                accountEntity.Email = email;
                accountEntity.UserName = userName;
                accountEntity.PasswordHash = passwordHash;
                accountEntity.Roles = roles;

                await _dbContext.SaveChangesAsync();

                return userId;
            }

            return Guid.Empty;
        }

        public async Task<Guid> Delete(Guid userId)
        {
            var accountEntity = await _dbContext.Accounts
                .Where(p => p.UserId == userId)
                .FirstOrDefaultAsync();

            if (accountEntity != null)
            {
                _dbContext.Remove(accountEntity);
                await _dbContext.SaveChangesAsync();

                return userId;
            }

            return Guid.Empty;
        }
    }
}

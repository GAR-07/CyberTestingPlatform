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

        public async Task<List<Account>?> GetSelectionAsync(string? searchText, int page, int pageSize)
        {
            var query = _dbContext.Accounts.AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(x => x.UserName.Contains(searchText));
            }

            var totalCount = await query.CountAsync();
            var startIndex = Math.Max(0, totalCount - pageSize * page);
            var countToTake = Math.Min(pageSize, totalCount - startIndex);

            var accountEntities = await query
                .Skip(startIndex)
                .Take(countToTake)
                .AsNoTracking()
                .ToListAsync();

            if (accountEntities == null)
            {
                return null;
            }

            var accounts = accountEntities
               .Select(x => new Account(x.UserId, x.Birthday, x.RegistrationDate, x.Email, x.UserName, "", x.Roles, x.ImagePath))
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
                accountEntity.RegistrationDate,
                accountEntity.Email,
                accountEntity.UserName,
                accountEntity.PasswordHash,
                accountEntity.Roles,
                accountEntity.ImagePath);
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
                accountEntity.RegistrationDate,
                accountEntity.Email,
                accountEntity.UserName,
                accountEntity.PasswordHash,
                accountEntity.Roles,
                accountEntity.ImagePath);
        }

        public async Task<Guid?> CreateAsync(Account account)
        {
            var accountEntity = new AccountEntity
            {
                UserId = account.UserId,
                Birthday = account.Birthday,
                RegistrationDate = account.RegistrationDate,
                Email = account.Email,
                UserName = account.UserName,
                PasswordHash = account.PasswordHash,
                Roles = account.Roles,
                ImagePath = account.ImagePath,
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
            accountEntity.ImagePath = account.ImagePath;
            accountEntity.RegistrationDate = account.RegistrationDate;

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

        public async Task<Account?> UpdateRolesAsync(Guid userId, string roles)
        {
            var accountEntity = await _dbContext.Accounts
                .Where(p => p.UserId == userId)
                .FirstOrDefaultAsync();

            if (accountEntity == null)
            {
                return null;
            }

            accountEntity.Roles = roles;

            await _dbContext.SaveChangesAsync();

            return new Account(
                accountEntity.UserId,
                accountEntity.Birthday,
                accountEntity.RegistrationDate,
                accountEntity.Email,
                accountEntity.UserName,
                accountEntity.PasswordHash,
                accountEntity.Roles,
                accountEntity.ImagePath);
        }

        public async Task<Guid?> BlockAccountAsync(Guid userId)
        {
            var accountEntity = await _dbContext.Accounts
                .Where(p => p.UserId == userId)
                .FirstOrDefaultAsync();

            if (accountEntity == null)
            {
                return null;
            }

            var roles = accountEntity.Roles.Split(',');
            if (!roles.Contains("Banned"))
            {
                accountEntity.Roles += ",Banned";
                await _dbContext.SaveChangesAsync();
            }

            await _dbContext.SaveChangesAsync();

            return accountEntity.UserId;
        }
    }
}

using CyberTestingPlatform.DataAccess.Repositories;
using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountsRepository _accountsRepository;

        public AccountService(IAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }

        public async Task<List<Account>> GetAccountsAsync(string? searchText, int page, int pageSize)
        {
            if (pageSize <= 0 || page < 0)
            {
                throw new Exception($"Заданы невозможные параметры для поиска");
            }

            var account = await _accountsRepository.GetSelectionAsync(searchText, page, pageSize)
                ?? throw new Exception($"Ничего не найдено");

            return account;
        }

        public async Task<Account> GetAccountByEmailAsync(string email)
        {
            var account = await _accountsRepository.GetByEmailAsync(email)
                ?? throw new Exception($"Аккаунт с почтой {email} не найден");

            account.PasswordHash = "";

            return account;
        }

        public async Task<Account> GetAccountAsync(Guid userId)
        {
            var account = await _accountsRepository.GetAsync(userId)
                ?? throw new Exception($"Аккаунт {userId} не найден");
            account.PasswordHash = "";
            return account;
        }

        public async Task<Guid> CreateAccountAsync(Account account)
        {
            return await _accountsRepository.CreateAsync(account)
                ?? throw new Exception($"Ошибка создания аккаунта");
        }

        public async Task<Guid> UpdateAccountAsync(Account account)
        {
            return await _accountsRepository.UpdateAsync(account)
                ?? throw new Exception($"Аккаунт {account.UserId} не найден");
        }

        public async Task<Guid> DeleteAccountAsync(Guid userId)
        {
            return await _accountsRepository.DeleteAsync(userId)
                ?? throw new Exception($"Аккаунт {userId} не найден");
        }
    }
}

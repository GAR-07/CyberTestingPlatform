using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BCrypt.Net;
using CyberTestingPlatform.DataAccess.Repositories;
using CyberTestingPlatform.Core.Models;
using CyberTestingPlatform.Core.Shared;

namespace CyberTestingPlatform.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthOptions _authOptions;
        private readonly IAccountsRepository _accountsRepository;

        public AuthService(
            IConfiguration configuration,
            IAccountsRepository accountsRepository)
        {
            _authOptions = configuration.GetSection("AuthOptions").Get<AuthOptions>();
            _accountsRepository = accountsRepository;
        }

        public string GenerateJwt(Account account)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, account.UserName),
                new Claim(JwtRegisteredClaimNames.Email, account.Email),
                new Claim(JwtRegisteredClaimNames.Birthdate, account.Birthday.ToString())
            };

            foreach (var role in account.Roles.Split(','))
            {
                claims.Add(new Claim("role", role.ToString()));
            }

            var jwtToken = new JwtSecurityToken(
                issuer: _authOptions.Issuer,
                audience: _authOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddSeconds(_authOptions.TokenLifeTime),
                signingCredentials: new SigningCredentials(_authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        public async Task<Account> GetAccountByEmailAsync(string email)
        {
            return await _accountsRepository.GetByEmailAsync(email) ?? throw new Exception($"Аккаунт с почтой {email} не найден");
        }

        public bool ValidateAccount(Account account, string password)
        {
            if (account == null)
                throw new Exception($"Аккаунт {account} не существует");

            if (IsAccountBanned(account.Roles))
                throw new Exception($"Аккаунт {account} заблокирован");

            if (!IsPasswordValid(password, account.PasswordHash))
                throw new Exception($"Неверный пароль");

            return true;
        }

        public async Task<bool> ValidateRegistration(string email, string role)
        {
            if (role != "User" && role != "Teacher")
                throw new Exception($"Задана неверная роль пользователя");

            if (await IsEmailAlreadyExists(email))
                throw new Exception($"Адрес электронной почты уже занят");

            return true;
        }

        public string GetPasswordHash(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, HashType.SHA512);
        }

        public DateTime ConvertBirtdayDate(string birthday)
        {
            var date = birthday.Split('-').Select(Int32.Parse).ToArray();
            return new DateTime(date[2], date[1], date[0]);
        }

        private static bool IsPasswordValid(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash, HashType.SHA512);
        }

        private static bool IsAccountBanned(string roles)
        {
            return roles.Split(',').Contains("Banned");
        }

        private async Task<bool> IsEmailAlreadyExists(string email)
        {
            return await _accountsRepository.GetByEmailAsync(email) != null;
        }
    }
}

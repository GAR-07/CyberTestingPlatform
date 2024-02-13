namespace CyberTestingPlatform.Core.Models
{
    public class Account
    {
        public static readonly DateTime MIN_BIRTHDAY_DATE = new(1900, 1, 1);
        public static readonly DateTime MAX_BIRTHDAY_DATE = DateTime.Now;

        private Account(Guid userId, DateTime birthday, string email, string userName, string passwordHash, string roles)
        {
            UserId = userId;
            Birthday = birthday;
            Email = email;
            UserName = userName;
            PasswordHash = passwordHash;
            Roles = roles;
        }

        public Guid UserId { get; }
        public DateTime Birthday { get; }
        public string Email { get; }
        public string UserName { get; }
        public string PasswordHash { get; }
        public string Roles { get; }

        public static (Account account, string Error) Create(Guid userId, DateTime birthday, string email, string userName, string passwordHash, string roles)
        {
            var error = string.Empty;

            if (birthday < MIN_BIRTHDAY_DATE || birthday > MAX_BIRTHDAY_DATE)
            {
                error = $"День рождения не вписывается в допустимые временные рамки: ({MIN_BIRTHDAY_DATE} - {MAX_BIRTHDAY_DATE}";
            }

            var account = new Account(userId, birthday, email, userName, passwordHash, roles);

            return (account, error);
        }
    }
}

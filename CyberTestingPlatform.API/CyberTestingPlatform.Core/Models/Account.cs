namespace CyberTestingPlatform.Core.Models
{
    public class Account(Guid userId, DateTime birthday, string email, string userName, string passwordHash, string roles)
    {
        public static readonly DateTime MIN_BIRTHDAY_DATE = new(1900, 1, 1);
        public static readonly DateTime MAX_BIRTHDAY_DATE = DateTime.Now;

        public Guid UserId { get; set; } = userId;
        public DateTime Birthday { get; set; } = birthday;
        public string Email { get; set; } = email;
        public string UserName { get; set; } = userName;
        public string PasswordHash { get; set; } = passwordHash;
        public string Roles { get; set; } = roles;

        //if (birthday < MIN_BIRTHDAY_DATE || birthday > MAX_BIRTHDAY_DATE)
        //{
        //    error = $"День рождения не вписывается в допустимые временные рамки: ({MIN_BIRTHDAY_DATE} - {MAX_BIRTHDAY_DATE}";
        //}
    }
}

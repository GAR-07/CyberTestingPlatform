namespace CyberTestingPlatform.Core.Models
{
    public class Account
    {
        public static readonly DateTime MIN_REGISTER_DATE = new DateTime(2024, 1, 1);
        public static readonly DateTime MIN_BIRTHDAY_DATE = new DateTime(1900, 1, 1);
        public static readonly DateTime MAX_DATE = DateTime.Now;

        public Guid UserId { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Roles { get; set; }
        public string? ImagePath { get; set; }

        public Account(Guid userId, DateTime birthday, DateTime registrationDate, string email, string userName, string passwordHash, string roles, string? imagePath)
        {
            UserId = userId;
            Birthday = birthday;
            RegistrationDate = registrationDate;
            Email = email;
            UserName = userName;
            PasswordHash = passwordHash;
            Roles = roles;
            ImagePath = imagePath;

            if (birthday < MIN_BIRTHDAY_DATE || birthday > MAX_DATE)
            {
                throw new Exception($"День рождения не вписывается в допустимые временные рамки: ({MIN_BIRTHDAY_DATE} - {MAX_DATE}");
            }

            if (registrationDate < MIN_REGISTER_DATE || registrationDate > MAX_DATE)
            {
                throw new Exception($"Дата регистрации не вписывается в допустимые временные рамки: ({MIN_REGISTER_DATE} - {MAX_DATE}");
            }
        }
    }
}
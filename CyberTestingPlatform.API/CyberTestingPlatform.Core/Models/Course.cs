namespace CyberTestingPlatform.Core.Models
{
    public class Course
    {
        public const int MAX_TITLE_LENGTH = 255;

        private Course(Guid id, string theme, string title, string description, Guid creatorID, DateTime creationDate, DateTime lastUpdationDate)
        {
            Id = id;
            Theme = theme;
            Title = title;
            Description = description;
            CreatorID = creatorID;
            CreationDate = creationDate;
            LastUpdationDate = lastUpdationDate;
        }

        public Guid Id { get; }
        public string Theme { get; }
        public string Title { get; }
        public string Description { get; }
        public Guid CreatorID { get; }
        public DateTime CreationDate { get; }
        public DateTime LastUpdationDate { get; }

        public static (Course course, string Error) Create(Guid id, string theme, string title, string description, Guid creatorID, DateTime creationDate, DateTime lastUpdationDate)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(theme))
            {
                error = $"Тема не может быть пустой";
            }
            if (string.IsNullOrEmpty(title) || title.Length > MAX_TITLE_LENGTH)
            {
                error = $"Заголовок не может быть пустым или превышать {MAX_TITLE_LENGTH} символов";
            }

            var course = new Course(id, theme, title, description, creatorID, creationDate, lastUpdationDate);

            return (course, error);
        }
    }
}

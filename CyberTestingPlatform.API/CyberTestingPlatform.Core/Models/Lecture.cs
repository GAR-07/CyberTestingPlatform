namespace CyberTestingPlatform.Core.Models
{
    public class Lecture
    {
        public const int MAX_THEME_LENGTH = 255;
        public const int MAX_TITLE_LENGTH = 255;

        private Lecture(Guid id, string theme, string title, string content, Guid creatorID, DateTime creationDate, DateTime lastUpdationDate, Guid courseId)
        {
            Id = id;
            Theme = theme;
            Title = title;
            Content = content;
            CreatorID = creatorID;
            CreationDate = creationDate;
            LastUpdationDate = lastUpdationDate;
            CourseId = courseId;
        }

        public Guid Id { get; }
        public string Theme { get; }
        public string Title { get; }
        public string Content { get; }
        public Guid CreatorID { get; }
        public DateTime CreationDate { get; }
        public DateTime LastUpdationDate { get; }
        public Guid CourseId { get; }

        public static (Lecture lecture, string Error) Create(Guid id, string theme, string title, string content, Guid creatorID, DateTime creationDate, DateTime lastUpdationDate, Guid courseId)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(theme) || theme.Length > MAX_THEME_LENGTH)
            {
                error = $"Тема не может быть пустой или превышать {MAX_THEME_LENGTH} символов";
            }
            if (string.IsNullOrEmpty(title) || title.Length > MAX_TITLE_LENGTH)
            {
                error = $"Заголовок не может быть пустым или превышать {MAX_TITLE_LENGTH} символов";
            }

            var lecture = new Lecture(id, theme, title, content, creatorID, creationDate, lastUpdationDate, courseId);

            return (lecture, error);
        }
    }
}

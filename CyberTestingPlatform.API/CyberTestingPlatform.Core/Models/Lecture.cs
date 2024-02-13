namespace CyberTestingPlatform.Core.Models
{
    public class Lecture
    {
        public const int MAX_TITLE_LENGTH = 255;

        private Lecture(Guid id, string theme, string title, string content, Guid creatorID, DateTime creationDate, DateTime lastUpdationDate)
        {
            Id = id;
            Theme = theme;
            Title = title;
            Content = content;
            CreatorID = creatorID;
            CreationDate = creationDate;
            LastUpdationDate = lastUpdationDate;
        }

        public Guid Id { get; }
        public string Theme { get; }
        public string Title { get; }
        public string Content { get; }
        public Guid CreatorID { get; }
        public DateTime CreationDate { get; }
        public DateTime LastUpdationDate { get; }

        public static (Lecture lecture, string Error) Create(Guid id, string theme, string title, string content, Guid creatorID, DateTime creationDate, DateTime lastUpdationDate)
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

            var lecture = new Lecture(id, theme, title, content, creatorID, creationDate, lastUpdationDate);

            return (lecture, error);
        }
    }
}

namespace CyberTestingPlatform.Core.Models
{
    public class Course
    {
        public const int MAX_THEME_LENGTH = 255;
        public const int MAX_TITLE_LENGTH = 255;

        private Course(Guid id, string theme, string title, string description, string content, int price, string imagePath, Guid creatorID, DateTime creationDate, DateTime lastUpdationDate)
        {
            Id = id;
            Theme = theme;
            Title = title;
            Description = description;
            Content = content;
            Price = price;
            ImagePath = imagePath;
            CreatorID = creatorID;
            CreationDate = creationDate;
            LastUpdationDate = lastUpdationDate;
        }

        public Guid Id { get; }
        public string Theme { get; }
        public string Title { get; }
        public string Description { get; }
        public string Content { get; }
        public int Price { get; }
        public string ImagePath { get; }
        public Guid CreatorID { get; }
        public DateTime CreationDate { get; }
        public DateTime LastUpdationDate { get; }

        public static (Course course, string Error) Create(Guid id, string theme, string title, string description, string content, int price, string imagePath, Guid creatorID, DateTime creationDate, DateTime lastUpdationDate)
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
            if (price < 0)
            {
                error = $"Цена не может иметь отрицательное значение";
            }

            var course = new Course(id, theme, title, description, content, price, imagePath, creatorID, creationDate, lastUpdationDate);

            return (course, error);
        }
    }
}

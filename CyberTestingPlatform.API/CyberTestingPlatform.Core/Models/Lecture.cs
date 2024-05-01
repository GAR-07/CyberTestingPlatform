namespace CyberTestingPlatform.Core.Models
{
    public class Lecture(Guid id, string theme, string title, string content, int position, Guid creatorID, DateTime creationDate, DateTime lastUpdationDate, Guid courseId)
    {
        public const int MAX_THEME_LENGTH = 255;
        public const int MAX_TITLE_LENGTH = 255;

        public Guid Id { get; set; } = id;
        public string Theme { get; set; } = theme;
        public string Title { get; set; } = title;
        public string Content { get; set; } = content;
        public int Position { get; set; } = position;
        public Guid CreatorID { get; set; } = creatorID;
        public DateTime CreationDate { get; set; } = creationDate;
        public DateTime LastUpdationDate { get; set; } = lastUpdationDate;
        public Guid CourseId { get; set; } = courseId;


        //    if (string.IsNullOrEmpty(theme) || theme.Length > MAX_THEME_LENGTH)
        //    {
        //        error = $"Тема не может быть пустой или превышать {MAX_THEME_LENGTH} символов";
        //    }
        //    if (string.IsNullOrEmpty(title) || title.Length > MAX_TITLE_LENGTH)
        //    {
        //        error = $"Заголовок не может быть пустым или превышать {MAX_TITLE_LENGTH} символов";
        //    }
        //    if (position < 0)
        //    {
        //        error = $"Позиция не должна повторяться или быть меньше нуля";
        //    }
    }
}

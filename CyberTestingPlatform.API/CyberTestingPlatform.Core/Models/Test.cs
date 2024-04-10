namespace CyberTestingPlatform.Core.Models
{
    public class Test
    {
        public const int MAX_THEME_LENGTH = 255;
        public const int MAX_TITLE_LENGTH = 255;

        private Test(Guid id, string theme, string title, string questions, string answerOptions, string answerCorrect, int position, Guid creatorID, DateTime creationDate, DateTime lastUpdationDate, Guid courseId)
        {
            Id = id;
            Theme = theme;
            Title = title;
            Questions = questions;
            AnswerOptions = answerOptions;
            AnswerCorrect = answerCorrect;
            Position = position;
            CreatorID = creatorID;
            CreationDate = creationDate;
            LastUpdationDate = lastUpdationDate;
            CourseId = courseId;
        }

        public Guid Id { get; }
        public string Theme { get; }
        public string Title { get; }
        public string Questions { get; }
        public string AnswerOptions { get; }
        public string AnswerCorrect { get; }
        public int Position { get; set; }
        public Guid CreatorID { get; }
        public DateTime CreationDate { get; }
        public DateTime LastUpdationDate { get; }
        public Guid CourseId { get; }

        public static (Test test, string Error) Create(Guid id, string theme, string title, string questions, string answerOptions, string answerCorrect, int position, Guid creatorID, DateTime creationDate, DateTime lastUpdationDate, Guid courseId)
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
            if (string.IsNullOrEmpty(questions) || questions.Split(';').Length <= 0)
            {
                error = $"Должен присутствовать как минимум 1 вопрос";
            }
            if (string.IsNullOrEmpty(answerOptions) || answerOptions.Split(';').Length <= 0)
            {
                error = $"Должен присутствовать как минимум 1 вариант ответа";
            }
            if (string.IsNullOrEmpty(answerCorrect) || answerCorrect.Split(';').Length <= 0)
            {
                error = $"Должен присутствовать как минимум 1 правильный ответ";
            }
            if (questions.Split(';').Length != answerCorrect.Split(';').Length || questions.Split(';').Length != answerOptions.Split(';').Length)
            {
                error = $"Количество вопросов и ответов должно совпадать";
            }
            if (position < 0)
            {
                error = $"Позиция не должна повторяться или быть меньше нуля";
            }
            
            var test = new Test(id, theme, title, questions, answerOptions, answerCorrect, position, creatorID, creationDate, lastUpdationDate, courseId);

            return (test, error);
        }
    }
}

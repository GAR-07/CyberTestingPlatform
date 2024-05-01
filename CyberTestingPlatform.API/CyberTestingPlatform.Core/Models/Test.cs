namespace CyberTestingPlatform.Core.Models
{
    public class Test(Guid id, string theme, string title, string questions, string answerOptions, string correctAnswers, int position, Guid creatorID, DateTime creationDate, DateTime lastUpdationDate, Guid courseId)
    {
        public const int MAX_THEME_LENGTH = 255;
        public const int MAX_TITLE_LENGTH = 255;

        public Guid Id { get; set; } = id;
        public string Theme { get; set; } = theme;
        public string Title { get; set; } = title;
        public string Questions { get; set; } = questions;
        public string AnswerOptions { get; set; } = answerOptions;
        public string CorrectAnswers { get; set; } = correctAnswers;
        public int Position { get; set; } = position;
        public Guid CreatorID { get; set; } = creatorID;
        public DateTime CreationDate { get; set; } = creationDate;
        public DateTime LastUpdationDate { get; set; } = lastUpdationDate;
        public Guid CourseId { get; set; } = courseId;

        //if (string.IsNullOrEmpty(theme) || theme.Length > MAX_THEME_LENGTH)
        //{
        //    error = $"Тема не может быть пустой или превышать {MAX_THEME_LENGTH} символов";
        //}
        //if (string.IsNullOrEmpty(title) || title.Length > MAX_TITLE_LENGTH)
        //{
        //    error = $"Заголовок не может быть пустым или превышать {MAX_TITLE_LENGTH} символов";
        //}
        //if (string.IsNullOrEmpty(questions) || questions.Split('\n').Length <= 0)
        //{
        //    error = $"Должен присутствовать как минимум 1 вопрос";
        //}
        //if (string.IsNullOrEmpty(answerOptions) || answerOptions.Split('\n').Length <= 0)
        //{
        //    error = $"Должен присутствовать как минимум 1 вариант ответа";
        //}
        //if (string.IsNullOrEmpty(correctAnswers) || correctAnswers.Split('\n').Length <= 0)
        //{
        //    error = $"Должен присутствовать как минимум 1 правильный ответ";
        //}
        //if (questions.Split('\n').Length != correctAnswers.Split('\n').Length || questions.Split('\n').Length != answerOptions.Split('\n').Length)
        //{
        //    error = $"Количество вопросов и ответов должно совпадать";
        //}
        //if (position < 0)
        //{
        //    error = $"Позиция не должна повторяться или быть меньше нуля";
        //}
    }
}

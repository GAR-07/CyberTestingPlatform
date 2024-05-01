namespace CyberTestingPlatform.Resourse.API.Models
{
    public record TestsResponse (
        Guid Id,
        string Theme,
        string Title,
        string Questions,
        string AnswerOptions,
        string CorrectAnswers,
        int Position,
        Guid CreatorId,
        DateTime CreationDate,
        DateTime LastUpdationDate,
        Guid CourseId);
}

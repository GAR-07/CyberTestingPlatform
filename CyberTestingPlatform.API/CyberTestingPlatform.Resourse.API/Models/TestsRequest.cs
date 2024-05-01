namespace CyberTestingPlatform.Resourse.API.Models
{
    public record TestsRequest(
        string Id,
        string Theme,
        string Title,
        string Questions,
        string AnswerOptions,
        string CorrectAnswers,
        int Position,
        Guid CreatorId,
        string CreationDate,
        string LastUpdationDate,
        Guid CourseId);
}

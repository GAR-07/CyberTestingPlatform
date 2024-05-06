namespace CyberTestingPlatform.Resourse.API.Models
{
    public record TestResultsResponse (
        Guid Id,
        Guid TestId,
        Guid UserId,
        string Answers,
        string Results,
        DateTime CreationDate);
}

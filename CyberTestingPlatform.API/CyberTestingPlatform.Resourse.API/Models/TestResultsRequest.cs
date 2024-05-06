namespace CyberTestingPlatform.Resourse.API.Models
{
    public record TestResultsRequest (
        string TestId,
        string UserId,
        string Answers);
}

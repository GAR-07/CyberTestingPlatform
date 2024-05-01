namespace CyberTestingPlatform.Resourse.API.Models
{
    public record TestResultRequest (
        string Id,
        string TestId,
        string UserId,
        string Answers,
        string Results,
        string CreationDate);
}

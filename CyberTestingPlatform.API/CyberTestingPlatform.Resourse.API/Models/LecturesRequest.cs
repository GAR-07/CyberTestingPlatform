namespace CyberTestingPlatform.Resourse.API.Models
{
    public record LecturesRequest(
        string Id,
        string Theme,
        string Title,
        string Content,
        int Position,
        Guid CreatorId,
        string CreationDate,
        string LastUpdationDate,
        Guid CourseId);
}

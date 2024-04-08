namespace CyberTestingPlatform.Resourse.API.Models
{
    public record LecturesResponse (
        Guid Id,
        string Theme,
        string Title,
        string Content,
        int Position,
        Guid CreatorId,
        DateTime CreationDate,
        DateTime LastUpdationDate,
        Guid CourseId);
}

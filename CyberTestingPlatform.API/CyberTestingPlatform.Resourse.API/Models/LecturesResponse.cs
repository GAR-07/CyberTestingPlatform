namespace CyberTestingPlatform.Resourse.API.Models
{
    public record LecturesResponse (
        Guid Id,
        string Theme,
        string Title,
        string Content,
        Guid CreatorId,
        DateTime CreationDate,
        DateTime LastUpdationDate,
        Guid CourseId);
}

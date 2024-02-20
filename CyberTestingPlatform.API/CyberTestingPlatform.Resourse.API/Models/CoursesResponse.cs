namespace CyberTestingPlatform.Resourse.API.Models
{
    public record CoursesResponse (
        Guid Id,
        string Theme,
        string Title,
        string Description,
        string Content,
        int Price,
        string ImagePath,
        Guid CreatorId,
        DateTime CreationDate,
        DateTime LastUpdationDate);
}

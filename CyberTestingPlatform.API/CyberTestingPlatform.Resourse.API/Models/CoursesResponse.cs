namespace CyberTestingPlatform.Resourse.API.Models
{
    public record CoursesResponse (
        Guid Id,
        string Name,
        string Description,
        int Price,
        string ImagePath,
        Guid CreatorId,
        DateTime CreationDate,
        DateTime LastUpdationDate);
}

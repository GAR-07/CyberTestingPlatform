namespace CyberTestingPlatform.Resourse.API.Models
{
    public record CoursesRequest (
        string Name,
        string Description,
        int Price,
        string ImagePath,
        Guid CreatorId,
        string CreationDate,
        string LastUpdationDate);
}

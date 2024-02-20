namespace CyberTestingPlatform.Resourse.API.Models
{
    public record CoursesRequest (
        string Theme,
        string Title,
        string Description,
        string Content,
        int Price,
        string ImagePath,
        Guid CreatorId,
        string CreationDate,
        string LastUpdationDate);
}

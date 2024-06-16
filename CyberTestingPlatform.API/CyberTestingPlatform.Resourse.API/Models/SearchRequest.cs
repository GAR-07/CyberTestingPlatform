namespace CyberTestingPlatform.Resourse.API.Models
{
    public record class SearchRequest(
        string? SearchText,
        int PageSize,
        int Page);
}
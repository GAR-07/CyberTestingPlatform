namespace CyberTestingPlatform.User.API.Models
{
    public record class SearchRequest(
        string? SearchText,
        int PageSize,
        int Page);
}
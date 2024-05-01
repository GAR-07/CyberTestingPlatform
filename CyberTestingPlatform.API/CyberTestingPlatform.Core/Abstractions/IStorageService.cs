using Microsoft.AspNetCore.Http;

namespace CyberTestingPlatform.Application.Services
{
    public interface IStorageService
    {
        Task<(string, string)> SaveFile(IFormFile file);
    }
}
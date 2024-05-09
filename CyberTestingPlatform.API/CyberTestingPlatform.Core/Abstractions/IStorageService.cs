using Microsoft.AspNetCore.Http;

namespace CyberTestingPlatform.Application.Services
{
    public interface IStorageService
    {
        Task<string> SaveFile(IFormFile file);
        void ValidationImageFile(IFormFile file);
    }
}
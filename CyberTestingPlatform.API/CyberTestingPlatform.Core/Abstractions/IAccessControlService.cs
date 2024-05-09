using System.Security.Claims;

namespace CyberTestingPlatform.Application.Services
{
    public interface IAccessControlService
    {
        bool CanAccessAccount(Guid accountId, ClaimsPrincipal user);
        Task<bool> HasAccessToCourseAsync(Guid userId, Guid courseId);
    }
}
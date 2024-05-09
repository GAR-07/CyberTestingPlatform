using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.DataAccess.Repositories
{
    public interface IAccessControlsRepository
    {
        Task<AccessControl?> CreateUserCourseAccessAsync(AccessControl accessControl);
        Task<AccessControl?> GetUserCourseAccessAsync(Guid userId, Guid courseId);
    }
}
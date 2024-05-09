using CyberTestingPlatform.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberTestingPlatform.DataAccess.Repositories
{
    public class AccessControlsRepository : IAccessControlsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AccessControlsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AccessControl?> GetUserCourseAccessAsync(Guid userId, Guid courseId)
        {
            var accessControlEntity = await _dbContext.AccessControl
                .FirstOrDefaultAsync(ac => ac.UserId == userId && ac.ObjectId == courseId);

            if (accessControlEntity == null)
            {
                return null;
            }

            var accessControl = new AccessControl(
                accessControlEntity.Id,
                accessControlEntity.UserId,
                accessControlEntity.ObjectId,
                accessControlEntity.HasAccess,
                accessControlEntity.AccessType,
                accessControlEntity.StartDate,
                accessControlEntity.EndDate);

            return accessControl;
        }

        public async Task<AccessControl?> CreateUserCourseAccessAsync(AccessControl accessControl)
        {
            var a = accessControl;
            return null;
        }
    }
}

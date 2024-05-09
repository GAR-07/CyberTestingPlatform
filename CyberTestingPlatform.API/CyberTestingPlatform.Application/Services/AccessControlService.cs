using CyberTestingPlatform.DataAccess.Repositories;
using System.Security.Claims;

namespace CyberTestingPlatform.Application.Services
{
    public class AccessControlService : IAccessControlService
    {
        private readonly IAccessControlsRepository _accessControlRepository;
        private readonly ICourseService _courseService;

        public AccessControlService(
            IAccessControlsRepository accessControlRepository,
            ICourseService courseService)
        {
            _accessControlRepository = accessControlRepository;
            _courseService = courseService;
        }

        public async Task<bool> HasAccessToCourseAsync(Guid userId, Guid courseId)
        {
            var accessControl = await _accessControlRepository.GetUserCourseAccessAsync(userId, courseId)
                ?? throw new Exception($"Курс {courseId} не найден");

            return accessControl.HasAccess;
        }

        public bool CanAccessAccount(Guid accountId, ClaimsPrincipal user)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (user.IsInRole("Admin"))
            {
                return true;
            }

            return userId != null && accountId == Guid.Parse(userId);
        }

        private async Task<bool> IsCoursePaidAsync(Guid courseId)
        {
            var course = await _courseService.GetCourseAsync(courseId);

            return course.Price != 0;
        }
    }
}

using CyberTestingPlatform.DataAccess.Repositories;
using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICoursesRepository _coursesRepository;

        public CourseService(ICoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }
        public async Task<Course> GetCourseAsync(Guid id)
        {
            return await _coursesRepository.GetAsync(id);
        }
        public async Task<List<Course>> GetSelectCoursesAsync(int sampleSize, int page)
        {
            if (sampleSize <= 0 || page < 0)
            {
                throw new Exception($"Заданы невозможные параметры для выборки");
            }
            return await _coursesRepository.GetSelectionAsync(sampleSize, page);
        }
        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await _coursesRepository.GetAllAsync();
        }
        public async Task<Guid> CreateCourseAsync(Course course)
        {
            return await _coursesRepository.CreateAsync(course);
        }
        public async Task<Guid> UpdateCourseAsync(Course course)
        {
            return await _coursesRepository.UpdateAsync(course);
        }
        public async Task<Guid> DeleteCourseAsync(Guid id)
        {
            return await _coursesRepository.DeleteAsync(id);
        }
    }
}

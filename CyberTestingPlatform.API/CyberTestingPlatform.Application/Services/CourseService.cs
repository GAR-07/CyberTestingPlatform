using CyberTestingPlatform.DataAccess.Repositories;
using CyberTestingPlatform.Core.Models;
using CyberTestingPlatform.Core.Shared;

namespace CyberTestingPlatform.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICoursesRepository _coursesRepository;

        public CourseService(ICoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await _coursesRepository.GetAllAsync() ?? throw new CustomHttpException($"Ничего не найдено", 422);
        }
        public async Task<List<Course>> GetSelectCoursesAsync(int sampleSize, int page)
        {
            if (sampleSize <= 0 || page < 0)
            {
                throw new CustomHttpException($"Заданы невозможные параметры для выборки", 422);
            }
            return await _coursesRepository.GetSelectionAsync(sampleSize, page) ?? throw new CustomHttpException($"Ничего не найдено", 422);
        }
        public async Task<Course> GetCourseAsync(Guid id)
        {
            return await _coursesRepository.GetAsync(id) ?? throw new CustomHttpException($"Курс {id} не найден", 422); ;
        }
        public async Task<Guid> CreateCourseAsync(Course course)
        {
            return await _coursesRepository.CreateAsync(course) ?? throw new CustomHttpException($"Ошибка создания курса", 422);
        }
        public async Task<Guid> UpdateCourseAsync(Course course)
        {
            return await _coursesRepository.UpdateAsync(course) ?? throw new CustomHttpException($"Курс {course.Id} не найден", 422);
        }
        public async Task<Guid> DeleteCourseAsync(Guid id)
        {
            return await _coursesRepository.DeleteAsync(id) ?? throw new CustomHttpException($"Курс {id} не найден", 422);
        }
    }
}

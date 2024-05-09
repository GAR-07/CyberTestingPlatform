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

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await _coursesRepository.GetAllAsync() ?? throw new Exception($"Ничего не найдено");
        }
        public async Task<List<Course>> GetSelectCoursesAsync(int sampleSize, int page)
        {
            if (sampleSize <= 0 || page < 0)
            {
                throw new Exception($"Заданы невозможные параметры для выборки");
            }
            return await _coursesRepository.GetSelectionAsync(sampleSize, page) ?? throw new Exception($"Ничего не найдено");
        }
        public async Task<Course> GetCourseAsync(Guid id)
        {
            return await _coursesRepository.GetAsync(id) ?? throw new Exception($"Курс {id} не найден"); ;
        }
        public async Task<Guid> CreateCourseAsync(Course course)
        {
            return await _coursesRepository.CreateAsync(course) ?? throw new Exception($"Ошибка создания курса");
        }
        public async Task<Guid> UpdateCourseAsync(Course course)
        {
            return await _coursesRepository.UpdateAsync(course) ?? throw new Exception($"Курс {course.Id} не найден");
        }
        public async Task<Guid> DeleteCourseAsync(Guid id)
        {
            return await _coursesRepository.DeleteAsync(id) ?? throw new Exception($"Курс {id} не найден");
        }
    }
}

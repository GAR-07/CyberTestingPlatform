using CyberTestingPlatform.DataAccess.Repositories;
using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.Application.Services
{
    public class StorageService : IStorageService
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly ILecturesRepository _lecturesRepository;

        public StorageService(
            ICoursesRepository coursesRepository,
            ILecturesRepository lecturesRepository)
        {
            _coursesRepository = coursesRepository;
            _lecturesRepository = lecturesRepository;
        }

        // Далее идут методы для курсов

        public async Task<List<Course>> GetAllCourses()
        {
            return await _coursesRepository.GetAll();
        }

        public async Task<List<Course>?> GetSelectCourses(int sampleSize, int page)
        {
            if (sampleSize > 0 && page > 0)
            {
                return await _coursesRepository.GetSelection(sampleSize, page);
            }

            return null;
        }

        public async Task<Course?> GetCourse(Guid id)
        {
            return await _coursesRepository.Get(id);
        }

        public async Task<Guid> CreateCourse(Course course)
        {
            return await _coursesRepository.Create(course);
        }

        public async Task<Guid> UpdateCourse(Guid id, string theme, string title, string description, string content, int price, string imagePath, DateTime lastUpdationDate)
        {
            return await _coursesRepository.Update(id, theme, title, description, content, price, imagePath, lastUpdationDate);
        }

        public async Task<Guid> DeleteCourse(Guid userId)
        {
            return await _coursesRepository.Delete(userId);
        }

        // Далее идут методы для лекций

        public async Task<List<Lecture>> GetAllLectures()
        {
            return await _lecturesRepository.GetAll();
        }

        public async Task<List<Lecture>?> GetSelectLectures(int sampleSize, int page)
        {
            if (sampleSize > 0 && page > 0)
            {
                return await _lecturesRepository.GetSelection(sampleSize, page);
            }

            return null;
        }

        public async Task<Lecture?> GetLecture(Guid id)
        {
            return await _lecturesRepository.Get(id);
        }

        public async Task<Guid> CreateLecture(Lecture lecture)
        {
            return await _lecturesRepository.Create(lecture);
        }

        public async Task<Guid> UpdateLecture(Guid id, string theme, string title, string content, DateTime lastUpdationDate, Guid courseId)
        {
            return await _lecturesRepository.Update(id, theme, title, content, lastUpdationDate, courseId);
        }

        public async Task<Guid> DeleteLecture(Guid userId)
        {
            return await _lecturesRepository.Delete(userId);
        }

        public DateTime ConvertToDateTime(string inputDate)
        {
            var dateSplit = inputDate.Split('-').Select(Int32.Parse).ToArray();
            return new DateTime(dateSplit[0], dateSplit[1], dateSplit[2]);
        }

        /// Далее идут неиспользующиеся методы

        //public async Task<string?> ValidateRegistration(string email, string role)
        //{
        //    if (role == "Admin" || role == "Moder")
        //        return "Incorrect roles";

        //    if (await IsEmailAlreadyExists(email))
        //        return "Email already exists";

        //    return null;
        //}
    }
}

using CyberTestingPlatform.DataAccess.Repositories;
using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.Application.Services
{
    public class LectureService : ILectureService
    {
        private readonly ILecturesRepository _lecturesRepository;

        public LectureService(ILecturesRepository lecturesRepository)
        {
            _lecturesRepository = lecturesRepository;
        }

        public async Task<List<Lecture>> GetSelectLecturesAsync(int sampleSize, int page)
        {
            if (sampleSize <= 0 || page < 0)
            {
                throw new Exception($"Заданы невозможные параметры для выборки");
            }
            return await _lecturesRepository.GetSelectionAsync(sampleSize, page) ?? throw new Exception($"Ничего не найдено");
        }
        public async Task<List<Lecture>> GetLecturesByCourseIdAsync(Guid id)
        {
            return await _lecturesRepository.GetByCourseIdAsync(id) ?? throw new Exception($"Ничего не найдено");
        }
        public async Task<Lecture> GetLectureAsync(Guid id)
        {
            return await _lecturesRepository.GetAsync(id) ?? throw new Exception($"Лекцкия {id} не найдена");
        }
        public async Task<Guid> CreateLectureAsync(Lecture lecture)
        {
            return await _lecturesRepository.CreateAsync(lecture) ?? throw new Exception($"Ошибка создания лекции");
        }
        public async Task<Guid> UpdateLectureAsync(Lecture lecture)
        {
            return await _lecturesRepository.UpdateAsync(lecture) ?? throw new Exception($"Лекция {lecture.Id} не найдена");
        }
        public async Task<Guid> DeleteLectureAsync(Guid id)
        {
            return await _lecturesRepository.DeleteAsync(id) ?? throw new Exception($"Лекция {id} не найдена");
        }
    }
}

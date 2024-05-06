using CyberTestingPlatform.DataAccess.Repositories;
using CyberTestingPlatform.Core.Models;
using CyberTestingPlatform.Core.Shared;

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
                throw new CustomHttpException($"Заданы невозможные параметры для выборки", 422);
            }
            return await _lecturesRepository.GetSelectionAsync(sampleSize, page) ?? throw new CustomHttpException($"Ничего не найдено", 422);
        }
        public async Task<List<Lecture>> GetLecturesByCourseIdAsync(Guid id)
        {
            return await _lecturesRepository.GetByCourseIdAsync(id) ?? throw new CustomHttpException($"Ничего не найдено", 422);
        }
        public async Task<Lecture> GetLectureAsync(Guid id)
        {
            return await _lecturesRepository.GetAsync(id) ?? throw new CustomHttpException($"Лекцкия {id} не найдена", 422);
        }
        public async Task<Guid> CreateLectureAsync(Lecture lecture)
        {
            return await _lecturesRepository.CreateAsync(lecture) ?? throw new CustomHttpException($"Ошибка создания лекции", 422);
        }
        public async Task<Guid> UpdateLectureAsync(Lecture lecture)
        {
            return await _lecturesRepository.UpdateAsync(lecture) ?? throw new CustomHttpException($"Лекция {lecture.Id} не найдена", 422);
        }
        public async Task<Guid> DeleteLectureAsync(Guid id)
        {
            return await _lecturesRepository.DeleteAsync(id) ?? throw new CustomHttpException($"Лекция {id} не найдена", 422);
        }
    }
}

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
            return await _lecturesRepository.GetSelectionAsync(sampleSize, page);
        }
        public async Task<List<Lecture>> GetLecturesByCourseIdAsync(Guid id)
        {
            return await _lecturesRepository.GetByCourseIdAsync(id);
        }
        public async Task<Lecture> GetLectureAsync(Guid id)
        {
            return await _lecturesRepository.GetAsync(id);
        }
        public async Task<Guid> CreateLectureAsync(Lecture lecture)
        {
            return await _lecturesRepository.CreateAsync(lecture);
        }
        public async Task<Guid> UpdateLectureAsync(Lecture lecture)
        {
            return await _lecturesRepository.UpdateAsync(lecture);
        }
        public async Task<Guid> DeleteLectureAsync(Guid id)
        {
            return await _lecturesRepository.DeleteAsync(id);
        }
    }
}

using CyberTestingPlatform.Core.Models;
using CyberTestingPlatform.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CyberTestingPlatform.DataAccess.Repositories
{
    public class LecturesRepository : ILecturesRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public LecturesRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Lecture>> GetSelectionAsync(int sampleSize, int page)
        {
            var totalCount = await _dbContext.Lectures.AsNoTracking().CountAsync();
            var startIndex = Math.Max(0, totalCount - sampleSize * page);
            var countToTake = Math.Min(sampleSize, totalCount - startIndex);

            var lectureEntities = await _dbContext.Lectures
                .Skip(startIndex)
                .Take(countToTake)
                .AsNoTracking()
                .ToListAsync()
                ?? throw new Exception($"Результатов не найдено");

            var lectures = lectureEntities
               .Select(x => new Lecture(x.Id, x.Theme, x.Title, x.Content, x.Position, x.CreatorID, x.CreationDate, x.LastUpdationDate, x.CourseId))
               .ToList();

            return lectures;
        }

        public async Task<List<Lecture>> GetByCourseIdAsync(Guid id)
        {
            var lectureEntities = await _dbContext.Lectures
                .Where(lecture => lecture.CourseId == id)
                .AsNoTracking()
                .ToListAsync()
                ?? throw new Exception($"Результатов не найдено");

            var lectures = lectureEntities
               .Select(x => new Lecture(x.Id, x.Theme, x.Title, x.Content, x.Position, x.CreatorID, x.CreationDate, x.LastUpdationDate, x.CourseId))
               .ToList();

            return lectures;
        }

        public async Task<Lecture> GetAsync(Guid id)
        {
            var lectureEntity = await _dbContext.Lectures.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id)
                 ?? throw new Exception($"Лекция {id} не найдена");

            return new Lecture(
                lectureEntity.Id,
                lectureEntity.Theme,
                lectureEntity.Title,
                lectureEntity.Content,
                lectureEntity.Position,
                lectureEntity.CreatorID,
                lectureEntity.CreationDate,
                lectureEntity.LastUpdationDate,
                lectureEntity.CourseId);
        }

        public async Task<Guid> CreateAsync(Lecture lecture)
        {
            var lectureEntity = new LectureEntity
            {
                Id = lecture.Id,
                Theme = lecture.Theme,
                Title = lecture.Title,
                Content = lecture.Content,
                Position = lecture.Position,
                CreatorID = lecture.CreatorID,
                CreationDate = lecture.CreationDate,
                LastUpdationDate = lecture.LastUpdationDate,
                CourseId = lecture.CourseId
            };

            await _dbContext.Lectures.AddAsync(lectureEntity);
            await _dbContext.SaveChangesAsync();

            return lectureEntity.Id;
        }

        public async Task<Guid> UpdateAsync(Lecture lecture)
        {
            var lectureEntity = await _dbContext.Lectures
                .Where(p => p.Id == lecture.Id)
                .FirstOrDefaultAsync()
                ?? throw new Exception($"Лекция {lecture.Id} не найдена");

            lectureEntity.Theme = lecture.Theme;
            lectureEntity.Title = lecture.Title;
            lectureEntity.Content = lecture.Content;
            lectureEntity.Position = lecture.Position;
            lectureEntity.LastUpdationDate = lecture.LastUpdationDate;
            lectureEntity.CourseId = lecture.CourseId;

            await _dbContext.SaveChangesAsync();

            return lectureEntity.Id;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            var lectureEntity = await _dbContext.Lectures
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync()
                ?? throw new Exception($"Лекция {id} не найдена");

            _dbContext.Remove(lectureEntity);
            await _dbContext.SaveChangesAsync();

            return lectureEntity.Id;
        }
    }
}

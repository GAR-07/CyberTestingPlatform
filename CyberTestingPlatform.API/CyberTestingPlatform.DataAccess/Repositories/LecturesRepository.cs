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

        public async Task<List<Lecture>> GetAll()
        {
            var lectureEntities = await _dbContext.Lectures
                .AsNoTracking()
                .ToListAsync();

            var lectures = lectureEntities
                .Select(x => Lecture.Create(x.Id, x.Theme, x.Title, x.Content, x.CreatorID, x.CreationDate, x.LastUpdationDate, x.CourseId).lecture)
                .ToList();

            return lectures;
        }

        public async Task<List<Lecture>> GetSelection(int sampleSize, int page)
        {
            var totalCount = await _dbContext.Lectures.CountAsync();
            var startIndex = Math.Max(0, totalCount - sampleSize * page);
            var countToTake = Math.Min(sampleSize, totalCount - startIndex);

            var lectureEntities = await _dbContext.Lectures
                .Skip(startIndex)
                .Take(countToTake)
                .AsNoTracking()
                .ToListAsync();

            var lectures = lectureEntities
               .Select(x => Lecture.Create(x.Id, x.Theme, x.Title, x.Content, x.CreatorID, x.CreationDate, x.LastUpdationDate, x.CourseId).lecture)
               .ToList();

            return lectures;
        }

        public async Task<Lecture?> Get(Guid id)
        {
            var lectureEntity = await _dbContext.Lectures.FirstOrDefaultAsync(p => p.Id == id);
            return lectureEntity != null ? Lecture.Create(
                lectureEntity.Id,
                lectureEntity.Theme,
                lectureEntity.Title,
                lectureEntity.Content,
                lectureEntity.CreatorID,
                lectureEntity.CreationDate,
                lectureEntity.LastUpdationDate,
                lectureEntity.CourseId).lecture
                : null;
        }

        public async Task<Guid> Create(Lecture lecture)
        {
            var lectureEntity = new LectureEntity
            {
                Id = lecture.Id,
                Theme = lecture.Theme,
                Title = lecture.Title,
                Content = lecture.Content,
                CreatorID = lecture.CreatorID,
                CreationDate = lecture.CreationDate,
                LastUpdationDate = lecture.LastUpdationDate,
                CourseId = lecture.CourseId
            };

            await _dbContext.Lectures.AddAsync(lectureEntity);
            await _dbContext.SaveChangesAsync();

            return lectureEntity.Id;
        }

        public async Task<Guid> Update(Guid id, string theme, string title, string content, DateTime lastUpdationDate, Guid courseId)
        {
            var lectureEntity = await _dbContext.Lectures
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (lectureEntity != null)
            {
                lectureEntity.Theme = theme;
                lectureEntity.Title = title;
                lectureEntity.Content = content;
                lectureEntity.LastUpdationDate = lastUpdationDate;
                lectureEntity.CourseId = courseId;

                await _dbContext.SaveChangesAsync();

                return id;
            }

            return Guid.Empty;
        }

        public async Task<Guid> Delete(Guid id)
        {
            var lectureEntity = await _dbContext.Lectures
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (lectureEntity != null)
            {
                _dbContext.Remove(lectureEntity);
                await _dbContext.SaveChangesAsync();

                return id;
            }

            return Guid.Empty;
        }
    }
}

using CyberTestingPlatform.Core.Models;
using CyberTestingPlatform.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CyberTestingPlatform.DataAccess.Repositories
{
    public class CoursesRepository : ICoursesRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CoursesRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Course>> GetAll()
        {
            var courseEntities = await _dbContext.Courses
                .AsNoTracking()
                .ToListAsync();

            var courses = courseEntities
                .Select(x => Course.Create(x.Id, x.Name, x.Description, x.Price, x.ImagePath, x.CreatorID, x.CreationDate, x.LastUpdationDate).course)
                .ToList();

            return courses;
        }

        public async Task<List<Course>> GetSelection(int sampleSize, int page)
        {
            var totalCount = await _dbContext.Courses.CountAsync();
            var startIndex = Math.Max(0, totalCount - sampleSize * page);
            var countToTake = Math.Min(sampleSize, totalCount - startIndex);

            var courseEntities = await _dbContext.Courses
                .Skip(startIndex)
                .Take(countToTake)
                .AsNoTracking()
                .ToListAsync();

            var courses = courseEntities
               .Select(x => Course.Create(x.Id, x.Name, x.Description, x.Price, x.ImagePath, x.CreatorID, x.CreationDate, x.LastUpdationDate).course)
               .ToList();

            return courses;
        }

        public async Task<Course?> Get(Guid id)
        {
            var courseEntity = await _dbContext.Courses.FirstOrDefaultAsync(p => p.Id == id);
            return courseEntity != null ? Course.Create(
                courseEntity.Id,
                courseEntity.Name,
                courseEntity.Description,
                courseEntity.Price,
                courseEntity.ImagePath,
                courseEntity.CreatorID,
                courseEntity.CreationDate,
                courseEntity.LastUpdationDate).course
                : null;
        }

        public async Task<Guid> Create(Course lecture)
        {
            var courseEntity = new CourseEntity
            {
                Id = lecture.Id,
                Name = lecture.Name,
                Description = lecture.Description,
                Price = lecture.Price,
                ImagePath = lecture.ImagePath,
                CreatorID = lecture.CreatorID,
                CreationDate = lecture.CreationDate,
                LastUpdationDate = lecture.LastUpdationDate,
            };

            await _dbContext.Courses.AddAsync(courseEntity);
            await _dbContext.SaveChangesAsync();

            return courseEntity.Id;
        }

        public async Task<Guid> Update(Guid id, string name, string description, int price, string imagePath, DateTime lastUpdationDate)
        {
            var courseEntity = await _dbContext.Courses
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (courseEntity != null)
            {
                courseEntity.Name = name;
                courseEntity.Description = description;
                courseEntity.Price = price;
                courseEntity.ImagePath = imagePath;
                courseEntity.LastUpdationDate = lastUpdationDate;

                await _dbContext.SaveChangesAsync();

                return id;
            }

            return Guid.Empty;
        }

        public async Task<Guid> Delete(Guid id)
        {
            var courseEntity = await _dbContext.Courses
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (courseEntity != null)
            {
                _dbContext.Remove(courseEntity);
                await _dbContext.SaveChangesAsync();

                return id;
            }

            return Guid.Empty;
        }
    }
}

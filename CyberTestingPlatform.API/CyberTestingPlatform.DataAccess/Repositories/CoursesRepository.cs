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

        public async Task<List<Course>> GetAllAsync()
        {
            var courseEntities = await _dbContext.Courses
                .AsNoTracking()
                .ToListAsync()
                ?? throw new Exception($"Результатов не найдено");

            var courses = courseEntities
                .Select(x => new Course(x.Id, x.Name, x.Description, x.Price, x.ImagePath, x.CreatorID, x.CreationDate, x.LastUpdationDate))
                .ToList();

            return courses;
        }

        public async Task<List<Course>> GetSelectionAsync(int sampleSize, int page)
        {
            var totalCount = await _dbContext.Courses.AsNoTracking().CountAsync();
            var startIndex = Math.Max(0, totalCount - sampleSize * page);
            var countToTake = Math.Min(sampleSize, totalCount - startIndex);

            var courseEntities = await _dbContext.Courses
                .Skip(startIndex)
                .Take(countToTake)
                .AsNoTracking()
                .ToListAsync()
                ?? throw new Exception($"Результатов не найдено");

            var courses = courseEntities
               .Select(x => new Course(x.Id, x.Name, x.Description, x.Price, x.ImagePath, x.CreatorID, x.CreationDate, x.LastUpdationDate))
               .ToList();

            return courses;
        }

        public async Task<Course> GetAsync(Guid id)
        {
            var courseEntity = await _dbContext.Courses.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new Exception($"Курс {id} не найден");

            return new Course(
                courseEntity.Id,
                courseEntity.Name,
                courseEntity.Description,
                courseEntity.Price,
                courseEntity.ImagePath,
                courseEntity.CreatorID,
                courseEntity.CreationDate,
                courseEntity.LastUpdationDate);
        }

        public async Task<Guid> CreateAsync(Course course)
        {
            var courseEntity = new CourseEntity
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Price = course.Price,
                ImagePath = course.ImagePath,
                CreatorID = course.CreatorID,
                CreationDate = course.CreationDate,
                LastUpdationDate = course.LastUpdationDate,
            };

            await _dbContext.Courses.AddAsync(courseEntity);
            await _dbContext.SaveChangesAsync();

            return courseEntity.Id;
        }

        public async Task<Guid> UpdateAsync(Course course)
        {
            var courseEntity = await _dbContext.Courses
                .Where(p => p.Id == course.Id)
                .FirstOrDefaultAsync() 
                ?? throw new Exception($"Курс {course.Id} не найден");

            courseEntity.Name = course.Name;
            courseEntity.Description = course.Description;
            courseEntity.Price = course.Price;
            courseEntity.ImagePath = course.ImagePath;
            courseEntity.LastUpdationDate = course.LastUpdationDate;

            await _dbContext.SaveChangesAsync();

            return courseEntity.Id;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            var courseEntity = await _dbContext.Courses
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync()
                ?? throw new Exception($"Курс {id} не найден");

            _dbContext.Remove(courseEntity);
            await _dbContext.SaveChangesAsync();

            return courseEntity.Id;
        }
    }
}

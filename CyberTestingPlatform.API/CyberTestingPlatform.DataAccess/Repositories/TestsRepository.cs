using CyberTestingPlatform.Core.Models;
using CyberTestingPlatform.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CyberTestingPlatform.DataAccess.Repositories
{
    public class TestsRepository : ITestsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TestsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Test>?> GetSelectionAsync(int sampleSize, int page)
        {
            var totalCount = await _dbContext.Tests.AsNoTracking().CountAsync();
            var startIndex = Math.Max(0, totalCount - sampleSize * page);
            var countToTake = Math.Min(sampleSize, totalCount - startIndex);

            var testEntities = await _dbContext.Tests
                .Skip(startIndex)
                .Take(countToTake)
                .AsNoTracking()
                .ToListAsync();

            if (testEntities == null)
            {
                return null;
            }

            var tests = testEntities
               .Select(x => new Test(x.Id, x.Theme, x.Title, x.Questions, x.AnswerOptions, x.CorrectAnswers, x.Position, x.CreatorID, x.CreationDate, x.LastUpdationDate, x.CourseId))
               .ToList();

            return tests;
        }

        public async Task<List<Test>?> GetByCourseIdAsync(Guid id)
        {
            var testEntities = await _dbContext.Tests
                .Where(test => test.CourseId == id)
                .AsNoTracking()
                .ToListAsync();

            if (testEntities == null)
            {
                return null;
            }

            var tests = testEntities
               .Select(x => new Test(x.Id, x.Theme, x.Title, x.Questions, x.AnswerOptions, x.CorrectAnswers, x.Position, x.CreatorID, x.CreationDate, x.LastUpdationDate, x.CourseId))
               .ToList();

            return tests;
        }

        public async Task<Test?> GetAsync(Guid id)
        {
            var testEntity = await _dbContext.Tests.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            if (testEntity == null)
            {
                return null;
            }

            return new Test(
                testEntity.Id,
                testEntity.Theme,
                testEntity.Title,
                testEntity.Questions,
                testEntity.AnswerOptions,
                testEntity.CorrectAnswers,
                testEntity.Position,
                testEntity.CreatorID,
                testEntity.CreationDate,
                testEntity.LastUpdationDate,
                testEntity.CourseId);
        }

        public async Task<Guid?> CreateAsync(Test test)
        {
            var testEntity = new TestEntity
            {
                Id = test.Id,
                Theme = test.Theme,
                Title = test.Title,
                Questions = test.Questions,
                AnswerOptions = test.AnswerOptions,
                CorrectAnswers = test.CorrectAnswers,
                Position = test.Position,
                CreatorID = test.CreatorID,
                CreationDate = test.CreationDate,
                LastUpdationDate = test.LastUpdationDate,
                CourseId = test.CourseId
            };

            await _dbContext.Tests.AddAsync(testEntity);
            await _dbContext.SaveChangesAsync();

            return testEntity.Id;
        }

        public async Task<Guid?> UpdateAsync(Test test)
        {
            var testEntity = await _dbContext.Tests
                .Where(p => p.Id == test.Id)
                .FirstOrDefaultAsync();

            if (testEntity == null)
            {
                return null;
            }

            testEntity.Theme = test.Theme;
            testEntity.Title = test.Title;
            testEntity.Questions = test.Questions;
            testEntity.AnswerOptions = test.AnswerOptions;
            testEntity.CorrectAnswers = test.CorrectAnswers;
            testEntity.Position = test.Position;
            testEntity.LastUpdationDate = test.LastUpdationDate;
            testEntity.CourseId = test.CourseId;

            await _dbContext.SaveChangesAsync();

            return testEntity.Id;
        }

        public async Task<Guid?> DeleteAsync(Guid id)
        {
            var testEntity = await _dbContext.Tests
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (testEntity == null)
            {
                return null;
            }

            _dbContext.Remove(testEntity);
            await _dbContext.SaveChangesAsync();

            return id;
        }
    }
}

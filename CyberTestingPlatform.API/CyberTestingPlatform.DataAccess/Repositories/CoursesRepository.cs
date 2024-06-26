﻿using CyberTestingPlatform.Core.Models;
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

        public async Task<List<Course>?> GetAllAsync()
        {
            var courseEntities = await _dbContext.Courses
                .AsNoTracking()
                .ToListAsync();

            if (courseEntities == null)
            {
                return null;
            }

            var courses = courseEntities
                .Select(x => new Course(x.Id, x.Name, x.Description, x.Price, x.ImagePath, x.CreatorID, x.CreationDate, x.LastUpdationDate))
                .ToList();

            return courses;
        }

        public async Task<List<Course>?> GetSelectionAsync(string? searchText, int page, int pageSize)
        {
            var query = _dbContext.Courses.AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(x => x.Name.Contains(searchText));
            }

            var totalCount = await query.AsNoTracking().CountAsync();
            var startIndex = Math.Max(0, totalCount - pageSize * page);
            var countToTake = Math.Min(pageSize, totalCount - startIndex);

            var courseEntities = await query
                .Skip(startIndex)
                .Take(countToTake)
                .AsNoTracking()
                .ToListAsync();

            if (courseEntities == null)
            {
                return null;
            }

            var courses = courseEntities
               .Select(x => new Course(x.Id, x.Name, x.Description, x.Price, x.ImagePath, x.CreatorID, x.CreationDate, x.LastUpdationDate))
               .ToList();

            return courses;
        }

        public async Task<Course?> GetAsync(Guid id)
        {
            var courseEntity = await _dbContext.Courses.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            if (courseEntity == null)
            {
                return null;
            }

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

        public async Task<Guid?> CreateAsync(Course course)
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

        public async Task<Guid?> UpdateAsync(Course course)
        {
            var courseEntity = await _dbContext.Courses
                .Where(p => p.Id == course.Id)
                .FirstOrDefaultAsync();

            if (courseEntity == null)
            {
                return null;
            }

            courseEntity.Name = course.Name;
            courseEntity.Description = course.Description;
            courseEntity.Price = course.Price;
            courseEntity.ImagePath = course.ImagePath;
            courseEntity.LastUpdationDate = course.LastUpdationDate;

            await _dbContext.SaveChangesAsync();

            return courseEntity.Id;
        }

        public async Task<Guid?> DeleteAsync(Guid id)
        {
            var courseEntity = await _dbContext.Courses
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (courseEntity == null)
            {
                return null;
            }

            _dbContext.Remove(courseEntity);
            await _dbContext.SaveChangesAsync();

            return courseEntity.Id;
        }
    }
}

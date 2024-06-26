﻿using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.Application.Services
{
    public interface ICourseService
    {
        Task<Guid> CreateCourseAsync(Course course);
        Task<Guid> DeleteCourseAsync(Guid userId);
        Task<Course> GetCourseAsync(Guid id);
        Task<List<Course>> GetSelectCoursesAsync(string? searchText, int page, int pageSize);
        Task<List<Course>> GetAllCoursesAsync();
        Task<Guid> UpdateCourseAsync(Course course);
    }
}
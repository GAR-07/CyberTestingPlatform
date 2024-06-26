﻿using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.DataAccess.Repositories
{
    public interface ILecturesRepository
    {
        Task<Guid?> CreateAsync(Lecture lecture);
        Task<Guid?> DeleteAsync(Guid id);
        Task<Lecture?> GetAsync(Guid id);
        Task<List<Lecture>?> GetByCourseIdAsync(Guid id);
        Task<List<Lecture>?> GetSelectionAsync(string? searchText, int page, int pageSize);
        Task<Guid?> UpdateAsync(Lecture lecture);
    }
}
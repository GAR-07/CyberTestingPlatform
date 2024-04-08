using CyberTestingPlatform.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace CyberTestingPlatform.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }
        public DbSet<AccountEntity> Accounts { get; set; }
        public DbSet<AccessControlEntity> AccessControl { get; set; }
        public DbSet<CourseEntity> Courses { get; set; }
        public DbSet<LectureEntity> Lectures { get; set; }
        public DbSet<TestEntity> Tests { get; set; }
    }
}
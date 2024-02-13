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
        public DbSet<CourseEntity> Courses { get; set; }
        public DbSet<LectureEntity> Lectures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseEntity>()
                .HasMany(c => c.Lectures)
                .WithOne()
                .HasForeignKey("CourseId");
        }
    }
}
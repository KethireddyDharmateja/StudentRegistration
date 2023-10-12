using Microsoft.EntityFrameworkCore;
using StudentRegistration.Models;

namespace StudentRegistration.Data
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Students> Students { get; set; }
        public DbSet<Course> Courses { get; set; } 
        public DbSet<StudentCourse> StudentCourses { get; set; }
    }
}

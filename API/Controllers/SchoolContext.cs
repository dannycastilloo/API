using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Grade> Enrollments { get; set; }
        public DbSet<Grade> Courses { get; set; }
        public DbSet<API.Models.Course>? Course { get; set; }
        public DbSet<API.Models.Enrollment>? Enrollment { get; set; }

    }
}
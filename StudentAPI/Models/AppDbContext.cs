using Microsoft.EntityFrameworkCore;
namespace StudentAPI.Models
{
    
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<CompletedTopic> CompletedTopics { get; set; }
        public DbSet<StudentSubject> StudentSubjects { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subject>().ToTable("subjects");
            modelBuilder.Entity<Topic>().ToTable("topics");
            modelBuilder.Entity<Student>().ToTable("students");
            modelBuilder.Entity<CompletedTopic>().ToTable("completedtopics");
            modelBuilder.Entity<StudentSubject>().ToTable("student_subjects");
            base.OnModelCreating(modelBuilder);
        }

    }
}


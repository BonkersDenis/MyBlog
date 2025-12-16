using Microsoft.EntityFrameworkCore;
using MyBlog.Models;

namespace MyBlog.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка статьи
            modelBuilder.Entity<Article>(entity =>
            {
                entity.Property(a => a.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(a => a.Content)
                    .IsRequired();

                entity.Property(a => a.Excerpt)
                    .HasMaxLength(500);

                entity.Property(a => a.PublishDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("GETDATE()");
            });
        }
    }
}
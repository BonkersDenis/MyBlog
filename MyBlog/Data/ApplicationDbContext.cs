using Microsoft.EntityFrameworkCore;
using MyBlog.Models.Entities;

namespace MyBlog.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация для Article
            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Content)
                    .IsRequired();

                entity.Property(e => e.Excerpt)
                    .HasMaxLength(500);

                entity.Property(e => e.PublishDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.IsPublished)
                    .HasDefaultValue(true);

                entity.HasIndex(e => e.PublishDate);
                entity.HasIndex(e => e.IsPublished);
            });

            // Конфигурация для Comment
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.AuthorName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.IsApproved)
                    .HasDefaultValue(false);

                // Связь с Article
                entity.HasOne(e => e.Article)
                    .WithMany()
                    .HasForeignKey(e => e.ArticleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.ArticleId);
                entity.HasIndex(e => e.IsApproved);
                entity.HasIndex(e => e.CreatedDate);
            });
        }
    }
}
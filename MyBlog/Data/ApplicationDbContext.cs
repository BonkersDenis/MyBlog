using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBlog.Models.Entities;

namespace MyBlog.Data
{
    //TODO: Разобраться как работает IEntityTypeConfiguration

    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

            builder.Property(e => e.Content)
                    .IsRequired();

            builder.Property(e => e.Excerpt)
                    .HasMaxLength(500);

            builder.Property(e => e.PublishDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("GETDATE()");

            builder.Property(e => e.IsPublished)
                    .HasDefaultValue(true);

            builder.HasIndex(e => e.PublishDate);
            builder.HasIndex(e => e.IsPublished);
        }
    }

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация для Article
           // modelBuilder.ApplyConfiguration<ArticleConfiguration>();

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
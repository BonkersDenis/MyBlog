using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Models;

namespace MyBlog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Добавляем контекст базы данных
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            // Создаем базу данных при запуске (если не существует)
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.EnsureCreated();

                // Если таблица Articles пуста, добавляем тестовые данные
                if (!dbContext.Articles.Any())
                {
                    dbContext.Articles.AddRange(
                        new Article
                        {
                            Title = "Добро пожаловать в блог!",
                            Content = "Это мой первый пост в блоге. Здесь я буду делиться своими мыслями и идеями.",
                            Excerpt = "Приветственное сообщение в блоге",
                            PublishDate = DateTime.Now.AddDays(-2)
                        },
                        new Article
                        {
                            Title = "О планах на будущее",
                            Content = "В этом блоге я планирую писать о технологиях, программировании и других интересных темах.",
                            Excerpt = "Рассказ о планах развития блога",
                            PublishDate = DateTime.Now.AddDays(-1)
                        }
                    );
                    dbContext.SaveChanges();
                }
            }

            app.MapControllerRoute(
                name: "article",
                pattern: "article/{id}",
                defaults: new { controller = "Home", action = "Article" });

            app.MapControllerRoute(
                name: "admin",
                pattern: "admin/{action=Index}/{id?}",
                defaults: new { controller = "Admin" });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Interfaces.Services;
using MyBlog.Services;
using MyBlog.Models.Entities;
using Microsoft.Extensions.DependencyInjection;

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

            // Регистрируем сервисы через интерфейсы
            builder.Services.AddScoped<IArticleService, ArticleService>();

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

                // Применяем миграции автоматически
                dbContext.Database.Migrate();

                // Если таблица Articles пуста, добавляем тестовые данные
                if (!dbContext.Articles.Any())
                {
                    // Используем доменные модели для создания тестовых статей
                    var articles = new List<Article>
                    {
                        // Используем конструктор доменной модели
                        new Article(
                            title: "Добро пожаловать в блог!",
                            content: "Это мой первый пост в блоге. Здесь я буду делиться своими мыслями и идеями.",
                            excerpt: "Приветственное сообщение в блоге"
                        ),
                        new Article(
                            title: "О планах на будущее",
                            content: "В этом блоге я планирую писать о технологиях, программировании и других интересных темах.",
                            excerpt: "Рассказ о планах развития блога"
                        ),
                        new Article(
                            title: "Основы ASP.NET Core",
                            content: "ASP.NET Core - это кроссплатформенный фреймворк для создания веб-приложений.",
                            excerpt: "Введение в ASP.NET Core для начинающих"
                        )
                    };

                    // Добавляем статьи в контекст
                    dbContext.Articles.AddRange(articles);

                    // Сохраняем изменения
                    dbContext.SaveChanges();

                    // Логируем создание тестовых данных
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogInformation("Добавлены тестовые статьи в базу данных");
                }
            }

            // Настраиваем маршруты
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
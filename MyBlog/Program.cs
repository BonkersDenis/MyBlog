namespace MyBlog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(); // Важно!
            app.UseRouting();
            app.UseAuthorization();

            // Добавляем маршруты
            app.MapControllerRoute(
                name: "article",
                pattern: "article/{id}",
                defaults: new { controller = "Home", action = "Article" });

            app.MapControllerRoute(
                name: "admin",
                pattern: "admin/{action=Index}/{id?}",
                defaults: new { controller = "Admin" });

            // Дефолтный маршрут (должен быть последним!)
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
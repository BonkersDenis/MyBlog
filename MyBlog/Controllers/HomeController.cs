using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Models;
using System.Diagnostics;

namespace MyBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        /// <summary>
        /// Метод для отображения главной страницы (списка статей)
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var articles = await _context.Articles
                .OrderByDescending(a => a.PublishDate)
                .ToListAsync();

            return View(articles);
        }
        /// <summary>
        ///  Метод для отображения конкретной статьи
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Article(int id)
        {
            var article = await _context.Articles.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }
        /// <summary>
        /// Метод для отображения страницы конфиденциальности
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            return View();
        }
        /// <summary>
        /// Метод для обработки ошибок
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyBlog.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    // Статический список статей для хранения данных в памяти
    // В реальном проекте заменили бы на базу данных
    private static List<Article> _articles = new List<Article>
    {
        new Article
        {
            Id = 1,
            Title = "Добро пожаловать в блог!",
            Content = "Это мой первый пост в блоге. Здесь я буду делиться своими мыслями и идеями.",
            PublishDate = DateTime.Now.AddDays(-2),
            Excerpt = "Приветственное сообщение в блоге"
        },
        new Article
        {
            Id = 2,
            Title = "О планах на будущее",
            Content = "В этом блоге я планирую писать о технологиях, программировании и других интересных темах.",
            PublishDate = DateTime.Now.AddDays(-1),
            Excerpt = "Рассказ о планах развития блога"
        }
    };

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // Главная страница - список всех статей
    public IActionResult Index()
    {
        // Сортируем статьи по дате публикации (новые сверху)
        var articles = _articles
            .OrderByDescending(a => a.PublishDate)
            .ToList();

        // Передаем список статей в представление
        return View(articles);
    }

    // Страница конкретной статьи
    public IActionResult Article(int id)
    {
        // Находим статью по ID
        var article = _articles.FirstOrDefault(a => a.Id == id);

        // Если статья не найдена - возвращаем 404
        if (article == null)
        {
            return NotFound();
        }

        return View(article);
    }

    // Метод для доступа к статьям из AdminController
    public static List<Article> GetArticles()
    {
        return _articles;
    }

    // Метод для обновления списка статей из AdminController
    public static void SetArticles(List<Article> articles)
    {
        _articles = articles;
    }

   

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
using Microsoft.AspNetCore.Mvc;
using MyBlog.Models;

namespace MyBlog.Controllers;

//TODO: Сделать две ветки. Одну - для фронта, вторую - для бэка. По результату смерджить их в main.

//TODO: Поправить стиль кода(переименовать метод).
//TODO: Добавить во все эндпоинты тип Http-запроса.
//TODO: Используй Summary стиль документации.
//TODO: Внедрить ef-core.
//TODO: Создать отдельные "доменные" модели.
//TODO: Нужно выделить репозиторий в HomeController. ArticleRepository будет заниматься получением/записью в бд через ef core.
//TODO: Нужно выделить сервисы. ArticleService будет заниматься бизнес логикой(созданием и редактированием статей)
//внедрить сервис в контроллеры.

//TODO: Добавить на форму результат валидации.
//TODO: Сделать в формах нормальный modelBinding(использовать @model вместо биндинга по name).
//TODO: В формах использовать asp-for вместо не прямого указания эндпоинтов.

public class AdminController : Controller
{
    // Используем тот же список статей, что и в HomeController
    private List<Article> _articles;

    public AdminController()
    {
        // Получаем ссылку на общий список статей из HomeController
        _articles = HomeController.GetArticles();
    }

    // Главная страница админ-панели - список всех статей
    public IActionResult Index()
    {
        var articles = _articles
            .OrderByDescending(a => a.PublishDate)
            .ToList();

        return View(articles);
    }

    // GET: Форма для создания новой статьи
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Создание новой статьи
    [HttpPost]
    public IActionResult Create(Article article)
    {
        if (ModelState.IsValid)
        {
            // Генерируем новый ID
            int newId = _articles.Count > 0 ? _articles.Max(a => a.Id) + 1 : 1;
            article.Id = newId;

            // Устанавливаем дату публикации
            article.PublishDate = System.DateTime.Now;

            // Добавляем статью в список
            _articles.Add(article);

            // Обновляем список в HomeController
            HomeController.SetArticles(_articles);

            // Перенаправляем на список статей
            return RedirectToAction("Index");
        }

        // Если есть ошибки валидации, показываем форму снова
        return View(article);
    }

    // GET: Форма для редактирования статьи
    [HttpGet]
    public IActionResult Edit(int id)
    {
        // Ищем статью по ID
        var article = _articles.FirstOrDefault(a => a.Id == id);

        if (article == null)
        {
            return NotFound();
        }

        return View(article);
    }

    // POST: Обновление статьи
    [HttpPost]
    public IActionResult Edit(Article updatedArticle)
    {
        if (ModelState.IsValid)
        {
            // Ищем существующую статью
            var article = _articles.FirstOrDefault(a => a.Id == updatedArticle.Id);

            if (article == null)
            {
                return NotFound();
            }

            // Обновляем поля статьи
            article.Title = updatedArticle.Title;
            article.Content = updatedArticle.Content;
            article.Excerpt = updatedArticle.Excerpt;

            // Обновляем список в HomeController
            HomeController.SetArticles(_articles);

            return RedirectToAction("Index");
        }

        return View(updatedArticle);
    }

    // GET: Подтверждение удаления
    public IActionResult Delete(int id)
    {
        var article = _articles.FirstOrDefault(a => a.Id == id);

        if (article == null)
        {
            return NotFound();
        }

        return View(article);
    }

    // POST: Удаление статьи
    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        var article = _articles.FirstOrDefault(a => a.Id == id);

        if (article != null)
        {
            // Удаляем статью
            _articles.Remove(article);

            // Обновляем список в HomeController
            HomeController.SetArticles(_articles);
        }

        return RedirectToAction("Index");
    }
}

using Microsoft.AspNetCore.Mvc;
using MyBlog.Models;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Interfaces.Services;
using MyBlog.Models.DTOs;

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
    private readonly IArticleService _articleService;
    private readonly ILogger<AdminController> _logger;

    public AdminController(IArticleService articleService, ILogger<AdminController> logger)
    {
        _articleService = articleService ?? throw new ArgumentNullException(nameof(articleService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Администратор зашел в панель управления");

        try
        {
            var articles = await _articleService.GetAllArticlesAsync();
            _logger.LogDebug($"Получено {articles.Count()} статей для отображения");

            return View(articles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении списка статей");
            return View("Error", new { message = "Не удалось загрузить список статей" });
        }
    }

    public IActionResult Create()
    {
        _logger.LogInformation("Отображение формы создания статьи");
        return View(new CreateArticleDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateArticleDto createDto)
    {
        _logger.LogInformation($"Попытка создания статьи: {createDto.Title}");

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Невалидные данные при создании статьи");
            return View(createDto);
        }

        try
        {
            var createdArticle = await _articleService.CreateArticleAsync(createDto);

            _logger.LogInformation($"Статья создана успешно. ID: {createdArticle.Id}");

            TempData["SuccessMessage"] = $"Статья '{createdArticle.Title}' успешно создана!";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при создании статьи");

            ModelState.AddModelError(string.Empty, "Произошла ошибка при создании статьи. Пожалуйста, попробуйте еще раз.");

            return View(createDto);
        }
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            _logger.LogWarning("Попытка редактирования статьи без указания ID");
            return NotFound();
        }

        _logger.LogInformation($"Запрос на редактирование статьи с ID: {id}");

        try
        {
            var article = await _articleService.GetArticleByIdAsync(id.Value);

            if (article == null)
            {
                _logger.LogWarning($"Статья с ID {id} не найдена для редактирования");
                return NotFound();
            }

            // Создаем UpdateArticleDto без PublishDate, если оно не нужно
            var updateDto = new UpdateArticleDto
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                Excerpt = article.Excerpt,
                IsPublished = article.IsPublished
                // PublishDate убрали, если не хотим редактировать
            };

            // Если нужно показывать дату, но не редактировать
            ViewBag.OriginalPublishDate = article.PublishDate.ToString("dd.MM.yyyy HH:mm");

            return View(updateDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка при получении статьи с ID {id} для редактирования");
            return View("Error", new { message = "Не удалось загрузить статью для редактирования" });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateArticleDto updateDto)
    {
        _logger.LogInformation($"Попытка обновления статьи с ID: {id}");

        if (id != updateDto.Id)
        {
            _logger.LogWarning($"Несоответствие ID: маршрут={id}, DTO={updateDto.Id}");
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Невалидные данные при обновлении статьи");
            return View(updateDto);
        }

        try
        {
            await _articleService.UpdateArticleAsync(updateDto);

            _logger.LogInformation($"Статья с ID {id} успешно обновлена");

            TempData["SuccessMessage"] = $"Статья '{updateDto.Title}' успешно обновлена!";

            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, $"Статья с ID {id} не найдена для обновления");
            ModelState.AddModelError(string.Empty, "Статья не найдена. Возможно, она была удалена.");
            return View(updateDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка при обновлении статьи с ID {id}");

            ModelState.AddModelError(string.Empty, "Произошла ошибка при обновлении статьи. Пожалуйста, попробуйте еще раз.");

            return View(updateDto);
        }
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            _logger.LogWarning("Попытка удаления статьи без указания ID");
            return NotFound();
        }

        _logger.LogInformation($"Запрос на удаление статьи с ID: {id}");

        try
        {
            var article = await _articleService.GetArticleByIdAsync(id.Value);

            if (article == null)
            {
                _logger.LogWarning($"Статья с ID {id} не найдена для удаления");
                return NotFound();
            }

            return View(article);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка при получении статьи с ID {id} для удаления");
            return View("Error", new { message = "Не удалось загрузить статью для удаления" });
        }
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        _logger.LogInformation($"Подтверждение удаления статьи с ID: {id}");

        try
        {
            await _articleService.DeleteArticleAsync(id);

            _logger.LogInformation($"Статья с ID {id} успешно удалена");

            TempData["SuccessMessage"] = "Статья успешно удалена!";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка при удалении статьи с ID {id}");

            TempData["ErrorMessage"] = "Произошла ошибка при удалении статьи. Пожалуйста, попробуйте еще раз.";

            return RedirectToAction(nameof(Index));
        }
    }
}

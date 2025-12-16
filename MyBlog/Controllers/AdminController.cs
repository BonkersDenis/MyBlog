using Microsoft.AspNetCore.Mvc;
using MyBlog.Models;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;

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
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Admin
    /// <summary>
    /// Основная страница административной панели - отображает список всех статей
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> Index()
    {
        var articles = await _context.Articles
            .OrderByDescending(a => a.PublishDate)
            .ToListAsync();

        return View(articles);
    }

    // GET: Admin/Create
    /// <summary>
    /// Метод для отображения формы создания новой статьи
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        return View();
    }

    // POST: Admin/Create
    /// <summary>
    /// Метод для обработки данных из формы создания статьи
    /// </summary>
    /// <param name="article"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Article article)
    {
        if (ModelState.IsValid)
        {
            _context.Add(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(article);
    }

    // GET: Admin/Edit/5
    /// <summary>
    /// Метод для отображения формы редактирования существующей статьи
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var article = await _context.Articles.FindAsync(id);

        if (article == null)
        {
            return NotFound();
        }

        return View(article);
    }

    // POST: Admin/Edit/5
    /// <summary>
    /// Метод для обработки данных из формы редактирования статьи
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updatedArticle"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Article updatedArticle)
    {
        if (id != updatedArticle.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(updatedArticle);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(updatedArticle.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        return View(updatedArticle);
    }

    // GET: Admin/Delete/5
    /// <summary>
    /// Метод для отображения страницы подтверждения удаления статьи
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var article = await _context.Articles
            .FirstOrDefaultAsync(m => m.Id == id);

        if (article == null)
        {
            return NotFound();
        }

        return View(article);
    }

    // POST: Admin/Delete/5
    /// <summary>
    /// Метод для обработки подтверждения удаления статьи
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var article = await _context.Articles.FindAsync(id);

        if (article != null)
        {
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
    /// <summary>
    /// Вспомогательный метод для проверки существования статьи по ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private bool ArticleExists(int id)
    {
        return _context.Articles.Any(e => e.Id == id);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using MyBlog.Models.DTOs;

namespace MyBlog.Interfaces.Services
{
    /// <summary>
    /// Интерфейс для сервиса работы со статьями
    /// Определяет контракт, который должен реализовать ArticleService
    /// </summary>
    public interface IArticleService
    {
        /// <summary>
        /// Получить статью по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор статьи</param>
        /// <returns>DTO статьи или null, если статья не найдена</returns>
        Task<ArticleDto> GetArticleByIdAsync(int id);

        /// <summary>
        /// Получить все опубликованные статьи
        /// </summary>
        /// <returns>Коллекция DTO опубликованных статей</returns>
        Task<IEnumerable<ArticleDto>> GetPublishedArticlesAsync();

        /// <summary>
        /// Получить все статьи (включая неопубликованные)
        /// </summary>
        /// <returns>Коллекция DTO всех статей</returns>
        Task<IEnumerable<ArticleDto>> GetAllArticlesAsync();

        /// <summary>
        /// Получить статьи с пагинацией
        /// </summary>
        /// <param name="pageNumber">Номер страницы (начиная с 1)</param>
        /// <param name="pageSize">Количество статей на странице</param>
        /// <returns>Коллекция DTO статей для указанной страницы</returns>
        Task<IEnumerable<ArticleDto>> GetArticlesPagedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Создать новую статью
        /// </summary>
        /// <param name="createDto">DTO с данными для создания статьи</param>
        /// <returns>DTO созданной статьи</returns>
        Task<ArticleDto> CreateArticleAsync(CreateArticleDto createDto);

        /// <summary>
        /// Обновить существующую статью
        /// </summary>
        /// <param name="updateDto">DTO с данными для обновления</param>
        /// <exception cref="ArgumentException">Выбрасывается, если статья не найдена</exception>
        Task UpdateArticleAsync(UpdateArticleDto updateDto);

        /// <summary>
        /// Удалить статью по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор статьи</param>
        Task DeleteArticleAsync(int id);

        /// <summary>
        /// Проверить существование статьи
        /// </summary>
        /// <param name="id">Идентификатор статьи</param>
        /// <returns>True, если статья существует</returns>
        Task<bool> ArticleExistsAsync(int id);

        /// <summary>
        /// Получить количество опубликованных статей
        /// </summary>
        /// <returns>Количество статей</returns>
        Task<int> GetPublishedArticlesCountAsync();

        /// <summary>
        /// Получить количество всех статей
        /// </summary>
        /// <returns>Количество статей</returns>
        Task<int> GetAllArticlesCountAsync();

        /// <summary>
        /// Поиск статей по заголовку или содержанию
        /// </summary>
        /// <param name="searchQuery">Поисковый запрос</param>
        /// <returns>Коллекция DTO найденных статей</returns>
        Task<IEnumerable<ArticleDto>> SearchArticlesAsync(string searchQuery);
    }
}
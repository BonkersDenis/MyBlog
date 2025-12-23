using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyBlog.Data;
using MyBlog.Interfaces.Services;
using MyBlog.Models.Entities;
using MyBlog.Models.DTOs;

namespace MyBlog.Services
{
    /// <summary>
    /// Реализация сервиса для работы со статьями
    /// Наследует интерфейс IArticleService
    /// </summary>
    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ArticleService> _logger;

        /// <summary>
        /// Конструктор сервиса
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        /// <param name="logger">Логгер</param>
        public ArticleService(ApplicationDbContext context, ILogger<ArticleService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ArticleDto> GetArticleByIdAsync(int id)
        {
            _logger.LogInformation($"Получение статьи по ID: {id}");

            var article = await _context.Articles.FindAsync(id);

            if (article == null)
            {
                _logger.LogWarning($"Статья с ID {id} не найдена");
                return null;
            }

            _logger.LogDebug($"Статья найдена: {article.Title}");
            return MapToDto(article);
        }

        public async Task<IEnumerable<ArticleDto>> GetPublishedArticlesAsync()
        {
            _logger.LogInformation("Получение опубликованных статей");

            var articles = await _context.Articles
                .Where(a => a.IsPublished)
                .OrderByDescending(a => a.PublishDate)
                .ToListAsync();

            _logger.LogDebug($"Найдено {articles.Count} опубликованных статей");
            return articles.Select(MapToDto);
        }

        public async Task<IEnumerable<ArticleDto>> GetAllArticlesAsync()
        {
            _logger.LogInformation("Получение всех статей");

            var articles = await _context.Articles
                .OrderByDescending(a => a.PublishDate)
                .ToListAsync();

            _logger.LogDebug($"Найдено {articles.Count} статей");
            return articles.Select(MapToDto);
        }

        public async Task<IEnumerable<ArticleDto>> GetArticlesPagedAsync(int pageNumber, int pageSize)
        {
            _logger.LogInformation($"Получение статей для страницы {pageNumber} (размер: {pageSize})");

            if (pageNumber < 1 || pageSize < 1)
            {
                throw new ArgumentException("Номер страницы и размер страницы должны быть положительными числами");
            }

            var articles = await _context.Articles
                .Where(a => a.IsPublished)
                .OrderByDescending(a => a.PublishDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            _logger.LogDebug($"Найдено {articles.Count} статей для страницы {pageNumber}");
            return articles.Select(MapToDto);
        }

        public async Task<ArticleDto> CreateArticleAsync(CreateArticleDto createDto)
        {
            _logger.LogInformation($"Создание новой статьи: {createDto.Title}");

            if (createDto == null)
                throw new ArgumentNullException(nameof(createDto));

            try
            {
                var article = new Article(createDto.Title, createDto.Content, createDto.Excerpt);

                _context.Articles.Add(article);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Статья создана успешно. ID: {article.Id}");
                return MapToDto(article);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании статьи");
                throw;
            }
        }

        public async Task UpdateArticleAsync(UpdateArticleDto updateDto)
        {
            _logger.LogInformation($"Обновление статьи ID: {updateDto.Id}");

            if (updateDto == null)
                throw new ArgumentNullException(nameof(updateDto));

            var article = await _context.Articles.FindAsync(updateDto.Id);

            if (article == null)
            {
                _logger.LogWarning($"Статья с ID {updateDto.Id} не найдена");
                throw new ArgumentException($"Статья с ID {updateDto.Id} не найдена");
            }

            try
            {
                article.Update(updateDto.Title, updateDto.Content, updateDto.Excerpt);

                if (updateDto.IsPublished && !article.IsPublished)
                    article.Publish();
                else if (!updateDto.IsPublished && article.IsPublished)
                    article.Unpublish();

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Статья ID {updateDto.Id} успешно обновлена");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обновлении статьи ID: {updateDto.Id}");
                throw;
            }
        }

        public async Task DeleteArticleAsync(int id)
        {
            _logger.LogInformation($"Удаление статьи ID: {id}");

            var article = await _context.Articles.FindAsync(id);

            if (article == null)
            {
                _logger.LogWarning($"Статья с ID {id} не найдена для удаления");
                return;
            }

            try
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Статья ID {id} успешно удалена");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении статьи ID: {id}");
                throw;
            }
        }

        public async Task<bool> ArticleExistsAsync(int id)
        {
            return await _context.Articles.AnyAsync(e => e.Id == id);
        }

        public async Task<int> GetPublishedArticlesCountAsync()
        {
            return await _context.Articles.CountAsync(a => a.IsPublished);
        }

        public async Task<int> GetAllArticlesCountAsync()
        {
            return await _context.Articles.CountAsync();
        }

        public async Task<IEnumerable<ArticleDto>> SearchArticlesAsync(string searchQuery)
        {
            _logger.LogInformation($"Поиск статей по запросу: {searchQuery}");

            if (string.IsNullOrWhiteSpace(searchQuery))
                return await GetPublishedArticlesAsync();

            var normalizedQuery = searchQuery.ToLower();

            var articles = await _context.Articles
                .Where(a => a.IsPublished &&
                    (a.Title.ToLower().Contains(normalizedQuery) ||
                     a.Content.ToLower().Contains(normalizedQuery) ||
                     a.Excerpt.ToLower().Contains(normalizedQuery)))
                .OrderByDescending(a => a.PublishDate)
                .ToListAsync();

            _logger.LogDebug($"По запросу '{searchQuery}' найдено {articles.Count} статей");
            return articles.Select(MapToDto);
        }

        /// <summary>
        /// Преобразование доменной модели в DTO
        /// </summary>
        private ArticleDto MapToDto(Article article)
        {
            return new ArticleDto
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                Excerpt = article.Excerpt,
                PublishDate = article.PublishDate,
                IsPublished = article.IsPublished
            };
        }

        /// <summary>
        /// Преобразование DTO в доменную модель (для внутреннего использования)
        /// </summary>
        private Article MapToEntity(ArticleDto dto)
        {
            if (dto.Id > 0)
            {
                // Для существующих статей
                var article = new Article(dto.Title, dto.Content, dto.Excerpt);
                // Здесь нужен рефлексия или другой способ установки ID
                return article;
            }
            else
            {
                // Для новых статей
                return new Article(dto.Title, dto.Content, dto.Excerpt);
            }
        }
    }
}
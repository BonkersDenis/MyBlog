using System.Collections.Generic;
using MyBlog.Models.DTOs;

namespace MyBlog.Models.ViewModels
{
    /// <summary>
    /// ViewModel для страницы со списком статей
    /// Может содержать дополнительные данные, специфичные для представления
    /// </summary>
    public class ArticleListViewModel
    {
        public IEnumerable<ArticleDto> Articles { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }

        // Дополнительные данные для представления
        public string SearchQuery { get; set; }
        public string Category { get; set; }
        public string SortBy { get; set; }
    }
}
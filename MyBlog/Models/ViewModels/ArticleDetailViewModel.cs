using MyBlog.Models.DTOs;

namespace MyBlog.Models.ViewModels
{
    public class ArticleDetailViewModel
    {
        public ArticleDto Article { get; set; }

        // Дополнительные данные для страницы статьи
        public string AuthorName { get; set; }
        public int ReadingTime { get; set; } // время чтения в минутах
        public string[] Tags { get; set; }
        public ArticleDto PreviousArticle { get; set; }
        public ArticleDto NextArticle { get; set; }

        // Для SEO
        public string MetaDescription { get; set; }
        public string[] MetaKeywords { get; set; }
    }
}
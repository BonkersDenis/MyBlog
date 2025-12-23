using System;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models.DTOs
{
    /// <summary>
    /// DTO для передачи данных о статье между слоями приложения
    /// Не содержит бизнес-логики, только данные
    /// </summary>
    public class ArticleDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Заголовок обязателен")]
        [StringLength(200, ErrorMessage = "Заголовок не должен превышать 200 символов")]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Содержание обязательно")]
        [Display(Name = "Содержание")]
        public string Content { get; set; }

        [Display(Name = "Дата публикации")]
        [DataType(DataType.DateTime)]
        public DateTime PublishDate { get; set; }

        [StringLength(500, ErrorMessage = "Краткое описание не должно превышать 500 символов")]
        [Display(Name = "Краткое описание")]
        public string Excerpt { get; set; }

        [Display(Name = "Опубликовано")]
        public bool IsPublished { get; set; }

        // Вспомогательные свойства (только для чтения)
        public string FormattedDate => PublishDate.ToString("dd.MM.yyyy HH:mm");
        public string ShortContent => Content.Length > 150 ? Content.Substring(0, 150) + "..." : Content;
    }

    /// <summary>
    /// DTO для создания новой статьи
    /// </summary>
    public class CreateArticleDto
    {
        [Required(ErrorMessage = "Заголовок обязателен")]
        [StringLength(200, ErrorMessage = "Заголовок не должен превышать 200 символов")]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Содержание обязательно")]
        [Display(Name = "Содержание")]
        public string Content { get; set; }

        [StringLength(500, ErrorMessage = "Краткое описание не должно превышать 500 символов")]
        [Display(Name = "Краткое описание")]
        public string Excerpt { get; set; }
    }

    /// <summary>
    /// DTO для обновления статьи
    /// </summary>
    public class UpdateArticleDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Заголовок обязателен")]
        [StringLength(200, ErrorMessage = "Заголовок не должен превышать 200 символов")]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Содержание обязательно")]
        [Display(Name = "Содержание")]
        public string Content { get; set; }

        [StringLength(500, ErrorMessage = "Краткое описание не должно превышать 500 символов")]
        [Display(Name = "Краткое описание")]
        public string Excerpt { get; set; }

        [Display(Name = "Опубликовано")]
        public bool IsPublished { get; set; }
    }
}
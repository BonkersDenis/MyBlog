using System.ComponentModel.DataAnnotations;

namespace MyBlog.Requests
{
    /// <summary>
    /// DTO для обновления статьи
    /// </summary>
    public class UpdateArticleRequest
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
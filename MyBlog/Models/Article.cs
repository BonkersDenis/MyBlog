using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models
{
    public class Article
    {
        public int Id { get; set; }   // Свойство Id - уникальный идентификатор статьи

        [Required]// [Required] - атрибут валидации из пространства имен System.ComponentModel.DataAnnotations
        public string Title { get; set; }// Свойство Title - заголовок статьи

        [Required]// [Required] - атрибут валидации из пространства имен System.ComponentModel.DataAnnotations
        public string Content { get; set; } // Свойство Content - основное содержание статьи

        public DateTime PublishDate { get; set; }  // Свойство PublishDate - дата и время публикации статьи

        public string Excerpt { get; set; } // Свойство Excerpt - краткое описание или анонс статьи
    }
}

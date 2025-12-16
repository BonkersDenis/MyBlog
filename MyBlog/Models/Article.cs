using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBlog.Models
{
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // Свойство Id - уникальный идентификатор статьи

        [Required(ErrorMessage = "Заголовок обязателен")]
        [StringLength(200, ErrorMessage = "Заголовок не должен превышать 200 символов")]// [Required] - атрибут валидации из пространства имен System.ComponentModel.DataAnnotations
        public string Title { get; set; }// Свойство Title - заголовок статьи


        [Required(ErrorMessage = "Содержание обязательно")]
        public string Content { get; set; }// Свойство Content - основное содержание статьи

        [Display(Name = "Дата публикации")]
        [DataType(DataType.DateTime)]
        public DateTime PublishDate { get; set; } // Свойство PublishDate - дата и время публикации статьи

        [StringLength(500, ErrorMessage = "Краткое описание не должно превышать 500 символов")]
        public string Excerpt { get; set; }// Свойство Excerpt - краткое описание или анонс статьи

        // Конструктор для установки даты по умолчанию
        public Article()
        {
            PublishDate = DateTime.Now;
        }
    }
}

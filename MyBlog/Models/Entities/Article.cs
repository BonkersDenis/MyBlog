using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBlog.Models.Entities
{
    /// <summary>
    /// Доменная модель статьи - основная бизнес-сущность
    /// Отвечает за бизнес-логику и правила домена
    /// </summary>
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [Required]
        [StringLength(200)]
        public string Title { get; private set; }

        [Required]
        public string Content { get; private set; }

        public DateTime PublishDate { get; private set; }

        [StringLength(500)]
        public string Excerpt { get; private set; }

        public bool IsPublished { get; private set; }

        // Закрытый конструктор для EF Core
        private Article()
        {
            // Требуется для Entity Framework
        }

        // Основной конструктор доменной модели
        public Article(string title, string content, string excerpt = null)
        {
            SetTitle(title);
            SetContent(content);
            SetExcerpt(excerpt);
            PublishDate = DateTime.Now;
            IsPublished = true;
        }

        // Методы для изменения состояния (бизнес-логика)
        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Заголовок не может быть пустым");

            if (title.Length > 200)
                throw new ArgumentException("Заголовок не должен превышать 200 символов");

            Title = title;
        }

        public void SetContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Содержание не может быть пустым");

            Content = content;
        }

        public void SetExcerpt(string excerpt)
        {
            if (excerpt?.Length > 500)
                throw new ArgumentException("Краткое описание не должно превышать 500 символов");

            Excerpt = excerpt;
        }

        public void Update(string title, string content, string excerpt)
        {
            SetTitle(title);
            SetContent(content);
            SetExcerpt(excerpt);
        }

        public void Unpublish()
        {
            IsPublished = false;
        }

        public void Publish()
        {
            IsPublished = true;
            PublishDate = DateTime.Now;
        }

        // Бизнес-методы
        public string GetFirstParagraph()
        {
            if (string.IsNullOrEmpty(Content))
                return string.Empty;

            var firstParagraph = Content.Split(new[] { "\n\n", "\r\n\r\n" }, StringSplitOptions.None)
                .FirstOrDefault();

            return firstParagraph ?? Content.Substring(0, Math.Min(Content.Length, 200));
        }

        public int GetReadingTime()
        {
            // Приблизительное время чтения (200 слов в минуту)
            var wordCount = Content.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
            return (int)Math.Ceiling(wordCount / 200.0);
        }
    }
}
using System;

namespace MyBlog.Models.Entities
{
    /// <summary>
    /// Добавить summary
    /// </summary>
    public class Comment
    {
        public int Id { get; private set; }

        public int ArticleId { get; private set; }
        public Article Article { get; private set; }

        public string AuthorName { get; private set; }

        public string Content { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public bool IsApproved { get; private set; }

        private Comment() { }

        public Comment(int articleId, string authorName, string content)
        {
            ArticleId = articleId;
            SetAuthorName(authorName);
            SetContent(content);
            CreatedDate = DateTime.Now;
            IsApproved = false;
        }

        public void SetAuthorName(string authorName)
        {
            if (string.IsNullOrWhiteSpace(authorName))
                throw new ArgumentException("Имя автора не может быть пустым");

            AuthorName = authorName.Trim();
        }

        public void SetContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Комментарий не может быть пустым");

            Content = content.Trim();
        }

        public void Approve()
        {
            IsApproved = true;
        }

        public void Reject()
        {
            IsApproved = false;
        }
    }
}
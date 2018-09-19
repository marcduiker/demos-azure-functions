using System;

namespace Demo.AzureFunctions.Models
{
    public class Comment
    {
        public Guid Id { get; set; }

        public string AuthorName { get; set; }

        public string Text { get; set; }

        public Guid ParentId { get; set; }
    }
}

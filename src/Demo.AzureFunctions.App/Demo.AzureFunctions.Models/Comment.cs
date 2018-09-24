using System;

namespace Demo.AzureFunctions.Models
{
    public class Comment
    {
        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        public string AuthorName { get; set; }

        public string Text { get; set; }
    }
}

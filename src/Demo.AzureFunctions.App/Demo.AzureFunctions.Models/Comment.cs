using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Demo.AzureFunctions.Models
{
    public class Comment : TableEntity
    {
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        public string AuthorName { get; set; }

        public string Text { get; set; }
    }
}

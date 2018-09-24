using System;

namespace Demo.AzureFunctions.Models
{
    public class EmptyComment : Comment
    {
        public EmptyComment()
        {
            PartitionKey = string.Empty;
            RowKey = string.Empty;
            Id = Guid.Empty;
            ParentId = Guid.Empty;
            AuthorName = string.Empty;
            Text = string.Empty;
        }
    }
}

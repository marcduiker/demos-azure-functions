using System;

namespace Demo.AzureFunctions.App.Configuration
{
    public class TableStorage
    {
        public const string TableName = "Blog";

        public const string CommentsPartitionKey = "Comments";

        public const string Connection = "TableStorageConnection";
    }
}

namespace Demo.AzureFunctions.App.Configuration
{
    public class UserContent
    {
        public class TableStorage
        {
            public const string CommentsTable = "Comments";
        }

        public class QueueStorage
        {
            public const string CommentModerationQueue = "comment-moderation";
            public const string ModeratedCommentsQueue = "moderated-comments";
        }

        public const string Connection = "UserStorageConnection";
    }
}

using System;
using System.IO;
using System.Threading.Tasks;
using Demo.AzureFunctions.App.Configuration;
using Demo.AzureFunctions.Models;
using Demo.AzureFunctions.Models.Factories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Demo.AzureFunctions.App.Functions
{
    public static class AddComment
    {
        [FunctionName(nameof(AddComment))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequest req,
            [Table(TableStorage.TableName, TableStorage.CommentsPartitionKey, Connection = TableStorage.Connection)]ICollector<Comment> tableBinding,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var comment = CreateComment(requestBody, log);
            if (comment.GetType() == typeof(EmptyComment))
            {
                return new BadRequestObjectResult(MessageFactory.CreateInvalidCommentMessage());
            }

            tableBinding.Add(comment);

            return new OkObjectResult(comment);
        }

        public static Comment CreateComment(string jsonComment, ILogger log)
        {
            Comment result = CommentFactory.CreateEmptyComment();

            var comment = JsonConvert.DeserializeObject<Comment>(jsonComment);
            if (!IsValid(comment))
            {
                return result;
            }

            var commentId = Guid.NewGuid();

            result = new Comment
            {
                PartitionKey = TableStorage.CommentsPartitionKey,
                RowKey = commentId.ToString("D"),
                Id = commentId,
                AuthorName = comment.AuthorName,
                ParentId = comment.ParentId,
                Text = comment.Text
            };

            return result;
        }

        private static bool IsValid(Comment comment)
        {
            return !string.IsNullOrEmpty(comment.AuthorName) &&
                   !string.IsNullOrEmpty(comment.Text) &&
                   comment.ParentId != Guid.Empty;
        }

    }
}

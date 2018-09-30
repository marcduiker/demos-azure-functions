using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.AzureFunctions.App.Configuration;
using Demo.AzureFunctions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace Demo.AzureFunctions.App.Functions
{
    public static class GetComments
    {
        [FunctionName(nameof(GetComments))]
        [StorageAccount(UserContent.Connection)]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Comments/{parentId}")]HttpRequest req,
            string parentId,
            [Table(UserContent.TableStorage.CommentsTable)] CloudTable commentsTable,
            ILogger log)
        {
            IActionResult result;
            if (Guid.TryParse(parentId, out Guid parsedParentId))
            {
                var comments = await GetCommentsByParentId(commentsTable, parsedParentId);
                result = new OkObjectResult(comments);
            }
            else
            {
                result = new BadRequestObjectResult("Please provide a valid parentId in the route.");
            }

            return result;
        }

        private static async Task<IEnumerable<Comment>> GetCommentsByParentId(CloudTable cloudTable, Guid parentId)
        {

            var query = new TableQuery<Comment>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal,
                    parentId.ToString("D")));

            var result = new List<Comment>();

            foreach (var comment in await cloudTable.ExecuteQuerySegmentedAsync(query, null))
            {
                result.Add(comment);
            }

            return result;
        }
    }
}

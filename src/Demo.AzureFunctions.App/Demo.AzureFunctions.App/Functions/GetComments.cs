using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.AzureFunctions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Demo.AzureFunctions.App.Functions
{
    public static class GetComments
    {
        [FunctionName(nameof(GetComments))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetComments/{parentId}")]
            HttpRequest req,
            string parentId,
            ILogger log)
        {
            IActionResult result;
            if (Guid.TryParse(parentId, out Guid parsedParentId))
            {
                var comments = GetCommentsByParentId(parsedParentId);
                result = new OkObjectResult(comments);
            }
            else
            {
                result = new BadRequestObjectResult("Please provide a valid parentId in the route.");
            }

            return result;
        }

        private static IEnumerable<Comment> GetCommentsByParentId(Guid parentId)
        {
            return new List<Comment>
            {
                new Comment
                {
                    AuthorName = "Marc",
                    Id = Guid.NewGuid(),
                    ParentId = Guid.NewGuid(),
                    Text = "Your blogpost changed my life!"
                }
            };
        }
    }
}

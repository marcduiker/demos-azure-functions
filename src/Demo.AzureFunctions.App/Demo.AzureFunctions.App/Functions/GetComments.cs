using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Demo.AzureFunctions.Models;

namespace Demo.AzureFunctions.App
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
            if (!string.IsNullOrEmpty(parentId))
            {
                var comments = await GetCommentsByParentId(parentId);
                result = new OkObjectResult(comments);
            }
            else
            {
                result = new BadRequestObjectResult("Please provide a parentId in the route.");
            }

            return result;
        }

        private static Task<IEnumerable<Comment>> GetCommentsByParentId(string parentId)
        {
            throw new NotImplementedException();
        }
    }
}

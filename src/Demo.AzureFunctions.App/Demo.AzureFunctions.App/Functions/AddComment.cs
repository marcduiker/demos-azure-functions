using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Demo.AzureFunctions.Models;

namespace Demo.AzureFunctions.App
{
    public static class AddComment
    {
        [FunctionName(nameof(AddComment))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            HttpRequest req,
            ILogger log)
        {

            IActionResult result;
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            try
            {
                var comment = JsonConvert.DeserializeObject<Comment>(requestBody);
                // Store in Table Storage
                result = new OkObjectResult(comment);
            }
            catch (Exception e)
            {
                log.LogError($"Failed to deserialize body to comment.", e);
                result = new BadRequestObjectResult("Please pass in a valid comment in the request body.");
            }

            return result;
        }
    }
}

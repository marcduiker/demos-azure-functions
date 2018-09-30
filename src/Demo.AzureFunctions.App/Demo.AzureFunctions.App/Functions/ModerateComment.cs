using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Demo.AzureFunctions.App.Configuration;
using Demo.AzureFunctions.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Demo.AzureFunctions.App.Functions
{
    public static class ModerateComment
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private const double ScoreThreshold = 0.6;

        [FunctionName(nameof(ModerateComment))]
        [StorageAccount(UserContent.Connection)]
        public static async Task Run(
            [QueueTrigger(UserContent.QueueStorage.CommentModerationQueue)]string queueItem,
            [Queue(UserContent.QueueStorage.ModeratedCommentsQueue)]ICollector<Comment> moderatedQueue,
            ILogger log)
        {
            var comment = JsonConvert.DeserializeObject<Comment>(queueItem);
            if (await IsCommentSafe(comment.Text))
            {
                moderatedQueue.Add(comment);
            }
            else
            {
                log.LogWarning($"Comment did not pass moderation: {comment.Text}");
            }
        }

        private static async Task<bool> IsCommentSafe(string text)
        {
            var apiKey = Environment.GetEnvironmentVariable("ContentModeratorApiKey");
            var endPoint = Environment.GetEnvironmentVariable("ContentModeratorEndPoint");
            HttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{ apiKey }");
            var serializedContent = new StringContent(text, Encoding.UTF8, "text/plain");

            var response = await HttpClient.PostAsync(endPoint, serializedContent);
            var stringResponse = response.Content.ReadAsStringAsync();

            dynamic jsonResult = JsonConvert.DeserializeObject(stringResponse.Result);
           
            // For more info on the categories:
            // https://docs.microsoft.com/en-us/azure/cognitive-services/content-moderator/text-moderation-api#classification-preview 
            var scores = new List<double>
            {
                jsonResult.Classification.Category1.Score.Value,
                jsonResult.Classification.Category2.Score.Value,
                jsonResult.Classification.Category3.Score.Value
            };

            var isSafe = scores.Max() < ScoreThreshold;

            return isSafe;
        }
    }
}

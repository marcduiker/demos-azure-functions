using System.IO;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctionDemos
{
    public static class UploadFile
    {
        [FunctionName("UploadFile")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "upload/{name}")]HttpRequestMessage req, 
            [Blob("images/{name}", FileAccess.Read, Connection = "BlobConnectionString")]out Stream blobStream,
            string name, 
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
            var bytes = req.Content.ReadAsByteArrayAsync().Result;
            blobStream = new MemoryStream(bytes);

            // Fetching the name from the path parameter in the request URL
            return req.CreateResponse(HttpStatusCode.OK, name);
        }
    }
}

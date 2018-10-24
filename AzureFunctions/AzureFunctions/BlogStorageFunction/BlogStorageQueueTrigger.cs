using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace BlogStorageFunction
{
    public static class BlogStorageQueueTrigger
    {
        [FunctionName("BlogStorageQueueTrigger")]
        public static void Run([QueueTrigger("cosmosdb-queue", Connection = "DungStorageAccount")]string myQueueItem,
            [Blob("dung-blogstorage/{queueTrigger}", FileAccess.Read, Connection = "DungStorageAccount")] Stream myBlob,
            [Blob("dung-blogstorage/COPY-{queueTrigger}", FileAccess.Write, Connection = "DungStorageAccount")] Stream imageSmall,
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            myBlob.Position = 0;
            myBlob.CopyTo(imageSmall);
        }
    }
}

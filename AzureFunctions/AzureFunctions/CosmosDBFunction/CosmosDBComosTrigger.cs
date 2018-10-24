using System;
using System.Collections.Generic;
using CosmosDBFunction.Model;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CosmosDBFunction
{
    public static class CosmosDBComosTrigger
    {
        [FunctionName("CosmosDBFunctionInOut")]
        public static void Run(
            [CosmosDBTrigger(
            databaseName: "DungComosDB",
            collectionName: "Collection1",
            ConnectionStringSetting = "CosmosDBConnectionString",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input,
            [CosmosDB(
            databaseName: "DungComosDB",
            collectionName: "Collection2",
            ConnectionStringSetting = "CosmosDBConnectionString")]out dynamic document,
            ILogger log)
        {
            var test2 = new ToDoItem();
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);

                var test = JObject.Parse(input[0].ToString());

                var test3 = test.ToObject<ToDoItem>();

                test2 = JsonConvert.DeserializeObject<ToDoItem>(input[0].ToString());

            }
            document = new { id = Guid.NewGuid() , Name = test2.Name, Title = test2.Title };
        }
    }
}

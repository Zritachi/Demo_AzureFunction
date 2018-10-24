using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CosmosDBFunction.Model;

namespace CosmosDBFunction
{
    public static class CosmosDBHttpTrigger
    {
        [FunctionName("CosmosDBHttpTrigger")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "DungComosDB",
                collectionName: "Collection1",
                ConnectionStringSetting = "CosmosDBConnectionString",
                Id = "{Query.id}")] ToDoItem toDoItem,
             [CosmosDB(
            databaseName: "DungComosDB",
                collectionName: "Collection2",
                ConnectionStringSetting = "CosmosDBConnectionString")]out dynamic document,
                ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (toDoItem == null)
            {
                log.LogInformation($"ToDo item not found");
            }
            else
            {
                log.LogInformation($"Found ToDo item, Description={toDoItem.Name}");
            }

            var newId = (int.Parse(toDoItem.id));

            newId++;

            document = new { id = newId.ToString(), Name = toDoItem.Name, Title = toDoItem.Title };
            //document = new { id = ++toDoItem.id, Name = toDoItem.Name, Title = toDoItem.Title };

            return new OkResult();
        }
    }
}

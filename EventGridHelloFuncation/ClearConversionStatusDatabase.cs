using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ConversionTracking.Manager;

namespace EventGridHelloFuncation
{
    public static class ClearConversionStatusDatabase
    {
        [FunctionName("ClearConversionStatusDatabase")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Clearing the conversion database");
            var flowManager = new ConversionFlowManager(log);
            await flowManager.CleanAndCreateDatabase();

            
            return new OkObjectResult("Database is clear");
        }
    }
}

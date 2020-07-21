using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace EventGridHelloFuncation
{
    public static class ConversionTrackerCreatedHandler
    {
        [FunctionName("ConversionTrackerCreatedHandler")]
        public static void Run([CosmosDBTrigger(
            databaseName: "ConversionTrackingDb",
            collectionName: "FlowEvents",
            ConnectionStringSetting = "CosmosDBConnection",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
              //  log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("Conversion Tracker With id created " + input[0].Id);
                   
            }
        }
    }
}

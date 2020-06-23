// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;

namespace ServiceBusFuncation
{
    public static class EventGridHelloWorldFuncation
    {
        [FunctionName("EventGridHelloWorldFuncation")]
        public static void Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            var StevesName = "NametoLogJoe";
            log.LogInformation($"This is the log of my name {StevesName}");

            log.LogInformation(eventGridEvent.Data.ToString());
        }
    }
}

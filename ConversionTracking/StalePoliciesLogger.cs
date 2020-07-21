using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConversionTracking.Manager.Data;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;

namespace ConversionTracking
{
    public class StalePoliciesLogger 
    {
        private readonly TelemetryClient _telemetry;
        private readonly ILogger<StateCountLogger> _logger;
        private readonly CosmosDatabase cosmosDatabase;

        public StalePoliciesLogger(ILogger<StateCountLogger> logger, TelemetryClient telemetry)
        {
            _logger = logger;
            _telemetry = telemetry;
            cosmosDatabase = new CosmosDatabase();
        }

        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{

        //    var properties = new Dictionary<string, string>();
        //    var metrics = new Dictionary<string, double>();

        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        var PolicyList = await cosmosDatabase.GetPoliciesOpenLongerThanMinutes(5);
        //        if (PolicyList.Count > 0)
        //        {
        //            properties.Clear();
        //            metrics.Clear();
        //            metrics.Add("CountofPolicies", PolicyList.Count);
        //            _telemetry.TrackEvent("PoliciesOpenLongerThanFiveMinutes", properties, metrics);
        //        }
          
        //        _logger.LogInformation("Aging policies logger running at: {time}", DateTimeOffset.Now);
        //        await Task.Delay(60000, stoppingToken);
        //    }
        //}
    }
}

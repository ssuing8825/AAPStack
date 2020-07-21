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
    public class StateCountLogger
    {
        private readonly TelemetryClient _telemetry;
        private readonly ILogger<StateCountLogger> _logger;
        private readonly CosmosDatabase cosmosDatabase;

        public StateCountLogger(ILogger<StateCountLogger> logger, TelemetryClient telemetry)
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
        //        var sb = new System.Text.StringBuilder();

        //        var stateCount = await cosmosDatabase.GetStateCount();
        //        foreach (var item in stateCount.Keys)
        //        {
        //            properties.Clear();
        //            metrics.Clear();
        //            properties.Add("statename", item);
        //            metrics.Add("countofstate", stateCount[item]);
        //            _telemetry.TrackEvent("ConversionStateCount", properties, metrics);
        //            sb.AppendFormat("{0} - {1}{2}", item, stateCount[item], Environment.NewLine);
        //        }

        //        _logger.LogInformation("State Count Logger at: {time} {newline} {sb}", DateTimeOffset.Now, Environment.NewLine, sb);
        //        await Task.Delay(50000, stoppingToken);
        //    }
        //}
    }
}

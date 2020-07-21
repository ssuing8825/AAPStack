
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ConversionTracking.Manager.Data;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.Model;

namespace ConversionTracking.Manager
{
    public class ConversionFlowManager : IConversionFlowManager
    {
        private readonly TelemetryClient _telemetry;
        private readonly ILogger _logger;
        private readonly CosmosDatabase cosmosDatabase;
        private readonly IConfiguration _configuration;

        public ConversionFlowManager(ILogger logger)
        {
            cosmosDatabase = new CosmosDatabase();
            _logger = logger;
        }

        public ConversionFlowManager(ILogger<ConversionFlowManager> logger,
            TelemetryClient telemetry,
            IConfiguration configuration)
        {
            _logger = logger;
            _telemetry = telemetry;
            _configuration = configuration;
            cosmosDatabase = new CosmosDatabase();

        }

        public async Task SetProcessToStarted_Extracting(ConversionEvent conversionEvent)
        {
            _logger.LogInformation("Logging that I'm in SetProcessToStarted_Extracting");
          //  _telemetry.TrackEvent("SetProcessToStarted_Extracting");
            var conversionProcess = new ConversionProcess();
            conversionProcess.ConversionStates = new List<ConversionState>();
            conversionProcess.ProcessStartTimeUtc = DateTime.UtcNow;
            conversionProcess.PolicyNumber = conversionEvent.PolicyNumber;
            conversionProcess.CorrelationId = conversionEvent.CorrelationId;

            var currentState = new ConversionState() { StateName = "ConversionStarted", DateTimeOfStateChangeUtc = DateTime.UtcNow, DurationInMillisecondsBetweenLastStateAndThisState = 0 };
            conversionProcess.MostRecentState = currentState;
            conversionProcess.ConversionStates.Add(currentState);

            await cosmosDatabase.CreateConversionProcess(conversionProcess);
            //Sending this to policy so they can start. In the real world the mainframe starts the process
        //    await _messageSender.SendAsync(conversionEvent, MessagingConfiguration.StartOverallProcessCommand);
           // _logger.LogWarning("Start sent for overallprocess {PolicyNumber}", conversionEvent.PolicyNumber);
        }

        public async Task CleanAndCreateDatabase()
        {
            await this.cosmosDatabase.CleanAndCreateDatabase();
        }

        public async Task<List<Policy>> GetPoliciesOpenLongerThanMinutes(int minutes)
        {
            return await this.cosmosDatabase.GetPoliciesOpenLongerThanMinutes(minutes);
        }
        public void PurgeMessagesFromSubscription()
        {
            PurgeMessagesFromSubscriptionAndTopic("policyextracted", "conversiontracking");
            PurgeMessagesFromSubscriptionAndTopic("startautocycleconversioncommand", "policy");
            PurgeMessagesFromSubscriptionAndTopic("startpremiumreportingconversioncommand", "premiumreporting");
            PurgeMessagesFromSubscriptionAndTopic("startbillingconversioncommand", "billing");
            PurgeMessagesFromSubscriptionAndTopic("policysuccessfullyconverted", "conversiontracking");
            PurgeMessagesFromSubscriptionAndTopic("premiumreportingsuccessfullyconverted", "conversiontracking");
            PurgeMessagesFromSubscriptionAndTopic("billingsuccessfullyconverted", "conversiontracking");
            PurgeMessagesFromSubscriptionAndTopic("policyfailed", "conversiontracking");
            PurgeMessagesFromSubscriptionAndTopic("premiumreportingfailed", "conversiontracking");
            PurgeMessagesFromSubscriptionAndTopic("billingfailed", "conversiontracking");
            PurgeMessagesFromSubscriptionAndTopic("rollbackpolicy", "billing");
            PurgeMessagesFromSubscriptionAndTopic("rollbackpolicy", "policy");
            PurgeMessagesFromSubscriptionAndTopic("rollbackpolicy", "premiumreporting");
            PurgeMessagesFromSubscriptionAndTopic("policyrollbacksucceeded", "conversiontracking");
            PurgeMessagesFromSubscriptionAndTopic("billingrollbacksucceeded", "conversiontracking");
            PurgeMessagesFromSubscriptionAndTopic("premiumreportingrollbacksucceeded", "conversiontracking");
            PurgeMessagesFromSubscriptionAndTopic("premiumreportingrollbackfailed", "conversiontracking");
            PurgeMessagesFromSubscriptionAndTopic("billingrollbackfailed", "conversiontracking");
            PurgeMessagesFromSubscriptionAndTopic("policyrollbackfailed", "conversiontracking");
            PurgeMessagesFromSubscriptionAndTopic("startconversioncommand", "policy");
        }

        private void PurgeMessagesFromSubscriptionAndTopic(string topic, string subscription)
        {
            var connectionString = _configuration.GetConnectionString("ServiceBusConnectionString");
            int batchSize = 100;
            var subscriptionClient = new SubscriptionClient(connectionString, topic, subscription, ReceiveMode.ReceiveAndDelete);
            subscriptionClient.PrefetchCount = batchSize;
            RegisterOnMessageHandlerAndReceiveMessages(subscriptionClient);
        }

        private void RegisterOnMessageHandlerAndReceiveMessages(SubscriptionClient client)
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 10,
                AutoComplete = true
            };

            client.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }


        private Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            return Task.CompletedTask;
            // await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            _logger.LogError(exceptionReceivedEventArgs.Exception, "Message handler encountered an exception");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;

            _logger.LogDebug($"- Endpoint: {context.Endpoint}");
            _logger.LogDebug($"- Entity Path: {context.EntityPath}");
            _logger.LogDebug($"- Executing Action: {context.Action}");

            return Task.CompletedTask;
        }
    }
}
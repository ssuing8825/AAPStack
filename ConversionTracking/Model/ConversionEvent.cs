using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Shared.Model
{
    public class ConversionState
    {
        public string StateName { get; set; }

        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime DateTimeOfStateChangeUtc { get; set; }
        public double DurationInMillisecondsBetweenLastStateAndThisState { get; set; }
    }

    public class ConversionEvent
    {
        public string PolicyNumber { get; set; }
        public string CorrelationId { get; set; }
    }
   
    public class ConversionRollbackResponse
    {
        public string SystemName { get; set; }

        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime DateTimeOfEventUtc { get; set; }
        public double DurationInMillisecondsBetweenLastStateAndThisEvent { get; set; }
    }

    public class ConversionFailureEvent : ConversionEvent
    {
        public string Exception { get; set; }
    }
}

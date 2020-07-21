using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Model
{
   public class ConversionFailedEvent : ConversionEvent
    {
        public string Error { get; set; }
    }
}

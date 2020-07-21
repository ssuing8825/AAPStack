using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Model;

namespace ConversionTracking.Manager
{
    public interface IConversionFlowManager
    {
        Task SetProcessToStarted_Extracting(ConversionEvent conversionEvent);
        void PurgeMessagesFromSubscription();
        Task CleanAndCreateDatabase();

        Task<List<Policy>> GetPoliciesOpenLongerThanMinutes(int minutes);
    }
}
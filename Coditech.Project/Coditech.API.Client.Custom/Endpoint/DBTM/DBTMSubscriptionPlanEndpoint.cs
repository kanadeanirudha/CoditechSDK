using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Endpoint
{
    public class DBTMSubscriptionPlanEndpoint : BaseEndpoint
    {
        public string ListAsync(IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMSubscriptionPlan/GetDBTMSubscriptionPlanList{BuildEndpointQueryString(expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }
        public string CreateDBTMSubscriptionPlanAsync() =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMSubscriptionPlan/CreateDBTMSubscriptionPlan";

        public string GetDBTMSubscriptionPlanAsync(int dBTMSubscriptionPlanId) =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMSubscriptionPlan/GetDBTMSubscriptionPlan?dBTMSubscriptionPlanId={dBTMSubscriptionPlanId}";

        public string UpdateDBTMSubscriptionPlanAsync() =>
               $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMSubscriptionPlan/UpdateDBTMSubscriptionPlan";

        public string DeleteDBTMSubscriptionPlanAsync() =>
                  $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMSubscriptionPlan/DeleteDBTMSubscriptionPlan";

        public string GetDBTMSubscriptionPlanActivityListAsync(int dBTMSubscriptionPlanId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMSubscriptionPlan/GetDBTMSubscriptionPlanActivityList?dBTMSubscriptionPlanId={dBTMSubscriptionPlanId}{BuildEndpointQueryString(true, expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }

        public string AssociateUnAssociatePlanActivityAsync() =>
               $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMSubscriptionPlan/AssociateUnAssociatePlanActivity";
    }
}

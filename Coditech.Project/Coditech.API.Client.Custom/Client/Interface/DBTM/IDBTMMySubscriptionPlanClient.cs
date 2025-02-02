using Coditech.Common.API.Model.Response;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Client
{
    public interface IDBTMMySubscriptionPlanClient : IBaseClient
    {
        /// <summary>
        /// Get list of DBTMMySubscriptionPlan.
        /// </summary>
        /// <returns>DBTMMySubscriptionPlanListResponse</returns>
        DBTMMySubscriptionPlanListResponse List(long entityId,IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);
    }
}

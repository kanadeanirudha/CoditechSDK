using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Client
{
    public interface IDBTMSubscriptionPlanClient : IBaseClient
    {
        /// <summary>
        /// Get list of DBTMSubscriptionPlan.
        /// </summary>
        /// <returns>DBTMSubscriptionPlanListResponse</returns>
        DBTMSubscriptionPlanListResponse List(IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);

        /// <summary>
        /// Create DBTMSubscriptionPlan.
        /// </summary>
        /// <param name="DBTMSubscriptionPlanModel">DBTMSubscriptionPlanModel.</param>
        /// <returns>Returns DBTMSubscriptionPlanResponse.</returns>
        DBTMSubscriptionPlanResponse CreateDBTMSubscriptionPlan(DBTMSubscriptionPlanModel body);

        /// <summary>
        /// Get DBTMSubscriptionPlan by dBTMSubscriptionPlanId.
        /// </summary>
        /// <param name="dBTMSubscriptionPlanId">dBTMSubscriptionPlanId</param>
        /// <returns>Returns DBTMSubscriptionPlanResponse.</returns>
        DBTMSubscriptionPlanResponse GetDBTMSubscriptionPlan(int dBTMSubscriptionPlanId);

        /// <summary>
        /// Update DBTMSubscriptionPlan.
        /// </summary>
        /// <param name="DBTMSubscriptionPlanModel">DBTMSubscriptionPlanModel.</param>
        /// <returns>Returns updated DBTMSubscriptionPlanResponse</returns>
        DBTMSubscriptionPlanResponse UpdateDBTMSubscriptionPlan(DBTMSubscriptionPlanModel body);

        /// <summary>
        /// Delete DBTMSubscriptionPlan.
        /// </summary>
        /// <param name="ParameterModel">ParameterModel.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        TrueFalseResponse DeleteDBTMSubscriptionPlan(ParameterModel body);

        #region 
        /// <summary>
        /// Get list of dBTMSubscriptionPlan.
        /// </summary>
        /// <param name="dBTMSubscriptionPlanId">dBTMSubscriptionPlanId</param>
        /// <returns>DBTMSubscriptionPlanActivityListResponse</returns>
        DBTMSubscriptionPlanActivityListResponse GetDBTMSubscriptionPlanActivityList(int dBTMSubscriptionPlanId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);

        /// <summary>
        /// Update Associate UnAssociate DBTM Subscription Plan Activity.
        /// </summary>
        /// <param name="DBTMSubscriptionPlanActivityModel">DBTMSubscriptionPlanActivityModel.</param>
        /// <returns>Returns updated DBTMSubscriptionPlanActivityResponse</returns>
        DBTMSubscriptionPlanActivityResponse AssociateUnAssociatePlanActivity(DBTMSubscriptionPlanActivityModel body);
        #endregion

    }
}

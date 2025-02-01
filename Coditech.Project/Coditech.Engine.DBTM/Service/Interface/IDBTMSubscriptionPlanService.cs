using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;

using System.Collections.Specialized;

namespace Coditech.API.Service
{
    public interface IDBTMSubscriptionPlanService
    {
        DBTMSubscriptionPlanListModel GetDBTMSubscriptionPlanList(FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        DBTMSubscriptionPlanModel CreateDBTMSubscriptionPlan(DBTMSubscriptionPlanModel model);
        DBTMSubscriptionPlanModel GetDBTMSubscriptionPlan(int dBTMSubscriptionPlanId);
        bool UpdateDBTMSubscriptionPlan(DBTMSubscriptionPlanModel model);
        bool DeleteDBTMSubscriptionPlan(ParameterModel parameterModel);
        DBTMSubscriptionPlanActivityListModel GetDBTMSubscriptionPlanActivityList(int dBTMSubscriptionPlanId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        bool AssociateUnAssociatePlanActivity(DBTMSubscriptionPlanActivityModel model);
    }
}

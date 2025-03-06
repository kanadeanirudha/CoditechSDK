using Coditech.Common.API.Model.Responses;
namespace Coditech.API.Client
{
    public interface IDBTMDashboardClient : IBaseClient
    {
        /// <summary>
        /// Get DBTM Dashboard by selectedAdminRoleMasterId.
        /// </summary>
        /// <param name="numberOfDaysRecord">selectedAdminRoleMasterId</param>
        /// <param name="selectedAdminRoleMasterId">selectedAdminRoleMasterId</param>
        /// <param name="userMasterId">userMasterId</param>
        /// <returns>Returns DBTMDashboardResponse.</returns>
        DBTMDashboardResponse GetDBTMDashboardDetails(short numberOfDaysRecord,int selectedAdminRoleMasterId, long userMasterId);
    }
}

using Coditech.Common.API.Model.Responses;
namespace Coditech.API.Client
{
    public interface ICustomDashboardClient : IBaseClient
    {
        /// <summary>
        /// Get Custom Dashboard by selectedAdminRoleMasterId.
        /// </summary>
        /// <param name="selectedAdminRoleMasterId">selectedAdminRoleMasterId</param>
        /// <param name="userMasterId">userMasterId</param>
        /// <returns>Returns DashboardResponse.</returns>
        CustomDashboardResponse GetCustomDashboardDetails(int selectedAdminRoleMasterId, long userMasterId);
    }
}

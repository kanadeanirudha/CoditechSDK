using Coditech.Admin.ViewModel;

namespace Coditech.Admin.Agents
{
    public interface IDBTMDashboardAgent
    {
        /// <summary>
        /// Get GetDBTMDashboardDetails.
        /// </summary>
        /// <returns>Returns DBTMDashboardViewModel.</returns>
        DBTMDashboardViewModel GetDBTMDashboardDetails();
    }
}

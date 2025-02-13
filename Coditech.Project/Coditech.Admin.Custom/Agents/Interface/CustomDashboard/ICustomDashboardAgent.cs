using Coditech.Admin.ViewModel;

namespace Coditech.Admin.Agents
{
    public interface ICustomDashboardAgent
    {
        /// <summary>
        /// Get GetCustomDashboardDetails.
        /// </summary>
        /// <returns>Returns CustomDashboardViewModel.</returns>
        CustomDashboardViewModel GetCustomDashboardDetails();
    }
}

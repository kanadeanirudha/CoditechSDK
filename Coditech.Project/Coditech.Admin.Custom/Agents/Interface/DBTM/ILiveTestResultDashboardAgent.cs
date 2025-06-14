using Coditech.Admin.ViewModel;

namespace Coditech.Admin.Agents
{
    public interface ILiveTestResultDashboardAgent
    {
        /// <summary>
        /// Get GetLiveTestResultDashboardDetails.
        /// </summary>
        /// <returns>Returns LiveTestResultDashboardViewModel.</returns>
        LiveTestResultDashboardViewModel GetLiveTestResultDashboard();
    }
}

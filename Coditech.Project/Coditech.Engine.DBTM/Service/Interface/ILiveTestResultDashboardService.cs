using Coditech.Common.API.Model;

namespace Coditech.API.Service
{
    public interface ILiveTestResultDashboardService
    {
        LiveTestResultDashboardModel GetLiveTestResultDashboard(string selectedCentreCode, long entityId);
        LiveTestResultLoginModel GetLiveTestResultLogin(LiveTestResultLoginModel model);
    }
}

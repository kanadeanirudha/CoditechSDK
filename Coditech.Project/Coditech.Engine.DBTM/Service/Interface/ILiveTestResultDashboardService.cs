using Coditech.Common.API.Model;

namespace Coditech.API.Service
{
    public interface ILiveTestResultDashboardService
    {
        LiveTestResultDashboardModel GetLiveTestResultLogin(LiveTestResultLoginModel liveTestResultLoginModel);
    }
}

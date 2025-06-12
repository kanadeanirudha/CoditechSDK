using Coditech.Common.API.Model.Responses;

namespace Coditech.API.Client
{
    public interface ILiveTestResultDashboardClient : IBaseClient
    {
        LiveTestResultDashboardResponse GetLiveTestResultDashboard(string selectedCentreCode, long entityId);
    }
}

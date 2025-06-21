using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;
namespace Coditech.API.Client
{
    public interface ILiveTestResultDashboardClient : IBaseClient
    {
        LiveTestResultLoginResponse GetLiveTestResultDashboard(LiveTestResultLoginModel body); 
    }
}

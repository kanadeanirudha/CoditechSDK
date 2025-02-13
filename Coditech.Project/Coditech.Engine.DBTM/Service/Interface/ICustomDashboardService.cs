using Coditech.Common.API.Model;

namespace Coditech.API.Service
{
    public interface ICustomDashboardService
    {
        CustomDashboardModel GetCustomDashboardDetails(int selectedAdminRoleMasterId, long userMasterId);
    }
}

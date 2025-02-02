using Coditech.Common.API.Model;

namespace Coditech.API.Service
{
    public interface IDBTMDashboardService
    {
        DBTMDashboardModel GetDBTMDashboardDetails(int selectedAdminRoleMasterId, long userMasterId);
    }
}

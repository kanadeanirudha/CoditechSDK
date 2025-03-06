using Coditech.Common.API.Model;

namespace Coditech.API.Service
{
    public interface IDBTMDashboardService
    {
        DBTMDashboardModel GetDBTMDashboardDetails(short numberOfDaysRecord, int selectedAdminRoleMasterId, long userMasterId);
    }
}

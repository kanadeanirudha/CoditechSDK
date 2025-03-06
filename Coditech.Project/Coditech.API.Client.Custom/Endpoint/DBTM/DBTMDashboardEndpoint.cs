using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;

namespace Coditech.API.Endpoint
{
    public class DBTMDashboardEndpoint : BaseEndpoint
    {
        public string GetDBTMDashboardDetailsAsync(short numberOfDaysRecord, int selectedAdminRoleMasterId,long userMasterId) =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMDashboardController/GetDBTMDashboardDetails?numberOfDaysRecord={numberOfDaysRecord}&selectedAdminRoleMasterId={selectedAdminRoleMasterId}&userMasterId={userMasterId}";
    }
}

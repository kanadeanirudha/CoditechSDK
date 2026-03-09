using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Endpoint
{
    public class DBTMCampEndpoint : BaseEndpoint
    {
        public string ListAsync(string selectedCentreCode, long userMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMCampMaster/GetDBTMCampList?selectedCentreCode={selectedCentreCode}&userMasterId={userMasterId}{BuildEndpointQueryString(true,expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }
        public string CreateDBTMCampAsync() =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMCampMaster/CreateDBTMCamp";

        public string GetDBTMCampAsync(long dBTMCampMasterId) =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMCampMaster/GetDBTMCamp?dBTMCampMasterId={dBTMCampMasterId}";
       
        public string UpdateDBTMCampAsync() =>
               $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMCampMaster/UpdateDBTMCamp";

        public string DeleteDBTMCampAsync() =>
                  $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMCampMaster/DeleteDBTMCamp";

        public string DBTMCampUserListAsync(long dBTMCampMasterId, string userType, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMCampMaster/GetDBTMCampUserList?dBTMCampMasterId={dBTMCampMasterId}&userType={userType}{BuildEndpointQueryString(true, expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }
        public string AssociateUnAssociateCampwiseUserAsync() =>
       $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMCampMaster/AssociateUnAssociateCampwiseUser";
        public string GetCampUserListByCentreCodeAndGeneralTrainerMasterIdAsync(string selectedCentreCode, long generalTrainerMasterId, long dBTMCampMasterId)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMCampMaster/GetCampUserListByCentreCodeAndGeneralTrainerMasterId?selectedCentreCode={selectedCentreCode}&generalTrainerMasterId={generalTrainerMasterId}&dBTMCampMasterId={dBTMCampMasterId}{BuildEndpointQueryString(true)}";
            return endpoint;
        }
    }
}

using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Endpoint
{
    public class DBTMBatchEndpoint : BaseEndpoint
    {
        public string DBTMBatchAsync(long entityId, string userType)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMApi/GetBatchList?entityId={entityId}&userType={userType}";
            return endpoint;
        }
        public string GetDBTMBatchUserListAsync(string selectedCentreCode, long generalTrainerMasterId,int generalBatchMasterId)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechOrganisationApiRootUri}/DBTMGeneralBatchMaster/GetDBTMBatchUserList?selectedCentreCode={selectedCentreCode}&generalTrainerMasterId={generalTrainerMasterId}&generalBatchMasterId={generalBatchMasterId}{BuildEndpointQueryString(true)}";
            return endpoint;
        }
    }
}

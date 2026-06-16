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
        public string GetCalendarBatchesAsync(string centreCode, long userMasterId, DateTime startDate, DateTime endDate)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechOrganisationApiRootUri}/DBTMGeneralBatchMaster/GetCalendarBatches?centreCode={centreCode}&userMasterId={userMasterId}&startDate={startDate}&endDate={endDate}{BuildEndpointQueryString(true)}";
            return endpoint;
        }
        public string GetDBTMCentrAndTrainerewiseBatchListAsync(string centreCode, int joiningCodeTypeEnumId, long generalTrainerMasterId)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMApi/GetDBTMCentrAndTrainerewiseBatchList?centreCode={centreCode}&joiningCodeTypeEnumId={joiningCodeTypeEnumId}&generalTrainerMasterId={generalTrainerMasterId}{BuildEndpointQueryString(true)}";
            return endpoint;
        }
        public string TransferBatchAsync()
        {
            return $"{CoditechCustomAdminSettings.CoditechOrganisationApiRootUri}/DBTMGeneralBatchMaster/TransferBatch";
        }
    }
}

using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Endpoint
{
    public class DBTMTraineeDetailsEndpoint : BaseEndpoint
    {
        public string ListAsync(string selectedCentreCode, long generalTrainerMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTraineeDetails/GetDBTMTraineeDetailsList?selectedCentreCode={selectedCentreCode}&generalTrainerMasterId={generalTrainerMasterId}{BuildEndpointQueryString(true, expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }

        public string GetDBTMTraineeOtherDetailsAsync(long dBTMTraineeDetailId) =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTraineeDetails/GetDBTMTraineeOtherDetails?dBTMTraineeDetailId={dBTMTraineeDetailId}";

        public string UpdateDBTMTraineeOtherDetailsAsync() =>
               $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTraineeDetails/UpdateDBTMTraineeOtherDetails";

        public string DeleteDBTMTraineeDetailsAsync() =>
                  $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTraineeDetails/DeleteDBTMTraineeDetails";

        public string GetTraineeActivitiesListAsync(string personCode, int numberOfDaysRecord, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTraineeDetails/GetTraineeActivitiesList?personCode={personCode}&numberOfDaysRecord={numberOfDaysRecord}{BuildEndpointQueryString(true, expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }

        public string GetTraineeActivitiesDetailsListAsync(long dBTMDeviceDataId, long entityId, string userType, string centreCode, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTraineeDetails/GetTraineeActivitiesDetailsList?dBTMDeviceDataId={dBTMDeviceDataId}&entityId={entityId}&userType={userType}&centreCode={centreCode}{BuildEndpointQueryString(true, expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }

        public string GetProfileDetailsAsync(long dBTMTraineeDetailId) =>
          $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTraineeDetails/GetProfileDetails?dBTMTraineeDetailId={dBTMTraineeDetailId}";

        public string GenerateAthletePdfRemarkAsync(long dBTMTraineeDetailId, string remarks) =>
          $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTraineeDetails/GenerateAthletePdfRemark?dBTMTraineeDetailId={dBTMTraineeDetailId}&remarks={remarks}";
        public string UploadTraineeAsync()
        {
            return $"{CoditechCustomAdminSettings.CoditechOrganisationApiRootUri}/DBTMUser/UploadTrainee";
        }
    }
}

using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Endpoint
{
    public class DBTMTraineeDetailsEndpoint : BaseEndpoint
    {
        public string ListAsync(string selectedCentreCode,long generalTrainerMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
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

        public string GetTraineeActivitiesListAsync(string personCode,int numberOfDaysRecord,IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTraineeDetails/GetTraineeActivitiesList?personCode={personCode}&numberOfDaysRecord={numberOfDaysRecord}{BuildEndpointQueryString(true,expand,filter,sort,pageIndex,pageSize)}";
            return endpoint;
        }

        public string GetTraineeActivitiesDetailsListAsync(long dBTMDeviceDataId,IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTraineeDetails/GetTraineeActivitiesDetailsList?dBTMDeviceDataId={dBTMDeviceDataId}{BuildEndpointQueryString(true,expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }
    }
}

using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Endpoint
{
    public class DBTMTraineeAssignmentEndpoint : BaseEndpoint
    {
        public string ListAsync(long generalTrainerMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTraineeAssignment/GetDBTMTraineeAssignmentList?generalTrainerMasterId={generalTrainerMasterId}{BuildEndpointQueryString(true,expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }
        public string CreateDBTMTraineeAssignmentAsync() =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTraineeAssignment/CreateDBTMTraineeAssignment";

        public string GetDBTMTraineeAssignmentAsync(long dBTMTraineeAssignmentUserId) =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTraineeAssignment/GetDBTMTraineeAssignment?dBTMTraineeAssignmentUserId={dBTMTraineeAssignmentUserId}";

        public string UpdateDBTMTraineeAssignmentAsync() =>
               $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTraineeAssignment/UpdateDBTMTraineeAssignment";

        public string DeleteDBTMTraineeAssignmentAsync() =>
                  $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTraineeAssignment/DeleteDBTMTraineeAssignment";
        public string GetDBTMTrainerByCentreCode(string centreCode, long entityId)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTraineeAssignment/GetTrainerByCentreCode?centreCode={centreCode}&entityId={entityId}";
            return endpoint;
        }
        public string GetTraineeDetailsByCentreCodeAndgeneralTrainerId(string centreCode, long generalTrainerId) =>
           $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTraineeAssignment/GetTraineeDetailByCentreCodeAndgeneralTrainerId?centreCode={centreCode}&generalTrainerId={generalTrainerId}";

        public string SendAssignmentReminderAsync(long dBTMTraineeAssignmentId, long dBTMTraineeAssignmentUserId) =>
           $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTraineeAssignment/SendAssignmentReminder?dBTMTraineeAssignmentId={dBTMTraineeAssignmentId}&dBTMTraineeAssignmentUserId={dBTMTraineeAssignmentUserId}";

        public string DBTMTraineeAssignmentToUserListAsync(long dBTMTraineeAssignmentId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTraineeAssignment/GetDBTMTraineeAssignmentToUserList?dBTMTraineeAssignmentId={dBTMTraineeAssignmentId}{BuildEndpointQueryString(true, expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }
        public string AssociateUnAssociateAssignmentwiseUserAsync() =>
       $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTraineeAssignment/AssociateUnAssociateAssignmentwisewiseUser";
    }
}

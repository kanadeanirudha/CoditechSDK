using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
namespace Coditech.API.Endpoint
{
    public class DBTMOrganisationCentrewiseJoiningCodeEndpoint : BaseEndpoint
    {
        public string GetTraineeActiveJoiningCodeAsync(string centreCode, string trainerId)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechOrganisationApiRootUri}/DBTMOrganisationCentrewiseJoiningCode/GetTraineeActiveJoiningCode?centreCode={centreCode}&trainerId={trainerId}";
            return endpoint;
        }
        public string GetTrainerActiveJoiningCodeAsync(string centreCode)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechOrganisationApiRootUri}/DBTMOrganisationCentrewiseJoiningCode/GetTrainerActiveJoiningCode?centreCode={centreCode}";
            return endpoint;
        }
        public string DeleteJoiningCodeFileAsync() =>
                 $"{CoditechCustomAdminSettings.CoditechOrganisationApiRootUri}/DBTMOrganisationCentrewiseJoiningCode/DeleteOrganisationCentrewiseJoiningCodeFile";
        public string GetTraineeActiveJoiningCodeListAsync(string centreCode, string trainerId, int rows)
        {
            return $"{CoditechCustomAdminSettings.CoditechOrganisationApiRootUri}/DBTMOrganisationCentrewiseJoiningCode/GetTraineeActiveJoiningCodeList?centreCode={centreCode}&trainerId={trainerId}&rows={rows}";
        }
        public string IsTrainerJoiningCodeLockedAsync(string joiningCode)
        {
            return $"{CoditechCustomAdminSettings.CoditechOrganisationApiRootUri}/DBTMOrganisationCentrewiseJoiningCode/IsTrainerJoiningCodeLocked?joiningCode={joiningCode}";
        }
    }
}

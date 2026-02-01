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
        public string DeleteJoiningCodeFileAsync() =>
                 $"{CoditechCustomAdminSettings.CoditechOrganisationApiRootUri}/DBTMOrganisationCentrewiseJoiningCode/DeleteOrganisationCentrewiseJoiningCodeFile";     
    }
}

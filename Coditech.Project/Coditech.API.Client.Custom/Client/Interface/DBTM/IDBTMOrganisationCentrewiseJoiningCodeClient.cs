using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;
namespace Coditech.API.Client
{
    public interface IDBTMOrganisationCentrewiseJoiningCodeClient : IBaseClient
    {
        /// <summary>
        /// Get GetTraineeActiveJoiningCode by centreCode.
        /// </summary>
        /// <param name="centreCode">centreCode</param>
        /// <returns>Returns DBTMOrganisationCentrewiseJoiningCodeResponse.</returns>
        DBTMOrganisationCentrewiseJoiningCodeResponse GetTraineeActiveJoiningCode(string centreCode, string trainerId);
        DBTMOrganisationCentrewiseJoiningCodeResponse GetTrainerActiveJoiningCode(string centreCode);
        OrganisationCentrewiseJoiningCodeListResponse GetTraineeActiveJoiningCodeList(string centreCode, string trainerId, int rows);
        TrueFalseResponse DeleteJoiningCodeFile(ParameterModel body);
        bool IsTrainerJoiningCodeLocked(string joiningCode);
    }
}

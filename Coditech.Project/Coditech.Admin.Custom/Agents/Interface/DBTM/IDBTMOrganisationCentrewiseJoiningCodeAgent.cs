using Coditech.Admin.ViewModel;
namespace Coditech.Admin.Agents
{
    public interface IDBTMOrganisationCentrewiseJoiningCodeAgent
    {
        /// <summary>
        /// Get DBTMCentrewiseJoiningCode by dBTMOrganisationCentreMasterId.
        /// </summary>
        /// <param name="dBTMOrganisationCentreMasterId">dBTMOrganisationCentreMasterId</param>
        /// <returns>Returns DBTMOrganisationCentrewiseJoiningCodeViewModel.</returns>
        DBTMOrganisationCentrewiseJoiningCodeViewModel GetTraineeActiveJoiningCode(string centreCode);
        List<OrganisationCentrewiseJoiningCodeViewModel> GetTraineeActiveJoiningCodeList(string centreCode, string trainerId, int rows);
        DBTMOrganisationCentrewiseJoiningCodeViewModel GetTrainerActiveJoiningCode(string centreCode);
        bool DeleteJoiningCodeFile(string fileName);
        bool IsTrainerJoiningCodeLocked(string joiningCode);
    }
}

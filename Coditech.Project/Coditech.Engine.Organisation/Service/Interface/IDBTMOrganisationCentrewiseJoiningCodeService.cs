using Coditech.Common.API.Model;
using System.Collections.Specialized;

namespace Coditech.API.Service
{
    public interface IDBTMOrganisationCentrewiseJoiningCodeService
    {
        List<OrganisationCentrewiseJoiningCodeModel> GetTraineeActiveJoiningCodeList(string centreCode, string trainerId,int rows);
        DBTMOrganisationCentrewiseJoiningCodeModel GetTraineeActiveJoiningCode(string centreCode, string trainerId,int rows);
        bool DeleteOrganisationCentrewiseJoiningCodeFile(string fileName);
    }
}

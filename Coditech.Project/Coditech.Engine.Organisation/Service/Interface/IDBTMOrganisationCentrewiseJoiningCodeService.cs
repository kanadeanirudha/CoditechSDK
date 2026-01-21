using Coditech.Common.API.Model;
using System.Collections.Specialized;

namespace Coditech.API.Service
{
    public interface IDBTMOrganisationCentrewiseJoiningCodeService
    {
        DBTMOrganisationCentrewiseJoiningCodeModel GetTraineeActiveJoiningCode(string centreCode, string trainerId);
        bool DeleteOrganisationCentrewiseJoiningCodeFile(string fileName);
    }
}

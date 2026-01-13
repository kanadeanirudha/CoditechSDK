using Coditech.Common.API.Model;
using System.Collections.Specialized;

namespace Coditech.API.Service
{
    public interface IDBTMOrganisationCentrewiseJoiningCodeService
    {
        DBTMOrganisationCentrewiseJoiningCodeModel GetTraineeActiveJoiningCode(string centreCode);
        bool DeleteOrganisationCentrewiseJoiningCodeFile(string fileName);
    }
}

using Coditech.Common.API.Model;
using System.Collections.Specialized;

namespace Coditech.API.Service
{
    public interface IDBTMBatchMasterService
    {
        GeneralBatchUserListModel GetDBTMBatchUserList(string selectedCentreCode, long generalTrainerMasterId, int generalBatchMasterId);
    }
}

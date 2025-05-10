using Coditech.Common.API.Model;

namespace Coditech.API.Service
{
    public interface IDBTMApiService
    {
        List<DBTMBatchModel> GetBatchList(long entityId, string userType);
        DBTMBatchModel GetBatchDetails(int generalBatchMasterId);
        List<DBTMTestApiModel> GetAssignmentList(long entityId, string userType);
        DBTMTestApiModel GetAssignmentDetails(long dBTMTraineeAssignmentId);
    }
}

using Coditech.Common.API.Model;

namespace Coditech.API.Service
{
    public interface IDBTMApiService
    {
        bool InsertDeviceData(List<DBTMDeviceDataModel> model);
        List<DBTMBatchModel> GetBatchList(long entityId, string userType);
        DBTMBatchModel GetBatchDetails(int generalBatchMasterId);
        List<DBTMTestApiModel> GetAssignmentList(long entityId, string userType);
        DBTMTestApiModel GetAssignmentDetails(long dBTMTraineeAssignmentId);
        DBTMMobileDashboardModel GetTrainerDashboard(long userMasterId);
        DBTMMobileTraineeDashboardModel GetTraineeDashboard(long userMasterId);
        string GetJoiningCode(string generalTrainerMasterId);
        bool InsertDeviceDataViaFile(IFormFile file);
    }
}

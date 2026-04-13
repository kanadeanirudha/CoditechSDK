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
        string GetCentreWiseJoiningCode(string centreCode, int joiningCodeTypeEnumId);
        bool InsertDeviceDataViaFile(IFormFile file);
        DBTMTraineeDetailsListModel GetTraineesByPerformedActivity(string dBTMTestMasterIds, string centreCode, long generalTrainerMasterId);
        DBTMTestListModel GetactivitiesBytrainee(long selectedTraineeId);
        List<DBTMGeneralBatchUserModel> GetBatchAndActivityWiseUserDetails(int generalBatchMasterId, int dbtmTestMasterId);
        List<DBTMGeneralBatchUserModel> GetCampAndActivityWiseUserDetails(int dBTMcampMasterId, int dbtmTestMasterId, string userType);
        List<DBTMBatchModel> GetCampList(long entityId, string userType);
        DBTMBatchModel GetCampDetails(int dBTMCampMasterId);
        bool UpdateValidRecord(long dBTMDeviceDataId, bool isValidRecord);
    }
}
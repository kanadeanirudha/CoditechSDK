using Coditech.Common.API.Model;
namespace Coditech.API.Service
{
    public interface IDBTMApiService
    {
        bool InsertDeviceDataViaFile(IFormFile file);
        bool InsertDeviceData(List<DBTMDeviceDataModel> model);
        bool InsertDeviceDataV2(string rawJson);
        List<DBTMBatchModel> GetBatchList(long entityId, string userType, bool isCheckTestPerformed);
        DBTMBatchModel GetBatchDetails(int generalBatchMasterId);
        List<DBTMTestApiModel> GetAssignmentList(long entityId, string userType);
        DBTMTestApiModel GetAssignmentDetails(long dBTMTraineeAssignmentId);
        DBTMMobileDashboardModel GetTrainerDashboard(long userMasterId);
        DBTMMobileTraineeDashboardModel GetTraineeDashboard(long userMasterId);
        OrganisationCentrewiseJoiningCodeModel GetJoiningCode(string generalTrainerMasterId);
        string GetCentreWiseJoiningCode(string centreCode, int joiningCodeTypeEnumId);
        DBTMTraineeDetailsListModel GetTraineesByPerformedActivity(string dBTMTestMasterIds, string centreCode, long generalTrainerMasterId);
        DBTMTestListModel GetactivitiesBytrainee(long selectedTraineeId);
        List<DBTMGeneralBatchUserModel> GetBatchAndActivityWiseUserDetails(int generalBatchMasterId, int dbtmTestMasterId);
        List<DBTMGeneralBatchUserModel> GetCampAndActivityWiseUserDetails(int dBTMcampMasterId, int dbtmTestMasterId, string userType);
        List<DBTMBatchModel> GetCampList(long entityId, string userType);
        DBTMBatchModel GetCampDetails(int dBTMCampMasterId);
        bool UpdateValidRecord(long dBTMDeviceDataId, bool isValidRecord);
        DBTMBatchListModel GetDBTMCentrAndTrainerewiseBatchList(string centreCode, int joiningCodeTypeEnumId, long generalTrainerMasterId);
        DBTMBatchListModel GetDBTMTrainerwiseBatchList(string centreCode, long generalTrainerMasterId);
        DBTMTraineeDetailsModel GetDBTMTraineeDetails(long dBTMTraineeDetailId);
        bool UpdateDBTMTraineeDetails(DBTMTraineeDetailsModel model);
    }
}
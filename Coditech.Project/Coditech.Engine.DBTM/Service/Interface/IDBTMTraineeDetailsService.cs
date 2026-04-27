using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using System.Collections.Specialized;

namespace Coditech.API.Service
{
    public interface IDBTMTraineeDetailsService
    {
        DBTMTraineeDetailsListModel GetDBTMTraineeDetailsList(string selectedCentreCode, long generalTrainerMasterId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        DBTMTraineeDetailsModel GetDBTMTraineeOtherDetails(long dBTMTraineeDetailId);
        bool UpdateDBTMTraineeOtherDetails(DBTMTraineeDetailsModel model);
        bool DeleteDBTMTraineeDetails(ParameterModel parameterModel);
        DBTMActivitiesListModel GetTraineeActivitiesList(string personCode, int numberOfDaysRecord, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        DBTMActivitiesDetailsListModel GetTraineeActivitiesDetailsList(long dBTMDeviceDataId, long entityId, string userType, string centreCode, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        DBTMTraineeProfileModel GetProfileDetails(long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate);
        DBTMReportsListModel GenerateAthletePdfRemark(long dBTMTraineeDetailId, string remarks, DateTime FromDate, DateTime ToDate);
        string GetTraineeProfileHtml(long dBTMTraineeDetailId, string remarks, DateTime FromDate, DateTime ToDate);
        DBTMTraineeProfileListModel GetProfileDetailsList(long generalBatchMasterId, string dBTMTraineeDetailIds, string orderBy,DateTime FromDate, DateTime ToDate);
        List<DateTime> GetTraineeListActivityDates(string dBTMTraineeDetailIds, int generalBatchMasterId);
    }
}

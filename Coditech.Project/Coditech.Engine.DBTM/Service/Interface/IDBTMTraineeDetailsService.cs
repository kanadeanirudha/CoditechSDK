using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;

using System.Collections.Specialized;

namespace Coditech.API.Service
{
    public interface IDBTMTraineeDetailsService
    {
        DBTMTraineeDetailsListModel GetDBTMTraineeDetailsList(string selectedCentreCode, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        DBTMTraineeDetailsModel GetDBTMTraineeOtherDetails(long dBTMTraineeDetailId);
        bool UpdateDBTMTraineeOtherDetails(DBTMTraineeDetailsModel model);
        bool DeleteDBTMTraineeDetails(ParameterModel parameterModel);
        DBTMActivitiesListModel GetTraineeActivitiesList(string personCode,int numberOfDaysRecord,FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        DBTMActivitiesDetailsListModel GetTraineeActivitiesDetailsList(long dBTMDeviceDataId,FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
    }
}

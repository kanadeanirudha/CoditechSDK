using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;

using System.Collections.Specialized;

namespace Coditech.API.Service
{
    public interface IDBTMTraineeAssignmentService
    {
        DBTMTraineeAssignmentListModel GetDBTMTraineeAssignmentList(long generalTrainerMasterId,FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        DBTMTraineeAssignmentModel CreateDBTMTraineeAssignment(DBTMTraineeAssignmentModel model);
        DBTMTraineeAssignmentModel GetDBTMTraineeAssignment(long dBTMTraineeAssignmentId);
        bool UpdateDBTMTraineeAssignment(DBTMTraineeAssignmentModel model);
        bool DeleteDBTMTraineeAssignment(ParameterModel parameterModel);
        GeneralTrainerListModel GetTrainerByCentreCode(string centreCode);
        DBTMTraineeDetailsListModel GetTraineeDetailByCentreCodeAndgeneralTrainerId(string centreCode, long generalTrainerId);
        bool SendAssignmentReminder(string dBTMTraineeAssignmentId);
    }
}

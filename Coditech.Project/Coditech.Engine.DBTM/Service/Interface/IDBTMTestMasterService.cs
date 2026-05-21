using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using System.Collections.Specialized;
namespace Coditech.API.Service
{
    public interface IDBTMTestMasterService
    {
        DBTMTestListModel GetDBTMTestList(FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        DBTMTestModel CreateDBTMTest(DBTMTestModel model);
        DBTMTestModel GetDBTMTest(int dBTMTestMasterId);
        bool UpdateDBTMTest(DBTMTestModel model);
        bool DeleteDBTMTest(ParameterModel parameterModel);
        DBTMGraphMasterListModel GetDBTMGraph(int dBTMTestMasterId);
        DBTMGraphMasterListModel GetDBTMGraphByDBTMTestMasterId(int dBTMTestMasterId, string graphMode);
        DBTMPerformanceMatrixListModel GetDBTMPerformanceMatrixList(FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        bool DeleteActivityListViewSequence(ParameterModel parameterModel);
        DBTMActivityListViewSequenceModel GetActivityListViewSequence(int dBTMTestParameterListViewSequenceId);
        bool UpdateActivityListViewSequence(DBTMActivityListViewSequenceModel model);
        DBTMActivityListViewSequenceListModel GetActivityListViewSequenceList(int dBTMTestMasterId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        DBTMActivityListViewSequenceModel UpdateSequenceNumber(DBTMActivityListViewSequenceModel model);
        DBTMActivityListViewSequenceModel CreateActivityListViewSequence(DBTMActivityListViewSequenceModel model);
        bool DeleteActivityVerticalViewSequence(ParameterModel parameterModel);
        DBTMActivityVerticalViewSequenceModel GetActivityVerticalViewSequence(int dBTMTestParameterVerticalViewSequenceId);
        bool UpdateActivityVerticalViewSequence(DBTMActivityVerticalViewSequenceModel model);
        DBTMActivityVerticalViewSequenceListModel GetActivityVerticalViewSequenceList(int dBTMTestMasterId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        DBTMActivityVerticalViewSequenceModel UpdateVerticalSequenceNumber(DBTMActivityVerticalViewSequenceModel model);
        DBTMActivityVerticalViewSequenceModel CreateActivityVerticalViewSequence(DBTMActivityVerticalViewSequenceModel model);
        DBTMCentreWiseTestListModel GetTestsByCentreCode(string centreCode);
        DBTMTestWisePerformanceStandardListModel GetDBTMTestWisePerformanceStandardList(int dBTMTestMasterId);
        DBTMTestWisePerformanceStandardModel CreateDBTMTestWisePerformanceStandard(DBTMTestWisePerformanceStandardModel model);
        bool UpdateDBTMTestWisePerformanceStandard(DBTMTestWisePerformanceStandardModel model);
    }
}

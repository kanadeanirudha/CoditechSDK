using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using System.Collections.Specialized;

namespace Coditech.API.Service
{
    public interface IDBTMGraphMasterService
    {
        DBTMGraphMasterListModel GetDBTMGraphList(FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        DBTMGraphMasterModel CreateDBTMGraph(DBTMGraphMasterModel model);
        DBTMGraphMasterModel GetDBTMGraph(string graphCode);
        bool UpdateDBTMGraph(DBTMGraphMasterModel model);
        bool DeleteDBTMGraph(ParameterModel parameterModel);
        DBTMTestListModel GetDBTMGraphTestCode();
        DBTMGraphVerticalViewSequenceModel GetGraphVerticalViewSequence(int dBTMTestParameterVerticalViewSequenceId);
        bool UpdateGraphVerticalViewSequence(DBTMGraphVerticalViewSequenceModel model);
        DBTMGraphVerticalViewSequenceListModel GetGraphVerticalViewSequenceList(int dBTMTestMasterId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        DBTMGraphVerticalViewSequenceModel UpdateGraphVerticalSequenceNumber(DBTMGraphVerticalViewSequenceModel model);
        DBTMGraphVerticalViewSequenceModel CreateGraphVerticalViewSequence(DBTMGraphVerticalViewSequenceModel model);
        bool DeleteGraphVerticalViewSequence(ParameterModel parameterModel);
    }
}

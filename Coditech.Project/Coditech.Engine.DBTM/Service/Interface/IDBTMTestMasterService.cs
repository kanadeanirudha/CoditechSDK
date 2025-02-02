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
        DBTMTestParameterListModel GetDBTMTestParameter();
    }
}

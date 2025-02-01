using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using System.Collections.Specialized;

namespace Coditech.API.Service
{
    public interface IDBTMBatchActivityService
    {
        DBTMBatchActivityListModel GetDBTMBatchActivityList(int generalBatchMasterId, bool isAssociated, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        DBTMBatchActivityModel CreateDBTMBatchActivity(DBTMBatchActivityModel model);
        bool DeleteDBTMBatchActivity(ParameterModel parameterModel);
    }
}

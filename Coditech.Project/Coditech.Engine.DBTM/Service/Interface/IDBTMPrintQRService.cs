using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using System.Collections.Specialized;
namespace Coditech.API.Service
{
    public interface IDBTMPrintQRService
    {
        DBTMPrintQRListModel DownloadPrintQR(ParameterModel parameterModel);
        DBTMPrintQRListModel GetDBTMPrintQRTraineeList(int generalBatchMasterId, string userType, FilterCollection filter, NameValueCollection nameValueCollection1, NameValueCollection nameValueCollection2, int pageIndex, int pageSize);
    }
}

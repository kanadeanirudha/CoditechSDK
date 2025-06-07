using Coditech.Common.API.Model;

namespace Coditech.API.Service
{
    public interface IDBTMReportsService
    {
        DBTMBatchWiseReportsListModel BatchWiseReports(int generalBatchMasterId);
    }
}

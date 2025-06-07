using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model.Response;
using Coditech.Common.Logger;

namespace Coditech.Admin.Agents
{
    public class DBTMReportsAgent : BaseAgent, IDBTMReportsAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IDBTMReportsClient _dBTMReportsClient;
        #endregion

        #region Public Constructor
        public DBTMReportsAgent(ICoditechLogging coditechLogging, IDBTMReportsClient dBTMReportsClient)
        {
            _coditechLogging = coditechLogging;
            _dBTMReportsClient = GetClient<IDBTMReportsClient>(dBTMReportsClient);
        }
        #endregion

        #region Public Methods
        public virtual DBTMBatchWiseReportsListViewModel BatchWiseReports(int generalBatchMasterId)
        {
            DBTMBatchWiseReportsListResponse response = _dBTMReportsClient.BatchWiseReports(generalBatchMasterId);

            DBTMBatchWiseReportsListViewModel listViewModel = new DBTMBatchWiseReportsListViewModel
            {
                DataTable = response.DataTable
            };
            return listViewModel;
        }
        #endregion
    }
}

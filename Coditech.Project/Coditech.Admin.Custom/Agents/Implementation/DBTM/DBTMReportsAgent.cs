using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model;
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
        //Batch Wise Reports 
        public virtual DBTMBatchWiseReportsListViewModel BatchWiseReports(int generalBatchMasterId)
        {
            DBTMBatchWiseReportsListResponse response = _dBTMReportsClient.BatchWiseReports(generalBatchMasterId);

            DBTMBatchWiseReportsListViewModel listViewModel = new DBTMBatchWiseReportsListViewModel
            {
                DataTable = response.DataTable
            };
            return listViewModel;
        }

        //Test Wise Reports 
        public virtual DBTMTestWiseReportsListViewModel TestWiseReports(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate)
        {
            long entityId = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession).EntityId;
            DBTMTestWiseReportsListResponse response = _dBTMReportsClient.TestWiseReports(dBTMTestMasterId,dBTMTraineeDetailId,FromDate,ToDate,entityId);

            DBTMTestWiseReportsListViewModel listViewModel = new DBTMTestWiseReportsListViewModel
            {
                DataTable = response.DataTable
            };
            return listViewModel;
        }
        #endregion
    }
}

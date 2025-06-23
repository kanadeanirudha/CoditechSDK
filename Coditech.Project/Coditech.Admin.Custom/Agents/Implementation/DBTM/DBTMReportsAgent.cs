using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Newtonsoft.Json;

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
        public virtual DBTMBatchWiseReportsListViewModel BatchWiseReports(int generalBatchMasterId, DateTime FromDate, DateTime ToDate)
        {
            DBTMBatchWiseReportsListResponse response = _dBTMReportsClient.BatchWiseReports(generalBatchMasterId, FromDate, ToDate);

            DBTMBatchWiseReportsListViewModel listViewModel = new DBTMBatchWiseReportsListViewModel
            {
                DataTable = response.DataTable
            };
            return listViewModel;
        }

        //Test Wise Reports 
        public virtual DBTMTestWiseReportsListViewModel TestWiseReports(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate)
        {
            DBTMCustomUserModel dBTMCustomUserModel = new DBTMCustomUserModel();
            UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);
            if (userModel?.Custom1 == CustomConstants.DBTMTrainer)
            {
                dBTMCustomUserModel = JsonConvert.DeserializeObject<DBTMCustomUserModel>(SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession).Custom3);
            }

            DBTMTestWiseReportsListResponse response = _dBTMReportsClient.TestWiseReports(dBTMTestMasterId, dBTMTraineeDetailId, FromDate, ToDate, Convert.ToInt64(dBTMCustomUserModel.GeneralTrainerMasterId));

            DBTMTestWiseReportsListViewModel listViewModel = new DBTMTestWiseReportsListViewModel
            {
                DataTable = response.DataTable
            };
            return listViewModel;
        }
        #endregion
    }
}

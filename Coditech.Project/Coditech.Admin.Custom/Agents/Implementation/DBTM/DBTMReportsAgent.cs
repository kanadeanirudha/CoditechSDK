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
        public virtual DBTMReportsListViewModel BatchWiseReports(int generalBatchMasterId, int dBTMTestMasterId, DateTime FromDate, DateTime ToDate)
        {
            DBTMReportsListViewModel listViewModel = new DBTMReportsListViewModel();
            if (generalBatchMasterId > 0 && dBTMTestMasterId > 0)
            {
                DBTMBatchWiseReportsListResponse response = _dBTMReportsClient.BatchWiseReports(generalBatchMasterId, dBTMTestMasterId, FromDate, ToDate);
                listViewModel.DataTable = response.DataTable;
            }
            return listViewModel;
        }

        //Test Wise Reports 
        public virtual DBTMReportsListViewModel TestWiseReports(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate)
        {
            DBTMReportsListViewModel listViewModel = new DBTMReportsListViewModel();
            if (dBTMTestMasterId > 0)
            {
                long generalTrainerMasterId = 0;
                UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);
                string usertype = userModel.UserType;
                if (userModel?.Custom1 == CustomConstants.DBTMTrainer)
                {
                    DBTMCustomUserModel dBTMCustomUserModel = JsonConvert.DeserializeObject<DBTMCustomUserModel>(userModel.Custom3);
                    generalTrainerMasterId = Convert.ToInt64(dBTMCustomUserModel.GeneralTrainerMasterId);
                    usertype = userModel?.Custom1;
                }
                DBTMTestWiseReportsListResponse response = _dBTMReportsClient.TestWiseReports(dBTMTestMasterId, dBTMTraineeDetailId, FromDate, ToDate, generalTrainerMasterId, usertype, userModel.SelectedCentreCode);
                listViewModel.DataTable = response.DataTable;
            }
            return listViewModel;
        }

        //Graph Reports
        public virtual GraphModel TestWiseGraphReports(int dBTMTestMasterId, long dBTMTraineeDetailId, int dBTMGraphMasterId, DateTime FromDate, DateTime ToDate)
        {
            GraphModel graphModel = new GraphModel();
            if (dBTMTestMasterId > 0)
            {
                long generalTrainerMasterId = 0;
                UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);
                string usertype = userModel.UserType;
                if (userModel?.Custom1 == CustomConstants.DBTMTrainer)
                {
                    DBTMCustomUserModel dBTMCustomUserModel = JsonConvert.DeserializeObject<DBTMCustomUserModel>(userModel.Custom3);
                    generalTrainerMasterId = Convert.ToInt64(dBTMCustomUserModel.GeneralTrainerMasterId);
                    usertype = userModel?.Custom1;
                }
                GraphResponse response = _dBTMReportsClient.TestWiseGraphReports(dBTMTestMasterId, dBTMTraineeDetailId, dBTMGraphMasterId, FromDate, ToDate, generalTrainerMasterId, usertype, userModel.SelectedCentreCode);
                graphModel = response.GraphModel;

            }
            return graphModel;
        }
        #endregion
    }
}

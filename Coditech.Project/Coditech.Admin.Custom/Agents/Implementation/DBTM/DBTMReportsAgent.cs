using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Newtonsoft.Json;
using System.Diagnostics;

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
        public virtual DBTMReportsListViewModel BatchWiseMultipleReports(string dBTMTestMasterIds, int generalBatchMasterId, DateTime FromDate, DateTime ToDate)
        {
            DBTMReportsListViewModel listViewModel = new DBTMReportsListViewModel();
            if (!string.IsNullOrEmpty(dBTMTestMasterIds))
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
                DBTMTestWiseReportsListResponse response = _dBTMReportsClient.BatchWiseMultipleReports(dBTMTestMasterIds, generalBatchMasterId, FromDate, ToDate);
                listViewModel.DataTable = response.DataTable;
                listViewModel.DataTableList = response.DataTableList;
            }
            return listViewModel;
        }

        //Batch Wise Reports File
        public virtual DBTMReportsListViewModel BatchWiseMultipleReportsFile(string dBTMTestMasterIds, int generalBatchMasterId, DateTime FromDate, DateTime ToDate, string ReportType)
        {
            DBTMReportsListViewModel listViewModel = new DBTMReportsListViewModel();
            if (!string.IsNullOrEmpty(dBTMTestMasterIds))
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
                DBTMTestWiseReportsListResponse response = _dBTMReportsClient.BatchWiseMultipleReportsFile(dBTMTestMasterIds, generalBatchMasterId, FromDate, ToDate, generalTrainerMasterId, usertype, userModel.SelectedCentreCode, ReportType);
                listViewModel.DataTable = response.DataTable;
                listViewModel.DataTableList = response.DataTableList;
                listViewModel.FilePath = response.FilePath;
                listViewModel.FileName = response.FileName;
            }
            return listViewModel;
        }

        //Test Wise Reports 
        public virtual DBTMReportsListViewModel TestWiseMultipleReports(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate)
        {
            DBTMReportsListViewModel listViewModel = new DBTMReportsListViewModel();
            if (!string.IsNullOrEmpty(dBTMTestMasterIds))
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
                DBTMTestWiseReportsListResponse response = _dBTMReportsClient.TestWiseMultipleReports(dBTMTestMasterIds, dBTMTraineeDetailId, FromDate, ToDate, generalTrainerMasterId, usertype, userModel.SelectedCentreCode);
                listViewModel.DataTable = response.DataTable;
                listViewModel.DataTableList = response.DataTableList;
            }
            return listViewModel;
        }

        //Test Wise Reports File
        public virtual DBTMReportsListViewModel TestWiseMultipleReportsFile(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate, string ReportType)
        {
            DBTMReportsListViewModel listViewModel = new DBTMReportsListViewModel();
            if (!string.IsNullOrEmpty(dBTMTestMasterIds))
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
                DBTMTestWiseReportsListResponse response = _dBTMReportsClient.TestWiseMultipleReportsFile(dBTMTestMasterIds, dBTMTraineeDetailId, FromDate, ToDate, generalTrainerMasterId, usertype, userModel.SelectedCentreCode, ReportType);
                listViewModel.DataTable = response.DataTable;
                listViewModel.DataTableList = response.DataTableList;
                listViewModel.FilePath = response.FilePath;
                listViewModel.FileName = response.FileName;
            }
            return listViewModel;
        }

        //Name Wise Reports 
        public virtual DBTMReportsListViewModel NameWiseReports(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate)
        {
            DBTMReportsListViewModel listViewModel = new DBTMReportsListViewModel();
            if (!string.IsNullOrEmpty(dBTMTestMasterIds))
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
                DBTMTestWiseReportsListResponse response = _dBTMReportsClient.NameWiseReports(dBTMTestMasterIds, dBTMTraineeDetailId, FromDate, ToDate, generalTrainerMasterId, usertype, userModel.SelectedCentreCode);
                listViewModel.DataTable = response.DataTable;
                listViewModel.DataTableList = response.DataTableList;
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

        //Delete Report .
        public virtual bool DeleteTestWiseMultipleReportsFile(string fileName, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMTestWiseReports", TraceLevel.Info);
                TrueFalseResponse response = _dBTMReportsClient.DeleteTestWiseMultipleReportsFile(new ParameterModel { Ids = fileName });
                return response?.IsSuccess ?? false;
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTestWiseReports", TraceLevel.Error);
                return false;
            }
        }
        #endregion
    }
}

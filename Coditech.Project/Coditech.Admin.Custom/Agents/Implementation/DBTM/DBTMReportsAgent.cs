using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;

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

        //Vertical Popup Details
        public virtual DBTMReportVerticalDataViewModel GetActivityVerticalDetails(long dBTMDeviceDataId, string typeOfRecord)
        {
            if (dBTMDeviceDataId <= 0)
                return new DBTMReportVerticalDataViewModel();
            try
            {
                DBTMReportVerticalDataResponse response =  _dBTMReportsClient.GetActivityVerticalDetails(dBTMDeviceDataId, typeOfRecord);
                return response?.DBTMReportVerticalDataModel.ToViewModel<DBTMReportVerticalDataViewModel>();
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage( ex, "GetActivityVerticalDetails", TraceLevel.Error);
                return new DBTMReportVerticalDataViewModel();
            }
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
        public virtual GraphModel TestWiseGraphReports(int dBTMTestMasterId, long dBTMTraineeDetailId, int dBTMGraphMasterId, string graphMode, DateTime FromDate, DateTime ToDate)
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
                GraphResponse response = _dBTMReportsClient.TestWiseGraphReports(dBTMTestMasterId, dBTMTraineeDetailId, dBTMGraphMasterId, graphMode, FromDate, ToDate, generalTrainerMasterId, usertype, userModel.SelectedCentreCode);
                graphModel = response.GraphModel;

            }
            return graphModel;
        }

        public virtual List<GraphModel> TestWiseGraphReportsV2(int dBTMTestMasterId, long dBTMTraineeDetailId, string dBTMGraphMasterIds, string graphMode, DateTime FromDate, DateTime ToDate)
        {
            List<GraphModel> graphModel = new List<GraphModel>();
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
                GraphListResponse response = _dBTMReportsClient.TestWiseGraphReportsV2(dBTMTestMasterId, dBTMTraineeDetailId, dBTMGraphMasterIds, graphMode, FromDate, ToDate, generalTrainerMasterId, usertype, userModel.SelectedCentreCode);
                graphModel = response.GraphList;

            }
            return graphModel;
        }
        public virtual List<DateTime> GetActivityPerformedDates(string dBTMTestMasterIds, long dBTMTraineeDetailId)
        {
            UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);
            string centreCode = userModel?.SelectedCentreCode;
            List<string> dateStrings = _dBTMReportsClient.GetActivityPerformedDates(dBTMTestMasterIds, dBTMTraineeDetailId, centreCode);

            if (dateStrings == null || !dateStrings.Any())
                return new List<DateTime>();

            return dateStrings.Select(d => DateTime.ParseExact(d, "yyyy-MM-dd", CultureInfo.InvariantCulture)).ToList();
        }
        public virtual List<DateTime> GetBatchActivityPerformedDates(string dBTMTestMasterIds, int generalBatchMasterId)
        {
            List<string> dateStrings = _dBTMReportsClient.GetBatchActivityPerformedDates(dBTMTestMasterIds, generalBatchMasterId);
            if (dateStrings == null || !dateStrings.Any())
                return new List<DateTime>();
            return dateStrings.Select(d => DateTime.ParseExact(d, "yyyy-MM-dd", CultureInfo.InvariantCulture)).ToList();
        }
        public virtual List<DateTime> GetTraineeListActivityDates(string dBTMTraineeDetailIds, int generalBatchMasterId)
        {
            List<string> dateStrings = _dBTMReportsClient.GetTraineeListActivityDates(dBTMTraineeDetailIds, generalBatchMasterId);
            if (dateStrings == null || !dateStrings.Any())
                return new List<DateTime>();
            return dateStrings.Select(d => DateTime.ParseExact(d, "yyyy-MM-dd", CultureInfo.InvariantCulture)).ToList();
        }
        //Delete Report .
        public virtual bool DeleteReportsFile(string fileName)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMTestWiseReports", TraceLevel.Info);
                TrueFalseResponse response = _dBTMReportsClient.DeleteReportsFile(new ParameterModel { Ids = fileName });
                return response?.IsSuccess ?? false;
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTestWiseReports", TraceLevel.Error);
                return false;
            }
        }
        public virtual DBTMTraineeProfileListViewModel GetBatchWiseTraineeProfileDetailsList(long generalBatchMasterId, string dbtmTraineeDetailIds, string orderBy, DateTime FromDate, DateTime ToDate)
        {
            DBTMTraineeProfileListResponse response = _dBTMReportsClient.GetBatchWiseTraineeProfileDetailsList(generalBatchMasterId, dbtmTraineeDetailIds, orderBy, FromDate, ToDate);
            DBTMTraineeProfileListModel dBTMTraineeProfileList = new DBTMTraineeProfileListModel { DBTMTraineeProfileList = response?.DBTMTraineeProfileList };
            DBTMTraineeProfileListViewModel listViewModel = new DBTMTraineeProfileListViewModel();
            listViewModel.DBTMTraineeProfileList = dBTMTraineeProfileList?.DBTMTraineeProfileList?.ToViewModel<DBTMTraineeProfileViewModel>().ToList();
            return listViewModel;
        }
        public virtual DBTMReportsListViewModel CampWiseMultipleReports(string dBTMTestMasterIds, int dBTMCampMasterId, DateTime FromDate, DateTime ToDate)
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
                DBTMTestWiseReportsListResponse response = _dBTMReportsClient.CampWiseMultipleReports(dBTMTestMasterIds, dBTMCampMasterId, FromDate, ToDate, generalTrainerMasterId, usertype, userModel.SelectedCentreCode);
                listViewModel.DataTable = response.DataTable;
                listViewModel.DataTableList = response.DataTableList;
            }
            return listViewModel;
        }
        public virtual DBTMReportsListViewModel CampWiseMultipleReportsFile(string dBTMTestMasterIds, int dBTMCampMasterId, DateTime FromDate, DateTime ToDate, string ReportType)
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
                DBTMTestWiseReportsListResponse response = _dBTMReportsClient.CampWiseMultipleReportsFile(dBTMTestMasterIds, dBTMCampMasterId, FromDate, ToDate, generalTrainerMasterId, usertype, userModel.SelectedCentreCode,  ReportType );
                listViewModel.DataTable = response.DataTable;
                listViewModel.DataTableList = response.DataTableList;
                listViewModel.FilePath = response.FilePath;
                listViewModel.FileName = response.FileName;
            }
            return listViewModel;
        }
        public virtual List<DateTime> GetCampActivityPerformedDates(string dBTMTestMasterIds, int dBTMCampMasterId)
        {
            List<string> dates = _dBTMReportsClient.GetCampActivityPerformedDates(dBTMTestMasterIds, dBTMCampMasterId);
            if (dates == null || !dates.Any())
                return new List<DateTime>();
            return dates.Select(d => DateTime.ParseExact(d, "yyyy-MM-dd", CultureInfo.InvariantCulture)).ToList();
        }
        #endregion
    }
}

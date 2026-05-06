using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.Admin.Agents
{
    public class DBTMBatchAgent : GeneralBatchAgent, IDBTMBatchAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IGeneralBatchClient _generalBatchClient;
        private readonly IDBTMBatchClient _dBTMBatchClient;
        #endregion

        #region Public Constructor
        public DBTMBatchAgent(ICoditechLogging coditechLogging, IGeneralBatchClient generalBatchClient, IDBTMBatchClient dBTMBatchClient, ITaskSchedulerClient taskSchedulerClient) : base(coditechLogging, generalBatchClient, taskSchedulerClient)
        {
            _coditechLogging = coditechLogging;
            _generalBatchClient = GetClient<IGeneralBatchClient>(generalBatchClient);
            _dBTMBatchClient = GetClient<IDBTMBatchClient>(dBTMBatchClient);
        }
        #endregion

        #region Public Methods
        public override GeneralBatchListViewModel GetBatchList(DataTableViewModel dataTableModel)
        {
            FilterCollection filters = null;
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters = new FilterCollection();
                filters.Add("BatchName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("BatchStartDate", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("BatchStartTime", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("BatchFrequency", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }
            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "createddate" : dataTableModel.SortByColumn, string.IsNullOrEmpty(dataTableModel.SortBy) ? "asc" : dataTableModel.SortBy);
            UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);
            long userId = 0;
            if (userModel.Custom1 == CustomConstants.DBTMTrainer)
                userId = userModel.UserMasterId;

            GeneralBatchListResponse response = _generalBatchClient.List(dataTableModel.SelectedCentreCode, userId, null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            GeneralBatchListModel generalBatchList = new GeneralBatchListModel { GeneralBatchList = response?.GeneralBatchList };
            GeneralBatchListViewModel listViewModel = new GeneralBatchListViewModel();
            listViewModel.GeneralBatchList = generalBatchList?.GeneralBatchList?.ToViewModel<GeneralBatchViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.GeneralBatchList.Count, BindColumns());
            return listViewModel;
        }
        #region GeneralBatchUserList
        public override GeneralBatchUserListViewModel GetGeneralBatchUserList( int generalBatchMasterId, string userType, DataTableViewModel dataTableModel)
        {
            FilterCollection filters = new FilterCollection();
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters.Add("FirstName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("LastName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("EmailId", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("MobileNumber", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }
            SortCollection sortlist = SortingData( dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "" : dataTableModel.SortByColumn, dataTableModel.SortBy );
            UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession); 
            if (userModel.Custom1 == CustomConstants.DBTMTrainer)
            {
                userType = CustomConstants.DBTMTrainer; 
            }
            else if (userModel.Custom1 == CustomConstants.DBTMCentreOwner)
            {
                userType = CustomConstants.DBTMCentreOwner; 
            }

            GeneralBatchUserListResponse response = _generalBatchClient.GetGeneralBatchUserList( generalBatchMasterId, userType, null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            GeneralBatchUserListModel generalBatchUserList = new GeneralBatchUserListModel{ GeneralBatchUserList = response?.GeneralBatchUserList };
            GeneralBatchUserListViewModel listViewModel = new GeneralBatchUserListViewModel();
            listViewModel.GeneralBatchUserList = generalBatchUserList?.GeneralBatchUserList?.ToViewModel<GeneralBatchUserViewModel>().ToList();
            SetListPagingData( listViewModel.PageListViewModel, response, dataTableModel, listViewModel.GeneralBatchUserList.Count, BindAssociatedBatchColumns());
            listViewModel.GeneralBatchMasterId = generalBatchMasterId;
            listViewModel.BatchName = response.BatchName;
            return listViewModel;
        }
        public virtual GeneralBatchUserListViewModel GetBatchUserListByCentreCodeAndGeneralTrainerMasterId(string selectedCentreCode, long generalTrainerMasterId, int generalBatchMasterId)
        {
            GeneralBatchUserListResponse response = _dBTMBatchClient.GetDBTMBatchUserList(selectedCentreCode, generalTrainerMasterId, generalBatchMasterId);
            GeneralBatchUserListModel generalBatchUserList = new GeneralBatchUserListModel { GeneralBatchUserList = response?.GeneralBatchUserList };
            GeneralBatchUserListViewModel listViewModel = new GeneralBatchUserListViewModel();
            listViewModel.GeneralBatchUserList = generalBatchUserList?.GeneralBatchUserList?.ToViewModel<GeneralBatchUserViewModel>().ToList();
            return listViewModel;
        }
        #endregion
        #region Calendar
        public virtual GeneralBatchListViewModel GetCalendarBatches(string centreCode, long userMasterId, DateTime startDate, DateTime endDate)
        {
            userMasterId = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession).UserMasterId;
            GeneralBatchListResponse response = _dBTMBatchClient.GetCalendarBatches(centreCode, userMasterId, startDate, endDate);
            GeneralBatchListModel generalBatchList = new GeneralBatchListModel { GeneralBatchList = response?.GeneralBatchList };
            GeneralBatchListViewModel listViewModel = new GeneralBatchListViewModel();
            listViewModel.GeneralBatchList = generalBatchList?.GeneralBatchList?.ToViewModel<GeneralBatchViewModel>().ToList();
            return listViewModel;
        }
        #endregion
        #endregion
    }
}

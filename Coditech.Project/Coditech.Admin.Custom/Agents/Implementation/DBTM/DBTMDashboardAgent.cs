using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.Admin.Agents
{
    public class DBTMDashboardAgent : BaseAgent, IDBTMDashboardAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IDBTMDashboardClient _dashboardClient;
        private readonly IUserClient _userClient;
        private readonly IGeneralBatchClient _generalBatchClient;
        #endregion

        #region Public Constructor
        public DBTMDashboardAgent(ICoditechLogging coditechLogging, IDBTMDashboardClient dashboardClient, IUserClient userClient, IGeneralBatchClient generalBatchClient)
        {
            _coditechLogging = coditechLogging;
            _dashboardClient = GetClient<IDBTMDashboardClient>(dashboardClient);
            _userClient = userClient;
            _generalBatchClient = generalBatchClient;
        }
        #endregion

        #region Public Methods

        //Get DBTM Dashboard by general selected Admin Role Master id.
        public virtual DBTMDashboardViewModel GetDBTMDashboardDetails(short numberOfDaysRecord)
        {
            int selectedAdminRoleMasterId = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession)?.SelectedAdminRoleMasterId ?? 0;
            long userMasterId = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession)?.UserMasterId ?? 0;
            DBTMDashboardViewModel dashboardViewModel = new DBTMDashboardViewModel();
            numberOfDaysRecord = numberOfDaysRecord == 0 ? CoditechAdminSettings.DefaultDashboardDataDays : numberOfDaysRecord;
            if (selectedAdminRoleMasterId > 0 && userMasterId > 0)
            {
                DBTMDashboardResponse response = _dashboardClient.GetDBTMDashboardDetails(numberOfDaysRecord, selectedAdminRoleMasterId, userMasterId);
                dashboardViewModel = response?.DBTMDashboardModel?.ToViewModel<DBTMDashboardViewModel>();
            }
            dashboardViewModel.NumberOfDaysRecord = numberOfDaysRecord;
            return dashboardViewModel;
        }

        public virtual DBTMDashboardViewModel GetTrainerDashBoard(short numberOfDaysRecord, long generalTrainerMasterId, int adminRoleMasterId, long userMasterId)
        {
            DBTMDashboardViewModel dashboardViewModel = new DBTMDashboardViewModel();
            numberOfDaysRecord = numberOfDaysRecord == 0 ? CoditechAdminSettings.DefaultDashboardDataDays : numberOfDaysRecord;
            DBTMDashboardResponse response = _dashboardClient.GetDBTMDashboardDetails(numberOfDaysRecord, adminRoleMasterId, userMasterId);
            dashboardViewModel = response?.DBTMDashboardModel?.ToViewModel<DBTMDashboardViewModel>();

            return dashboardViewModel;
        }
        public virtual UserProfileViewModel GetUserProfile(long userMasterId)
        {
            string userType = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession)?.UserType;

            UserProfileResponse response = _userClient.GetUserProfile(userMasterId, userType);
            return response?.UserProfileModel.ToViewModel<UserProfileViewModel>();
        }
        public virtual GeneralBatchListViewModel GetBatchList(DataTableViewModel dataTableModel)
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

            long userId;
            if (string.IsNullOrEmpty(dataTableModel.SelectedParameter2))
            {
                userId = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession).UserMasterId;
            }
            else
            {
                if (!long.TryParse(dataTableModel.SelectedParameter2, out userId))
                {
                    userId = SessionHelper .GetDataFromSession<UserModel>(AdminConstants.UserDataSession) .UserMasterId;
                }
            }
            SortCollection sortlist = SortingData( dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "createddate" : dataTableModel.SortByColumn, dataTableModel.SortBy = IsNotNull(dataTableModel.SortByColumn) ? "desc" : string.IsNullOrEmpty(dataTableModel.SortBy) ? "asc" : dataTableModel.SortBy);
            GeneralBatchListResponse response = _generalBatchClient.List(dataTableModel.SelectedCentreCode, userId, null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);

            GeneralBatchListModel generalBatchList = new GeneralBatchListModel{ GeneralBatchList = response?.GeneralBatchList };

            GeneralBatchListViewModel listViewModel = new GeneralBatchListViewModel{ GeneralBatchList = generalBatchList?.GeneralBatchList?.ToViewModel<GeneralBatchViewModel>().ToList() };

            return listViewModel;
        }
        #endregion
    }
}

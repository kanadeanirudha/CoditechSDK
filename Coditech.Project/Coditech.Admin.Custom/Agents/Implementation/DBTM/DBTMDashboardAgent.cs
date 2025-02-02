using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;

namespace Coditech.Admin.Agents
{
    public class DBTMDashboardAgent : BaseAgent, IDBTMDashboardAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IDBTMDashboardClient _dashboardClient;
        #endregion

        #region Public Constructor
        public DBTMDashboardAgent(ICoditechLogging coditechLogging, IDBTMDashboardClient dashboardClient)
        {
            _coditechLogging = coditechLogging;
            _dashboardClient = GetClient<IDBTMDashboardClient>(dashboardClient);
        }
        #endregion

        #region Public Methods

        //Get DBTM Dashboard by general selected Admin Role Master id.
        public virtual DBTMDashboardViewModel GetDBTMDashboardDetails()
        {
            int selectedAdminRoleMasterId = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession)?.SelectedAdminRoleMasterId ?? 0;
            long userMasterId = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession)?.UserMasterId ?? 0;
            DBTMDashboardViewModel dashboardViewModel = new DBTMDashboardViewModel();
            if (selectedAdminRoleMasterId > 0 && userMasterId > 0)
            {
                DBTMDashboardResponse response = _dashboardClient.GetDBTMDashboardDetails(selectedAdminRoleMasterId, userMasterId);
                dashboardViewModel = response?.DBTMDashboardModel?.ToViewModel<DBTMDashboardViewModel>();
            }
            return dashboardViewModel;
        }

        #endregion
    }
}

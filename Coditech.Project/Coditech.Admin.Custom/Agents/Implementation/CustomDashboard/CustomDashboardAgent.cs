using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;

namespace Coditech.Admin.Agents
{
    public class CustomDashboardAgent : BaseAgent, ICustomDashboardAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICustomDashboardClient _dashboardClient;
        #endregion

        #region Public Constructor
        public CustomDashboardAgent(ICoditechLogging coditechLogging, ICustomDashboardClient dashboardClient)
        {
            _coditechLogging = coditechLogging;
            _dashboardClient = GetClient<ICustomDashboardClient>(dashboardClient);
        }
        #endregion

        #region Public Methods

        //Get Custom Dashboard by general selected Admin Role Master id.
        public virtual CustomDashboardViewModel GetCustomDashboardDetails()
        {
            int selectedAdminRoleMasterId = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession)?.SelectedAdminRoleMasterId ?? 0;
            long userMasterId = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession)?.UserMasterId ?? 0;
            CustomDashboardViewModel dashboardViewModel = new CustomDashboardViewModel();
            if (selectedAdminRoleMasterId > 0 && userMasterId > 0)
            {
                CustomDashboardResponse response = _dashboardClient.GetCustomDashboardDetails(selectedAdminRoleMasterId, userMasterId);
                dashboardViewModel = response?.CustomDashboardModel?.ToViewModel<CustomDashboardViewModel>();
            }
            return dashboardViewModel;
        }

        #endregion
    }
}

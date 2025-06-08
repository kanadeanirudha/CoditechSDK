using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;

namespace Coditech.Admin.Agents
{
    public class LiveTestResultDashboardAgent : BaseAgent, ILiveTestResultDashboardAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ILiveTestResultDashboardClient _liveTestResultDashboardClient;
        #endregion
        #region Public Constructor
        public LiveTestResultDashboardAgent(ICoditechLogging coditechLogging, ILiveTestResultDashboardClient liveTestResultDashboardClient)
        {
            _coditechLogging = coditechLogging;
            _liveTestResultDashboardClient = GetClient<ILiveTestResultDashboardClient>(liveTestResultDashboardClient);
        }
        #endregion

        #region Public Methods

        //Get Dashboard by general selected Admin Role Master id.
        public virtual LiveTestResultDashboardViewModel GetLiveTestResultDashboard()
        {
            string selectedCentreCode = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession).SelectedCentreCode;
            long entityId = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession)?.UserMasterId ?? 0;
            LiveTestResultDashboardViewModel liveTestResultDashboardViewModel = new LiveTestResultDashboardViewModel();
            if (!string.IsNullOrEmpty(selectedCentreCode) && entityId > 0)
            {
                LiveTestResultDashboardResponse response = _liveTestResultDashboardClient.GetLiveTestResultDashboard(selectedCentreCode, entityId);
                liveTestResultDashboardViewModel = response?.LiveTestResultDashboardModel?.ToViewModel<LiveTestResultDashboardViewModel>();
            }
            return liveTestResultDashboardViewModel;
        }
        #endregion
    }
}

using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Resources;
using System.Diagnostics;
using static Coditech.Common.Helper.HelperUtility;


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
        //Get Live Test Result Dashboard
        public virtual LiveTestResultLoginViewModel GetLiveTestResultDashboard(LiveTestResultLoginViewModel liveTestResultLoginViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMTraineeAssignment", TraceLevel.Info);
                LiveTestResultLoginResponse response = _liveTestResultDashboardClient.GetLiveTestResultDashboard(liveTestResultLoginViewModel.ToModel<LiveTestResultLoginModel>());
                LiveTestResultLoginModel liveTestResultLoginModel = response?.LiveTestResultLoginModel;
                _coditechLogging.LogMessage("Agent method execution done.", "DBTMTraineeAssignment", TraceLevel.Info);
                return IsNotNull(liveTestResultLoginModel) ? liveTestResultLoginModel.ToViewModel<LiveTestResultLoginViewModel>() : (LiveTestResultLoginViewModel)GetViewModelWithErrorMessage(new LiveTestResultLoginViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "LiveTestResultLogin", TraceLevel.Error);
                return (LiveTestResultLoginViewModel)GetViewModelWithErrorMessage(liveTestResultLoginViewModel, GeneralResources.UpdateErrorMessage);
            }
        }
        #endregion
    }
}

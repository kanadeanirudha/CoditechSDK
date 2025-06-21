using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
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
        public virtual LiveTestResultDashboardViewModel GetLiveTestResultDashboard(LiveTestResultLoginViewModel liveTestResultLoginViewModel)
        {
            LiveTestResultDashboardViewModel liveTestResultDashboardViewModel = new LiveTestResultDashboardViewModel();
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMTraineeAssignment", TraceLevel.Info);
                LiveTestResultDashboardResponse response = _liveTestResultDashboardClient.GetLiveTestResultDashboard(liveTestResultLoginViewModel.ToModel<LiveTestResultLoginModel>());
                LiveTestResultDashboardModel liveTestResultDashboardModel = response?.LiveTestResultDashboardModel;
                _coditechLogging.LogMessage("Agent method execution done.", "DBTMTraineeAssignment", TraceLevel.Info);
                liveTestResultDashboardViewModel = IsNotNull(liveTestResultDashboardModel) ? liveTestResultDashboardModel.ToViewModel<LiveTestResultDashboardViewModel>() : (LiveTestResultDashboardViewModel)GetViewModelWithErrorMessage(new LiveTestResultDashboardViewModel(), GeneralResources.UpdateErrorMessage);
                return liveTestResultDashboardViewModel;
            }
            catch (CoditechException ex)
            {
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.InvalidData:
                        return (LiveTestResultDashboardViewModel)GetViewModelWithErrorMessage(liveTestResultDashboardViewModel, liveTestResultDashboardViewModel.ErrorMessage);
                    case ErrorCodes.NotFound:
                        return (LiveTestResultDashboardViewModel)GetViewModelWithErrorMessage(liveTestResultDashboardViewModel, AdminResources.ErrorMessage_ThisaccountdoesnotexistEnteravalidemailaddressorpassword);
                    case ErrorCodes.ContactAdministrator:
                        return (LiveTestResultDashboardViewModel)GetViewModelWithErrorMessage(liveTestResultDashboardViewModel, GeneralResources.ErrorMessage_PleaseContactYourAdministrator);
                    default:
                        return (LiveTestResultDashboardViewModel)GetViewModelWithErrorMessage(liveTestResultDashboardViewModel, GeneralResources.ErrorMessage_PleaseContactYourAdministrator);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "LiveTestResultDashboard", TraceLevel.Error);
                return (LiveTestResultDashboardViewModel)GetViewModelWithErrorMessage(liveTestResultDashboardViewModel, GeneralResources.ErrorMessage_PleaseContactYourAdministrator);
            }
        }
        #endregion
    }
}

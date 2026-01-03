using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Resources;
using System.Diagnostics;
namespace Coditech.Admin.Agents
{
    public class DBTMCentreWiseSettingAgent : BaseAgent, IDBTMCentreWiseSettingAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IDBTMCentreWiseSettingClient _dBTMCentreWiseSettingClient;
        #endregion

        #region Public Constructor
        public DBTMCentreWiseSettingAgent(ICoditechLogging coditechLogging, IDBTMCentreWiseSettingClient dBTMCentreWiseSettingClient, IUserClient userClient)
        {
            _coditechLogging = coditechLogging;
            _dBTMCentreWiseSettingClient = GetClient<IDBTMCentreWiseSettingClient>(dBTMCentreWiseSettingClient);
        }
        #endregion

        #region Public Methods
        #region DBTMPrivacySetting

        //Get DBTMCentreWiseSetting by  DBTMPrivacySetting id.
        public virtual DBTMCentreWiseSettingViewModel GetDBTMCentreWiseSetting(int organisationCentreId)
        {
            DBTMCentreWiseSettingResponse response = _dBTMCentreWiseSettingClient.GetDBTMCentreWiseSetting(organisationCentreId);
            return response?.DBTMCentreWiseSettingModel.ToViewModel<DBTMCentreWiseSettingViewModel>();
        }

        //Update DBTMCentreWiseSetting .
        public virtual DBTMCentreWiseSettingViewModel UpdateDBTMCentreWiseSetting(DBTMCentreWiseSettingViewModel dBTMCentreWiseSettingViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMCentreWiseSetting", TraceLevel.Info);
                DBTMCentreWiseSettingResponse response = _dBTMCentreWiseSettingClient.UpdateDBTMCentreWiseSetting(dBTMCentreWiseSettingViewModel.ToModel<DBTMCentreWiseSettingModel>());
                DBTMCentreWiseSettingModel dBTMCentreWiseSettingModel = response?.DBTMCentreWiseSettingModel;
                _coditechLogging.LogMessage("Agent method execution done.", "DBTMPrivacySetting", TraceLevel.Info);
                return HelperUtility.IsNotNull(dBTMCentreWiseSettingModel) ? dBTMCentreWiseSettingModel.ToViewModel<DBTMCentreWiseSettingViewModel>() : (DBTMCentreWiseSettingViewModel)GetViewModelWithErrorMessage(new DBTMCentreWiseSettingViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMPrivacySetting", TraceLevel.Error);
                return (DBTMCentreWiseSettingViewModel)GetViewModelWithErrorMessage(dBTMCentreWiseSettingViewModel, GeneralResources.UpdateErrorMessage);
            }
        }
        #endregion
        #endregion
    }
}

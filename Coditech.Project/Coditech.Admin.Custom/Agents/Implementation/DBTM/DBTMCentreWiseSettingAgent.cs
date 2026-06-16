using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Resources;
using System.Diagnostics;
using static Coditech.Common.Helper.HelperUtility;
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
            DBTMCentreWiseSettingViewModel dBTMCentreWiseSettingViewModel = response?.DBTMCentreWiseSettingModel?.ToViewModel<DBTMCentreWiseSettingViewModel>();
            if (dBTMCentreWiseSettingViewModel == null)
                return new DBTMCentreWiseSettingViewModel();
            if (response?.DBTMCentreWiseSettingModel?.TestListModel != null)
            {
                dBTMCentreWiseSettingViewModel.TestListViewModel = response.DBTMCentreWiseSettingModel.TestListModel.ToViewModel<DBTMCentreWiseTestListViewModel>();
            }
            else
            {
                dBTMCentreWiseSettingViewModel.TestListViewModel = new DBTMCentreWiseTestListViewModel();
            }
            return dBTMCentreWiseSettingViewModel;
        }

        //Update Associate UnAssociate CentrewiseTest.
        public virtual DBTMCentreWiseTestViewModel AssociateUnAssociateCentreTest(DBTMCentreWiseTestViewModel dBTMCentreWiseTestViewModel)
        {
            try
            {
                int organisationCentreMasterId = dBTMCentreWiseTestViewModel.OrganisationCentreMasterId;
                long dBTMCentreWiseTestId = dBTMCentreWiseTestViewModel.DBTMCentreWiseTestId;
                DBTMCentreWiseTestResponse response = _dBTMCentreWiseSettingClient.AssociateUnAssociateCentreTest(dBTMCentreWiseTestViewModel.ToModel<DBTMCentreWiseTestModel>());
                DBTMCentreWiseTestModel dBTMCentreWiseTestModel = response?.DBTMCentreWiseTestModel;
                dBTMCentreWiseTestViewModel = IsNotNull(dBTMCentreWiseTestModel) ? dBTMCentreWiseTestModel.ToViewModel<DBTMCentreWiseTestViewModel>() : new DBTMCentreWiseTestViewModel();
                dBTMCentreWiseTestViewModel.OrganisationCentreMasterId = organisationCentreMasterId;
                dBTMCentreWiseTestViewModel.DBTMCentreWiseTestId = dBTMCentreWiseTestId;
                return dBTMCentreWiseTestViewModel;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage( ex, "DBTMCentreWiseSetting", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMCentreWiseTestViewModel)GetViewModelWithErrorMessage(dBTMCentreWiseTestViewModel, ex.ErrorMessage);

                    default:
                        return (DBTMCentreWiseTestViewModel)GetViewModelWithErrorMessage(dBTMCentreWiseTestViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCentreWiseSetting", TraceLevel.Error);
                return (DBTMCentreWiseTestViewModel)GetViewModelWithErrorMessage(dBTMCentreWiseTestViewModel, GeneralResources.ErrorFailedToCreate);
            }
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

        public virtual DBTMCentreWiseTestViewModel AssociateCentreTests(int organisationCentreId, string centreCode, List<int> testIds)
        {
            try
            {
                DBTMCentreWiseTestResponse response = _dBTMCentreWiseSettingClient.AssociateCentreTests(organisationCentreId, centreCode, testIds);
                DBTMCentreWiseTestModel resultModel = response?.DBTMCentreWiseTestModel;
                return resultModel != null ? resultModel.ToViewModel<DBTMCentreWiseTestViewModel>() : new DBTMCentreWiseTestViewModel { HasError = true, ErrorMessage = "Failed to associate tests." };
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCentreWiseSetting", TraceLevel.Error);
                return new DBTMCentreWiseTestViewModel { HasError = true, ErrorMessage = ex.Message };
            }
        }

        public virtual DBTMCentreWiseTestViewModel UnAssociateCentreTests(int organisationCentreId, string centreCode, List<int> testIds)
        {
            try
            {
                DBTMCentreWiseTestResponse response = _dBTMCentreWiseSettingClient.UnAssociateCentreTests(organisationCentreId, centreCode, testIds);
                DBTMCentreWiseTestModel resultModel = response?.DBTMCentreWiseTestModel;
                return resultModel != null ? resultModel.ToViewModel<DBTMCentreWiseTestViewModel>() : new DBTMCentreWiseTestViewModel { HasError = true, ErrorMessage = "Failed to unassociate tests." };
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCentreWiseSetting", TraceLevel.Error);
                return new DBTMCentreWiseTestViewModel { HasError = true, ErrorMessage = ex.Message };
            }
        }
        #endregion
        #endregion
    }
}

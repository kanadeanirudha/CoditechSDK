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
    public class DBTMNewRegistrationAgent : BaseAgent, IDBTMNewRegistrationAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IDBTMNewRegistrationClient _dBTMNewRegistrationClient;
        #endregion

        #region Public Constructor
        public DBTMNewRegistrationAgent(ICoditechLogging coditechLogging, IDBTMNewRegistrationClient dBTMNewRegistrationClient)
        {
            _coditechLogging = coditechLogging;
            _dBTMNewRegistrationClient = GetClient<IDBTMNewRegistrationClient>(dBTMNewRegistrationClient);
        }
        #endregion

        #region Public Methods

        //Add NewRegistration.
        public virtual DBTMNewRegistrationViewModel DBTMCentreRegistration(DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel)
        {
            try
            {
                DBTMNewRegistrationResponse response = _dBTMNewRegistrationClient.DBTMCentreRegistration(dBTMNewRegistrationViewModel.ToModel<DBTMNewRegistrationModel>());
                DBTMNewRegistrationModel dBTMNewRegistrationModel = response?.DBTMNewRegistrationModel;
                return IsNotNull(dBTMNewRegistrationModel) ? dBTMNewRegistrationModel.ToViewModel<DBTMNewRegistrationViewModel>() : new DBTMNewRegistrationViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, LogComponentCustomEnum.DBTMCentreRegistration.ToString(), TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMNewRegistrationViewModel)GetViewModelWithErrorMessage(dBTMNewRegistrationViewModel, ex.ErrorMessage);
                    case ErrorCodes.InvalidData:
                        return (DBTMNewRegistrationViewModel)GetViewModelWithErrorMessage(dBTMNewRegistrationViewModel, ex.ErrorMessage);
                    default:
                        return (DBTMNewRegistrationViewModel)GetViewModelWithErrorMessage(dBTMNewRegistrationViewModel, GeneralResources.UpdateErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, LogComponentCustomEnum.DBTMCentreRegistration.ToString(), TraceLevel.Error);
                return (DBTMNewRegistrationViewModel)GetViewModelWithErrorMessage(dBTMNewRegistrationViewModel, GeneralResources.UpdateErrorMessage);
            }
        }
        #endregion
    }
}

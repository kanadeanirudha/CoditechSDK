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
        private readonly IDBTMUserClient _userClient;

        #endregion

        #region Public Constructor
        public DBTMNewRegistrationAgent(ICoditechLogging coditechLogging, IDBTMNewRegistrationClient dBTMNewRegistrationClient, IDBTMUserClient userClient)
        {
            _coditechLogging = coditechLogging;
            _dBTMNewRegistrationClient = GetClient<IDBTMNewRegistrationClient>(dBTMNewRegistrationClient);
            _userClient = GetClient<IDBTMUserClient>(userClient);
        }
        #endregion

        #region Public Methods
        //Add NewRegistration.
        public virtual DBTMNewRegistrationViewModel DBTMCentreRegistration(DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel)
        {
            try
            {
                dBTMNewRegistrationViewModel.CentreCode = "BlankData";
                dBTMNewRegistrationViewModel.TrainerSpecializationEnumId =0;
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

        //Add TrainerRegistration.
        public virtual DBTMNewRegistrationViewModel TrainerRegistration(DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel)
        {
            dBTMNewRegistrationViewModel.DeviceSerialCode ="BlankData";
            dBTMNewRegistrationViewModel.CentreName = "BlankData";
            try
            { 
                DBTMNewRegistrationResponse response = _dBTMNewRegistrationClient.TrainerRegistration(dBTMNewRegistrationViewModel.ToModel<DBTMNewRegistrationModel>());
                DBTMNewRegistrationModel dBTMNewRegistrationModel = response?.DBTMNewRegistrationModel;
                return IsNotNull(dBTMNewRegistrationModel) ? dBTMNewRegistrationModel.ToViewModel<DBTMNewRegistrationViewModel>() : new DBTMNewRegistrationViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, LogComponentCustomEnum.TrainerRegistration.ToString(), TraceLevel.Warning);
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
                _coditechLogging.LogMessage(ex, LogComponentCustomEnum.TrainerRegistration.ToString(), TraceLevel.Error);
                return (DBTMNewRegistrationViewModel)GetViewModelWithErrorMessage(dBTMNewRegistrationViewModel, GeneralResources.UpdateErrorMessage);
            }
        }

        //Add Individual Registration.
        public virtual DBTMNewRegistrationViewModel IndividualRegistration(DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel)
        {
            try
            {
                dBTMNewRegistrationViewModel.UserType = UserTypeCustomEnum.DBTMIndividualRegister.ToString();
                GeneralPersonResponse response = _userClient.IndividualRegistration(dBTMNewRegistrationViewModel.ToModel<GeneralPersonModel>());
                GeneralPersonModel dBTMNewRegistrationModel = response?.GeneralPersonModel;
                return IsNotNull(dBTMNewRegistrationModel) ? dBTMNewRegistrationModel.ToViewModel<DBTMNewRegistrationViewModel>() : new DBTMNewRegistrationViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, LogComponentCustomEnum.IndividualRegistration.ToString(), TraceLevel.Warning);
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
                _coditechLogging.LogMessage(ex, LogComponentCustomEnum.IndividualRegistration.ToString(), TraceLevel.Error);
                return (DBTMNewRegistrationViewModel)GetViewModelWithErrorMessage(dBTMNewRegistrationViewModel, GeneralResources.UpdateErrorMessage);
            }
        }

        //Add Trainee Registration.
        public virtual DBTMNewRegistrationViewModel TraineeRegistration(DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel)
        {
            try
            {
                dBTMNewRegistrationViewModel.UserType = UserTypeEnum.Trainee.ToString();
                GeneralPersonResponse response = _userClient.TraineeRegistration(dBTMNewRegistrationViewModel.ToModel<GeneralPersonModel>());
                GeneralPersonModel dBTMNewRegistrationModel = response?.GeneralPersonModel;
                return IsNotNull(dBTMNewRegistrationModel) ? dBTMNewRegistrationModel.ToViewModel<DBTMNewRegistrationViewModel>() : new DBTMNewRegistrationViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, LogComponentCustomEnum.TraineeRegistration.ToString(), TraceLevel.Warning);
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
                _coditechLogging.LogMessage(ex, LogComponentCustomEnum.TraineeRegistration.ToString(), TraceLevel.Error);
                return (DBTMNewRegistrationViewModel)GetViewModelWithErrorMessage(dBTMNewRegistrationViewModel, GeneralResources.UpdateErrorMessage);
            }
        }
        #endregion
    }
}

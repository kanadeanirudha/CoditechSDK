using System.Diagnostics;
using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Resources;
using Newtonsoft.Json;
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
                dBTMNewRegistrationViewModel.TrainerSpecializationEnumId = 0;
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
            dBTMNewRegistrationViewModel.DeviceSerialCode = "BlankData";
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

        // Add Individual Registration.
        public virtual DBTMNewRegistrationViewModel IndividualRegistration(DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel)
        {
            try
            {
                dBTMNewRegistrationViewModel.UserType = UserTypeCustomEnum.DBTMIndividualRegister.ToString();

                DBTMCustomNewRegistrationModel dBTMCustomNewRegistrationModel = new DBTMCustomNewRegistrationModel
                {
                    weight = dBTMNewRegistrationViewModel.Weight,
                    height = dBTMNewRegistrationViewModel.Height,
                    GeneralTraineeAssociatedToTrainerIds = dBTMNewRegistrationViewModel.SelectedTrainer,
                    SpecializationEnumId = dBTMNewRegistrationViewModel.SpecializationEnumId
                };

                dBTMNewRegistrationViewModel.Custom1 = JsonConvert.SerializeObject(dBTMCustomNewRegistrationModel);

                GeneralPersonResponse response = _userClient.IndividualRegistration(dBTMNewRegistrationViewModel.ToModel<GeneralPersonModel>());

                GeneralPersonModel dBTMNewRegistrationModel = response?.GeneralPersonModel;

                return IsNotNull(dBTMNewRegistrationModel) ? dBTMNewRegistrationModel.ToViewModel<DBTMNewRegistrationViewModel>()
                    : new DBTMNewRegistrationViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, LogComponentCustomEnum.IndividualRegistration.ToString(), TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
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

                DBTMCustomNewRegistrationModel dBTMCustomNewRegistrationModel = new DBTMCustomNewRegistrationModel
                {
                    weight = dBTMNewRegistrationViewModel.Weight,
                    height = dBTMNewRegistrationViewModel.Height,
                    GeneralTraineeAssociatedToTrainerIds = dBTMNewRegistrationViewModel.SelectedTrainer,
                    JoiningCode = dBTMNewRegistrationViewModel.JoiningCode,
                    SpecializationEnumId = dBTMNewRegistrationViewModel.SpecializationEnumId,
                    RegistrationType = dBTMNewRegistrationViewModel.RegistrationType,
                };

                dBTMNewRegistrationViewModel.Custom1 = JsonConvert.SerializeObject(dBTMCustomNewRegistrationModel);

                GeneralPersonResponse response = _userClient.TraineeRegistration(dBTMNewRegistrationViewModel.ToModel<GeneralPersonModel>());

                GeneralPersonModel dBTMNewRegistrationModel = response?.GeneralPersonModel;
                dBTMNewRegistrationModel.EntityId = response.GeneralPersonModel.EntityId;
                dBTMNewRegistrationModel.PersonId = response.GeneralPersonModel.PersonId;
                return IsNotNull(dBTMNewRegistrationModel) ? dBTMNewRegistrationModel.ToViewModel<DBTMNewRegistrationViewModel>() : new DBTMNewRegistrationViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, LogComponentCustomEnum.TraineeRegistration.ToString(), TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
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
        public bool ConvertCampUserToBatchUser(long dBTMTraineeDetailId, out string message)
        {
            message = string.Empty;
            try
            {
                TrueFalseResponse response = _dBTMNewRegistrationClient.ConvertCampUserToBatchUser(dBTMTraineeDetailId);
                if (response.IsSuccess)
                {
                    message = "Camp user converted to batch user successfully.";
                    return true;
                }
                else
                {
                    message = GeneralResources.UpdateErrorMessage;
                    return false;
                }
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "ConvertCampUserToBatchUser", TraceLevel.Warning);
                message = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "ConvertCampUserToBatchUser", TraceLevel.Error);
                message = GeneralResources.UpdateErrorMessage;
                return false;
            }
        }
        //GetGeneralTrainerByJoiningCode
        public virtual DBTMNewRegistrationListViewModel GetGeneralTrainerByJoiningCode(string joiningCode,long generalTrainerMasterId)
        {
            DBTMNewRegistrationListViewModel dBTMNewRegistrationViewModel = new DBTMNewRegistrationListViewModel();
            try
            {
                DBTMNewRegistrationListResponse response = _dBTMNewRegistrationClient.GetGeneralTrainerByJoiningCode(joiningCode, generalTrainerMasterId);
                DBTMNewRegistrationListModel dBTMNewRegistrationList = new DBTMNewRegistrationListModel { DBTMNewRegistrationList = response?.DBTMNewRegistrationList, SelectedTrainerId = response.SelectedTrainerId };
                DBTMNewRegistrationListViewModel listViewModel = new DBTMNewRegistrationListViewModel();
                listViewModel.JoiningCode = response.JoiningCode;
                listViewModel.SelectedTrainerId = response.SelectedTrainerId;
                listViewModel.DBTMNewRegistrationList = dBTMNewRegistrationList?.DBTMNewRegistrationList?.ToViewModel<DBTMNewRegistrationViewModel>().ToList();
                return listViewModel;
            }

            catch (CoditechException ex)
            {
                dBTMNewRegistrationViewModel.ErrorMessage = ex.Message;
                _coditechLogging.LogMessage(ex, LogComponentCustomEnum.DBTMCentreRegistration.ToString(), TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMNewRegistrationListViewModel)GetViewModelWithErrorMessage(dBTMNewRegistrationViewModel, ex.ErrorMessage);


                    case ErrorCodes.InvalidData:
                        return (DBTMNewRegistrationListViewModel)GetViewModelWithErrorMessage(dBTMNewRegistrationViewModel, ex.ErrorMessage);
                    default:
                        return (DBTMNewRegistrationListViewModel)GetViewModelWithErrorMessage(dBTMNewRegistrationViewModel, GeneralResources.ErrorCodeExists);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, LogComponentCustomEnum.DBTMCentreRegistration.ToString(), TraceLevel.Error);
                return (DBTMNewRegistrationListViewModel)GetViewModelWithErrorMessage(dBTMNewRegistrationViewModel, GeneralResources.UpdateErrorMessage);
            }
        }

        public DBTMNewRegistrationViewModel ValidateTrainerJoiningCode(string joiningCode)
        {
            DBTMNewRegistrationViewModel model = new DBTMNewRegistrationViewModel();
            try
            {
                DBTMNewRegistrationResponse response = _dBTMNewRegistrationClient.ValidateTrainerJoiningCode(joiningCode);
                if (response?.DBTMNewRegistrationModel != null)
                {
                    model = response.DBTMNewRegistrationModel.ToViewModel<DBTMNewRegistrationViewModel>();
                }
            }
            catch (CoditechException ex)
            {
                model.HasError = true;
                model.ErrorMessage = ex.Message;
            }
            return model;
        }
        public DBTMNewRegistrationViewModel ValidateTraineeJoiningCode(string joiningCode)
        {
            DBTMNewRegistrationViewModel model = new DBTMNewRegistrationViewModel();
            try
            {
                DBTMNewRegistrationResponse response = _dBTMNewRegistrationClient.ValidateTraineeJoiningCode(joiningCode);
                if (response?.DBTMNewRegistrationModel != null)
                {
                    model = response.DBTMNewRegistrationModel.ToViewModel<DBTMNewRegistrationViewModel>();
                }
            }
            catch (CoditechException ex)
            {
                model.HasError = true;
                model.ErrorMessage = ex.Message;
            }
            return model;
        }
        public virtual OrganisationCentrewiseJoiningCodeViewModel GetJoiningCode(string trainerId)
        {
            OrganisationCentrewiseJoiningCodeViewModel organisationCentrewiseJoiningCodeViewModel = new OrganisationCentrewiseJoiningCodeViewModel();
            try
            {
                OrganisationCentrewiseJoiningCodeResponse response = _dBTMNewRegistrationClient.GetJoiningCode(trainerId);
                OrganisationCentrewiseJoiningCodeModel model = response?.OrganisationCentrewiseJoiningCodeModel;
                return IsNotNull(model) ? model.ToViewModel<OrganisationCentrewiseJoiningCodeViewModel>() : new OrganisationCentrewiseJoiningCodeViewModel();
            }
            catch (CoditechException ex)
            {
                organisationCentrewiseJoiningCodeViewModel.ErrorMessage = ex.Message;
                _coditechLogging.LogMessage(ex, LogComponentCustomEnum.TraineeRegistration.ToString(), TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (OrganisationCentrewiseJoiningCodeViewModel)GetViewModelWithErrorMessage(organisationCentrewiseJoiningCodeViewModel, ex.ErrorMessage);
                    case ErrorCodes.InvalidData:
                        return (OrganisationCentrewiseJoiningCodeViewModel)GetViewModelWithErrorMessage(organisationCentrewiseJoiningCodeViewModel, ex.ErrorMessage);
                    default:
                        return (OrganisationCentrewiseJoiningCodeViewModel)GetViewModelWithErrorMessage(organisationCentrewiseJoiningCodeViewModel, GeneralResources.ErrorCodeExists);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, LogComponentCustomEnum.TraineeRegistration.ToString(), TraceLevel.Error);
                return (OrganisationCentrewiseJoiningCodeViewModel)GetViewModelWithErrorMessage(organisationCentrewiseJoiningCodeViewModel, GeneralResources.UpdateErrorMessage);
            }
        }
        #endregion
    }
}

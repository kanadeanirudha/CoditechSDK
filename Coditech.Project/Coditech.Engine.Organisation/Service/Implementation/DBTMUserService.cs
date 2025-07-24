using Coditech.API.Data;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Resources;
using Newtonsoft.Json;
using System.Diagnostics;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class DBTMUserService : UserService, IDBTMUserService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMTraineeDetails> _dBTMTraineeDetailsRepository;
        private readonly ICoditechRepository<UserMaster> _userMasterRepository;
        private readonly ICoditechRepository<GeneralTrainerMaster> _generalTrainerMasterRepository;
        private readonly ICoditechRepository<GeneralTraineeAssociatedToTrainer> _generalTraineeAssociatedToTrainerRepository;
        protected readonly ICoditechRepository<OrganisationCentrewiseJoiningCode> _organisationCentrewiseJoiningCodeRepository;
        protected readonly ICoditechRepository<DBTMDeviceMaster> _dBTMDeviceMasterRepository;
        protected readonly ICoditechRepository<DBTMDeviceRegistrationDetails> _dBTMDeviceRegistrationDetailsRepository;
        protected readonly ICoditechRepository<DBTMSubscriptionPlan> _dBTMSubscriptionPlanRepository;
        protected readonly ICoditechRepository<DBTMSubscriptionPlanAssociatedToUser> _dBTMSubscriptionPlanAssociatedToUserRepository;


        public DBTMUserService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider, ICoditechEmail coditechEmail, ICoditechSMS coditechSMS, ICoditechWhatsApp coditechWhatsApp) : base(coditechLogging, serviceProvider, coditechEmail, coditechSMS, coditechWhatsApp)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMTraineeDetailsRepository = new CoditechRepository<DBTMTraineeDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _userMasterRepository = new CoditechRepository<UserMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _generalTrainerMasterRepository = new CoditechRepository<GeneralTrainerMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _generalTraineeAssociatedToTrainerRepository = new CoditechRepository<GeneralTraineeAssociatedToTrainer>(_serviceProvider.GetService<Coditech_Entities>());
            _dBTMDeviceMasterRepository = new CoditechRepository<DBTMDeviceMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMDeviceRegistrationDetailsRepository = new CoditechRepository<DBTMDeviceRegistrationDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMSubscriptionPlanRepository = new CoditechRepository<DBTMSubscriptionPlan>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMSubscriptionPlanAssociatedToUserRepository = new CoditechRepository<DBTMSubscriptionPlanAssociatedToUser>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _organisationCentrewiseJoiningCodeRepository = new CoditechRepository<OrganisationCentrewiseJoiningCode>(_serviceProvider.GetService<Coditech_Entities>());
        }

        public override UserModel Login(UserLoginModel userLoginModel)
        {
            UserModel model = base.Login(userLoginModel);

            if (!model.HasError && model.UserType != UserTypeEnum.Admin.ToString())
            {
                GeneralPersonModel generalPersonModel = GetGeneralPersonDetailsByEntityType(model.EntityId, model.UserType);
                if (!string.IsNullOrEmpty(generalPersonModel.Custom1))
                {
                    model.Custom1 = generalPersonModel.Custom1;
                }
                if (model.Custom1 == CustomConstants.DBTMTrainer)
                {
                    DBTMCustomUserModel dBTMCustomUserModel = new DBTMCustomUserModel();
                    dBTMCustomUserModel.GeneralTrainerMasterId = _generalTrainerMasterRepository.Table.Where(x => x.EmployeeId == model.EntityId)?.Select(y => y.GeneralTrainerMasterId)?.FirstOrDefault();
                    model.Custom3 = JsonConvert.SerializeObject(dBTMCustomUserModel);
                }
            }
            return model;
        }

        protected override GeneralPersonModel GetGeneralPersonDetailsByEntityType(long entityId, string entityType)
        {
            long personId = 0;
            string centreCode = string.Empty;
            string personCode = string.Empty;
            short generalDepartmentMasterId = 0;
            if (entityType == UserTypeEnum.Trainee.ToString())
            {
                DBTMTraineeDetails dbtmTraineeDetails = new CoditechRepository<DBTMTraineeDetails>(_serviceProvider.GetService<CoditechCustom_Entities>()).Table.FirstOrDefault(x => x.DBTMTraineeDetailId == entityId);
                if (IsNotNull(dbtmTraineeDetails))
                {
                    personId = dbtmTraineeDetails.PersonId;
                    centreCode = dbtmTraineeDetails.CentreCode;
                }
                return base.BindGeneralPersonInformation(personId, centreCode, personCode, generalDepartmentMasterId, dbtmTraineeDetails.IsActive);
            }
            else
            {
                return base.GetGeneralPersonDetailsByEntityType(entityId, entityType);
            }
        }

        protected override void InsertPersonDetails(GeneralPersonModel generalPersonModel, List<GeneralSystemGlobleSettingModel> settingMasterList, string customData = null)
        {
            if (generalPersonModel.UserType.Equals(UserTypeEnum.Trainee.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                InsertDBTMTraineeDetails(generalPersonModel, settingMasterList, customData);
            }
            else
            {
                base.InsertPersonDetails(generalPersonModel, settingMasterList);
            }
        }
        protected override bool ValidateUserwiseGeneralPerson(GeneralPersonModel generalPersonModel, ref string errorMessage, ref int generalEnumaratorId)
        {
            if (generalPersonModel.UserType.Equals(UserTypeEnum.Trainee.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                if (string.IsNullOrEmpty(generalPersonModel.SelectedCentreCode))
                {
                    errorMessage = "SelectedCentreCode is null";
                    return false;
                }
                generalEnumaratorId = GetEnumIdByEnumCode(GeneralRunningNumberForCustomEnum.DBTMTraineeRegistration.ToString(), GeneralEnumaratorGroupCodeEnum.GeneralRunningNumberFor.ToString());
                if (generalEnumaratorId == 0)
                {
                    errorMessage = "DBTMTraineeRegistration is null";
                    return false;
                }
                return true;
            }
            else
            {
                return base.ValidateUserwiseGeneralPerson(generalPersonModel, ref errorMessage, ref generalEnumaratorId);
            }
        }
        private void InsertDBTMTraineeDetails(GeneralPersonModel generalPersonModel, List<GeneralSystemGlobleSettingModel> settingMasterList, string customData = null)
        {
            DBTMCustomNewRegistrationModel dBTMCustomNewRegistrationModel = !string.IsNullOrEmpty(customData) ? JsonConvert.DeserializeObject<DBTMCustomNewRegistrationModel>(customData) : new DBTMCustomNewRegistrationModel();
            generalPersonModel.PersonCode = GenerateRegistrationCode(GeneralRunningNumberForCustomEnum.DBTMTraineeRegistration.ToString(), generalPersonModel.SelectedCentreCode);
            DBTMTraineeDetails dBTMTraineeDetails = new DBTMTraineeDetails()
            {
                CentreCode = generalPersonModel.SelectedCentreCode,
                PersonId = generalPersonModel.PersonId,
                PersonCode = generalPersonModel.PersonCode,
                UserType = generalPersonModel.UserType,
                Height = dBTMCustomNewRegistrationModel.height,
                Weight = dBTMCustomNewRegistrationModel.weight,
                IsActive = true,
                SpecializationEnumId = dBTMCustomNewRegistrationModel.SpecializationEnumId
            };
            dBTMTraineeDetails = _dBTMTraineeDetailsRepository.Insert(dBTMTraineeDetails);

            //Check Is DBTM Trainee need to Login
            if (dBTMTraineeDetails?.DBTMTraineeDetailId > 0 && settingMasterList?.FirstOrDefault(x => x.FeatureName.Equals(GeneralSystemGlobleSettingCustomEnum.IsDBTMTraineeLogin.ToString(), StringComparison.InvariantCultureIgnoreCase)).FeatureValue == "1")
            {
                generalPersonModel.EntityId = dBTMTraineeDetails.DBTMTraineeDetailId;
                InsertUserMasterDetails(generalPersonModel, dBTMTraineeDetails.DBTMTraineeDetailId, false);
                try
                {
                    GeneralEmailTemplateModel emailTemplateModel = GetEmailTemplateByCode(generalPersonModel.SelectedCentreCode, EmailTemplateCodeCustomEnum.DBTMTraineeRegistration.ToString());
                    if (IsNotNull(emailTemplateModel) && !string.IsNullOrEmpty(emailTemplateModel?.EmailTemplateCode) && !string.IsNullOrEmpty(generalPersonModel?.EmailId))
                    {
                        string subject = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.CentreName, !string.IsNullOrEmpty(generalPersonModel.CentreName) ? generalPersonModel.CentreName : GetOrganisationCentreNameByCentreCode(generalPersonModel.SelectedCentreCode), emailTemplateModel.Subject);
                        string messageText = ReplaceDBTMTraineeEmailTemplate(generalPersonModel, emailTemplateModel.EmailTemplate);
                        _coditechEmail.SendEmail(generalPersonModel.SelectedCentreCode, generalPersonModel.EmailId, "", subject, messageText, true);
                    }
                }
                catch (Exception ex)
                {
                    _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.TraineeDetails.ToString(), TraceLevel.Error);
                }
            }
        }

        private string ReplaceDBTMTraineeEmailTemplate(GeneralPersonModel generalPersonModel, string emailTemplate)
        {
            string messageText = emailTemplate;
            messageText = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.FirstName, generalPersonModel.FirstName, messageText);
            messageText = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.LastName, generalPersonModel.LastName, messageText);
            return ReplaceEmailTemplateFooter(generalPersonModel.SelectedCentreCode, messageText);
        }

        #region DBTMRegisterTrainee

        public GeneralPersonModel DBTMRegisterTrainee(GeneralPersonModel generalPersonModel)
        {
            OrganisationCentrewiseJoiningCode joiningCodeDetails = null;
            string userType = generalPersonModel.UserType;
            DBTMDeviceMaster dBTMDeviceMaster = null;
            string customerData = generalPersonModel.Custom1;
            DBTMCustomNewRegistrationModel dBTMCustomNewRegistrationModel = JsonConvert.DeserializeObject<DBTMCustomNewRegistrationModel>(customerData);
            generalPersonModel.Custom1 = null;
            if (userType.Equals(UserTypeEnum.Trainee.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                joiningCodeDetails = _organisationCentrewiseJoiningCodeRepository.Table.Where(x => x.JoiningCode == dBTMCustomNewRegistrationModel.JoiningCode)?.FirstOrDefault();

                if (IsNull(joiningCodeDetails))
                    throw new CoditechException(ErrorCodes.AlreadyExist, string.Format("Invalid Joning Code."));

                if (joiningCodeDetails.IsExpired)
                    throw new CoditechException(ErrorCodes.InvalidData, "Joining Code has expired.");

                generalPersonModel.SelectedCentreCode = joiningCodeDetails.CentreCode;
            }
            else if (userType.Equals(UserTypeCustomEnum.DBTMIndividualRegister.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                dBTMDeviceMaster = GetDBTMDeviceMasterDetailsByCode(generalPersonModel.Custom2);

                if (dBTMDeviceMaster == null || dBTMDeviceMaster.DBTMDeviceMasterId <= 0)
                    throw new CoditechException(ErrorCodes.InvalidData, string.Format("Invalid Device Serial Code."));

                if (IsDeviceSerialCodeAlreadyExist(dBTMDeviceMaster.DBTMDeviceMasterId))
                    throw new CoditechException(ErrorCodes.AlreadyExist, string.Format(GeneralResources.ErrorCodeExists, "Device Already Added"));

                generalPersonModel.SelectedCentreCode = ApiCustomSettings.DBTMIndividualCentre;
            }


            generalPersonModel.UserType = UserTypeEnum.Trainee.ToString();
            if (string.IsNullOrWhiteSpace(generalPersonModel.Custom2))
            {
                generalPersonModel.Custom2 = $"{generalPersonModel.FirstName.ToFirstLetterCapital()} {generalPersonModel.LastName.ToFirstLetterCapital()}";
            }

            generalPersonModel = base.InsertPersonInformation(generalPersonModel, customerData);

            if (!generalPersonModel.HasError)
            {
                if (userType.Equals(UserTypeEnum.Trainee.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    joiningCodeDetails.IsExpired = true;
                    _organisationCentrewiseJoiningCodeRepository.Update(joiningCodeDetails);
                }
                else if (userType.Equals(UserTypeCustomEnum.DBTMIndividualRegister.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    int subscriptionPlanTypeEnumId = GetEnumIdByEnumCode("DBTMDeviceRegistrationPlan", DropdownCustomTypeEnum.DBTMSubscriptionPlanType.ToString());

                    DBTMSubscriptionPlan dBTMSubscriptionPlan = _dBTMSubscriptionPlanRepository.Table.Where(x => x.SubscriptionPlanTypeEnumId == subscriptionPlanTypeEnumId && x.IsActive)?.FirstOrDefault();

                    if (IsNull(dBTMSubscriptionPlan))
                        throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ErrorMessage_PleaseContactYourAdministrator);

                    DBTMDeviceRegistrationDetails dBTMDeviceRegistrationDetails = new DBTMDeviceRegistrationDetails()
                    {
                        DBTMDeviceMasterId = dBTMDeviceMaster.DBTMDeviceMasterId,
                        EntityId = generalPersonModel.EntityId,
                        UserType = generalPersonModel.UserType,
                        PurchaseDate = DateTime.Now,
                        WarrantyExpirationDate = DateTime.Now.AddMonths(dBTMDeviceMaster.WarrantyExpirationPeriodInMonth),
                    };

                    //Create new DBTMDeviceRegistrationDetails and return it.
                    DBTMDeviceRegistrationDetails dBTMDeviceRegistrationDetailsData = _dBTMDeviceRegistrationDetailsRepository.Insert(dBTMDeviceRegistrationDetails);
                    if (dBTMDeviceRegistrationDetailsData?.DBTMDeviceRegistrationDetailId > 0)
                    {
                        DBTMSubscriptionPlanAssociatedToUser dBTMSubscriptionPlanAssociatedToUser = new DBTMSubscriptionPlanAssociatedToUser()
                        {
                            DBTMSubscriptionPlanId = dBTMSubscriptionPlan.DBTMSubscriptionPlanId,
                            UserType = UserTypeEnum.Trainee.ToString(),
                            EntityId = generalPersonModel.EntityId,
                            DBTMDeviceMasterId = dBTMDeviceRegistrationDetails.DBTMDeviceMasterId,
                            DurationInDays = dBTMSubscriptionPlan.DurationInDays,
                            PlanCost = dBTMSubscriptionPlan.PlanCost,
                            PlanDiscount = dBTMSubscriptionPlan.PlanDiscount,
                            IsExpired = false,
                            PlanDurationExpirationDate = DateTime.Now.AddMonths(dBTMDeviceMaster.WarrantyExpirationPeriodInMonth),
                            SalesInvoiceMasterId = 0,
                        };

                        dBTMSubscriptionPlanAssociatedToUser = _dBTMSubscriptionPlanAssociatedToUserRepository.Insert(dBTMSubscriptionPlanAssociatedToUser);
                    }
                }
                List<GeneralTraineeAssociatedToTrainer> generalTraineeAssociatedToTrainerList = null;
                if (dBTMCustomNewRegistrationModel?.GeneralTraineeAssociatedToTrainerIds?.Count > 0)
                {
                    generalTraineeAssociatedToTrainerList = new List<GeneralTraineeAssociatedToTrainer>();
                    foreach (string generalTrainerMasterId in dBTMCustomNewRegistrationModel.GeneralTraineeAssociatedToTrainerIds)
                    {
                        generalTraineeAssociatedToTrainerList.Add(new GeneralTraineeAssociatedToTrainer
                        {
                            GeneralTrainerMasterId = Convert.ToInt64(generalTrainerMasterId),
                            EntityId = generalPersonModel.EntityId,
                            UserType = UserTypeEnum.Trainee.ToString(),
                            IsCurrentTrainer = true
                        });
                    }
                    _generalTraineeAssociatedToTrainerRepository.Insert(generalTraineeAssociatedToTrainerList);
                }

            }
            return generalPersonModel;
        }

        public DBTMDeviceMaster GetDBTMDeviceMasterDetailsByCode(string deviceSerialCode)
      => _dBTMDeviceMasterRepository.Table.Where(x => x.DeviceSerialCode == deviceSerialCode && x.IsActive).FirstOrDefault();

        private bool IsDeviceSerialCodeAlreadyExist(long dBTMDeviceMasterId)
        {
            return _dBTMDeviceRegistrationDetailsRepository.Table.Any(x => x.DBTMDeviceMasterId == dBTMDeviceMasterId);
        }

        private List<GeneralRunningNumbers> GetGeneralRunningNumbersList(string centreCode)
        {
            List<string> runningNumnereList = ("EmployeeRegistration,DBTMTraineeRegistration").Split(",").ToList();
            List<int> generalEnumaratorIdList = new CoditechRepository<GeneralEnumaratorMaster>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => runningNumnereList.Contains(x.EnumName))?.Select(x => x.GeneralEnumaratorId)?.ToList();
            List<GeneralRunningNumbers> generalRunningNumbersList = new CoditechRepository<GeneralRunningNumbers>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => x.CentreCode == centreCode && generalEnumaratorIdList.Contains(x.KeyFieldEnumId))?.ToList();
            return generalRunningNumbersList;
        }
        protected override void UpdateIsActiveFlagForUserType(GeneralPersonModel generalPersonModel)
        {
            if (generalPersonModel.UserType.Equals(UserTypeEnum.Trainee.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                DBTMTraineeDetails dBTMTraineeDetails = _dBTMTraineeDetailsRepository.Table.Where(x => x.DBTMTraineeDetailId == generalPersonModel.EntityId)?.FirstOrDefault();

                if (dBTMTraineeDetails != null)
                {
                    dBTMTraineeDetails.IsActive = generalPersonModel.IsActive;
                    _dBTMTraineeDetailsRepository.Update(dBTMTraineeDetails);
                }
            }
            else
            {
                base.UpdateIsActiveFlagForUserType(generalPersonModel);
            }
        }
        #endregion
    }
}

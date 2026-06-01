using ClosedXML.Excel;
using Coditech.API.Data;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Resources;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using static Coditech.Common.Helper.HelperUtility;
using static Coditech.Common.Helper.Utilities.CustomConstants;
namespace Coditech.API.Service
{
    public class DBTMUserService : UserService, IDBTMUserService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IGeneralTemplateService _generalTemplateService;
        private readonly ICoditechRepository<DBTMTraineeDetails> _dBTMTraineeDetailsRepository;
        private readonly ICoditechRepository<UserMaster> _userMasterRepository;
        private readonly ICoditechRepository<GeneralBatchUser> _generalBatchUserRepository;
        private readonly ICoditechRepository<GeneralTrainerMaster> _generalTrainerMasterRepository;
        private readonly ICoditechRepository<GeneralTraineeAssociatedToTrainer> _generalTraineeAssociatedToTrainerRepository;
        protected readonly ICoditechRepository<OrganisationCentrewiseJoiningCode> _organisationCentrewiseJoiningCodeRepository;
        protected readonly ICoditechRepository<DBTMDeviceMaster> _dBTMDeviceMasterRepository;
        protected readonly ICoditechRepository<DBTMDeviceRegistrationDetails> _dBTMDeviceRegistrationDetailsRepository;
        protected readonly ICoditechRepository<DBTMSubscriptionPlan> _dBTMSubscriptionPlanRepository;
        protected readonly ICoditechRepository<DBTMSubscriptionPlanAssociatedToUser> _dBTMSubscriptionPlanAssociatedToUserRepository;
        private readonly IDBTMOrganisationCentrewiseJoiningCodeService _joiningCodeService;
        private readonly ICoditechRepository<GeneralTemplateHeaderConfiguration> _generalTemplateHeaderConfigurationRepository;
        private readonly ICoditechRepository<GeneralBatchMaster> _generalBatchRepository;
        private readonly ICoditechRepository<DBTMCampUser> _dBTMCampUserRepository;
        private readonly ICoditechRepository<GeneralCountryMaster> _generalCountryRepository;
        protected readonly ICoditechRepository<DBTMCampUser> _dbtmCampUserRepository;
        protected readonly ICoditechRepository<DBTMCentreWiseSetting> _dBTMCentreWiseSettingRepository;
        private readonly ICoditechRepository<GeneralEnumaratorMaster> _generalEnumaratorMasterRepository;
        private readonly ICoditechRepository<GeneralEnumaratorGroup> _generalEnumaratorGroupRepository;

        public DBTMUserService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider, ICoditechEmail coditechEmail, ICoditechSMS coditechSMS, ICoditechWhatsApp coditechWhatsApp, IGeneralTemplateService generalTemplateService, IDBTMOrganisationCentrewiseJoiningCodeService joiningCodeService) : base(coditechLogging, serviceProvider, coditechEmail, coditechSMS, coditechWhatsApp)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _generalTemplateService = generalTemplateService;
            _joiningCodeService = joiningCodeService;
            _dBTMTraineeDetailsRepository = new CoditechRepository<DBTMTraineeDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _userMasterRepository = new CoditechRepository<UserMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _generalBatchUserRepository = new CoditechRepository<GeneralBatchUser>(_serviceProvider.GetService<Coditech_Entities>());
            _generalTrainerMasterRepository = new CoditechRepository<GeneralTrainerMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _generalTraineeAssociatedToTrainerRepository = new CoditechRepository<GeneralTraineeAssociatedToTrainer>(_serviceProvider.GetService<Coditech_Entities>());
            _dBTMDeviceMasterRepository = new CoditechRepository<DBTMDeviceMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMDeviceRegistrationDetailsRepository = new CoditechRepository<DBTMDeviceRegistrationDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMSubscriptionPlanRepository = new CoditechRepository<DBTMSubscriptionPlan>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMSubscriptionPlanAssociatedToUserRepository = new CoditechRepository<DBTMSubscriptionPlanAssociatedToUser>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _organisationCentrewiseJoiningCodeRepository = new CoditechRepository<OrganisationCentrewiseJoiningCode>(_serviceProvider.GetService<Coditech_Entities>());
            _generalTemplateHeaderConfigurationRepository = new CoditechRepository<GeneralTemplateHeaderConfiguration>(_serviceProvider.GetService<Coditech_Entities>());
            _generalBatchRepository = new CoditechRepository<GeneralBatchMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _dBTMCampUserRepository = new CoditechRepository<DBTMCampUser>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _generalCountryRepository = new CoditechRepository<GeneralCountryMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _dbtmCampUserRepository = new CoditechRepository<DBTMCampUser>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMCentreWiseSettingRepository = new CoditechRepository<DBTMCentreWiseSetting>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _generalEnumaratorMasterRepository = new CoditechRepository<GeneralEnumaratorMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _generalEnumaratorGroupRepository = new CoditechRepository<GeneralEnumaratorGroup>(_serviceProvider.GetService<Coditech_Entities>());
        }

        //public override UserModel Login(UserLoginModel userLoginModel)
        //{
        //    UserModel model = base.Login(userLoginModel);

        //    if (!model.HasError && model.UserType != UserTypeEnum.Admin.ToString())
        //    {
        //        GeneralPersonModel generalPersonModel = GetGeneralPersonDetailsByEntityType(model.EntityId, model.UserType);
        //        if (!string.IsNullOrEmpty(generalPersonModel.Custom1))
        //        {
        //            model.Custom1 = generalPersonModel.Custom1;
        //        }
        //        if (model.Custom1 == CustomConstants.DBTMTrainer || model.Custom1 == CustomConstants.DBTMCentreOwner)
        //        {
        //            DBTMCustomUserModel dBTMCustomUserModel = new DBTMCustomUserModel();
        //            dBTMCustomUserModel.GeneralTrainerMasterId = _generalTrainerMasterRepository.Table.Where(x => x.EmployeeId == model.EntityId)?.Select(y => y.GeneralTrainerMasterId)?.FirstOrDefault();
        //            model.Custom3 = JsonConvert.SerializeObject(dBTMCustomUserModel);
        //        }
        //    }
        //    return model;
        //}

        public override ChangePasswordModel ChangePassword(ChangePasswordModel changePasswordModel)
        {
            if (IsNull(changePasswordModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);
            if (string.IsNullOrEmpty(changePasswordModel.UserType))
                throw new CoditechException(ErrorCodes.IdLessThanOne, "UserType is null.");

            changePasswordModel.UserType = changePasswordModel.UserType == "DBTMTrainer" ? UserTypeEnum.Employee.ToString() : changePasswordModel.UserType;
            return base.ChangePassword(changePasswordModel);
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
        public override bool UpdatePersonInformation(GeneralPersonModel generalPersonModel)
        {
            bool isUpdated = base.UpdatePersonInformation(generalPersonModel);
            if (isUpdated && generalPersonModel.UserType == UserTypeEnum.Trainee.ToString())
            {
                DBTMTraineeDetails traineeDetails = _dBTMTraineeDetailsRepository.Table.FirstOrDefault(x => x.PersonId == generalPersonModel.PersonId);
                if (traineeDetails != null)
                {
                    int calculatedAgeGroupEnumId = GetAgeGroupEnumIdByDOB(generalPersonModel.DateOfBirth);
                    traineeDetails.AgeGroupEnumId = calculatedAgeGroupEnumId;
                    _dBTMTraineeDetailsRepository.Update(traineeDetails);
                }
            }
            return isUpdated;
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
        protected override List<string> BindAssociatedMenuToUser(UserModel userModel)
        {
            if (userModel.UserType != UserTypeEnum.Admin.ToString())
            {
                GeneralPersonModel generalPersonModel = GetGeneralPersonDetailsByEntityType(userModel.EntityId, userModel.UserType);
                if (IsNull(generalPersonModel) || string.IsNullOrEmpty(generalPersonModel.Custom1))
                {
                    return base.BindAssociatedMenuToUser(userModel);
                }

                if (!string.IsNullOrEmpty(generalPersonModel.Custom1))
                {
                    userModel.Custom1 = generalPersonModel.Custom1;
                }
                if (userModel.Custom1 == CustomConstants.DBTMTrainer || userModel.Custom1 == CustomConstants.DBTMCentreOwner)
                {
                    DBTMCustomUserModel dBTMCustomUserModel = new DBTMCustomUserModel();
                    dBTMCustomUserModel.GeneralTrainerMasterId = _generalTrainerMasterRepository.Table.Where(x => x.EmployeeId == userModel.EntityId)?.Select(y => y.GeneralTrainerMasterId)?.FirstOrDefault();
                    userModel.Custom3 = JsonConvert.SerializeObject(dBTMCustomUserModel);
                    if (userModel.Custom1.Equals(CustomConstants.DBTMTrainer, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return ApiCustomSettings.DBTMTrainerMenuCode.Split(",").ToList();
                    }
                    else if (userModel.Custom1.Equals(CustomConstants.DBTMCentreOwner, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return ApiCustomSettings.DBTMDirectorMenuCode.Split(",").ToList();
                    }
                }
                else
                    return base.BindAssociatedMenuToUser(userModel);
            }
            return base.BindAssociatedMenuToUser(userModel);
        }
        private void InsertDBTMTraineeDetails(GeneralPersonModel generalPersonModel, List<GeneralSystemGlobleSettingModel> settingMasterList, string customData = null)
        {
            DBTMCustomNewRegistrationModel dBTMCustomNewRegistrationModel = !string.IsNullOrEmpty(customData) ? JsonConvert.DeserializeObject<DBTMCustomNewRegistrationModel>(customData) : new DBTMCustomNewRegistrationModel();
            generalPersonModel.PersonCode = GenerateRegistrationCode(GeneralRunningNumberForCustomEnum.DBTMTraineeRegistration.ToString(), generalPersonModel.SelectedCentreCode);       
            if (dBTMCustomNewRegistrationModel.AgeGroupEnumId <= 0)
            {
                dBTMCustomNewRegistrationModel.AgeGroupEnumId = GetAgeGroupEnumIdByDOB(generalPersonModel.DateOfBirth);
            }
            DBTMTraineeDetails dBTMTraineeDetails = new DBTMTraineeDetails()
            {
                CentreCode = generalPersonModel.SelectedCentreCode,
                PersonId = generalPersonModel.PersonId,
                PersonCode = generalPersonModel.PersonCode,
                UserType = generalPersonModel.UserType,
                Height = dBTMCustomNewRegistrationModel.height,
                Weight = dBTMCustomNewRegistrationModel.weight,
                SchoolName = dBTMCustomNewRegistrationModel.SchoolName,
                AgeGroupEnumId = dBTMCustomNewRegistrationModel.AgeGroupEnumId,
                IsActive = true,
                SpecializationEnumId = dBTMCustomNewRegistrationModel.SpecializationEnumId
            };
            dBTMTraineeDetails = _dBTMTraineeDetailsRepository.Insert(dBTMTraineeDetails);

            //Check Is DBTM Trainee need to Login
            if (dBTMTraineeDetails?.DBTMTraineeDetailId > 0 && settingMasterList?.FirstOrDefault(x => x.FeatureName.Equals(GeneralSystemGlobleSettingCustomEnum.IsDBTMTraineeLogin.ToString(), StringComparison.InvariantCultureIgnoreCase)).FeatureValue == "1")
            {
                generalPersonModel.EntityId = dBTMTraineeDetails.DBTMTraineeDetailId;
                InsertUserMasterDetails(generalPersonModel, dBTMTraineeDetails.DBTMTraineeDetailId, true);
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
                int traineeEnumId = GetEnumIdByEnumCode("Trainee", "OrganisationJoiningCodeType");
                joiningCodeDetails = _organisationCentrewiseJoiningCodeRepository.Table.FirstOrDefault(x => x.JoiningCode == dBTMCustomNewRegistrationModel.JoiningCode && x.JoiningCodeTypeEnumId == traineeEnumId);
                if (IsNull(joiningCodeDetails))
                    throw new CoditechException(ErrorCodes.AlreadyExist, string.Format("Invalid Trainee Joining Code."));
                if (joiningCodeDetails.IsExpired)
                    throw new CoditechException(ErrorCodes.InvalidData, "Joining Code has expired.");
                ValidateCentreUserLimit(joiningCodeDetails.CentreCode, dBTMCustomNewRegistrationModel?.RegistrationType);
                if (dBTMCustomNewRegistrationModel.GeneralBatchMasterId == 0 && !string.IsNullOrEmpty(joiningCodeDetails.Custom3))
                {
                    dBTMCustomNewRegistrationModel.GeneralBatchMasterId = Convert.ToInt32(joiningCodeDetails.Custom3);
                }
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
                    string registrationType = dBTMCustomNewRegistrationModel?.RegistrationType;
                    joiningCodeDetails.IsExpired = true;
                    joiningCodeDetails.IsInQueue = false;
                    joiningCodeDetails.QueueValidTill = null;
                    joiningCodeDetails.Custom2 = registrationType;
                    _organisationCentrewiseJoiningCodeRepository.Update(joiningCodeDetails);
                    if (registrationType.Equals("Batch", StringComparison.InvariantCultureIgnoreCase))
                    {
                        GeneralBatchUser generalBatchUser = new GeneralBatchUser()
                        {
                            GeneralBatchMasterId = dBTMCustomNewRegistrationModel.GeneralBatchMasterId,
                            UserType = UserTypeEnum.Trainee.ToString(),
                            EntityId = generalPersonModel.EntityId,
                        };
                        _generalBatchUserRepository.Insert(generalBatchUser);
                        DBTMTraineeDetails trainee = _dBTMTraineeDetailsRepository.Table.FirstOrDefault(x => x.DBTMTraineeDetailId == generalPersonModel.EntityId);
                        if (trainee != null)
                        {
                            trainee.IsBatchUser = true;
                            trainee.IsCampUser = false;
                            _dBTMTraineeDetailsRepository.Update(trainee);
                        }
                    }
                    else if (registrationType.Equals("Camp", StringComparison.InvariantCultureIgnoreCase))
                    {
                        DBTMCampUser campUser = new DBTMCampUser()
                        {
                            DBTMCampMasterId = dBTMCustomNewRegistrationModel.DBTMCampMasterId,
                            EntityId = generalPersonModel.EntityId,
                            ActivityStatusEnumId = GetEnumIdByEnumCode("Pending", "DBTMTestStatus"),
                            UserType = UserTypeEnum.Trainee.ToString()
                        };
                        _dBTMCampUserRepository.Insert(campUser);
                        DBTMTraineeDetails trainee = _dBTMTraineeDetailsRepository.Table.FirstOrDefault(x => x.DBTMTraineeDetailId == generalPersonModel.EntityId);
                        if (trainee != null)
                        {
                            trainee.IsCampUser = true;
                            trainee.IsBatchUser = false;
                            _dBTMTraineeDetailsRepository.Update(trainee);
                        }
                    }
                    if (dBTMCustomNewRegistrationModel.DBTMCampMasterId > 0)
                    {
                        DBTMCampUser dbtmCampUser = new DBTMCampUser()
                        {
                            DBTMCampMasterId = dBTMCustomNewRegistrationModel.DBTMCampMasterId,
                            UserType = UserTypeEnum.Trainee.ToString(),
                            EntityId = generalPersonModel.EntityId,
                        };
                        _dbtmCampUserRepository.Insert(dbtmCampUser);
                    }
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
                if (dBTMCustomNewRegistrationModel?.GeneralTraineeAssociatedToTrainerIds?.Count == 0)
                {
                    dBTMCustomNewRegistrationModel.GeneralTraineeAssociatedToTrainerIds = new List<string>();
                    dBTMCustomNewRegistrationModel.GeneralTraineeAssociatedToTrainerIds.Add(joiningCodeDetails.Custom1);
                }
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

        public DBTMTraineeUploadModel DownloadTraineeUploadTemplate(string centreCode, long trainerId, string userType, int count, long entityId)
        {
            // Get joining codes
            string trainerIdStr = trainerId > 0 ? trainerId.ToString() : null;
            var joiningCodeModels = _joiningCodeService.GetTraineeActiveJoiningCodeList(centreCode, trainerIdStr, count);
            var joiningCodes = joiningCodeModels.Take(count).ToList();
            if (joiningCodes.Count < count)
            {
                return new DBTMTraineeUploadModel
                {
                    HasError = true,
                    ErrorMessage = $"Insufficient joining codes. Available: {joiningCodes.Count}"
                };
            }
            var trainerIds = joiningCodes.Where(x => !string.IsNullOrEmpty(x.Custom1)).Select(x => Convert.ToInt64(x.Custom1)).ToHashSet();
            var batchList = _generalBatchRepository.Table.Join(_userMasterRepository.Table,
                gbm => gbm.CreatedBy,
                um => um.UserMasterId,
                (gbm, um) => new { gbm, um }).Join(_generalTrainerMasterRepository.Table,
                temp => temp.um.EntityId,
                gtm => gtm.EmployeeId,
                (temp, gtm) => new
                {
                    gtm.GeneralTrainerMasterId,
                    temp.gbm.BatchName,
                    temp.gbm.GeneralBatchMasterId
                })
                .Where(x => trainerIds.Contains(x.GeneralTrainerMasterId) && x.BatchName != null).GroupBy(x => x.GeneralTrainerMasterId)
                .ToDictionary(g => g.Key, g => g.Select(x => x.BatchName).Distinct().ToList());
            // Get template
            int templateId = GetTemplateIdByCode("Trainee");
            if (templateId <= 0)
            {
                return new DBTMTraineeUploadModel
                {
                    HasError = true,
                    ErrorMessage = "Trainee template is not configured."
                };
            }
            List<GeneralTemplateHeaderConfiguration> headers = GetTraineeHeaders(centreCode);
            var template = _generalTemplateService.GetTemplate(templateId);
            template.HeaderConfigurationList = headers.Select(x =>
                new GeneralTemplateHeaderConfigurationModel
                {
                    GeneralTemplateHeaderConfigurationId = x.GeneralTemplateHeaderConfigurationId,
                    TemplateCode = x.TemplateCode,
                    HeaderCode = x.HeaderCode,
                    HeaderName = x.HeaderName,
                    HeaderType = x.HeaderType,
                    CentreCode = x.CentreCode,
                    OrderBy = x.OrderBy,
                    DropdownEnumGroupCode = x.DropdownEnumGroupCode
                }).ToList();
            string currentDir = Directory.GetCurrentDirectory();
            string dataFolder = Path.Combine(currentDir, "data", "TraineeUploadTemplate");
            if (!Directory.Exists(dataFolder))
                Directory.CreateDirectory(dataFolder);
            string fileUserName = "TraineeTemplate";
            if (userType == CustomConstants.DBTMCentreOwner)
            {
                fileUserName = _userMasterRepository.Table.Where(x => x.EntityId == entityId && x.UserType == UserTypeEnum.Employee.ToString()).Select(x => x.FirstName + "_" + x.LastName).FirstOrDefault() ?? "CentreOwner";
            }
            else
            {
                fileUserName = joiningCodeModels.FirstOrDefault()?.Custom2 ?? "Trainer";
            }
            fileUserName = fileUserName.Replace(" ", "_");
            string fileName = $"TraineeUploadTemplate_{centreCode}_{fileUserName}.xlsx";
            string filePath = Path.Combine(dataFolder, fileName);
            var callingCodes = GetCallingCodes();
            GenerateTraineeTemplateExcel(template, joiningCodes, batchList, filePath, callingCodes);
            return new DBTMTraineeUploadModel
            {
                FilePath = filePath,
                FileName = fileName
            };
        }

        public DBTMTraineeUploadModel UploadTraineeFromFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("File is empty.");
            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            DBTMTraineeUploadModel table;
            if (extension == ".xlsx" || extension == ".xls")
            {
                table = ExcelToListModel(file);
            }
            else
            {
                throw new Exception("Unsupported file format.");
            }
            if (table.DataTable == null || table.DataTable.Rows.Count == 0)
                throw new Exception("File contains no data.");
            DBTMTraineeUploadModel result = UploadTrainee(table);
            return new DBTMTraineeUploadModel
            {
                TotalRecords = result.TotalRecords,
                SuccessCount = result.SuccessCount,
                FailedCount = result.FailedCount,
                FailedRows = result.FailedRows,
                DataTable = result.DataTable
            };
        }

        public DBTMTraineeUploadModel UploadTrainee(DBTMTraineeUploadModel table)
        {
            DataTable dt = table.DataTable;
            if (!dt.Columns.Contains(ExcelTemplateColumns.SrNo))
            {
                dt.Columns.Add(ExcelTemplateColumns.SrNo, typeof(int));
                dt.Columns[ExcelTemplateColumns.SrNo].SetOrdinal(0);
            }
            if (!dt.Columns.Contains(ExcelTemplateColumns.ErrorMessage))
            {
                dt.Columns.Add(ExcelTemplateColumns.ErrorMessage);
            }
            dt.Columns[ExcelTemplateColumns.ErrorMessage].SetOrdinal(1);
            List<string> joiningCodes = dt.AsEnumerable().Select(r => r[ExcelTemplateColumns.JoiningCode]?.ToString()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            var joiningCodeList = _organisationCentrewiseJoiningCodeRepository.Table.Where(x => joiningCodes.Contains(x.JoiningCode)).ToList();
            var joiningTrainerMap = joiningCodeList.ToDictionary(x => x.JoiningCode, x => Convert.ToInt64(x.Custom1));
            // Validation
            int sr = 2;
            bool hasAnyError = false;
            var callingCodeSet = GetCallingCodeSet();
            foreach (DataRow row in dt.Rows)
            {
                row[ExcelTemplateColumns.SrNo] = sr++;
                if (!ValidateRow(row, joiningCodeList, joiningTrainerMap, callingCodeSet, out string error))
                {
                    row[ExcelTemplateColumns.ErrorMessage] = error;
                    hasAnyError = true;
                }
                else
                {
                    row[ExcelTemplateColumns.ErrorMessage] = null;
                }
            }
            int success = 0;
            if (hasAnyError)
            {
                DataTable failedTable = BuildFailedTable(dt);
                return BuildUploadResult(dt, 0, failedTable);
            }
            // Insert
            foreach (DataRow row in dt.Rows)
            {
                if (!InsertTrainee(row, joiningTrainerMap, out string err))
                {
                    row[ExcelTemplateColumns.ErrorMessage] = err;
                }
                else
                {
                    success++;
                }
            }
            if (success != dt.Rows.Count)
            {
                DataTable failedTable = BuildFailedTable(dt);
                return BuildUploadResult(dt, success, failedTable);
            }
            return BuildUploadResult(dt, success);
        }

        #region Private Methods
        private DBTMTraineeUploadModel BuildUploadResult(DataTable dt, int success, DataTable failedTable = null)
        {
            return new DBTMTraineeUploadModel
            {
                TotalRecords = dt.Rows.Count,
                SuccessCount = success,
                FailedCount = failedTable?.Rows.Count ?? 0,
                FailedRows = failedTable != null ? ToList(failedTable) : null,
                Data = failedTable == null ? ToList(dt) : null
            };
        }

        private string GetValue(DataRow row, string column)
        {
            return row.Table.Columns.Contains(column) ? row[column]?.ToString() : null;
        }

        // Validation 
        private bool ValidateRow(DataRow row, List<OrganisationCentrewiseJoiningCode> joiningCodeList, Dictionary<string, long> joiningTrainerMap, HashSet<string> callingCodeSet, out string errorMessage)
        {
            List<string> errors = new List<string>();
            string joiningCode = row.Table.Columns.Contains(ExcelTemplateColumns.JoiningCode) ? row[ExcelTemplateColumns.JoiningCode]?.ToString() : null;
            string title = GetValue(row, ExcelTemplateColumns.TraineeTitle);
            string firstName = GetValue(row, ExcelTemplateColumns.FirstName);
            string middleName = GetValue(row, ExcelTemplateColumns.MiddleName);
            string lastName = GetValue(row, ExcelTemplateColumns.LastName);
            string displayName = GetValue(row, ExcelTemplateColumns.DisplayName);
            string callingCode = GetValue(row, ExcelTemplateColumns.CallingCode);
            string mobile = GetValue(row, ExcelTemplateColumns.MobileNumber);
            string email = GetValue(row, ExcelTemplateColumns.EmailAddress);
            string gender = GetValue(row, ExcelTemplateColumns.Gender);
            string height = GetValue(row, ExcelTemplateColumns.HeightCm);
            string weight = GetValue(row, ExcelTemplateColumns.WeightKg);
            string dob = GetValue(row, ExcelTemplateColumns.DateOfBirth);
            string school = GetValue(row, ExcelTemplateColumns.SchoolOrCollegeOrClub);
            string ageGroup = GetValue(row, ExcelTemplateColumns.AgeGroup);
            string batchName = GetValue(row, ExcelTemplateColumns.BatchName);
            if (!ValidateJoiningCode(joiningCode, joiningCodeList, out string joiningCodeError))
                errors.Add(joiningCodeError);
            if (string.IsNullOrWhiteSpace(title))
            {
                errors.Add("Trainee Title is empty");
            }
            else
            {
                int titleEnumId = GetEnumIdByEnumCode(title, DropdownTypeEnum.Title.ToString());
                if (titleEnumId <= 0)
                {
                    errors.Add("Trainee Title is invalid");
                }
            }
            if (string.IsNullOrWhiteSpace(firstName))
                errors.Add("First Name is empty");
            else if (!Regex.IsMatch(firstName, @"^[a-zA-Z\s\-\(\)]+$"))
                errors.Add("First Name contains invalid characters");
            if (!string.IsNullOrWhiteSpace(middleName))
            {
                if (!Regex.IsMatch(middleName, @"^[a-zA-Z\s\-\(\)]+$"))
                    errors.Add("Middle Name contains invalid characters");
            }
            if (string.IsNullOrWhiteSpace(lastName))
                errors.Add("Last Name is empty");
            else if (!Regex.IsMatch(lastName, @"^[a-zA-Z\s\-\(\)]+$"))
                errors.Add("Last Name contains invalid characters");
            if (string.IsNullOrWhiteSpace(callingCode))
            {
                errors.Add("Calling Code is empty");
            }
            else
            {
                callingCode = callingCode?.Trim();
                if (!string.IsNullOrEmpty(callingCode) && !callingCode.StartsWith("+"))
                {
                    callingCode = "+" + callingCode;
                }
                if (!Regex.IsMatch(callingCode, @"^\+\d{1,4}$") || !callingCodeSet.Contains(callingCode))
                {
                    errors.Add("Calling Code is invalid");
                }
            }
            if (string.IsNullOrWhiteSpace(mobile))
            {
                errors.Add("Mobile Number is empty");
            }
            else
            {
                if (!mobile.All(char.IsDigit))
                    errors.Add("Mobile Number must contain only digits");
                if (mobile.Length != 10)
                    errors.Add("Mobile Number must be 10 digits");
            }
            if (string.IsNullOrWhiteSpace(height))
                errors.Add("Height is empty");
            else if (!decimal.TryParse(height, out _))
                errors.Add("Height must be numeric");
            if (string.IsNullOrWhiteSpace(weight))
                errors.Add("Weight is empty");
            else if (!decimal.TryParse(weight, out _))
                errors.Add("Weight must be numeric");
            if (string.IsNullOrWhiteSpace(dob))
                errors.Add("Date Of Birth is empty");
            else if (!DateTime.TryParse(dob, out _))
                errors.Add("Date Of Birth is invalid");
            if (string.IsNullOrWhiteSpace(email))
            {
                errors.Add("Email Address is empty");
            }
            else
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                    if (addr.Address != email)
                        errors.Add("Email Address format is invalid");
                }
                catch
                {
                    errors.Add("Email Address format is invalid");
                }
            }
            if (string.IsNullOrWhiteSpace(gender))
            {
                errors.Add("Gender is empty");
            }
            else
            {
                int genderEnumId = GetEnumIdByEnumCode(gender, DropdownTypeEnum.Gender.ToString());
                if (genderEnumId <= 0)
                {
                    errors.Add("Gender is invalid");
                }
            }
            if (string.IsNullOrWhiteSpace(GetValue(row, ExcelTemplateColumns.SchoolOrCollegeOrClub)))
                errors.Add("School Or College Or Club  is empty");
            if (string.IsNullOrWhiteSpace(batchName))
            {
                errors.Add("Batch is required");
            }
            if (!string.IsNullOrWhiteSpace(ageGroup))
            {
                ageGroup = ageGroup.Trim();
                int ageGroupEnumId = GetEnumIdByEnumCode(ageGroup.Replace(" ", ""), DropdownCustomTypeEnum.AgeGroup.ToString());
                if (ageGroupEnumId <= 0)
                {
                    errors.Add("Age Group is invalid");
                }
                else if (DateTime.TryParse(dob, out DateTime dobDate))
                {
                    int actualAgeGroupEnumId = GetAgeGroupEnumIdByDOB(dobDate);
                    if (ageGroupEnumId != actualAgeGroupEnumId)
                    {
                        errors.Add("Age Group does not match Date Of Birth");
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(joiningCode))
            {
                OrganisationCentrewiseJoiningCode organisationCentrewiseJoiningCode = joiningCodeList.FirstOrDefault(x => x.JoiningCode.Equals(joiningCode, StringComparison.OrdinalIgnoreCase));
                if (organisationCentrewiseJoiningCode != null)
                {
                    GeneralPersonModel person = new GeneralPersonModel
                    {
                        UserType = UserTypeEnum.Trainee.ToString(),
                        FirstName = firstName,
                        LastName = lastName,
                        MobileNumber = mobile,
                        EmailId = row[ExcelTemplateColumns.EmailAddress]?.ToString(),
                        SelectedCentreCode = organisationCentrewiseJoiningCode.CentreCode
                    };
                    if (!ValidatedGeneralPersonData(person, out string baseError))
                        errors.Add(baseError);
                }
            }
            errorMessage = errors.Count > 0 ? string.Join(", ", errors) : null;
            return errors.Count == 0;
        }

        //Insert Trainee 
        private bool InsertTrainee(DataRow row, Dictionary<string, long> joiningTrainerMap, out string error)
        {
            error = null;
            string joiningCode = GetValue(row, ExcelTemplateColumns.JoiningCode);
            var rawCallingCode = row.Table.Columns.Contains(ExcelTemplateColumns.CallingCode) ? row[ExcelTemplateColumns.CallingCode]?.ToString() : null;
            string schoolName = row.Table.Columns.Contains(ExcelTemplateColumns.SchoolOrCollegeOrClub) ? row[ExcelTemplateColumns.SchoolOrCollegeOrClub]?.ToString() : null;
            string ageGroup = row.Table.Columns.Contains(ExcelTemplateColumns.AgeGroup) ? row[ExcelTemplateColumns.AgeGroup]?.ToString() : null;
            string batchName = row.Table.Columns.Contains(ExcelTemplateColumns.BatchName) ? row[ExcelTemplateColumns.BatchName]?.ToString() : null;
            int batchId = 0;
            if (!string.IsNullOrWhiteSpace(batchName))
            {
                long trainerMasterId = joiningTrainerMap[joiningCode];
                batchId = (from gbm in _generalBatchRepository.Table
                           join um in _userMasterRepository.Table
                               on gbm.CreatedBy equals um.UserMasterId
                           join gtm in _generalTrainerMasterRepository.Table
                               on um.EntityId equals gtm.EmployeeId
                           where gtm.GeneralTrainerMasterId == trainerMasterId && gbm.BatchName == batchName && gbm.IsActive
                           select gbm.GeneralBatchMasterId
                          ).FirstOrDefault();
                if (batchId == 0)
                {
                    error = $"Batch '{batchName}' is not assigned to this trainer.";
                    return false;
                }
            }
            decimal height = Convert.ToDecimal(GetValue(row, ExcelTemplateColumns.HeightCm));
            decimal weight = Convert.ToDecimal(GetValue(row, ExcelTemplateColumns.WeightKg));
            string specialization = GetValue(row, ExcelTemplateColumns.Specialization);
            int ageGroupEnumId = 0;
            if (!string.IsNullOrWhiteSpace(ageGroup))
            {
                ageGroupEnumId = GetEnumIdByEnumCode(ageGroup.Replace(" ", ""), DropdownCustomTypeEnum.AgeGroup.ToString());
            }
            DBTMCustomNewRegistrationModel customModel = new DBTMCustomNewRegistrationModel
            {
                JoiningCode = joiningCode,
                height = height,
                weight = weight,
                SpecializationEnumId = GetEnumIdByEnumCode(specialization, DropdownCustomTypeEnum.TraineeSpecialization.ToString()),
                GeneralBatchMasterId = batchId,
                SchoolName = schoolName,
                AgeGroupEnumId = ageGroupEnumId,
                RegistrationType = "Batch",
                GeneralTraineeAssociatedToTrainerIds = new List<string>()
            };
            rawCallingCode = rawCallingCode?.Trim();
            if (!string.IsNullOrEmpty(rawCallingCode) && !rawCallingCode.StartsWith("+"))
            {
                rawCallingCode = "+" + rawCallingCode;
            }
            GeneralPersonModel model = new GeneralPersonModel
            {
                UserType = UserTypeEnum.Trainee.ToString(),
                PersonTitle = GetValue(row, ExcelTemplateColumns.TraineeTitle),
                FirstName = GetValue(row, ExcelTemplateColumns.FirstName),
                MiddleName = GetValue(row, ExcelTemplateColumns.MiddleName),
                LastName = GetValue(row, ExcelTemplateColumns.LastName),
                Custom2 = GetValue(row, ExcelTemplateColumns.DisplayName),
                EmailId = GetValue(row, ExcelTemplateColumns.EmailAddress),
                MobileNumber = GetValue(row, ExcelTemplateColumns.MobileNumber),
                CallingCode = rawCallingCode,
                GenderEnumId = GetEnumIdByEnumCode(GetValue(row, ExcelTemplateColumns.Gender), DropdownTypeEnum.Gender.ToString()),
                DateOfBirth = Convert.ToDateTime(GetValue(row, ExcelTemplateColumns.DateOfBirth)),
                Custom1 = JsonConvert.SerializeObject(customModel)
            };
            DBTMRegisterTrainee(model);
            return true;
        }
        private DataTable BuildFailedTable(DataTable source)
        {
            DataTable failed = new DataTable();
            foreach (DataColumn col in source.Columns)
                failed.Columns.Add(col.ColumnName, col.DataType);
            foreach (DataRow row in source.Rows)
            {
                if (!string.IsNullOrEmpty(row["ErrorMessage"]?.ToString()))
                {
                    DataRow newRow = failed.NewRow();
                    foreach (DataColumn col in source.Columns)
                        newRow[col.ColumnName] = row[col.ColumnName];
                    failed.Rows.Add(newRow);
                }
            }
            return failed;
        }
        private bool ValidateJoiningCode(string joiningCode, List<OrganisationCentrewiseJoiningCode> validList, out string error)
        {
            error = null;
            if (string.IsNullOrWhiteSpace(joiningCode))
            {
                error = "JoiningCode is empty";
                return false;
            }
            OrganisationCentrewiseJoiningCode organisationCentrewiseJoiningCode = validList.FirstOrDefault(x => x.JoiningCode.Equals(joiningCode, StringComparison.OrdinalIgnoreCase));
            if (organisationCentrewiseJoiningCode == null)
            {
                error = "JoiningCode is invalid";
                return false;
            }
            if (organisationCentrewiseJoiningCode.IsExpired)
            {
                error = "JoiningCode has expired";
                return false;
            }
            return true;
        }
        private DBTMTraineeUploadModel ExcelToListModel(IFormFile file)
        {
            var result = new DBTMTraineeUploadModel();
            var table = new DataTable();
            using var stream = file.OpenReadStream();
            using var workbook = new XLWorkbook(stream);
            var sheet = workbook.Worksheet(1);
            bool isHeader = true;
            int excelRowNo = 1;
            foreach (var row in sheet.RowsUsed())
            {
                excelRowNo++;
                if (isHeader)
                {
                    foreach (var cell in row.Cells())
                    {
                        var header = cell.GetString().Trim();
                        if (!string.IsNullOrEmpty(header))
                            table.Columns.Add(header);
                    }
                    isHeader = false;
                    continue;
                }
                var dr = table.NewRow();
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    var cell = row.Cell(i + 1);
                    dr[i] = cell.IsEmpty() ? null : cell.GetFormattedString().Trim();
                }
                table.Rows.Add(dr);
            }
            if (!table.Columns.Contains(ExcelTemplateColumns.SrNo))
                table.Columns.Add(ExcelTemplateColumns.SrNo, typeof(int));
            if (!table.Columns.Contains(ExcelTemplateColumns.ErrorMessage))
                table.Columns.Add(ExcelTemplateColumns.ErrorMessage);
            table.Columns[ExcelTemplateColumns.SrNo].SetOrdinal(0);
            table.Columns[ExcelTemplateColumns.ErrorMessage].SetOrdinal(1);
            result.DataTable = table;
            return result;
        }

        //GenerateTraineeTemplateExcel
        private void GenerateTraineeTemplateExcel(GeneralTemplateModel template, List<OrganisationCentrewiseJoiningCodeModel> joiningCodes, Dictionary<long, List<string>> batchList, string filePath, List<string> callingCodes)
        {
            using var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add("Upload Trainee");
            var lookupSheet = workbook.Worksheets.Add("Lookups");
            lookupSheet.Visibility = XLWorksheetVisibility.Hidden;
            var headerGroupCodeMap = new Dictionary<string, string>();
            foreach (var header in template.HeaderConfigurationList.Where(x => x.HeaderType == CustomConstants.Dropdown))
            {
                if (!headerGroupCodeMap.ContainsKey(header.HeaderCode))
                {
                    headerGroupCodeMap[header.HeaderCode] = header.DropdownEnumGroupCode;
                }
            }
            List<GeneralEnumaratorModel> enumList = BindEnumarator();
            int col = 1;
            int lookupCol = 1;
            foreach (var header in template.HeaderConfigurationList.OrderBy(x => x.OrderBy))
            {
                sheet.Cell(1, col).Value = header.HeaderCode;
                sheet.Cell(1, col).Style.Font.Bold = true;
                if (header.HeaderCode == ExcelTemplateColumns.BatchName && batchList.Any())
                {
                    for (int row = 0; row < joiningCodes.Count; row++)
                    {
                        if (string.IsNullOrEmpty(joiningCodes[row].Custom1))
                            continue;
                        long trainerId = Convert.ToInt64(joiningCodes[row].Custom1);
                        if (!batchList.ContainsKey(trainerId))
                            continue;
                        var batches = batchList[trainerId];
                        int startRow = 1;
                        for (int i = 0; i < batches.Count; i++)
                        {
                            lookupSheet.Cell(startRow + i, lookupCol).Value = batches[i];
                        }
                        var range = lookupSheet.Range(
                            lookupSheet.Cell(startRow, lookupCol),
                            lookupSheet.Cell(startRow + batches.Count - 1, lookupCol)
                        );
                        var validation = sheet.Cell(row + 2, col).CreateDataValidation();
                        validation.IgnoreBlanks = true;
                        validation.InCellDropdown = true;
                        validation.List(range, true);
                        lookupCol++;
                    }
                }
                if (header.HeaderCode == ExcelTemplateColumns.DateOfBirth)
                {
                    for (int row = 2; row <= joiningCodes.Count + 1; row++)
                    {
                        var cell = sheet.Cell(row, col);
                        cell.Style.DateFormat.Format = "yyyy-MM-dd";
                        var dv = cell.CreateDataValidation();
                        dv.IgnoreBlanks = true;
                        dv.AllowedValues = XLAllowedValues.Date;
                        dv.Operator = XLOperator.Between;
                        dv.InputTitle = "Date Format";
                        dv.InputMessage = "yyyy-MM-dd";
                    }
                }
                if (header.HeaderCode == ExcelTemplateColumns.CallingCode)
                {
                    for (int row = 2; row <= joiningCodes.Count + 1; row++)
                    {
                        var validation = sheet.Cell(row, col).CreateDataValidation();
                        validation.IgnoreBlanks = true;
                        validation.InCellDropdown = true;
                        string callingCodeList = string.Join(",", callingCodes);
                        validation.List($"\"{callingCodeList}\"", true);
                    }
                }
                if (headerGroupCodeMap.TryGetValue(header.HeaderCode, out string groupCode))
                {
                    var values = enumList.Where(x => x.EnumGroupCode == groupCode).OrderBy(x => x.SequenceNumber).Select(x => x.EnumDisplayText).ToList();
                    for (int i = 0; i < values.Count; i++)
                        lookupSheet.Cell(i + 1, lookupCol).Value = values[i];
                    for (int row = 2; row <= joiningCodes.Count + 1; row++)
                    {
                        var validation = sheet.Cell(row, col).CreateDataValidation();
                        validation.IgnoreBlanks = true;
                        validation.InCellDropdown = true;
                        validation.List(
                            lookupSheet.Range(
                                lookupSheet.Cell(1, lookupCol),
                                lookupSheet.Cell(values.Count, lookupCol)
                            ),
                            true
                        );
                    }
                    lookupCol++;
                }
                col++;
            }
            var headersOrdered = template.HeaderConfigurationList.OrderBy(x => x.OrderBy).ToList();
            for (int row = 0; row < joiningCodes.Count; row++)
            {
                var code = joiningCodes[row];
                for (int colIndex = 0; colIndex < headersOrdered.Count; colIndex++)
                {
                    var header = headersOrdered[colIndex].HeaderCode;
                    var cell = sheet.Cell(row + 2, colIndex + 1);
                    switch (header)
                    {
                        case ExcelTemplateColumns.JoiningCode:
                            cell.Value = code.JoiningCode;
                            break;

                        case ExcelTemplateColumns.TrainerName:
                            cell.Value = code.Custom2;
                            break;
                    }
                }
            }
            sheet.Columns().AdjustToContents();
            workbook.SaveAs(filePath);
        }
        private List<Dictionary<string, object>> ToList(DataTable table)
        {
            var list = new List<Dictionary<string, object>>();
            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                    dict[col.ColumnName] = row[col] == DBNull.Value ? null : row[col];
                list.Add(dict);
            }
            return list;
        }

        private List<string> GetCallingCodes()
        {
            return _generalCountryRepository.Table.Where(x => !string.IsNullOrEmpty(x.CallingCode)).Select(x => x.CallingCode).Distinct().OrderBy(x => x).ToList();
        }
        private HashSet<string> GetCallingCodeSet()
        {
            return _generalCountryRepository.Table.Select(x => x.CallingCode).ToHashSet();
        }
        private List<GeneralTemplateHeaderConfiguration> GetTraineeHeaders(string centreCode)
        {
            return _generalTemplateHeaderConfigurationRepository.Table.Where(x => x.TemplateCode == "Trainee" && (x.CentreCode == centreCode || x.CentreCode == null)).AsEnumerable()
                .GroupBy(x => x.HeaderCode).Select(g => g.OrderByDescending(x => x.CentreCode == centreCode).First()).OrderBy(x => x.OrderBy).ToList();
        }
        private int GetTemplateIdByCode(string templateCode)
        {
            return new CoditechRepository<GeneralTemplateMaster>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => x.TemplateCode == templateCode).Select(x => x.GeneralTemplateMasterId).FirstOrDefault();
        }      
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
        private void ValidateCentreUserLimit(string centreCode, string registrationType)
        {
            // Get centre settings
            DBTMCentreWiseSetting centreSetting = _dBTMCentreWiseSettingRepository.Table
                .FirstOrDefault(x => x.CentreCode == centreCode);

            if (centreSetting == null)
                throw new CoditechException(ErrorCodes.InvalidData, "Centre setting not found.");

            // Count based on type 
            int usedCount = 0;

            if (string.Equals(registrationType, "Batch", StringComparison.InvariantCultureIgnoreCase))
            {
                usedCount = _organisationCentrewiseJoiningCodeRepository.Table
                    .Count(x => x.CentreCode == centreCode
                             && x.IsExpired
                             && x.Custom2 == "Batch");

                if (usedCount >= centreSetting.AllowBatchUser)
                {
                    throw new CoditechException(ErrorCodes.InvalidData,
                        "Batch limit exceeded, Kindly contact Powered Sports Tech or raise a support ticket for assistance.");
                }
            }
            else if (string.Equals(registrationType, "Camp", StringComparison.InvariantCultureIgnoreCase))
            {
                usedCount = _organisationCentrewiseJoiningCodeRepository.Table
                    .Count(x => x.CentreCode == centreCode
                             && x.IsExpired
                             && x.Custom2 == "Camp");

                if (usedCount >= centreSetting.AllowCampUser)
                {
                    throw new CoditechException(ErrorCodes.InvalidData,
                        "Camp limit exceeded, Kindly contact Powered Sports Tech or raise a support ticket for assistance.");
                }
            }
            else
            {
                throw new CoditechException(ErrorCodes.InvalidData, "Invalid registration type.");
            }
        }

        private int GetAgeGroupEnumIdByDOB(DateTime? dob)
        {
            var ageGroups =
            (
                from gm in _generalEnumaratorMasterRepository.Table
                join gg in _generalEnumaratorGroupRepository.Table
                on gm.GeneralEnumaratorGroupId equals gg.GeneralEnumaratorGroupId
                where gg.EnumGroupCode == "AgeGroup" && gm.IsActive
                orderby gm.SequenceNumber
                select new
                {
                    EnumId = gm.GeneralEnumaratorId,
                    EnumValue = Convert.ToInt32(gm.EnumValue)
                }
            ).ToList();
            return DBTMCustomHelper.GetAgeGroupEnumIdByDOB(dob, ageGroups.Select(x => (x.EnumId, x.EnumValue)).ToList());
        }
        #endregion

        #region Protected Method
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
        #endregion
    }
}

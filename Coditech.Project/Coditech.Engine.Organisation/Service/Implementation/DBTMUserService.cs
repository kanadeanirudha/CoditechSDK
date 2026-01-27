using ClosedXML.Excel;
using Coditech.API.Data;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Resources;
using DocumentFormat.OpenXml.Spreadsheet;
using Hangfire.Common;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using static Coditech.Common.Helper.HelperUtility;
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

        public DBTMUserService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider, ICoditechEmail coditechEmail, ICoditechSMS coditechSMS, ICoditechWhatsApp coditechWhatsApp, IGeneralTemplateService generalTemplateService, IDBTMOrganisationCentrewiseJoiningCodeService joiningCodeService) : base(coditechLogging, serviceProvider, coditechEmail, coditechSMS, coditechWhatsApp)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _generalTemplateService = generalTemplateService;
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
            _joiningCodeService = joiningCodeService;
            _generalTemplateHeaderConfigurationRepository = new CoditechRepository<GeneralTemplateHeaderConfiguration>(_serviceProvider.GetService<Coditech_Entities>());
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
                if (model.Custom1 == CustomConstants.DBTMTrainer || model.Custom1 == CustomConstants.DBTMCentreOwner)
                {
                    DBTMCustomUserModel dBTMCustomUserModel = new DBTMCustomUserModel();
                    dBTMCustomUserModel.GeneralTrainerMasterId = _generalTrainerMasterRepository.Table.Where(x => x.EmployeeId == model.EntityId)?.Select(y => y.GeneralTrainerMasterId)?.FirstOrDefault();
                    model.Custom3 = JsonConvert.SerializeObject(dBTMCustomUserModel);
                }
            }
            return model;
        }

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
                joiningCodeDetails = _organisationCentrewiseJoiningCodeRepository.Table.Where(x => x.JoiningCode == dBTMCustomNewRegistrationModel.JoiningCode)?.FirstOrDefault();

                if (IsNull(joiningCodeDetails))
                    throw new CoditechException(ErrorCodes.AlreadyExist, string.Format("Invalid Joining Code."));

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
                    if (dBTMCustomNewRegistrationModel.GeneralBatchMasterId > 0)
                    {
                        GeneralBatchUser generalBatchUser = new GeneralBatchUser()
                        {
                            GeneralBatchMasterId = dBTMCustomNewRegistrationModel.GeneralBatchMasterId,
                            UserType = UserTypeEnum.Trainee.ToString(),
                            EntityId = generalPersonModel.EntityId,
                        };
                        _generalBatchUserRepository.Insert(generalBatchUser);
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

        private int GetTemplateIdByCode(string templateCode)
        {
            return new CoditechRepository<GeneralTemplateMaster>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => x.TemplateCode == templateCode).Select(x => x.GeneralTemplateMasterId).FirstOrDefault();
        }
        public DBTMTraineeUploadModel DownloadTraineeUploadTemplate(string centreCode, long trainerId, string userType, int count)
        {
            // Get joining codes
            string trainerIdStr = trainerId > 0 ? trainerId.ToString() : null;
            var joiningCodeModels = _joiningCodeService.GetTraineeActiveJoiningCodeList(centreCode, trainerIdStr, count);
            var joiningCodes = joiningCodeModels.Select(x => x.JoiningCode).Take(count).ToList();
            if (joiningCodes.Count < count)
            {
                return new DBTMTraineeUploadModel
                {
                    HasError = true,
                    ErrorMessage = $"Insufficient joining codes. Available: {joiningCodes.Count}"
                };
            }
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
            List<GeneralTemplateHeaderConfiguration> headers = _generalTemplateHeaderConfigurationRepository.Table.Where(x => x.TemplateCode == "Trainee").OrderBy(x => x.OrderBy).ToList();
            var template = _generalTemplateService.GetTemplate(templateId);
            template.HeaderConfigurationList = headers.Select(x =>
                new GeneralTemplateHeaderConfigurationModel
                {
                    GeneralTemplateHeaderConfigurationId = x.GeneralTemplateHeaderConfigurationId,
                    TemplateCode = x.TemplateCode,
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
            string trainerNameRaw = joiningCodeModels.FirstOrDefault()?.Custom2 ?? "Trainer";
            string trainerName = trainerNameRaw.Trim().Replace(" ", "_");  
            string fileName = $"TraineeUploadTemplate_{centreCode}_{trainerName}.xlsx";
            string filePath = Path.Combine(dataFolder, fileName);
            GenerateTraineeTemplateExcel(template, joiningCodes, filePath);
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
            if (!dt.Columns.Contains("SrNo"))
            {
                dt.Columns.Add("SrNo", typeof(int));
                dt.Columns["SrNo"].SetOrdinal(0);
            }
            if (!dt.Columns.Contains("ErrorMessage"))
            {
                dt.Columns.Add("ErrorMessage");
            }
            dt.Columns["ErrorMessage"].SetOrdinal(1);
            List<string> joiningCodes = dt.AsEnumerable().Select(r => r["JoiningCode"]?.ToString()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            var joiningCodeList = _organisationCentrewiseJoiningCodeRepository.Table.Where(x => joiningCodes.Contains(x.JoiningCode)).ToList();
            // Validation
            int sr = 2;
            bool hasAnyError = false;
            foreach (DataRow row in dt.Rows)
            {
                row["SrNo"] = sr++;
                if (!ValidateRow(row, joiningCodeList, out string error))
                {
                    row["ErrorMessage"] = error;
                    hasAnyError = true;
                }
                else
                {
                    row["ErrorMessage"] = null;
                }
            }
            if (hasAnyError)
            {
                DataTable failedTable = BuildFailedTable(dt);
                return new DBTMTraineeUploadModel
                {
                    TotalRecords = dt.Rows.Count,
                    SuccessCount = 0,
                    FailedCount = failedTable.Rows.Count,
                    FailedRows = ToList(failedTable)
                };
            }
            // Insert
            int success = 0;
            foreach (DataRow row in dt.Rows)
            {
                InsertTrainee(row);
                success++;
            }
            DataTable finalFailed = BuildFailedTable(dt);
            return new DBTMTraineeUploadModel
            {
                TotalRecords = dt.Rows.Count,
                SuccessCount = success,
                FailedCount = finalFailed.Rows.Count,
                FailedRows = ToList(finalFailed),
                Data = ToList(dt)
            };
        }

        // Validation 
        private bool ValidateRow(DataRow row, List<OrganisationCentrewiseJoiningCode> joiningCodeList, out string errorMessage)
        {
            List<string> errors = new List<string>();

            string joiningCode = row["JoiningCode"]?.ToString();
            string title = row["TraineeTitle"]?.ToString();
            string firstName = row["FirstName"]?.ToString();
            string lastName = row["LastName"]?.ToString();
            string callingCode = row["CallingCode"]?.ToString();
            string mobile = row["MobileNumber"]?.ToString();
            string email = row["EmailAddress"]?.ToString();
            string gender = row["Gender"]?.ToString();
            string height = row["HeightCm"]?.ToString();
            string weight = row["WeightKg"]?.ToString();
            string dob = row["DateOfBirth"]?.ToString();
            if (!ValidateJoiningCode(joiningCode, joiningCodeList, out string joiningCodeError))
                errors.Add(joiningCodeError);
            if (string.IsNullOrWhiteSpace(title))
            {
                errors.Add("TraineeTitle is empty");
            }
            else
            {
                int titleEnumId = GetEnumIdByEnumCode(
                    title,
                    DropdownTypeEnum.Title.ToString()
                );

                if (titleEnumId <= 0)
                {
                    errors.Add("TraineeTitle is invalid");
                }
            }
            if (string.IsNullOrWhiteSpace(firstName))
                errors.Add("FirstName is empty");
            else if (!firstName.All(char.IsLetter))
                errors.Add("FirstName must contain only letters");

            if (string.IsNullOrWhiteSpace(lastName))
                errors.Add("LastName is empty");
            else if (!lastName.All(char.IsLetter))
                errors.Add("LastName must contain only letters");
            if (string.IsNullOrWhiteSpace(callingCode))
            {
                errors.Add("CallingCode is empty");
            }
            else
            {
                if (!callingCode.All(char.IsDigit))
                    errors.Add("CallingCode must contain only digits");

                if (callingCode.Length < 1 || callingCode.Length > 4)
                    errors.Add("CallingCode length must be between 1 and 4 digits");
            }
            if (string.IsNullOrWhiteSpace(mobile))
            {
                errors.Add("MobileNumber is empty");
            }
            else
            {
                if (!mobile.All(char.IsDigit))
                    errors.Add("MobileNumber must contain only digits");

                if (mobile.Length < 10 || mobile.Length > 15)
                    errors.Add("MobileNumber length must be between 10 and 15 digits");
            }          
            if (string.IsNullOrWhiteSpace(height))
                errors.Add("HeightCm is empty");
            else if (!decimal.TryParse(height, out _))
                errors.Add("HeightCm must be numeric");
            if (string.IsNullOrWhiteSpace(weight))
                errors.Add("WeightKg is empty");
            else if (!decimal.TryParse(weight, out _))
                errors.Add("WeightKg must be numeric");
            if (string.IsNullOrWhiteSpace(dob))
                errors.Add("DateOfBirth is empty");
            else if (!DateTime.TryParse(dob, out _))
                errors.Add("DateOfBirth is invalid");
            if (!string.IsNullOrWhiteSpace(email))
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                    if (addr.Address != email)
                        errors.Add("EmailAddress format is invalid");
                }
                catch
                {
                    errors.Add("EmailAddress format is invalid");
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
            if (!string.IsNullOrWhiteSpace(joiningCode))
            {
                OrganisationCentrewiseJoiningCode organisationCentrewiseJoiningCode = joiningCodeList.FirstOrDefault(x =>
                    x.JoiningCode.Equals(joiningCode, StringComparison.OrdinalIgnoreCase));

                if (organisationCentrewiseJoiningCode != null)
                {
                    GeneralPersonModel person = new GeneralPersonModel
                    {
                        UserType = UserTypeEnum.Trainee.ToString(),
                        FirstName = firstName,
                        LastName = lastName,
                        MobileNumber = mobile,
                        EmailId = row["EmailAddress"]?.ToString(),
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
        private void InsertTrainee(DataRow row)
        {
            var rawCallingCode = row.Table.Columns.Contains("CallingCode") ? row["CallingCode"]?.ToString(): null;
            DBTMCustomNewRegistrationModel customModel = new DBTMCustomNewRegistrationModel
            {
                JoiningCode = row["JoiningCode"].ToString(),
                height = Convert.ToDecimal(row["HeightCm"]),
                weight = Convert.ToDecimal(row["WeightKg"]),
                SpecializationEnumId = GetEnumIdByEnumCode(row["Specialization"]?.ToString(), DropdownCustomTypeEnum.TraineeSpecialization.ToString()),
                GeneralBatchMasterId = 0,
                GeneralTraineeAssociatedToTrainerIds = new List<string>()
            };
            GeneralPersonModel model = new GeneralPersonModel
            {
                UserType = UserTypeEnum.Trainee.ToString(),
                PersonTitle = row["TraineeTitle"]?.ToString(),
                FirstName = row["FirstName"].ToString(),
                LastName = row["LastName"].ToString(),
                EmailId = row["EmailAddress"]?.ToString(),
                MobileNumber = row["MobileNumber"].ToString(),
                CallingCode = !string.IsNullOrWhiteSpace(rawCallingCode) ? "+" + rawCallingCode : null,
                GenderEnumId = GetEnumIdByEnumCode(row["Gender"]?.ToString(), DropdownTypeEnum.Gender.ToString()),
                DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]),
                Custom1 = JsonConvert.SerializeObject(customModel)
            };
            DBTMRegisterTrainee(model);
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
            if (!table.Columns.Contains("SrNo"))
                table.Columns.Add("SrNo", typeof(int));
            if (!table.Columns.Contains("ErrorMessage"))
                table.Columns.Add("ErrorMessage");
            table.Columns["SrNo"].SetOrdinal(0);
            table.Columns["ErrorMessage"].SetOrdinal(1);
            result.DataTable = table;
            return result;
        }

        //GenerateTraineeTemplateExcel
        private void GenerateTraineeTemplateExcel(GeneralTemplateModel template, List<string> joiningCodes, string filePath)
        {
            using var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add("Trainee Template");
            var lookupSheet = workbook.Worksheets.Add("Lookups");
            lookupSheet.Visibility = XLWorksheetVisibility.Hidden;
            var headerGroupCodeMap = new Dictionary<string, string>();
            foreach (var header in template.HeaderConfigurationList.Where(x => x.HeaderType == "Dropdown"))
            {
                if (!headerGroupCodeMap.ContainsKey(header.HeaderName))
                {
                    headerGroupCodeMap[header.HeaderName] = header.DropdownEnumGroupCode;
                }
            }
            List<GeneralEnumaratorModel> enumList = BindEnumarator();
            int col = 1;
            int lookupCol = 1;
            foreach (var header in template.HeaderConfigurationList.OrderBy(x => x.OrderBy))
            {
                sheet.Cell(1, col).Value = header.HeaderName;
                sheet.Cell(1, col).Style.Font.Bold = true;
                if (headerGroupCodeMap.TryGetValue(header.HeaderName, out string groupCode))
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
            for (int i = 0; i < joiningCodes.Count; i++)
            {
                sheet.Cell(i + 2, 1).Value = joiningCodes[i];
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

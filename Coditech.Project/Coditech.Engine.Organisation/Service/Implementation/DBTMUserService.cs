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
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class DBTMUserService : UserService, IDBTMUserService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
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


        public DBTMUserService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider, ICoditechEmail coditechEmail, ICoditechSMS coditechSMS, ICoditechWhatsApp coditechWhatsApp) : base(coditechLogging, serviceProvider, coditechEmail, coditechSMS, coditechWhatsApp)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
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

        public DBTMTraineeUploadModel UploadTraineeFromFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("File is empty.");
            DBTMTraineeUploadModel table = CsvToListModel(file);
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
            string title = row["PersonTitle"]?.ToString();
            string firstName = row["FirstName"]?.ToString();
            string lastName = row["LastName"]?.ToString();
            string mobile = row["MobileNumber"]?.ToString();
            string gender = row["Gender"]?.ToString();

            if (!ValidateJoiningCode(joiningCode, joiningCodeList, out string joiningCodeError))
                errors.Add(joiningCodeError);

            if (string.IsNullOrWhiteSpace(title))
                errors.Add("PersonTitle is empty");
            else if (title != "Mr" && title != "Ms")
                errors.Add("PersonTitle must be Mr or Ms");

            if (string.IsNullOrWhiteSpace(firstName))
                errors.Add("FirstName is empty");
            else if (!firstName.All(char.IsLetter))
                errors.Add("FirstName must contain only letters");

            if (string.IsNullOrWhiteSpace(lastName))
                errors.Add("LastName is empty");
            else if (!lastName.All(char.IsLetter))
                errors.Add("LastName must contain only letters");

            if (string.IsNullOrWhiteSpace(mobile))
                errors.Add("MobileNumber is empty");
            else if (!mobile.All(char.IsDigit))
                errors.Add("MobileNumber must contain only digits");

            if (!(gender?.Equals("male", StringComparison.OrdinalIgnoreCase) == true ||
                  gender?.Equals("female", StringComparison.OrdinalIgnoreCase) == true))
                errors.Add("Gender must be Male or Female");

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
            DBTMCustomNewRegistrationModel customModel = new DBTMCustomNewRegistrationModel
            {
                JoiningCode = row["JoiningCode"].ToString(),
                height = Convert.ToDecimal(row["HeightCm"]),
                weight = Convert.ToDecimal(row["WeightKg"]),
                SpecializationEnumId = Convert.ToInt32(row["SpecializationEnumId"]),
                GeneralBatchMasterId = 0,
                GeneralTraineeAssociatedToTrainerIds = new List<string>()
            };
            GeneralPersonModel model = new GeneralPersonModel
            {
                UserType = UserTypeEnum.Trainee.ToString(),
                PersonTitle = row["PersonTitle"].ToString(),
                FirstName = row["FirstName"].ToString(),
                LastName = row["LastName"].ToString(),
                EmailId = row["EmailAddress"]?.ToString(),
                MobileNumber = row["MobileNumber"].ToString(),
                CallingCode = row["CallingCode"]?.ToString(),
                GenderEnumId = row["Gender"].ToString().ToLower() == "male" ? 1 : 2,
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
        private DBTMTraineeUploadModel CsvToListModel(IFormFile file)
        {
            var result = new DBTMTraineeUploadModel();
            var table = new DataTable();
            using var reader = new StreamReader(file.OpenReadStream());
            bool isHeader = true;
            string[] headers = null;
            int lineNo = 0;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                lineNo++;
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                var values = line.Split(',');
                if (isHeader)
                {
                    headers = values.Select(x => x.Trim()).ToArray();
                    foreach (var col in headers)
                        table.Columns.Add(col);
                    isHeader = false;
                    continue;
                }
                if (values.Length != headers.Length)
                    throw new Exception($"CSV format error at line {lineNo}");
                var dr = table.NewRow();
                for (int i = 0; i < headers.Length; i++)
                    dr[i] = values[i].Trim();
                table.Rows.Add(dr);
            }
            result.DataTable = table;
            return result;
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

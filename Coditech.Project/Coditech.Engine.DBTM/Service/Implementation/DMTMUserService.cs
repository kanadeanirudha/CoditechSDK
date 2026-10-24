using Coditech.API.Data;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Resources;
using System.Data;
using static Coditech.Common.Helper.HelperUtility;

namespace Coditech.API.Service
{
    public class DBTMUserService : BaseService, IDBTMUserService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IDBTMApiService _dBTMApiService;
        private readonly ICoditechRepository<UserMaster> _userMasterRepository;
        private readonly ICoditechRepository<DBTMTraineeDetails> _dbtmTraineeDetailsRepository;
        private readonly ICoditechRepository<EmployeeMaster> _employeeMasterRepository;
        private readonly ICoditechRepository<GeneralPerson> _generalPersonRepository;
        protected readonly ICoditechRepository<OrganisationCentrewiseJoiningCode> _organisationCentrewiseJoiningCodeRepository;
        private readonly ICoditechRepository<GeneralTrainerMaster> _generalTrainerMasterRepository;
        private readonly ICoditechRepository<EmployeeDesignationMaster> _employeeDesignationMasterRepository;

        public DBTMUserService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider, IDBTMApiService dBTMApiService) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMApiService = dBTMApiService;
            _userMasterRepository = new CoditechRepository<UserMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _dbtmTraineeDetailsRepository = new CoditechRepository<DBTMTraineeDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _generalPersonRepository = new CoditechRepository<GeneralPerson>(_serviceProvider.GetService<Coditech_Entities>());
            _employeeMasterRepository = new CoditechRepository<EmployeeMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _organisationCentrewiseJoiningCodeRepository = new CoditechRepository<OrganisationCentrewiseJoiningCode>(_serviceProvider.GetService<Coditech_Entities>());
            _generalTrainerMasterRepository = new CoditechRepository<GeneralTrainerMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _employeeDesignationMasterRepository = new CoditechRepository<EmployeeDesignationMaster>(_serviceProvider.GetService<Coditech_Entities>());
        }

        public virtual DBTMUserModel Login(UserLoginModel userLoginModel)
        {
            if (IsNull(userLoginModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            userLoginModel.Password = MD5Hash(userLoginModel.Password);
            string encodedUserName = SearchHashHelper.CreateSearchHash(userLoginModel.UserName);

            UserMaster userMasterData = _userMasterRepository.Table.FirstOrDefault(x =>
                (x.UserType == UserTypeEnum.Admin.ToString() || x.UserType == UserTypeEnum.Employee.ToString()) &&
                (x.IsDataEncrypted ? x.UserNameHash == encodedUserName : x.UserName == userLoginModel.UserName));

            if (IsNull(userMasterData))
                throw new CoditechException(ErrorCodes.NotFound, null);
            else if (!userMasterData.IsActive)
                throw new CoditechException(ErrorCodes.ContactAdministrator, GeneralResources.ErrorMessage_PleaseContactYourAdministrator);

            DBTMUserModel userModel = new DBTMUserModel();
            long personId = 0;

            if (userMasterData.UserType == UserTypeEnum.Trainee.ToString())
            {
                var data = _dbtmTraineeDetailsRepository.Table.Where(x => x.DBTMTraineeDetailId == userMasterData.EntityId).Select(x => new { x.PersonId, x.CentreCode, x.SpecializationEnumId })?.FirstOrDefault();
                if (data != null)
                {
                    userModel.CentreCode = data.CentreCode;
                    personId = data.PersonId;
                    userModel.EmployeeDesignation = GetEnumDisplayTextByEnumId(Convert.ToInt32(data.SpecializationEnumId));
                }
            }
            else if (userMasterData.UserType == UserTypeEnum.Employee.ToString())
            {
                var data = _employeeMasterRepository.Table.Where(x => x.EmployeeId == userMasterData.EntityId).Select(x => new { x.PersonId, x.CentreCode, x.EmployeeDesignationMasterId })?.FirstOrDefault();
                if (data != null)
                {
                    personId = data.PersonId;
                    userModel.CentreCode = data.CentreCode;
                    userModel.EmployeeDesignation = _employeeDesignationMasterRepository.Table.Where(x => x.EmployeeDesignationMasterId == data.EmployeeDesignationMasterId)?.Select(x => x.Description)?.FirstOrDefault();
                }
            }
            GeneralPersonModel generalPersonModel = base.GetGeneralPersonDetails(personId);

            if (IsNull(generalPersonModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);
            long generalTrainerMasterId = 0;

            generalPersonModel.CentreName = new CoditechRepository<OrganisationCentreMaster>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => x.CentreCode == userModel.CentreCode)?.Select(x => x.CentreName)?.FirstOrDefault();

            if (userMasterData.UserType == UserTypeEnum.Employee.ToString() && (generalPersonModel.Custom1 == CustomConstants.DBTMTrainer || generalPersonModel.Custom1 == CustomConstants.DBTMCentreOwner))
                generalTrainerMasterId = Convert.ToInt64(_generalTrainerMasterRepository.Table.Where(x => x.EmployeeId == userMasterData.EntityId)?.Select(y => y.GeneralTrainerMasterId)?.FirstOrDefault());

            userModel.UserMasterId = userMasterData.UserMasterId;
            userModel.EntityId = userMasterData.EntityId;
            userModel.UserType = string.IsNullOrEmpty(generalPersonModel.Custom1) ? userMasterData.UserType : generalPersonModel.Custom1;
            userModel.EmailId = generalPersonModel.IsDataEncrypted ? DataEncryptionHelper.DataDecrypt(userMasterData.EmailId) : userMasterData.EmailId;
            userModel.IsPasswordChange = userMasterData.IsPasswordChange;
            userModel.IsAcceptedTermsAndConditions = userMasterData.IsAcceptedTermsAndConditions;
            userModel.IsTestUser = false;
            if (userMasterData.UserType == UserTypeEnum.Employee.ToString())
            {
                userModel.IsTestUser = Convert.ToBoolean(userMasterData.IsTestUser);
            }
            userModel.PhotoMediaPath = GetImagePath(generalPersonModel.PhotoMediaId);
            userModel.PersonTitle = generalPersonModel.PersonTitle;
            userModel.FirstName = generalPersonModel.FirstName;
            userModel.MiddleName = generalPersonModel.MiddleName;
            userModel.LastName = generalPersonModel.LastName;
            userModel.GeneralTrainerMasterId = generalTrainerMasterId;
            userModel.Custom1 = generalPersonModel.Custom1;
            userModel.CentreName = generalPersonModel.CentreName;
            userModel.DateOfBirth = generalPersonModel.DateOfBirth;
            userModel.DateOfJoining = userMasterData.CreatedDate.HasValue ? userMasterData.CreatedDate.Value : default(DateTime);
            return userModel;
        }

        public virtual DBTMUserModel GetDBTMTraineeDetails(long entityId, string userType)
        {
            if (entityId <= 0 || string.IsNullOrEmpty(userType))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            DBTMUserModel userModel = new DBTMUserModel();

            long personId = 0;
            if (userType == UserTypeEnum.Trainee.ToString())
            {
                DBTMTraineeDetails dbtmTraineeDetails = _dbtmTraineeDetailsRepository.Table.FirstOrDefault(x => x.DBTMTraineeDetailId == entityId);
                if (IsNotNull(dbtmTraineeDetails))
                {
                    userModel.PastInjuries = dbtmTraineeDetails.PastInjuries;
                    userModel.MedicalHistory = dbtmTraineeDetails.MedicalHistory;
                    userModel.OtherInformation = dbtmTraineeDetails.OtherInformation;
                    userModel.Height = dbtmTraineeDetails.Height;
                    userModel.Weight = dbtmTraineeDetails.Weight;
                    personId = dbtmTraineeDetails.PersonId;
                }
            }
            else if (userType == UserTypeEnum.Employee.ToString())
            {
                personId = _employeeMasterRepository.Table.Where(x => x.EmployeeId == entityId).Select(x => x.PersonId).FirstOrDefault();
            }

            if (personId <= 0)
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            GeneralPersonModel generalPersonModel = GetGeneralPersonDetails(personId);
            if (IsNull(generalPersonModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            userModel.EntityId = entityId;
            userModel.UserType = userType;
            userModel.PhotoMediaPath = GetImagePath(generalPersonModel.PhotoMediaId);
            userModel.PersonTitle = generalPersonModel.PersonTitle;
            userModel.FirstName = generalPersonModel.FirstName;
            userModel.MiddleName = generalPersonModel.MiddleName;
            userModel.LastName = generalPersonModel.LastName;
            userModel.EmailId = generalPersonModel.EmailId;
            userModel.DateOfBirth = generalPersonModel.DateOfBirth;
            userModel.Gender = GetEnumDisplayTextByEnumId(generalPersonModel.GenderEnumId);
            userModel.PhoneNumber = generalPersonModel.PhoneNumber;
            userModel.MobileNumber = generalPersonModel.MobileNumber;
            userModel.EmergencyContact = generalPersonModel.EmergencyContact;
            userModel.MaritalStatus = generalPersonModel.MaritalStatus;
            userModel.BirthMark = generalPersonModel.BirthMark;
            userModel.GeneralOccupationMasterId = generalPersonModel.GeneralOccupationMasterId;
            userModel.AnniversaryDate = generalPersonModel.AnniversaryDate;
            userModel.BloodGroup = generalPersonModel.BloodGroup ?? "NA";
            return userModel;
        }

        //Update Additional Information
        public virtual DBTMUserModel UpdateAdditionalInformation(DBTMUserModel dbtmUserModel)
        {
            if (IsNull(dbtmUserModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            dbtmUserModel.UserType = dbtmUserModel.UserType == "DBTMTrainer" ? UserTypeEnum.Employee.ToString() : dbtmUserModel.UserType;
            UserMaster userMasterData = _userMasterRepository.Table.Where(x => x.EntityId == dbtmUserModel.EntityId && x.UserType == dbtmUserModel.UserType)?.FirstOrDefault();
            bool status = false;
            long personId = 0;

            if (dbtmUserModel.UserType == UserTypeEnum.Trainee.ToString())
            {
                DBTMTraineeDetails dbtmTraineeDetails = _dbtmTraineeDetailsRepository.Table.Where(x => x.DBTMTraineeDetailId == dbtmUserModel.EntityId)?.FirstOrDefault();
                if (IsNull(dbtmTraineeDetails))
                {
                    dbtmUserModel.HasError = true;
                    dbtmUserModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
                }
                else
                {
                    personId = dbtmTraineeDetails.PersonId;
                    dbtmUserModel.ModifiedBy = Convert.ToInt64(userMasterData.ModifiedBy);
                    dbtmTraineeDetails.MedicalHistory = dbtmUserModel.MedicalHistory;
                    dbtmTraineeDetails.PastInjuries = dbtmUserModel.PastInjuries;
                    dbtmTraineeDetails.OtherInformation = dbtmUserModel.OtherInformation;
                    dbtmTraineeDetails.Height = dbtmUserModel.Height;
                    dbtmTraineeDetails.Weight = dbtmUserModel.Weight;
                    dbtmTraineeDetails.ModifiedBy = dbtmUserModel.ModifiedBy;
                    status = _dbtmTraineeDetailsRepository.Update(dbtmTraineeDetails);
                }
            }
            else if (dbtmUserModel.UserType == UserTypeEnum.Employee.ToString())
            {
                personId = _employeeMasterRepository.Table.Where(x => x.EmployeeId == dbtmUserModel.EntityId).Select(x => x.PersonId).FirstOrDefault();
                status = true;
            }

            if (personId <= 0)
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            if (status)
            {
                GeneralPerson generalPerson = _generalPersonRepository.Table.Where(x => x.PersonId == personId)?.FirstOrDefault();
                if (IsNotNull(generalPerson))
                {
                    generalPerson.EmailIdSearchTokens = SearchHashHelper.CreateSearchTokens(dbtmUserModel.EmailId);
                    generalPerson.EmergencyContactSearchTokens = SearchHashHelper.CreateSearchTokens(dbtmUserModel.EmergencyContact);
                    generalPerson.PhoneNumberSearchTokens = SearchHashHelper.CreateSearchTokens(dbtmUserModel.PhoneNumber);

                    generalPerson.MaritalStatus = dbtmUserModel.MaritalStatus;
                    generalPerson.BloodGroup = dbtmUserModel.BloodGroup;
                    generalPerson.BirthMark = dbtmUserModel.BirthMark;
                    generalPerson.GeneralOccupationMasterId = dbtmUserModel.GeneralOccupationMasterId;
                    generalPerson.AnniversaryDate = dbtmUserModel.AnniversaryDate;
                    generalPerson.EmailId = generalPerson.IsDataEncrypted ? DataEncryptionHelper.DataEncrypt(dbtmUserModel.EmailId) : dbtmUserModel.EmailId;
                    generalPerson.EmergencyContact = generalPerson.IsDataEncrypted ? DataEncryptionHelper.DataEncrypt(dbtmUserModel.EmergencyContact) : dbtmUserModel.EmergencyContact;
                    generalPerson.PhoneNumber = generalPerson.IsDataEncrypted ? DataEncryptionHelper.DataEncrypt(dbtmUserModel.PhoneNumber) : dbtmUserModel.PhoneNumber;
                    generalPerson.ModifiedBy = dbtmUserModel.ModifiedBy;
                    status = _generalPersonRepository.Update(generalPerson);
                    if (status)
                    {
                        string emailId = generalPerson.IsDataEncrypted ? DataEncryptionHelper.DataEncrypt(dbtmUserModel.EmailId) : dbtmUserModel.EmailId;
                        if (IsNotNull(userMasterData) && emailId != dbtmUserModel.EmailId)
                        {
                            userMasterData.EmailId = generalPerson.IsDataEncrypted ? DataEncryptionHelper.DataEncrypt(dbtmUserModel.EmailId) : dbtmUserModel.EmailId;
                            userMasterData.ModifiedBy = dbtmUserModel.ModifiedBy;
                            _userMasterRepository.Update(userMasterData);
                        }
                    }
                }
            }
            else
            {
                dbtmUserModel.HasError = true;
                dbtmUserModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return dbtmUserModel;
        }

        //validation Trainee Registration.
        public DBTMNewRegistrationModel ValidateTraineeJoiningCode(string joiningCode)
        {
            DBTMNewRegistrationModel model = new DBTMNewRegistrationModel();
            int traineeEnumId = GetEnumIdByEnumCode("Trainee", "OrganisationJoiningCodeType");
            OrganisationCentrewiseJoiningCode joiningCodeDetails = _organisationCentrewiseJoiningCodeRepository.Table.FirstOrDefault(x => x.JoiningCode == joiningCode && x.JoiningCodeTypeEnumId == traineeEnumId);
            if (IsNull(joiningCodeDetails))
            {
                model.HasError = true;
                model.ErrorMessage = "Invalid Trainee Joining Code.";
                return model;
            }
            if (joiningCodeDetails.IsExpired)
            {
                model.HasError = true;
                model.ErrorMessage = "Joining Code has expired.";
                return model;
            }
            if (!joiningCodeDetails.IsReserved && joiningCodeDetails.QueueValidTill.HasValue && joiningCodeDetails.QueueValidTill > DateTime.Now)
            {
                model.HasError = true;
                model.ErrorMessage = "This joining code is currently being used by another user. Please try again after some time.";
                return model;
            }
            model.CentreCode = joiningCode;
            return model;
        }

        public virtual DBTMNewRegistrationListModel GetGeneralTrainerByJoiningCode(string joiningCode, long generalTrainerMasterId)
        {
            if (string.IsNullOrEmpty(joiningCode))
                throw new CoditechException(ErrorCodes.AlreadyExist, "Joining Code is empty.");

            List<OrganisationCentrewiseJoiningCode> list = _organisationCentrewiseJoiningCodeRepository.Table.Where(x => x.JoiningCode == joiningCode.ToString() && x.QueueValidTill != null && x.QueueValidTill <= DateTime.Now).ToList();
            if (list?.Count > 0)
            {
                List<OrganisationCentrewiseJoiningCode> nonReservedList = list.Where(x => !x.IsReserved).ToList();
                if (nonReservedList.Count > 0)
                {
                    nonReservedList.ForEach(x => x.QueueValidTill = null);
                    _organisationCentrewiseJoiningCodeRepository.BatchUpdate(nonReservedList);
                }
            }

            int traineeEnumId = GetEnumIdByEnumCode("Trainee", "OrganisationJoiningCodeType");
            OrganisationCentrewiseJoiningCode joiningCodeDetails = _organisationCentrewiseJoiningCodeRepository.Table.FirstOrDefault(x => x.JoiningCode == joiningCode && x.JoiningCodeTypeEnumId == traineeEnumId);

            if (IsNull(joiningCodeDetails))
                throw new CoditechException(ErrorCodes.AlreadyExist, "Invalid Joining Code.");

            if (joiningCodeDetails.IsExpired)
                throw new CoditechException(ErrorCodes.InvalidData, "Joining Code has expired.");

            if (!joiningCodeDetails.IsReserved && IsNotNull(joiningCodeDetails.QueueValidTill) && generalTrainerMasterId == 0)
                throw new CoditechException(ErrorCodes.AlreadyExist, "This joining code is currently being used by another user. Please try again after some time.");

            DBTMNewRegistrationListModel listModel = new DBTMNewRegistrationListModel
            {
                JoiningCode = joiningCode,
                SelectedTrainerId = joiningCodeDetails.Custom1,
            };
            if (!joiningCodeDetails.IsReserved)
            {
                joiningCodeDetails.QueueValidTill = DateTime.Now.AddMinutes(ApiCustomSettings.JoiningCodeQueueTimeInMinutes);
                _organisationCentrewiseJoiningCodeRepository.Update(joiningCodeDetails);
            }
            return listModel;
        }

        //trainers by joiningcode 
        public virtual DBTMNewRegistrationListModel GetTrainerListByJoiningCode(string joiningCode)
        {

            PageListModel pageListModel = new PageListModel(null, null, 0, 0);
            CoditechViewRepository<DBTMNewRegistrationModel> objStoredProc = new CoditechViewRepository<DBTMNewRegistrationModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@JoiningCode", joiningCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMNewRegistrationModel> dBTMNewRegistrationList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetGeneralTrainerByJoiningCodeList @JoiningCode,@RowsCount OUT", 1, out pageListModel.TotalRowCount)?.ToList();
            if (dBTMNewRegistrationList?.Any() == true)
            {
                foreach (var item in dBTMNewRegistrationList?.Where(x => x.IsDataEncrypted))
                {
                    item.FirstName = DataEncryptionHelper.DataDecrypt(item.FirstName);
                    item.LastName = DataEncryptionHelper.DataDecrypt(item.LastName);
                }
                dBTMNewRegistrationList.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            }
            DBTMNewRegistrationListModel listModel = new DBTMNewRegistrationListModel
            {
                DBTMNewRegistrationList = dBTMNewRegistrationList?.Count > 0 ? dBTMNewRegistrationList : new List<DBTMNewRegistrationModel>()
            };
            if (listModel.DBTMNewRegistrationList == null || listModel.DBTMNewRegistrationList.Count == 0)
                throw new CoditechException(ErrorCodes.InvalidData, "No trainer is associated with this joining code. Please contact your administrator.");
            return listModel;
        }

        public virtual bool DeleteUserAccount(long entityId, string userType)
        {
            if (entityId <= 0)
            {
                throw new CoditechException(ErrorCodes.IdLessThanOne, "Invalid EntityId.");
            }
            if (string.IsNullOrWhiteSpace(userType))
            {
                throw new CoditechException(ErrorCodes.NullModel, "UserType is required.");
            }
            bool isTrainee = userType.Equals(UserTypeEnum.Trainee.ToString(), StringComparison.OrdinalIgnoreCase);
            if (!isTrainee)
            {
                throw new CoditechException(ErrorCodes.InvalidData, "Unsupported UserType.");
            }
            UserMaster userMaster = _userMasterRepository.Table.FirstOrDefault(x => x.EntityId == entityId && x.UserType == userType);
            if (IsNull(userMaster))
            {
                throw new CoditechException(ErrorCodes.NotFound, "User account not found.");
            }
            if (userMaster.IsDeleted)
            {
                throw new CoditechException(ErrorCodes.InvalidData, "User account is already deleted.");
            }
            long personId = 0;
            DBTMTraineeDetails trainee = null;
            if (isTrainee)
            {
                trainee = _dbtmTraineeDetailsRepository.Table.FirstOrDefault(x => x.DBTMTraineeDetailId == entityId);
                if (IsNull(trainee))
                {
                    throw new CoditechException(ErrorCodes.NotFound, "Trainee details not found.");
                }
                personId = trainee.PersonId;
            }
            GeneralPerson generalPerson = _generalPersonRepository.Table.FirstOrDefault(x => x.PersonId == personId);
            if (IsNull(generalPerson))
            {
                throw new CoditechException(ErrorCodes.NotFound, "Person details not found.");
            }
            DateTime deletedDate = DateTime.Now;
            generalPerson.FirstName = generalPerson.IsDataEncrypted ? DataEncryptionHelper.DataEncrypt("Deleted") : "Deleted";
            generalPerson.MiddleName = null;
            generalPerson.LastName = generalPerson.IsDataEncrypted ? DataEncryptionHelper.DataEncrypt("User") : "User";
            generalPerson.EmailId = generalPerson.IsDataEncrypted ? DataEncryptionHelper.DataEncrypt("deleteduser") : "deleteduser";
            generalPerson.MobileNumber = generalPerson.IsDataEncrypted ? DataEncryptionHelper.DataEncrypt("0000000000") : "0000000000";
            generalPerson.IsDeleted = true;

            generalPerson.EmergencyContact = null;
            generalPerson.EmergencyContactSearchTokens = null;
            generalPerson.PhoneNumber = null;
            generalPerson.IndentificationNumber = null;
            generalPerson.IndentificationNumberSearchTokens = null;
            generalPerson.AttendanceIntegrationId = null;
            generalPerson.AttendanceIntegrationIdSearchTokens = null;
            generalPerson.DateOfBirth = null;
            generalPerson.Custom2 = null;
            generalPerson.FirstNameSearchTokens = null;
            generalPerson.MiddleNameSearchTokens = null;
            generalPerson.LastNameSearchTokens = null;
            generalPerson.EmailIdSearchTokens = null;
            generalPerson.MobileNumberSearchTokens = null;
            generalPerson.Custom2 = "Deleted User";
            generalPerson.DeletedDate = deletedDate;
            generalPerson.ModifiedDate = deletedDate;

            userMaster.IsDeleted = true;
            userMaster.IsActive = false;
            userMaster.UserName = generalPerson.IsDataEncrypted ? DataEncryptionHelper.DataEncrypt("deleteduser") : "deleteduser";
            userMaster.Password = MD5Hash("deleteduser");
            userMaster.UserNameHash = null;
            userMaster.FirstName = generalPerson.IsDataEncrypted ? DataEncryptionHelper.DataEncrypt("Deleted") : "Deleted";
            userMaster.MiddleName = null;
            userMaster.LastName = generalPerson.IsDataEncrypted ? DataEncryptionHelper.DataEncrypt("User") : "User";
            userMaster.EmailId = generalPerson.IsDataEncrypted ? DataEncryptionHelper.DataEncrypt("deleteduser") : "deleteduser";
            userMaster.DeletedDate = deletedDate;
            userMaster.ModifiedDate = deletedDate;
            if (trainee != null)
            {
                trainee.IsActive = false;
                trainee.ModifiedDate = DateTime.Now;
            }
            bool personUpdated = _generalPersonRepository.Update(generalPerson);
            if (!personUpdated)
            {
                throw new CoditechException(ErrorCodes.InvalidData, "Unable to update person details.");
            }
            bool userUpdated = _userMasterRepository.Update(userMaster);
            if (!userUpdated)
            {
                throw new CoditechException(ErrorCodes.InvalidData, "Unable to update user account.");
            }
            if (trainee != null)
            {
                bool traineeUpdated = _dbtmTraineeDetailsRepository.Update(trainee);
                if (!traineeUpdated)
                {
                    throw new CoditechException(ErrorCodes.InvalidData, "Unable to deactivate trainee details.");
                }
            }
            return true;
        }
    }
}

using Coditech.API.Data;
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
        private readonly ICoditechRepository<UserMaster> _userMasterRepository;
        private readonly ICoditechRepository<DBTMTraineeDetails> _dbtmTraineeDetailsRepository;
        private readonly ICoditechRepository<EmployeeMaster> _employeeMasterRepository;
        private readonly ICoditechRepository<GeneralPerson> _generalPersonRepository;
        protected readonly ICoditechRepository<OrganisationCentrewiseJoiningCode> _organisationCentrewiseJoiningCodeRepository;
        private readonly ICoditechRepository<GeneralTrainerMaster> _generalTrainerMasterRepository;

        public DBTMUserService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _userMasterRepository = new CoditechRepository<UserMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _dbtmTraineeDetailsRepository = new CoditechRepository<DBTMTraineeDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _generalPersonRepository = new CoditechRepository<GeneralPerson>(_serviceProvider.GetService<Coditech_Entities>());
            _employeeMasterRepository = new CoditechRepository<EmployeeMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _organisationCentrewiseJoiningCodeRepository = new CoditechRepository<OrganisationCentrewiseJoiningCode>(_serviceProvider.GetService<Coditech_Entities>());
            _generalTrainerMasterRepository = new CoditechRepository<GeneralTrainerMaster>(_serviceProvider.GetService<Coditech_Entities>());
        }

        public virtual DBTMUserModel Login(UserLoginModel userLoginModel)
        {
            if (IsNull(userLoginModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            userLoginModel.Password = MD5Hash(userLoginModel.Password);
            UserMaster userMasterData = _userMasterRepository.Table.Where(x => x.UserName == userLoginModel.UserName && x.Password == userLoginModel.Password && (x.UserType == UserTypeEnum.Trainee.ToString() || x.UserType == UserTypeEnum.Employee.ToString()))?.FirstOrDefault();

            if (IsNull(userMasterData))
                throw new CoditechException(ErrorCodes.NotFound, null);
            else if (!userMasterData.IsActive)
                throw new CoditechException(ErrorCodes.ContactAdministrator, null);

            long personId = 0; string centreCode = string.Empty; string employeeDesignation = string.Empty;

            if (userMasterData.UserType == UserTypeEnum.Trainee.ToString())
            {
                var data = _dbtmTraineeDetailsRepository.Table.Where(x => x.DBTMTraineeDetailId == userMasterData.EntityId).Select(x => new { x.PersonId, x.CentreCode, x.SpecializationEnumId })?.FirstOrDefault();
                if (data != null)
                {
                    centreCode = data.CentreCode;
                    personId = data.PersonId;
                    employeeDesignation = GetEnumDisplayTextByEnumId(Convert.ToInt32(data.SpecializationEnumId));
                }
            }
            else if (userMasterData.UserType == UserTypeEnum.Employee.ToString())
            {
                var data = _employeeMasterRepository.Table.Where(x => x.EmployeeId == userMasterData.EntityId).Select(x => new { x.PersonId, x.CentreCode, x.EmployeeDesignationMasterId })?.FirstOrDefault();
                if (data != null)
                {
                    personId = data.PersonId;
                    centreCode = data.CentreCode;
                    employeeDesignation = GetEnumDisplayTextByEnumId(data.EmployeeDesignationMasterId);
                }
            }
            GeneralPersonModel generalPersonModel = base.GetGeneralPersonDetails(personId);

            if (IsNull(generalPersonModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);
            long generalTrainerMasterId = 0;

            generalPersonModel.CentreName = new CoditechRepository<OrganisationCentreMaster>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => x.CentreCode == centreCode)?.Select(x => x.CentreName)?.FirstOrDefault();

            if (userMasterData.UserType == UserTypeEnum.Employee.ToString() && (generalPersonModel.Custom1 == CustomConstants.DBTMTrainer || generalPersonModel.Custom1 == CustomConstants.DBTMCentreOwner))
                generalTrainerMasterId = Convert.ToInt64(_generalTrainerMasterRepository.Table.Where(x => x.EmployeeId == userMasterData.EntityId)?.Select(y => y.GeneralTrainerMasterId)?.FirstOrDefault());

            DBTMUserModel userModel = new DBTMUserModel()
            {
                UserMasterId = userMasterData.UserMasterId,
                EntityId = userMasterData.EntityId,
                UserType = string.IsNullOrEmpty(generalPersonModel.Custom1) ? userMasterData.UserType : generalPersonModel.Custom1,
                EmailId = userMasterData.EmailId,
                IsPasswordChange = userMasterData.IsPasswordChange,
                IsAcceptedTermsAndConditions = userMasterData.IsAcceptedTermsAndConditions,
                PhotoMediaPath = GetImagePath(generalPersonModel.PhotoMediaId),
                PersonTitle = generalPersonModel.PersonTitle,
                FirstName = generalPersonModel.FirstName,
                MiddleName = generalPersonModel.MiddleName,
                LastName = generalPersonModel.LastName,
                GeneralTrainerMasterId = generalTrainerMasterId,
                Custom1 = generalPersonModel.Custom1,
                CentreCode = centreCode,
                CentreName = generalPersonModel.CentreName,
                EmployeeDesignation = employeeDesignation,
            };
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
            }

            if (personId <= 0)
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            if (status)
            {
                GeneralPerson generalPerson = _generalPersonRepository.Table.Where(x => x.PersonId == personId)?.FirstOrDefault();
                if (IsNotNull(generalPerson))
                {
                    generalPerson.MaritalStatus = dbtmUserModel.MaritalStatus;
                    generalPerson.BloodGroup = dbtmUserModel.BloodGroup;
                    generalPerson.BirthMark = dbtmUserModel.BirthMark;
                    generalPerson.EmailId = dbtmUserModel.EmailId;
                    generalPerson.GeneralOccupationMasterId = dbtmUserModel.GeneralOccupationMasterId;
                    generalPerson.AnniversaryDate = dbtmUserModel.AnniversaryDate;
                    generalPerson.EmergencyContact = dbtmUserModel.EmergencyContact;
                    generalPerson.PhoneNumber = dbtmUserModel.PhoneNumber;
                    generalPerson.ModifiedBy = dbtmUserModel.ModifiedBy;
                    status = _generalPersonRepository.Update(generalPerson);
                    if (status)
                    {
                        if (IsNotNull(userMasterData))
                        {
                            userMasterData.EmailId = dbtmUserModel.EmailId;
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
        public virtual DBTMNewRegistrationListModel GetGeneralTrainerByJoiningCode(string joiningCode)
        {
            OrganisationCentrewiseJoiningCode joiningCodeDetails = null;
            joiningCodeDetails = _organisationCentrewiseJoiningCodeRepository.Table.Where(x => x.JoiningCode == joiningCode)?.FirstOrDefault();

            if (IsNull(joiningCodeDetails))
                throw new CoditechException(ErrorCodes.AlreadyExist, string.Format("Invalid Joning Code."));

            if (joiningCodeDetails.IsExpired)
                throw new CoditechException(ErrorCodes.InvalidData, "Joining Code has expired.");
            PageListModel pageListModel = new PageListModel(null, null, 0, 0);
            CoditechViewRepository<DBTMNewRegistrationModel> objStoredProc = new CoditechViewRepository<DBTMNewRegistrationModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@JoiningCode", joiningCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMNewRegistrationModel> dBTMNewRegistrationList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetGeneralTrainerByJoiningCodeList @JoiningCode,@RowsCount OUT", 1, out pageListModel.TotalRowCount)?.ToList();
            DBTMNewRegistrationListModel listModel = new DBTMNewRegistrationListModel();
            listModel.DBTMNewRegistrationList = dBTMNewRegistrationList?.Count > 0 ? dBTMNewRegistrationList : new List<DBTMNewRegistrationModel>();
            if (listModel.DBTMNewRegistrationList == null || listModel.DBTMNewRegistrationList.Count == 0)
                throw new CoditechException(ErrorCodes.InvalidData, "No trainer is associated with this joining code. Please contact your administrator ");
            return listModel;
        }



    }
}

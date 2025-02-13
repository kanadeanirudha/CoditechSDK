using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Resources;

using static Coditech.Common.Helper.HelperUtility;

namespace Coditech.API.Service
{
    public class DBTMUserService : BaseService, IDBTMUserService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<UserMaster> _userMasterRepository;
        private readonly ICoditechRepository<DBTMTraineeDetails> _dbtmTraineeDetailsRepository;
        private readonly ICoditechRepository<GeneralPerson> _generalPersonRepository;
        public DBTMUserService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _userMasterRepository = new CoditechRepository<UserMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _dbtmTraineeDetailsRepository = new CoditechRepository<DBTMTraineeDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _generalPersonRepository = new CoditechRepository<GeneralPerson>(_serviceProvider.GetService<Coditech_Entities>());
        }

        public virtual DBTMUserModel Login(UserLoginModel userLoginModel)
        {
            if (IsNull(userLoginModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            userLoginModel.Password = MD5Hash(userLoginModel.Password);
            UserMaster userMasterData = _userMasterRepository.Table.FirstOrDefault(x => x.UserName == userLoginModel.UserName && x.Password == userLoginModel.Password && x.UserType == UserTypeEnum.Trainee.ToString());

            if (IsNull(userMasterData))
                throw new CoditechException(ErrorCodes.NotFound, null);
            else if (!userMasterData.IsActive)
                throw new CoditechException(ErrorCodes.ContactAdministrator, null);

            long personId = _dbtmTraineeDetailsRepository.Table.Where(x => x.DBTMTraineeDetailId == userMasterData.EntityId).FirstOrDefault().PersonId;

            GeneralPersonModel generalPersonModel = GetGeneralPersonDetails(personId);
            if (IsNull(generalPersonModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            DBTMUserModel userModel = new DBTMUserModel()
            {
                EntityId = userMasterData.EntityId,
                IsPasswordChange = userMasterData.IsPasswordChange,
                IsAcceptedTermsAndConditions = userMasterData.IsAcceptedTermsAndConditions,
                PhotoMediaPath = GetImagePath(generalPersonModel.PhotoMediaId),
                PersonTitle = generalPersonModel.PersonTitle,
                FirstName = generalPersonModel.FirstName,
                MiddleName = generalPersonModel.MiddleName,
                LastName = generalPersonModel.LastName
            };
            return userModel;
        }

        public virtual DBTMUserModel GetDBTMTraineeDetails(long entityId)
        {
            if (entityId <= 0)
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            DBTMTraineeDetails dbtmTraineeDetails = _dbtmTraineeDetailsRepository.Table.FirstOrDefault(x => x.DBTMTraineeDetailId == entityId);
            DBTMUserModel userModel = new DBTMUserModel();

            if (IsNotNull(dbtmTraineeDetails))
            {
                userModel.PastInjuries = dbtmTraineeDetails.PastInjuries;
                userModel.MedicalHistory = dbtmTraineeDetails.MedicalHistory;
                userModel.OtherInformation = dbtmTraineeDetails.OtherInformation;
            }

            GeneralPersonModel generalPersonModel = GetGeneralPersonDetails(dbtmTraineeDetails.PersonId);
            if (IsNull(generalPersonModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            userModel.EntityId = entityId;
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
            userModel.BloodGroup = generalPersonModel.BloodGroup;
            return userModel;
        }

        //Update Additional Information
        public virtual DBTMUserModel UpdateAdditionalInformation(DBTMUserModel dbtmUserModel)
        {
            if (IsNull(dbtmUserModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            DBTMTraineeDetails dbtmTraineeDetails = _dbtmTraineeDetailsRepository.Table.Where(x => x.DBTMTraineeDetailId == dbtmUserModel.EntityId)?.FirstOrDefault();

            if (IsNull(dbtmTraineeDetails))
            {
                dbtmUserModel.HasError = true;
                dbtmUserModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            else
            {
                UserMaster userMasterData = _userMasterRepository.Table.Where(x => x.EntityId == dbtmUserModel.EntityId && x.UserType == UserTypeEnum.Trainee.ToString())?.FirstOrDefault();
                dbtmUserModel.ModifiedBy = Convert.ToInt64(userMasterData.ModifiedBy);
                dbtmTraineeDetails.MedicalHistory = dbtmUserModel.MedicalHistory;
                dbtmTraineeDetails.PastInjuries = dbtmUserModel.PastInjuries;
                dbtmTraineeDetails.OtherInformation = dbtmUserModel.OtherInformation;
                dbtmTraineeDetails.ModifiedBy = dbtmUserModel.ModifiedBy;
                bool status = _dbtmTraineeDetailsRepository.Update(dbtmTraineeDetails);
                if (status)
                {
                    GeneralPerson generalPerson = _generalPersonRepository.Table.Where(x => x.PersonId == dbtmTraineeDetails.PersonId)?.FirstOrDefault();
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
            }
            return dbtmUserModel;
        }

    }
}

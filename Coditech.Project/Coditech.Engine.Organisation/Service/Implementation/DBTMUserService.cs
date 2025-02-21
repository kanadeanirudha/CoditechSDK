using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using System.Diagnostics;

using static Coditech.Common.Helper.HelperUtility;

namespace Coditech.API.Service
{
    public class DBTMUserService : UserService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMTraineeDetails> _dBTMTraineeDetailsRepository;
        private readonly ICoditechRepository<UserMaster> _userMasterRepository;
        private readonly ICoditechRepository<GeneralTrainerMaster> _generalTrainerMasterRepository;


        public DBTMUserService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider, ICoditechEmail coditechEmail, ICoditechSMS coditechSMS, ICoditechWhatsApp coditechWhatsApp) : base(coditechLogging, serviceProvider, coditechEmail, coditechSMS, coditechWhatsApp)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMTraineeDetailsRepository = new CoditechRepository<DBTMTraineeDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _userMasterRepository = new CoditechRepository<UserMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _generalTrainerMasterRepository = new CoditechRepository<GeneralTrainerMaster>(_serviceProvider.GetService<Coditech_Entities>());
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
                return base.BindGeneralPersonInformation(personId, centreCode, personCode, generalDepartmentMasterId);
            }
            else
            {
                return base.GetGeneralPersonDetailsByEntityType(entityId, entityType);
            }
        }

        protected override void InsertPersonDetails(GeneralPersonModel generalPersonModel, List<GeneralSystemGlobleSettingModel> settingMasterList)
        {
            if (generalPersonModel.UserType.Equals(UserTypeEnum.Trainee.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                InsertDBTMTraineeDetails(generalPersonModel, settingMasterList);
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
        private void InsertDBTMTraineeDetails(GeneralPersonModel generalPersonModel, List<GeneralSystemGlobleSettingModel> settingMasterList)
        {
            generalPersonModel.PersonCode = GenerateRegistrationCode(GeneralRunningNumberForCustomEnum.DBTMTraineeRegistration.ToString(), generalPersonModel.SelectedCentreCode);
            DBTMTraineeDetails dBTMTraineeDetails = new DBTMTraineeDetails()
            {
                CentreCode = generalPersonModel.SelectedCentreCode,
                PersonId = generalPersonModel.PersonId,
                PersonCode = generalPersonModel.PersonCode,
                UserType = generalPersonModel.UserType
            };
            dBTMTraineeDetails = _dBTMTraineeDetailsRepository.Insert(dBTMTraineeDetails);

            //Check Is DBTM Trainee need to Login
            if (dBTMTraineeDetails?.DBTMTraineeDetailId > 0 && settingMasterList?.FirstOrDefault(x => x.FeatureName.Equals(GeneralSystemGlobleSettingCustomEnum.IsDBTMTraineeLogin.ToString(), StringComparison.InvariantCultureIgnoreCase)).FeatureValue == "1")
            {
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
    }
}

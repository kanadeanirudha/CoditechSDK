using Coditech.API.Data;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Resources;
using System.Data;
using System.Diagnostics;
using System.Text;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class DBTMNewRegistrationService : BaseService, IDBTMNewRegistrationService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        protected readonly ICoditechRepository<OrganisationCentreMaster> _organisationCentreMasterRepository;
        protected readonly ICoditechRepository<DBTMDeviceMaster> _dbtmDeviceMasterRepository;

        //protected virtual readonly ICoditech _dBTMDeviceMasterRepository;
        public DBTMNewRegistrationService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _organisationCentreMasterRepository = new CoditechRepository<OrganisationCentreMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _dbtmDeviceMasterRepository = new CoditechRepository<DBTMDeviceMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }

        //Create DBTMDevice.
        public virtual DBTMNewRegistrationModel DBTMNewRegistration(DBTMNewRegistrationModel dBTMNewRegistrationModel)
        {
            if (IsNull(dBTMNewRegistrationModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            if (IsCentreNameAlreadyExist(dBTMNewRegistrationModel.CentreName))
                throw new CoditechException(ErrorCodes.AlreadyExist, string.Format(GeneralResources.ErrorCodeExists, "Centre Name"));

            DBTMDeviceMaster dBTMDeviceMaster = new DBTMDeviceMasterService(_coditechLogging, _serviceProvider).GetDBTMDeviceMasterDetailsByCode(dBTMNewRegistrationModel.DeviceSerialCode);

            if (dBTMDeviceMaster == null || dBTMDeviceMaster.DBTMDeviceMasterId <= 0)
                throw new CoditechException(ErrorCodes.AlreadyExist, string.Format("Invalid Device Serial Code."));

            if (new DBTMDeviceRegistrationDetailsService(_coditechLogging, _serviceProvider).IsDeviceSerialCodeAlreadyExist(dBTMDeviceMaster.DBTMDeviceMasterId))
                throw new CoditechException(ErrorCodes.AlreadyExist, string.Format(GeneralResources.ErrorCodeExists, "Device Already Added"));

            string centreCode = "HO";
            List<GeneralRunningNumbers> generalRunningNumbersList = GetGeneralRunningNumbersList(centreCode);

            if (IsNull(generalRunningNumbersList) || generalRunningNumbersList.Count == 0)
                throw new CoditechException(ErrorCodes.InvalidData, string.Format("EmployeeRegistration and DBTMTraineeRegistration not set for HO."));

            if (dBTMNewRegistrationModel.IsCentreRegistration)
            {
                OrganisationCentreMaster organisationCentreMaster = null;
                long personId = 0;
                long employeeId = 0;
                string userType = UserTypeEnum.Employee.ToString();
                string sanctionPostCode = string.Empty;
                try
                {
                    DateTime currentDate = DateTime.Now;
                    //Save Centre 
                    organisationCentreMaster = InsertOrganisationCentreMaster(dBTMNewRegistrationModel, currentDate);

                    //Centre SMTP Setting
                    InsertOrganisationCentrewiseSmtpSetting(currentDate, organisationCentreMaster, centreCode);

                    //Centre SMS Setting
                    InsertOrganisationCentrewiseSmsSetting(currentDate, organisationCentreMaster, centreCode);

                    //Centre WhatsApp Setting
                    InsertOrganisationCentrewiseWhatsAppSetting(currentDate, organisationCentreMaster, centreCode);

                    //Centre Email Template
                    InsertOrganisationCentrewiseEmailTemplate(currentDate, organisationCentreMaster, centreCode);

                    //Centre UserName Registration
                    InsertOrganisationCentrewiseUserNameRegistration(currentDate, organisationCentreMaster, centreCode);

                    //Centre Department
                    List<short> generalDepartmentMasterList = InsertOrganisationCentrewiseDepartment(currentDate, organisationCentreMaster);

                    //Centre UserName Registration
                    InsertGeneralRunningNumbers(generalRunningNumbersList, currentDate, organisationCentreMaster, centreCode);

                    //Insert General Person and registor employee
                    employeeId = InsertEmployee(dBTMNewRegistrationModel, currentDate, organisationCentreMaster, generalDepartmentMasterList, out personId);

                    //Insert Employee Address
                    InsertEmployeeAddress(dBTMNewRegistrationModel, currentDate, personId);

                    //Insert Admin Role
                    InsertAdminRole(currentDate, generalDepartmentMasterList.FirstOrDefault(), organisationCentreMaster.CentreCode, employeeId, out sanctionPostCode);

                    //DBTM Device Registration Details
                    InsertDBTMDeviceRegistration(dBTMNewRegistrationModel, currentDate, employeeId, dBTMDeviceMaster.DBTMDeviceMasterId);
                }
                catch (Exception ex)
                {
                    dBTMNewRegistrationModel.HasError = true;
                    dBTMNewRegistrationModel.ErrorMessage = GeneralResources.ErrorFailedToCreate;
                    _coditechLogging.LogMessage(ex, "DBTMNewRegistration", TraceLevel.Error);
                    //Call Delete data method
                    CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>(_serviceProvider.GetService<CoditechCustom_Entities>());
                    objStoredProc.SetParameter("NewCentreCode", organisationCentreMaster.CentreCode, ParameterDirection.Input, DbType.String);
                    objStoredProc.SetParameter("EntityId", employeeId, ParameterDirection.Input, DbType.String);
                    objStoredProc.SetParameter("UserType", userType, ParameterDirection.Input, DbType.String);
                    objStoredProc.SetParameter("SanctionPostCode", sanctionPostCode, ParameterDirection.Input, DbType.String);
                    objStoredProc.SetParameter("PersonId", personId, ParameterDirection.Input, DbType.String);
                    objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
                    int status = 0;
                    objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteDBTMNewRegistration @NewCentreCode,@EntityId,@UserType,@SanctionPostCode,@PersonId,@Status OUT", 5, out status);
                }
            }
            return dBTMNewRegistrationModel;
        }

        protected List<GeneralRunningNumbers> GetGeneralRunningNumbersList(string centreCode)
        {
            List<string> runningNumnereList = ("EmployeeRegistration,DBTMTraineeRegistration").Split(",").ToList();
            List<int> generalEnumaratorIdList = new CoditechRepository<GeneralEnumaratorMaster>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => runningNumnereList.Contains(x.EnumName))?.Select(x => x.GeneralEnumaratorId)?.ToList();
            List<GeneralRunningNumbers> generalRunningNumbersList = new CoditechRepository<GeneralRunningNumbers>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => x.CentreCode == centreCode && generalEnumaratorIdList.Contains(x.KeyFieldEnumId))?.ToList();
            return generalRunningNumbersList;
        }

        protected virtual void InsertEmployeeAddress(DBTMNewRegistrationModel dBTMNewRegistrationModel, DateTime currentDate, long personId)
        {
            //Insert DBTM Devices
            GeneralPersonAddress generalPersonAddress = new GeneralPersonAddress()
            {
                AddressTypeEnum = AddressTypeEnum.PermanentAddress.ToString(),
                PersonId = personId,
                FirstName = dBTMNewRegistrationModel.FirstName,
                LastName = dBTMNewRegistrationModel.LastName,
                AddressLine1 = dBTMNewRegistrationModel.AddressLine1,
                AddressLine2 = dBTMNewRegistrationModel.AddressLine2,
                GeneralCountryMasterId = dBTMNewRegistrationModel.GeneralCountryMasterId,
                GeneralRegionMasterId = dBTMNewRegistrationModel.GeneralRegionMasterId,
                GeneralCityMasterId = dBTMNewRegistrationModel.GeneralCityMasterId,
                Postalcode = dBTMNewRegistrationModel.Pincode,
                CreatedDate = currentDate,
                ModifiedDate = currentDate,
            };
            new CoditechRepository<GeneralPersonAddress>(_serviceProvider.GetService<Coditech_Entities>()).Insert(generalPersonAddress);
        }

        protected virtual void InsertDBTMDeviceRegistration(DBTMNewRegistrationModel dBTMNewRegistrationModel, DateTime currentDate, long employeeId, long dBTMDeviceMasterId)
        {
            //Insert DBTM Devices
            DBTMDeviceRegistrationDetailsModel dBTMDeviceRegistrationDetailsModel = new DBTMDeviceRegistrationDetailsModel()
            {
                DBTMDeviceMasterId = dBTMDeviceMasterId,
                DeviceSerialCode = dBTMNewRegistrationModel.DeviceSerialCode,
                UserType = UserTypeEnum.Employee.ToString(),
                EntityId = employeeId,
                PurchaseDate = currentDate,
                CreatedDate = currentDate,
                ModifiedDate = currentDate,
            };
            new DBTMDeviceRegistrationDetailsService(_coditechLogging, _serviceProvider).CreateRegistrationDetails(dBTMDeviceRegistrationDetailsModel);
        }

        protected virtual long InsertEmployee(DBTMNewRegistrationModel dBTMNewRegistrationModel, DateTime currentDate, OrganisationCentreMaster organisationCentreMaster, List<short> generalDepartmentMasterList, out long personId)
        {
            personId = 0;
            GeneralPersonModel generalPersonModel = new GeneralPersonModel()
            {
                PersonTitle = dBTMNewRegistrationModel.PersonTitle,
                FirstName = dBTMNewRegistrationModel.FirstName,
                LastName = dBTMNewRegistrationModel.LastName,
                GenderEnumId = dBTMNewRegistrationModel.GenderEnumId,
                EmailId = dBTMNewRegistrationModel.EmailId,
                PhoneNumber = dBTMNewRegistrationModel.MobileNumber,
                CallingCode = dBTMNewRegistrationModel.CallingCode,
                MobileNumber = dBTMNewRegistrationModel.MobileNumber,
                Password = dBTMNewRegistrationModel.Password,
                UserType = UserTypeEnum.Employee.ToString(),
                CreatedDate = currentDate,
                ModifiedDate = currentDate,
                IsPasswordChange = true,
            };
            GeneralPerson personData = InsertGeneralPersonData(generalPersonModel);
            long employeeId = 0;
            if (personData?.PersonId > 0)
            {
                personId = personData.PersonId;
                generalPersonModel.CentreName = GetOrganisationCentreNameByCentreCode(generalPersonModel.SelectedCentreCode);
                List<GeneralSystemGlobleSettingModel> settingMasterList = GetSystemGlobleSettingList();
                generalPersonModel.PersonId = personData.PersonId;
                generalPersonModel.SelectedCentreCode = organisationCentreMaster.CentreCode;
                generalPersonModel.SelectedDepartmentId = generalDepartmentMasterList.FirstOrDefault().ToString();
                generalPersonModel.EmployeeDesignationMasterId = 1;
                employeeId = base.InsertEmployee(generalPersonModel, settingMasterList, true);
            }
            return employeeId;
        }

        protected virtual void InsertGeneralRunningNumbers(List<GeneralRunningNumbers> generalRunningNumbersList, DateTime currentDate, OrganisationCentreMaster organisationCentreMaster, string centreCode)
        {
            foreach (var item in generalRunningNumbersList)
            {
                item.GeneralRunningNumberId = 0;
                item.CentreCode = organisationCentreMaster.CentreCode;
                item.CurrentSequnce = 0;
                item.CreatedDate = currentDate;
                item.ModifiedDate = currentDate;
            }
            new CoditechRepository<GeneralRunningNumbers>(_serviceProvider.GetService<Coditech_Entities>()).Insert(generalRunningNumbersList);
        }

        protected virtual List<short> InsertOrganisationCentrewiseDepartment(DateTime currentDate, OrganisationCentreMaster organisationCentreMaster)
        {
            List<string> departmentList = new List<string>();
            departmentList = ("DBTMCentreDirector,DBTMManager,DBTMTrainer").Split(",").ToList();
            List<short> generalDepartmentMasterList = new CoditechRepository<GeneralDepartmentMaster>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => departmentList.Contains(x.DepartmentShortCode))?.Select(x => x.GeneralDepartmentMasterId).ToList();
            if (generalDepartmentMasterList?.Count == 0)
            {
                List<GeneralDepartmentMaster> departmentMasterList = new List<GeneralDepartmentMaster>();
                foreach (var item in departmentList)
                {
                    GeneralDepartmentMaster generalDepartmentMaster = new GeneralDepartmentMaster();
                    if (item == "DBTMCentreDirector")
                        generalDepartmentMaster.DepartmentName = "Director";
                    else if (item == "DBTMManager")
                        generalDepartmentMaster.DepartmentName = "Manager";
                    else if (item == "DBTMTrainer")
                        generalDepartmentMaster.DepartmentName = "Trainer";
                    else
                        generalDepartmentMaster.DepartmentName = item;

                    generalDepartmentMaster.DepartmentShortCode = item;
                    generalDepartmentMaster.CreatedDate = currentDate;
                    generalDepartmentMaster.ModifiedDate = currentDate;
                    departmentMasterList.Add(generalDepartmentMaster);
                }
                new CoditechRepository<GeneralDepartmentMaster>(_serviceProvider.GetService<Coditech_Entities>()).Insert(departmentMasterList);

                generalDepartmentMasterList = new CoditechRepository<GeneralDepartmentMaster>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => departmentList.Contains(x.DepartmentShortCode))?.Select(x => x.GeneralDepartmentMasterId).ToList();
            }

            List<OrganisationCentrewiseDepartment> organizationDeptList = new List<OrganisationCentrewiseDepartment>();
            foreach (var item in generalDepartmentMasterList)
            {
                OrganisationCentrewiseDepartment organisationCentrewiseDepartment = new OrganisationCentrewiseDepartment();
                organisationCentrewiseDepartment.CentreCode = organisationCentreMaster.CentreCode;
                organisationCentrewiseDepartment.GeneralDepartmentMasterId = item;
                organisationCentrewiseDepartment.ActiveFlag = true;
                organisationCentrewiseDepartment.CreatedDate = currentDate;
                organisationCentrewiseDepartment.ModifiedDate = currentDate;
                organizationDeptList.Add(organisationCentrewiseDepartment);
            }
            new CoditechRepository<OrganisationCentrewiseDepartment>(_serviceProvider.GetService<Coditech_Entities>()).Insert(organizationDeptList);

            return generalDepartmentMasterList;
        }

        protected virtual void InsertOrganisationCentrewiseUserNameRegistration(DateTime currentDate, OrganisationCentreMaster organisationCentreMaster, string centreCode)
        {
            List<string> userTypeList = new List<string>();
            userTypeList = ($"{UserTypeEnum.Employee.ToString()},{UserTypeEnum.Trainee.ToString()}").Split(",").ToList();
            List<OrganisationCentrewiseUserNameRegistration> organisationCentrewiseUserNameRegistrationList = new CoditechRepository<OrganisationCentrewiseUserNameRegistration>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => x.CentreCode == centreCode && userTypeList.Contains(x.UserType))?.ToList();
            if (IsNotNull(organisationCentrewiseUserNameRegistrationList))
            {
                foreach (var item in organisationCentrewiseUserNameRegistrationList)
                {
                    item.OrganisationCentrewiseUserNameRegistrationId = 0;
                    item.CentreCode = organisationCentreMaster.CentreCode;
                    item.CreatedDate = currentDate;
                    item.ModifiedDate = currentDate;
                }
                new CoditechRepository<OrganisationCentrewiseUserNameRegistration>(_serviceProvider.GetService<Coditech_Entities>()).Insert(organisationCentrewiseUserNameRegistrationList);
            }
        }

        protected virtual void InsertOrganisationCentrewiseEmailTemplate(DateTime currentDate, OrganisationCentreMaster organisationCentreMaster, string centreCode)
        {
            List<string> emailTemplateList = new List<string>();
            emailTemplateList = ("EmployeeRegistration,MobileResetPasswordLink,ResetPasswordLink").Split(",").ToList();
            List<OrganisationCentrewiseEmailTemplate> organisationCentrewiseEmailTemplateList = new CoditechRepository<OrganisationCentrewiseEmailTemplate>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => x.CentreCode == centreCode && emailTemplateList.Contains(x.EmailTemplateCode) && x.IsActive)?.ToList();
            if (IsNotNull(organisationCentrewiseEmailTemplateList))
            {
                foreach (var item in organisationCentrewiseEmailTemplateList)
                {
                    item.OrganisationCentrewiseEmailTemplateId = 0;
                    item.CentreCode = organisationCentreMaster.CentreCode;
                    item.CreatedDate = currentDate;
                    item.ModifiedDate = currentDate;
                }
                new CoditechRepository<OrganisationCentrewiseEmailTemplate>(_serviceProvider.GetService<Coditech_Entities>()).Insert(organisationCentrewiseEmailTemplateList);
            }
        }

        protected virtual void InsertOrganisationCentrewiseWhatsAppSetting(DateTime currentDate, OrganisationCentreMaster organisationCentreMaster, string centreCode)
        {
            OrganisationCentrewiseWhatsAppSetting organisationCentrewiseWhatsAppSetting = new CoditechRepository<OrganisationCentrewiseWhatsAppSetting>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => x.CentreCode == centreCode)?.FirstOrDefault();
            if (IsNotNull(organisationCentrewiseWhatsAppSetting))
            {
                organisationCentrewiseWhatsAppSetting.OrganisationCentrewiseWhatsAppSettingId = 0;
                organisationCentrewiseWhatsAppSetting.CentreCode = organisationCentreMaster.CentreCode;
                organisationCentrewiseWhatsAppSetting.CreatedDate = currentDate;
                organisationCentrewiseWhatsAppSetting.ModifiedDate = currentDate;
                new CoditechRepository<OrganisationCentrewiseWhatsAppSetting>(_serviceProvider.GetService<Coditech_Entities>()).Insert(organisationCentrewiseWhatsAppSetting);
            }
        }

        protected virtual void InsertOrganisationCentrewiseSmsSetting(DateTime currentDate, OrganisationCentreMaster organisationCentreMaster, string centreCode)
        {
            OrganisationCentrewiseSmsSetting organisationCentrewiseSms = new CoditechRepository<OrganisationCentrewiseSmsSetting>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => x.CentreCode == centreCode)?.FirstOrDefault();
            if (IsNotNull(organisationCentrewiseSms))
            {
                organisationCentrewiseSms.OrganisationCentrewiseSmsSettingId = 0;
                organisationCentrewiseSms.CentreCode = organisationCentreMaster.CentreCode;
                organisationCentrewiseSms.CreatedDate = currentDate;
                organisationCentrewiseSms.ModifiedDate = currentDate;
                new CoditechRepository<OrganisationCentrewiseSmsSetting>(_serviceProvider.GetService<Coditech_Entities>()).Insert(organisationCentrewiseSms);
            }
        }

        protected virtual void InsertOrganisationCentrewiseSmtpSetting(DateTime currentDate, OrganisationCentreMaster organisationCentreMaster, string centreCode)
        {
            OrganisationCentrewiseSmtpSetting organisationCentrewiseSmtp = new CoditechRepository<OrganisationCentrewiseSmtpSetting>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => x.CentreCode == centreCode)?.FirstOrDefault();
            if (IsNotNull(organisationCentrewiseSmtp))
            {
                organisationCentrewiseSmtp.OrganisationCentrewiseSmtpSettingId = 0;
                organisationCentrewiseSmtp.CentreCode = organisationCentreMaster.CentreCode;
                organisationCentrewiseSmtp.FromDisplayName = organisationCentreMaster.CentreName;
                organisationCentrewiseSmtp.CreatedDate = currentDate;
                organisationCentrewiseSmtp.ModifiedDate = currentDate;
                new CoditechRepository<OrganisationCentrewiseSmtpSetting>(_serviceProvider.GetService<Coditech_Entities>()).Insert(organisationCentrewiseSmtp);
            }
        }

        protected virtual OrganisationCentreMaster InsertOrganisationCentreMaster(DBTMNewRegistrationModel dBTMNewRegistrationModel, DateTime currentDate)
        {
            OrganisationCentreMaster organisationCentreMaster = new OrganisationCentreMaster()
            {
                CentreCode = GenerateCentreCode(8),
                OrganisationId = 1,
                CentreName = dBTMNewRegistrationModel.CentreName,
                HoCoRoScFlag = "CO",
                EmailId = dBTMNewRegistrationModel.EmailId,
                PhoneNumberOffice = dBTMNewRegistrationModel.MobileNumber,
                CellPhone = dBTMNewRegistrationModel.MobileNumber,
                CentreAddress = dBTMNewRegistrationModel.AddressLine1 + " " + dBTMNewRegistrationModel.AddressLine2,
                GeneralCityMasterId = dBTMNewRegistrationModel.GeneralCityMasterId,
                Pincode = dBTMNewRegistrationModel.Pincode,
                TimeZone = "Asia/Kolkata",
                CreatedDate = currentDate,
                ModifiedDate = currentDate
            };
            _organisationCentreMasterRepository.Insert(organisationCentreMaster);
            return organisationCentreMaster;
        }

        // Generates a random string with a given size.
        protected virtual string GenerateCentreCode(short size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length = 26
            Random _random = new Random();
            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        protected virtual bool IsCentreNameAlreadyExist(string centreName)
        {
            if (string.IsNullOrWhiteSpace(centreName))
            {
                throw new ArgumentException("Centre name cannot be null or empty");
            }
            return _organisationCentreMasterRepository.Table.Any(x => x.CentreName == centreName);
        }

        //Create adminSanctionPost.
        protected virtual void InsertAdminRole(DateTime currentDate, short departmentId, string centreCode, long employeeId, out string sanctionPostCode)
        {
            sanctionPostCode = string.Empty;
            AdminSanctionPostModel adminSanctionPostModel = new AdminSanctionPostModel()
            {
                DesignationId = 1,
                DepartmentId = departmentId,
                NoOfPost = 1,
                IsActive = true,
                CentreCode = centreCode,
                DesignationType = "Regular",
                PostType = "Permanent",
                CreatedDate = currentDate,
                ModifiedDate = currentDate
            };
            EmployeeDesignationMaster employeeDesignationMaster = GetDesignationDetails(adminSanctionPostModel.DesignationId);
            GeneralDepartmentMaster generalDepartmentMaster = GetDepartmentDetails(adminSanctionPostModel.DepartmentId);

            sanctionPostCode = adminSanctionPostModel.SanctionPostCode = $"{employeeDesignationMaster.ShortCode}-{generalDepartmentMaster.DepartmentShortCode}-{adminSanctionPostModel.CentreCode}";
            adminSanctionPostModel.SanctionedPostDescription = $"{employeeDesignationMaster.Description}-{generalDepartmentMaster.DepartmentName}-{adminSanctionPostModel.PostType}-{adminSanctionPostModel.DesignationType}";
            AdminSanctionPost adminSanctionPostEntity = adminSanctionPostModel.FromModelToEntity<AdminSanctionPost>();

            //Create new adminSanctionPost and return it.
            AdminSanctionPost adminSanctionPostData = new CoditechRepository<AdminSanctionPost>(_serviceProvider.GetService<Coditech_Entities>()).Insert(adminSanctionPostEntity);
            if (adminSanctionPostData?.AdminSanctionPostId > 0)
            {
                adminSanctionPostModel.AdminSanctionPostId = adminSanctionPostData.AdminSanctionPostId;

                AdminRoleMaster adminRoleMaster = new AdminRoleMaster()
                {
                    AdminSanctionPostId = adminSanctionPostModel.AdminSanctionPostId,
                    SanctionPostName = adminSanctionPostModel.SanctionedPostDescription,
                    MonitoringLevel = APIConstant.Self,
                    AdminRoleCode = adminSanctionPostModel.SanctionPostCode,
                    OthCentreLevel = string.Empty,
                    IsActive = true,
                    DashboardFormEnumId = GetEnumIdByEnumCode(DashboardFormEnum.DBTMCentreDashboard.ToString(), GeneralEnumaratorGroupCodeEnum.DashboardForm.ToString()),
                    CreatedDate = currentDate,
                    ModifiedDate = currentDate
                };
                //Create new adminRoleMaster
                adminRoleMaster = new CoditechRepository<AdminRoleMaster>(_serviceProvider.GetService<Coditech_Entities>()).Insert(adminRoleMaster);
                AdminRoleCentreRights adminRoleCentreRight = new AdminRoleCentreRights()
                {
                    AdminRoleMasterId = adminRoleMaster.AdminRoleMasterId,
                    CentreCode = adminSanctionPostModel.CentreCode,
                    IsActive = true,
                    CreatedDate = currentDate,
                    ModifiedDate = currentDate
                };

                //Create new adminRoleCentreRight
                adminRoleCentreRight = new CoditechRepository<AdminRoleCentreRights>(_serviceProvider.GetService<Coditech_Entities>()).Insert(adminRoleCentreRight);
                AdminRoleApplicableDetails adminRoleApplicableDetails = new AdminRoleApplicableDetails()
                {
                    AdminRoleMasterId = adminRoleMaster.AdminRoleMasterId,
                    EmployeeId = employeeId,
                    IsActive = true,
                    RoleType = "Regular",
                    CreatedDate = currentDate,
                    ModifiedDate = currentDate
                };
                new CoditechRepository<AdminRoleApplicableDetails>(_serviceProvider.GetService<Coditech_Entities>()).Insert(adminRoleApplicableDetails);

                //insert admin Role Menu Detail
                List<string> associateMenus = ApiCustomSettings.DBTMDirectorMenuCode.Split(",").ToList();
                List<UserMainMenuMaster> menuList = new CoditechRepository<UserMainMenuMaster>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => associateMenus.Contains(x.MenuCode)).ToList();
                List<AdminRoleMenuDetails> adminRoleMenuDetailList = new List<AdminRoleMenuDetails>();
                foreach (var menu in menuList)
                {
                    adminRoleMenuDetailList.Add(new AdminRoleMenuDetails()
                    {
                        AdminRoleMasterId = adminRoleMaster.AdminRoleMasterId,
                        AdminRoleCode = adminRoleMaster.AdminRoleCode,
                        ModuleCode = menu.ModuleCode,
                        MenuCode = menu.MenuCode,
                        EnableDate = currentDate,
                        IsActive = true,
                        CreatedDate = currentDate,
                        ModifiedDate = currentDate
                    });
                }
                new CoditechRepository<AdminRoleMenuDetails>(_serviceProvider.GetService<Coditech_Entities>()).Insert(adminRoleMenuDetailList);
            }
        }
    }
}
